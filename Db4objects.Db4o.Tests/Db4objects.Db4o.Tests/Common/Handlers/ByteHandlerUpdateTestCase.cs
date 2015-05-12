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
using Db4objects.Db4o.Tests.Util;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
    public class ByteHandlerUpdateTestCase : HandlerUpdateTestCaseBase
    {
        public static readonly byte[] data =
        {
            byte.MinValue, byte.MinValue +
                           1,
            unchecked(0xFB), unchecked(0xFF), 0, 1, 5, byte.MaxValue
                                                       - 1,
            byte.MaxValue
        };

        public static void Main(string[] args)
        {
            new ConsoleTestRunner(typeof (ByteHandlerUpdateTestCase)).Run();
        }

        protected override void AssertArrays(IExtObjectContainer objectContainer, object
            obj)
        {
            var itemArrays = (ItemArrays
                ) obj;
            AssertPrimitiveArray(itemArrays._typedPrimitiveArray);
            if (Db4oHeaderVersion() == VersionServices.Header3040)
            {
            }
            else
            {
                // Bug in the oldest format: It accidentally byte[] arrays to Byte[]
                // arrays.
                if (Db4oHandlerVersion() == 1 && Db4oHeaderVersion() == 100 && itemArrays._primitiveArrayInObject
                    == null)
                {
                }
                else
                {
                    // do nothing
                    // We started treating byte[] in untyped variables differently
                    // but we forgot to update the handler version.
                    // Concerns only 6.3.500: Updates are not possible.
                    AssertPrimitiveArray((byte[]) itemArrays._primitiveArrayInObject);
                }
            }
        }

        private void AssertPrimitiveArray(byte[] primitiveArray)
        {
            for (var i = 0; i < data.Length; i++)
            {
                AssertAreEqual(data[i], primitiveArray[i]);
            }
        }

        // FIXME: Arrays should also get a null Bitmap to fix.
        // Assert.isNull(wrapperArray[wrapperArray.length - 1]);
        protected override void AssertValues(IExtObjectContainer objectContainer, object[]
            values)
        {
            for (var i = 0; i < data.Length; i++)
            {
                var item = (Item) values[i];
                AssertAreEqual(data[i], item._typedPrimitive);
                AssertAreEqual(data[i], item._typedWrapper);
                AssertAreEqual(data[i], item._untyped);
            }
            var nullItem = (Item) values[
                values.Length - 1];
            AssertAreEqual(0, nullItem._typedPrimitive);
            Assert.IsNull(nullItem._untyped);
        }

        private void AssertAreEqual(byte expected, byte actual)
        {
            Assert.AreEqual(expected, actual);
        }

        private void AssertAreEqual(object expected, object actual)
        {
            Assert.AreEqual(expected, actual);
        }

        protected override object CreateArrays()
        {
            var itemArrays = new ItemArrays
                ();
            itemArrays._typedPrimitiveArray = new byte[data.Length];
            Array.Copy(data, 0, itemArrays._typedPrimitiveArray, 0, data.Length);
            var dataWrapper = new byte[data.Length];
            for (var i = 0; i < data.Length; i++)
            {
                dataWrapper[i] = data[i];
            }
            itemArrays._typedWrapperArray = new byte[data.Length + 1];
            Array.Copy(dataWrapper, 0, itemArrays._typedWrapperArray, 0, dataWrapper.Length
                );
            var primitiveArray = new byte[data.Length];
            Array.Copy(data, 0, primitiveArray, 0, data.Length);
            if (PrimitiveArrayInUntypedVariableSupported())
            {
                itemArrays._primitiveArrayInObject = primitiveArray;
            }
            var wrapperArray = new byte[data.Length + 1];
            Array.Copy(dataWrapper, 0, wrapperArray, 0, dataWrapper.Length);
            if (PrimitiveArrayInUntypedVariableSupported() || !Deploy.csharp)
            {
                itemArrays._wrapperArrayInObject = wrapperArray;
            }
            return itemArrays;
        }

        private bool PrimitiveArrayInUntypedVariableSupported()
        {
            // We can't convert primitive arrays in object variables in 6.3
            // Keeping the member to null value helps to identify the exact version.
            return !(Db4oMajorVersion() == 6 && Db4oMinorVersion() == 3);
        }

        protected override object[] CreateValues()
        {
            var values = new Item[data
                .Length + 1];
            for (var i = 0; i < data.Length; i++)
            {
                var item = new Item();
                item._typedPrimitive = data[i];
                item._typedWrapper = data[i];
                item._untyped = data[i];
                values[i] = item;
            }
            values[values.Length - 1] = new Item();
            return values;
        }

        // Bug when reading old format:
        // Null wrappers are converted to 0
        protected override string TypeName()
        {
            return "byte";
        }

        public class Item
        {
            public byte _typedPrimitive;
            public byte _typedWrapper;
            public object _untyped;
        }

        public class ItemArrays
        {
            public object _primitiveArrayInObject;
            public byte[] _typedPrimitiveArray;
            public byte[] _typedWrapperArray;
            public object[] _untypedObjectArray;
            public object _wrapperArrayInObject;
        }
    }
}