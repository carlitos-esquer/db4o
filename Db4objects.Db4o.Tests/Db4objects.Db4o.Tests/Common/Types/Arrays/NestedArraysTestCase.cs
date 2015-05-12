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

namespace Db4objects.Db4o.Tests.Common.Types.Arrays
{
    public class NestedArraysTestCase : AbstractDb4oTestCase
    {
        private const int Depth = 5;
        private const int Elements = 3;

        protected override void Store()
        {
            var obj = new object[Elements];
            Fill(obj, Depth);
            var arr = new object[Elements];
            Fill(arr, Depth);
            Db().Store(new Data(obj, arr));
        }

        private void Fill(object[] arr, int depth)
        {
            if (depth <= 0)
            {
                arr[0] = "somestring";
                arr[1] = 10;
                return;
            }
            depth--;
            for (var i = 0; i < Elements; i++)
            {
                arr[i] = new object[Elements];
                Fill((object[]) arr[i], depth);
            }
        }

        public virtual void TestOne()
        {
            var data = (Data
                ) RetrieveOnlyInstance(typeof (Data));
            Db().Activate(data, int.MaxValue);
            Check((object[]) data._obj, Depth);
            Check(data._arr, Depth);
        }

        private void Check(object[] arr, int depth)
        {
            if (depth <= 0)
            {
                Assert.AreEqual("somestring", arr[0]);
                Assert.AreEqual(10, arr[1]);
                return;
            }
            depth--;
            for (var i = 0; i < Elements; i++)
            {
                Check((object[]) arr[i], depth);
            }
        }

        public class Data
        {
            public object[] _arr;
            public object _obj;

            public Data(object obj, object[] arr)
            {
                _obj = obj;
                _arr = arr;
            }
        }
    }
}