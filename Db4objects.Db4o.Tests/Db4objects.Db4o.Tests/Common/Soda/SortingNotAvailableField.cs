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

using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.Common.Soda
{
    public class SortingNotAvailableField : AbstractDb4oTestCase
    {
        public static void Main(string[] args)
        {
            new SortingNotAvailableField().RunSolo();
        }

        /// <exception cref="System.Exception"></exception>
        protected override void Store()
        {
            base.Store();
            Db().Store(new OrderedItem());
            Db().Store(new OrderedItem());
        }

        public virtual void TestOrderWithRightFieldName()
        {
            var query = Db().Query();
            query.Constrain(typeof (OrderedItem));
            query.Descend("myOrder").OrderAscending();
            var result = query.Execute();
            Assert.AreEqual(2, result.Count);
        }

        public virtual void TestOrderWithWrongFieldName()
        {
            var query = Db().Query();
            query.Constrain(typeof (OrderedItem));
            query.Descend("myorder").OrderAscending();
            var result = query.Execute();
            Assert.AreEqual(2, result.Count);
        }

        public class OrderedItem
        {
            private int myOrder = 42;
        }
    }
}