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

using System.Collections.Generic;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI2.Types
{
    public class ArrayAsGenericListTestCase : AbstractDb4oTestCase
    {
        private static readonly string[] Elements = {"foo", "bar"};

        protected override void Store()
        {
            Store(new Item<string>(Elements));
        }

        public void Test()
        {
            var item = RetrieveOnlyInstance<Item<string>>();
            ArrayAssert.AreEqual(Elements, (string[]) item.List);
        }

        public class Item<T>
        {
            public IList<T> _list;

            public Item(IList<T> list)
            {
                _list = list;
            }

            public IList<T> List
            {
                get { return _list; }
            }
        }
    }
}