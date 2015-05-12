/* This file is part of the db4o object database http://www.db4o.com

Copyright (C) 2004 - 2011  Versant Corporation http://www.versant.com

db4o is free software; you can redistribute it and/or modify it under
the terms of version 3 of the GNU General Public License as published
by the Free Software Foundation.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program.  If not, see http://www.gnu.org/licenses/. */

using System;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Caching;
using Sharpen.Lang;

namespace Db4objects.Db4o.IO
{
    /// <exclude></exclude>
    internal class CachingBin : BinDecorator
    {
        private readonly ICache4 _cache;
        private readonly IProcedure4 _onDiscardPage;
        private readonly IObjectPool _pagePool;
        private readonly int _pageSize;
        internal readonly IFunction4 _producerFromDisk;
        internal readonly IFunction4 _producerFromPool;
        private long _fileLength;

        /// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
        public CachingBin(IBin bin, ICache4 cache, int pageCount, int pageSize) : base(bin
            )
        {
            _onDiscardPage = new _IProcedure4_22(this);
            _producerFromDisk = new _IFunction4_138(this);
            _producerFromPool = new _IFunction4_147(this);
            _pageSize = pageSize;
            _pagePool = new SimpleObjectPool(NewPagePool(pageCount));
            _cache = cache;
            _fileLength = _bin.Length();
        }

        private Page[] NewPagePool(int pageCount)
        {
            var pages = new Page[pageCount];
            for (var i = 0; i < pages.Length; ++i)
            {
                pages[i] = new Page(_pageSize);
            }
            return pages;
        }

        /// <summary>Reads the file into the buffer using pages from cache.</summary>
        /// <remarks>
        ///     Reads the file into the buffer using pages from cache. If the next page
        ///     is not cached it will be read from the file.
        /// </remarks>
        /// <param name="pos">
        ///     start position to read
        /// </param>
        /// <param name="buffer">destination buffer</param>
        /// <param name="length">how many bytes to read</param>
        /// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
        public override int Read(long pos, byte[] buffer, int length)
        {
            return ReadInternal(pos, buffer, length, false);
        }

        private int ReadInternal(long pos, byte[] buffer, int length, bool syncRead)
        {
            var startAddress = pos;
            var bytesToRead = length;
            var totalRead = 0;
            while (bytesToRead > 0)
            {
                var page = syncRead
                    ? SyncReadPage(startAddress)
                    : GetPage(startAddress
                        , _producerFromDisk);
                var readBytes = page.Read(buffer, totalRead, startAddress, bytesToRead);
                if (readBytes <= 0)
                {
                    break;
                }
                bytesToRead -= readBytes;
                startAddress += readBytes;
                totalRead += readBytes;
            }
            return totalRead == 0 ? -1 : totalRead;
        }

        /// <summary>Writes the buffer to cache using pages</summary>
        /// <param name="pos">start position to write</param>
        /// <param name="buffer">source buffer</param>
        /// <param name="length">how many bytes to write</param>
        /// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
        public override void Write(long pos, byte[] buffer, int length)
        {
            var startAddress = pos;
            var bytesToWrite = length;
            var bufferOffset = 0;
            while (bytesToWrite > 0)
            {
                // page doesn't need to loadFromDisk if the whole page is dirty
                var loadFromDisk = (bytesToWrite < _pageSize) || (startAddress%_pageSize != 0);
                var page = GetPage(startAddress, loadFromDisk);
                var writtenBytes = page.Write(buffer, bufferOffset, startAddress, bytesToWrite);
                bytesToWrite -= writtenBytes;
                startAddress += writtenBytes;
                bufferOffset += writtenBytes;
            }
            var endAddress = startAddress;
            _fileLength = Math.Max(endAddress, _fileLength);
        }

        /// <summary>Flushes cache to a physical storage</summary>
        /// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
        public override void Sync()
        {
            FlushAllPages();
            base.Sync();
        }

        public override void Sync(IRunnable runnable)
        {
            FlushAllPages();
            base.Sync(new _IRunnable_119(this, runnable));
        }

        public override int SyncRead(long position, byte[] bytes, int bytesToRead)
        {
            return ReadInternal(position, bytes, bytesToRead, true);
        }

        /// <summary>Returns the file length</summary>
        /// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
        public override long Length()
        {
            return _fileLength;
        }

        /// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
        private Page GetPage(long startAddress, bool loadFromDisk)
        {
            var producer = loadFromDisk ? _producerFromDisk : _producerFromPool;
            return GetPage(startAddress, producer);
        }

        private Page GetPage(long startAddress, IFunction4 producer)
        {
            var page = ((Page) _cache.Produce(PageAddressFor(startAddress
                ), producer, _onDiscardPage));
            page.EnsureEndAddress(_fileLength);
            return page;
        }

        private Page SyncReadPage(long startAddress)
        {
            var page = new Page(_pageSize);
            LoadPage(page, startAddress);
            page.EnsureEndAddress(_fileLength);
            return page;
        }

        private long PageAddressFor(long startAddress)
        {
            return (startAddress/_pageSize)*_pageSize;
        }

        private void ResetPageAddress(Page page, long startAddress)
        {
            page._startAddress = startAddress;
            page._endAddress = startAddress + _pageSize;
        }

        /// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
        protected virtual void FlushAllPages()
        {
            for (var pIter = _cache.GetEnumerator(); pIter.MoveNext();)
            {
                var p = ((Page) pIter.Current);
                FlushPage(p);
            }
        }

        /// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
        private void FlushPage(Page page)
        {
            if (!page._dirty)
            {
                return;
            }
            WritePageToDisk(page);
        }

        /// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
        private void LoadPage(Page page, long pos)
        {
            var startAddress = pos - pos%_pageSize;
            page._startAddress = startAddress;
            var count = _bin.Read(page._startAddress, page._buffer, page._bufferSize);
            if (count > 0)
            {
                page._endAddress = startAddress + count;
            }
            else
            {
                page._endAddress = startAddress;
            }
        }

        /// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
        private void WritePageToDisk(Page page)
        {
            base.Write(page._startAddress, page._buffer, page.Size());
            page._dirty = false;
        }

        private sealed class _IProcedure4_22 : IProcedure4
        {
            private readonly CachingBin _enclosing;

            public _IProcedure4_22(CachingBin _enclosing)
            {
                this._enclosing = _enclosing;
            }

            public void Apply(object discardedPage)
            {
                _enclosing.FlushPage(((Page) discardedPage));
                _enclosing._pagePool.ReturnObject(((Page) discardedPage));
            }
        }

        private sealed class _IRunnable_119 : IRunnable
        {
            private readonly CachingBin _enclosing;
            private readonly IRunnable runnable;

            public _IRunnable_119(CachingBin _enclosing, IRunnable runnable)
            {
                this._enclosing = _enclosing;
                this.runnable = runnable;
            }

            public void Run()
            {
                runnable.Run();
                _enclosing.FlushAllPages();
            }
        }

        private sealed class _IFunction4_138 : IFunction4
        {
            private readonly CachingBin _enclosing;

            public _IFunction4_138(CachingBin _enclosing)
            {
                this._enclosing = _enclosing;
            }

            public object Apply(object pageAddress)
            {
                // in case that page is not found in the cache
                var newPage = ((Page) _enclosing._pagePool.BorrowObject
                    ());
                _enclosing.LoadPage(newPage, ((long) pageAddress));
                return newPage;
            }
        }

        private sealed class _IFunction4_147 : IFunction4
        {
            private readonly CachingBin _enclosing;

            public _IFunction4_147(CachingBin _enclosing)
            {
                this._enclosing = _enclosing;
            }

            public object Apply(object pageAddress)
            {
                // in case that page is not found in the cache
                var newPage = ((Page) _enclosing._pagePool.BorrowObject
                    ());
                _enclosing.ResetPageAddress(newPage, ((long) pageAddress));
                return newPage;
            }
        }

        private class Page
        {
            public readonly byte[] _buffer;
            public readonly int _bufferSize;
            public bool _dirty;
            public long _endAddress;
            public long _startAddress = -1;
            private byte[] zeroBytes;

            public Page(int size)
            {
                _bufferSize = size;
                _buffer = new byte[_bufferSize];
            }

            internal virtual void EnsureEndAddress(long fileLength)
            {
                var bufferEndAddress = _startAddress + _bufferSize;
                if (_endAddress < bufferEndAddress && fileLength > _endAddress)
                {
                    var newEndAddress = Math.Min(fileLength, bufferEndAddress);
                    if (zeroBytes == null)
                    {
                        zeroBytes = new byte[_bufferSize];
                    }
                    Array.Copy(zeroBytes, 0, _buffer, (int) (_endAddress - _startAddress), (int
                        ) (newEndAddress - _endAddress));
                    _endAddress = newEndAddress;
                }
            }

            internal virtual int Size()
            {
                return (int) (_endAddress - _startAddress);
            }

            internal virtual int Read(byte[] @out, int outOffset, long startAddress, int length
                )
            {
                var bufferOffset = (int) (startAddress - _startAddress);
                var pageAvailbeDataSize = (int) (_endAddress - startAddress);
                var readBytes = Math.Min(pageAvailbeDataSize, length);
                if (readBytes <= 0)
                {
                    // meaning reach EOF
                    return -1;
                }
                Array.Copy(_buffer, bufferOffset, @out, outOffset, readBytes);
                return readBytes;
            }

            internal virtual int Write(byte[] data, int dataOffset, long startAddress, int length
                )
            {
                var bufferOffset = (int) (startAddress - _startAddress);
                var pageAvailabeBufferSize = _bufferSize - bufferOffset;
                var writtenBytes = Math.Min(pageAvailabeBufferSize, length);
                Array.Copy(data, dataOffset, _buffer, bufferOffset, writtenBytes);
                var endAddress = startAddress + writtenBytes;
                if (endAddress > _endAddress)
                {
                    _endAddress = endAddress;
                }
                _dirty = true;
                return writtenBytes;
            }
        }
    }
}