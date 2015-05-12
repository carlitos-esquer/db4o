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

using Db4objects.Db4o.Internal.Btree;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.Common.Btree
{
    /// <exclude></exclude>
    public class BTreePointerTestCase : BTreeTestCaseBase
    {
        private readonly int[] keys =
        {
            -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 7, 9
        };

        public static void Main(string[] args)
        {
            new BTreePointerTestCase().RunSolo();
        }

        /// <exception cref="System.Exception"></exception>
        protected override void Db4oSetupAfterStore()
        {
            base.Db4oSetupAfterStore();
            Add(keys);
            Commit();
        }

        public virtual void TestLastPointer()
        {
            var pointer = _btree.LastPointer(Trans());
            AssertPointerKey(9, pointer);
        }

        public virtual void TestPrevious()
        {
            var pointer = GetPointerForKey(3);
            var previousPointer = pointer.Previous();
            AssertPointerKey(2, previousPointer);
        }

        public virtual void TestNextOperatesInReadMode()
        {
            var pointer = _btree.FirstPointer(Trans());
            AssertReadModePointerIteration(keys, pointer);
        }

        public virtual void TestSearchOperatesInReadMode()
        {
            var pointer = GetPointerForKey(3);
            AssertReadModePointerIteration(new[] {3, 4, 7, 9}, pointer);
        }

        private BTreePointer GetPointerForKey(int key)
        {
            var range = Search(key);
            var pointers = range.Pointers();
            Assert.IsTrue(pointers.MoveNext());
            var pointer = (BTreePointer) pointers.Current;
            return pointer;
        }

        private void AssertReadModePointerIteration(int[] expectedKeys, BTreePointer pointer
            )
        {
            var expected = IntArrays4.ToObjectArray(expectedKeys);
            for (var i = 0; i < expected.Length; i++)
            {
                Assert.IsNotNull(pointer, "Expected '" + expected[i] + "'");
                Assert.AreNotSame(_btree.Root(), pointer.Node());
                AssertInReadModeOrCached(pointer.Node());
                Assert.AreEqual(expected[i], pointer.Key());
                AssertInReadModeOrCached(pointer.Node());
                pointer = pointer.Next();
            }
        }

        private void AssertInReadModeOrCached(BTreeNode node)
        {
            if (IsCached(node))
            {
                return;
            }
            Assert.IsFalse(node.CanWrite());
        }

        private bool IsCached(BTreeNode node)
        {
            for (var entryIter = _btree.NodeCache().GetEnumerator();
                entryIter.MoveNext
                    ();)
            {
                var entry = ((BTreeNodeCacheEntry) entryIter.Current);
                if (node == entry._node)
                {
                    return true;
                }
            }
            return false;
        }

        protected override BTree NewBTree()
        {
            return NewBTreeWithNoNodeCaching();
        }

        private BTree NewBTreeWithNoNodeCaching()
        {
            return BTreeAssert.CreateIntKeyBTree(Container(), 0, BtreeNodeSize);
        }
    }
}