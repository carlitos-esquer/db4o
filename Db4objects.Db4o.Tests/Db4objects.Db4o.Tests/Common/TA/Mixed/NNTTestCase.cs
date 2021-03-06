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

namespace Db4objects.Db4o.Tests.Common.TA.Mixed
{
    /// <exclude></exclude>
    public class NNTTestCase : ItemTestCaseBase
    {
        public static void Main(string[] args)
        {
            new NNTTestCase().RunAll();
        }

        /// <exception cref="System.Exception"></exception>
        protected override object CreateItem()
        {
            return new NNTItem(42);
        }

        /// <exception cref="System.Exception"></exception>
        protected override void AssertRetrievedItem(object obj)
        {
            var item = (NNTItem) obj;
            Assert.IsNotNull(item.ntItem);
            Assert.IsNotNull(item.ntItem.tItem);
            Assert.AreEqual(0, item.ntItem.tItem.value);
        }

        /// <exception cref="System.Exception"></exception>
        protected override void AssertItemValue(object obj)
        {
            var item = (NNTItem) obj;
            Assert.AreEqual(42, item.ntItem.tItem.Value());
        }

        /// <exception cref="System.Exception"></exception>
        public virtual void TestDeactivateDepth()
        {
            var item = (NNTItem) RetrieveOnlyInstance();
            var ntItem = item.ntItem;
            var tItem = ntItem.tItem;
            tItem.Value();
            // item.ntItem.tItem.value
            Assert.IsNotNull(ntItem.tItem);
            Db().Deactivate(item, 2);
            // FIXME: failure 
            //		Assert.isNull(ntItem.tItem);
            Db().Activate(item, 42);
            Db().Deactivate(item, 3);
            Assert.IsNull(ntItem.tItem);
        }
    }
}