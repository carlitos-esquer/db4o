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

using Db4objects.Db4o.Tests.Common.Soda.Util;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.Common.Soda.Classes.Simple
{
    public class STIntegerTestCase : SodaBaseTestCase
    {
        public int i_int;

        public STIntegerTestCase()
        {
        }

        private STIntegerTestCase(int a_int)
        {
            i_int = a_int;
        }

        public override object[] CreateData()
        {
            return new object[]
            {
                new STIntegerTestCase
                    (0),
                new STIntegerTestCase(1),
                new STIntegerTestCase(99), new
                    STIntegerTestCase(909)
            };
        }

        public virtual void TestEquals()
        {
            var q = NewQuery();
            q.Constrain(new STIntegerTestCase
                (0));
            // Primitive default values are ignored, so we need an 
            // additional constraint:
            q.Descend("i_int").Constrain(0);
            SodaTestUtil.ExpectOne(q, _array[0]);
        }

        public virtual void TestNotEquals()
        {
            var q = NewQuery();
            q.Constrain(_array[0]);
            q.Descend("i_int").Constrain(0).Not();
            Expect(q, new[] {1, 2, 3});
        }

        public virtual void TestGreater()
        {
            var q = NewQuery();
            q.Constrain(new STIntegerTestCase
                (9));
            q.Descend("i_int").Constraints().Greater();
            Expect(q, new[] {2, 3});
        }

        public virtual void TestSmaller()
        {
            var q = NewQuery();
            q.Constrain(new STIntegerTestCase
                (1));
            q.Descend("i_int").Constraints().Smaller();
            SodaTestUtil.ExpectOne(q, _array[0]);
        }

        public virtual void TestContains()
        {
            var q = NewQuery();
            q.Constrain(new STIntegerTestCase
                (9));
            q.Descend("i_int").Constraints().Contains();
            Expect(q, new[] {2, 3});
        }

        public virtual void TestNotContains()
        {
            var q = NewQuery();
            q.Constrain(new STIntegerTestCase
                (0));
            q.Descend("i_int").Constrain(0).Contains().Not();
            Expect(q, new[] {1, 2});
        }

        public virtual void TestLike()
        {
            var q = NewQuery();
            q.Constrain(new STIntegerTestCase
                (90));
            q.Descend("i_int").Constraints().Like();
            SodaTestUtil.ExpectOne(q, new STIntegerTestCase
                (909));
            q = NewQuery();
            q.Constrain(new STIntegerTestCase
                (10));
            q.Descend("i_int").Constraints().Like();
            Expect(q, new int[] {});
        }

        public virtual void TestNotLike()
        {
            var q = NewQuery();
            q.Constrain(new STIntegerTestCase
                (1));
            q.Descend("i_int").Constraints().Like().Not();
            Expect(q, new[] {0, 2, 3});
        }

        public virtual void TestIdentity()
        {
            var q = NewQuery();
            q.Constrain(new STIntegerTestCase
                (1));
            var set = q.Execute();
            var identityConstraint
                = (STIntegerTestCase) set.Next(
                    );
            identityConstraint.i_int = 9999;
            q = NewQuery();
            q.Constrain(identityConstraint).Identity();
            identityConstraint.i_int = 1;
            SodaTestUtil.ExpectOne(q, _array[1]);
        }

        public virtual void TestNotIdentity()
        {
            var q = NewQuery();
            q.Constrain(new STIntegerTestCase
                (1));
            var set = q.Execute();
            var identityConstraint
                = (STIntegerTestCase) set.Next(
                    );
            identityConstraint.i_int = 9080;
            q = NewQuery();
            q.Constrain(identityConstraint).Identity().Not();
            identityConstraint.i_int = 1;
            Expect(q, new[] {0, 2, 3});
        }

        public virtual void TestConstraints()
        {
            var q = NewQuery();
            q.Constrain(new STIntegerTestCase
                (1));
            q.Constrain(new STIntegerTestCase
                (0));
            var cs = q.Constraints();
            var csa = cs.ToArray();
            if (csa.Length != 2)
            {
                Assert.Fail("Constraints not returned");
            }
        }
    }
}