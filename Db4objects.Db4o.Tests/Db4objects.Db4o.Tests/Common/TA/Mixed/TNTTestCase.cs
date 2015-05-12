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
    public class TNTTestCase : ItemTestCaseBase
    {
        public static void Main(string[] args)
        {
            new TNTTestCase().RunAll();
        }

        /// <exception cref="System.Exception"></exception>
        protected override object CreateItem()
        {
            return new TNTItem(42);
        }

        /// <exception cref="System.Exception"></exception>
        protected override void AssertRetrievedItem(object obj)
        {
            var item = (TNTItem) obj;
            Assert.IsNull(item.ntItem);
        }

        /// <exception cref="System.Exception"></exception>
        protected override void AssertItemValue(object obj)
        {
            var item = (TNTItem) obj;
            var ntItem = item.Value();
            Assert.IsNotNull(ntItem);
            Assert.IsNotNull(ntItem.tItem);
            Assert.AreEqual(0, ntItem.tItem.value);
            Assert.AreEqual(42, ntItem.tItem.Value());
        }
    }
}