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

using System;
using Db4objects.Db4o.Ext;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
    internal class UIntHandlerUpdateTestCase : LenientHandlerUpdateTestCaseBase
    {
        private static readonly uint[] data =
        {
            uint.MinValue,
            uint.MinValue + 1,
            5,
            uint.MaxValue - 1,
            uint.MaxValue
        };

        protected override void AssertArrays(IExtObjectContainer objectContainer, object obj)
        {
            var itemArrays = (ItemArrays) obj;
            for (var i = 0; i < data.Length; i++)
            {
                AssertAreEqual(data[i], itemArrays._typedPrimitiveArray[i]);
                AssertAreEqual(data[i], ((uint[]) itemArrays._primitiveArrayInObject)[i]);
                if (NullableSupported())
                {
                    AssertAreEqual(data[i], (uint) itemArrays._nullableTypedPrimitiveArray[i]);
                }
            }
            AssertAreEqual(0, itemArrays._typedPrimitiveArray[data.Length]);
            AssertAreEqual(0, ((uint[]) itemArrays._primitiveArrayInObject)[data.Length]);
        }

        protected override void AssertValues(IExtObjectContainer objectContainer, object[] values)
        {
            for (var i = 0; i < data.Length; i++)
            {
                var item = (Item) values[i];
                AssertAreEqual(data[i], item._typedPrimitive);
                AssertAreEqual(data[i], (uint) item._untyped);
                AssertAreEqual(data[i], (uint) item._nullablePrimitive);
            }
            var nullItem = (Item) values[data.Length];
            AssertAreEqual(0, nullItem._typedPrimitive);
            Assert.IsNull(nullItem._untyped);
            Assert.IsNull(nullItem._nullablePrimitive);
        }

        private void AssertAreEqual(uint expected, uint actual)
        {
            Assert.AreEqual(expected, actual);
        }

        protected override object CreateArrays()
        {
            var itemArrays = new ItemArrays();
            itemArrays._typedPrimitiveArray = new uint[data.Length + 1];
            Array.Copy(data, 0, itemArrays._typedPrimitiveArray, 0, data.Length);

            var uintArray = new uint[data.Length + 1];
            Array.Copy(data, 0, uintArray, 0, data.Length);
            itemArrays._primitiveArrayInObject = uintArray;
            itemArrays._nullableTypedPrimitiveArray = new uint?[data.Length + 1];
            for (var i = 0; i < data.Length; i++)
            {
                itemArrays._nullableTypedPrimitiveArray[i] = data[i];
            }
            return itemArrays;
        }

        protected override object[] CreateValues()
        {
            var values = new Item[data.Length + 1];
            for (var i = 0; i < data.Length; i++)
            {
                var item = new Item();
                item._typedPrimitive = data[i];
                item._untyped = data[i];
                item._nullablePrimitive = data[i];
                values[i] = item;
            }

            values[data.Length] = new Item();
            return values;
        }

        protected override string TypeName()
        {
            return "uint";
        }

        public class Item
        {
            public uint? _nullablePrimitive;
            public uint _typedPrimitive;
            public object _untyped;
        }

        public class ItemArrays
        {
            public uint?[] _nullableTypedPrimitiveArray;
            public object _primitiveArrayInObject;
            public uint[] _typedPrimitiveArray;
        }
    }
}