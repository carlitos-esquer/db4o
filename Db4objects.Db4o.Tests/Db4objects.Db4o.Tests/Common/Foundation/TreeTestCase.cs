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
using Db4objects.Db4o.Internal;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.Common.Foundation
{
    public class TreeTestCase : ITestCase
    {
        private const int Count = 21;

        public virtual void TestTraversalWithStartingPointEmpty()
        {
            Tree.Traverse(null, new TreeInt(5), new _ICancellableVisitor4_18());
        }

        public virtual void TestCancelledTraversalWithStartingPointNotInTheTree()
        {
            var visits = new IntByRef();
            var tree = CreateTree();
            Tree.Traverse(tree, new TreeInt(5), new _ICancellableVisitor4_28(visits));
            Assert.AreEqual(1, visits.value);
        }

        public virtual void TestCancelledTraversalWithStartingPointInTheTree()
        {
            var visits = new IntByRef();
            var tree = CreateTree();
            Tree.Traverse(tree, new TreeInt(6), new _ICancellableVisitor4_41(visits));
            Assert.AreEqual(1, visits.value);
        }

        public virtual void TestUnCancelledTraversalWithStartingPointNotInTheTree()
        {
            IList actual = new ArrayList();
            var tree = CreateTree();
            Tree.Traverse(tree, new TreeInt(5), new _ICancellableVisitor4_54(actual));
            IteratorAssert.AreEqual(CreateList(6).GetEnumerator(), actual.GetEnumerator());
        }

        public virtual void TestUnCancelledTraversalWithStartingPointInTheTree()
        {
            IList actual = new ArrayList();
            var tree = CreateTree();
            Tree.Traverse(tree, new TreeInt(6), new _ICancellableVisitor4_66(actual));
            IteratorAssert.AreEqual(CreateList(6).GetEnumerator(), actual.GetEnumerator());
        }

        private IList CreateList(int start)
        {
            IList expected = new ArrayList();
            var expectedTree = CreateTree(start);
            Tree.Traverse(expectedTree, new _IVisitor4_79(expected));
            return expected;
        }

        private TreeInt CreateTree()
        {
            return CreateTree(0);
        }

        private TreeInt CreateTree(int start)
        {
            TreeInt tree = null;
            for (var i = start; i < Count; i += 3)
            {
                tree = ((TreeInt) Tree.Add(tree, new TreeInt(i)));
            }
            return tree;
        }

        private sealed class _ICancellableVisitor4_18 : ICancellableVisitor4
        {
            public bool Visit(object node)
            {
                return true;
            }
        }

        private sealed class _ICancellableVisitor4_28 : ICancellableVisitor4
        {
            private readonly IntByRef visits;

            public _ICancellableVisitor4_28(IntByRef visits)
            {
                this.visits = visits;
            }

            public bool Visit(object node)
            {
                visits.value++;
                Assert.AreEqual(new TreeInt(6), ((TreeInt) node));
                return false;
            }
        }

        private sealed class _ICancellableVisitor4_41 : ICancellableVisitor4
        {
            private readonly IntByRef visits;

            public _ICancellableVisitor4_41(IntByRef visits)
            {
                this.visits = visits;
            }

            public bool Visit(object node)
            {
                visits.value++;
                Assert.AreEqual(new TreeInt(6), ((TreeInt) node));
                return false;
            }
        }

        private sealed class _ICancellableVisitor4_54 : ICancellableVisitor4
        {
            private readonly IList actual;

            public _ICancellableVisitor4_54(IList actual)
            {
                this.actual = actual;
            }

            public bool Visit(object node)
            {
                actual.Add(((TreeInt) node));
                return true;
            }
        }

        private sealed class _ICancellableVisitor4_66 : ICancellableVisitor4
        {
            private readonly IList actual;

            public _ICancellableVisitor4_66(IList actual)
            {
                this.actual = actual;
            }

            public bool Visit(object node)
            {
                actual.Add(((TreeInt) node));
                return true;
            }
        }

        private sealed class _IVisitor4_79 : IVisitor4
        {
            private readonly IList expected;

            public _IVisitor4_79(IList expected)
            {
                this.expected = expected;
            }

            public void Visit(object node)
            {
                expected.Add(((TreeInt) node));
            }
        }
    }
}