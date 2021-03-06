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

using System.Collections;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Internal.Caching
{
    /// <exclude></exclude>
    internal class LRUCache : IPurgeableCache4
    {
        private readonly CircularBuffer4 _lru;
        private readonly int _maxSize;
        private readonly IDictionary _slots;

        internal LRUCache(int size)
        {
            _maxSize = size;
            _slots = new Hashtable(size);
            _lru = new CircularBuffer4(size);
        }

        public virtual object Produce(object key, IFunction4 producer, IProcedure4 finalizer
            )
        {
            var value = _slots[key];
            if (value == null)
            {
                var newValue = producer.Apply(key);
                if (newValue == null)
                {
                    return null;
                }
                if (_slots.Count >= _maxSize)
                {
                    var discarded = Sharpen.Collections.Remove(_slots, _lru.RemoveLast());
                    if (null != finalizer)
                    {
                        finalizer.Apply(discarded);
                    }
                }
                _slots[key] = newValue;
                _lru.AddFirst(key);
                return newValue;
            }
            _lru.Remove(key);
            // O(N) 
            _lru.AddFirst(key);
            return value;
        }

        public virtual IEnumerator GetEnumerator()
        {
            return _slots.Values.GetEnumerator();
        }

        public virtual object Purge(object key)
        {
            var removed = Sharpen.Collections.Remove(_slots, key);
            if (removed == null)
            {
                return null;
            }
            _lru.Remove(key);
            return removed;
        }
    }
}