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
using Db4objects.Db4o.Internal.Caching;
using Db4oUnit;
using Db4oUnit.Fixtures;
using Db4oUnit.Mocking;

namespace Db4objects.Db4o.Tests.Common.Caching
{
    public class CacheTestUnit : ITestCase
    {
        public virtual void TestOnDiscard()
        {
            var puppet = new TestPuppet();
            puppet.FillCache();
            var discarded = new ByRef();
            puppet.Produce(42, new _IProcedure4_19(discarded));
            Assert.AreEqual("0", ((string) discarded.value));
        }

        public virtual void TestIterable()
        {
            var puppet = new TestPuppet();
            Iterator4Assert.SameContent(new object[] {}, puppet.Values());
            puppet.Produce(0);
            Iterator4Assert.SameContent(new object[] {"0"}, puppet.Values());
            puppet.FillCache();
            Iterator4Assert.SameContent(new object[]
            {
                "0", "1", "2", "3", "4", "5", "6", "7"
                , "8", "9"
            }, puppet.Values());
        }

        public virtual void TestProduce()
        {
            var obj = new object();
            var cache = ((ICache4) SubjectFixtureProvider.Value());
            var value = cache.Produce(1, new _IFunction4_39(obj), null);
            Assert.AreSame(obj, value);
            Assert.AreSame(obj, cache.Produce(1, null, null));
        }

        public virtual void TestProducerIsNotCalledOnCacheHit()
        {
            var puppet = new TestPuppet();
            puppet.FillCache();
            puppet.Verify(ApplyCalls(10));
            puppet.FillCache();
            puppet.Verify(ApplyCalls(10));
            Assert.AreEqual("10", puppet.Produce(10));
            puppet.Verify(ApplyCalls(11));
            puppet.Reset();
            Assert.AreEqual("0", puppet.Produce(0));
            puppet.Verify(ApplyCalls(1));
            puppet.FillCache(2, 10);
            puppet.Verify(ApplyCalls(1));
        }

        public virtual void TestHotItemsAreEvictedLast()
        {
            var puppet = new TestPuppet();
            if (puppet.cache.GetType().FullName.IndexOf("LRU2QXCache") > 0)
            {
                // LRU2QXCache doesn't meet all the expectations
                return;
            }
            puppet.FillCache();
            puppet.FillCache(0, 2);
            // 0 and 1 are hot now
            puppet.CacheMiss(11);
            // 2 should have been evicted to make room for 11
            puppet.CacheMiss(2);
            puppet.CacheHit(11);
            // 11 is as hot as 0 and 1 now
            puppet.CacheMiss(12);
            puppet.CacheHit(0);
            puppet.CacheHit(1);
            puppet.CacheHit(2);
            puppet.CacheMiss(3);
            puppet.CacheMiss(4);
            puppet.CacheMiss(5);
            puppet.CacheMiss(13);
            puppet.CacheMiss(14);
            puppet.CacheMisses(new[] {6, 7, 8, 9});
            puppet.DumpCache();
            puppet.CacheHits(new[] {6, 7, 8, 9, 13, 14});
            puppet.CacheMiss(15);
            puppet.CacheMiss(11);
            puppet.CacheMiss(0);
            puppet.DumpCache();
        }

        private MethodCall[] ApplyCalls(int count)
        {
            var expectations = new MethodCall[count];
            for (var i = 0; i < count; ++i)
            {
                expectations[i] = ApplyCall(i);
            }
            return expectations;
        }

        private static MethodCall ApplyCall(int arg)
        {
            return new MethodCall("apply", new object[] {arg});
        }

        private sealed class _IProcedure4_19 : IProcedure4
        {
            private readonly ByRef discarded;

            public _IProcedure4_19(ByRef discarded)
            {
                this.discarded = discarded;
            }

            public void Apply(object discardedValue)
            {
                discarded.value = ((string) discardedValue);
            }
        }

        private sealed class _IFunction4_39 : IFunction4
        {
            private readonly object obj;

            public _IFunction4_39(object obj)
            {
                this.obj = obj;
            }

            public object Apply(object key)
            {
                return obj;
            }
        }

        internal class TestPuppet
        {
            public readonly ICache4 cache = ((ICache4) SubjectFixtureProvider.Value());
            internal readonly IFunction4 producer;
            internal readonly MethodCallRecorder producerCalls = new MethodCallRecorder();

            public TestPuppet()
            {
                producer = new _IFunction4_50(this);
            }

            public virtual void FillCache()
            {
                FillCache(0, 10);
            }

            public virtual IEnumerator Values()
            {
                var values = new Collection4();
                for (var sIter = cache.GetEnumerator(); sIter.MoveNext();)
                {
                    var s = ((string) sIter.Current);
                    values.Add(s);
                }
                return values.GetEnumerator();
            }

            public virtual void FillCache(int from, int to)
            {
                for (var i = from; i < to; ++i)
                {
                    Assert.AreEqual(i.ToString(), ((string) cache.Produce(i, producer, null)));
                }
            }

            public virtual void Verify(MethodCall[] calls)
            {
                producerCalls.Verify(calls);
            }

            public virtual string Produce(int key)
            {
                return Produce(key, null);
            }

            public virtual string Produce(int key, IProcedure4 onDiscard)
            {
                return ((string) cache.Produce(key, producer, onDiscard));
            }

            public virtual void Reset()
            {
                producerCalls.Reset();
            }

            public virtual void CacheHit(int key)
            {
                Reset();
                Produce(key);
                Verify(new MethodCall[] {});
            }

            public virtual void CacheMiss(int key)
            {
                Reset();
                Produce(key);
                Verify(new[] {ApplyCall(key)});
                Reset();
            }

            public virtual void DumpCache()
            {
            }

            //			System.out.println(cache);
            public virtual void CacheMisses(int[] keys)
            {
                for (var keyIndex = 0; keyIndex < keys.Length; ++keyIndex)
                {
                    var key = keys[keyIndex];
                    CacheMiss(key);
                }
            }

            public virtual void CacheHits(int[] keys)
            {
                for (var keyIndex = 0; keyIndex < keys.Length; ++keyIndex)
                {
                    var key = keys[keyIndex];
                    CacheHit(key);
                }
            }

            private sealed class _IFunction4_50 : IFunction4
            {
                private readonly TestPuppet _enclosing;

                public _IFunction4_50(TestPuppet _enclosing)
                {
                    this._enclosing = _enclosing;
                }

                public object Apply(object key)
                {
                    _enclosing.producerCalls.Record(new MethodCall("apply", new object[]
                    {
                        ((int
                            ) key)
                    }));
                    return ((int) key).ToString();
                }
            }
        }
    }
}