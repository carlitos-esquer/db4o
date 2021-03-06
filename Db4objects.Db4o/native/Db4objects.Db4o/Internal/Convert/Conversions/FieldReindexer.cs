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

using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Reflect.Net;

namespace Db4objects.Db4o.Internal.Convert.Conversions
{
    internal class FieldReindexer<T> : IProcedure4 where T : struct
    {
        public void Apply(object field)
        {
            if (!((FieldMetadata) field).HasIndex())
            {
                return;
            }
            ReindexDateTimeField(((FieldMetadata) field));
        }

        private static void ReindexDateTimeField(IStoredField field)
        {
            var claxx = field.GetStoredType();
            if (claxx == null)
            {
                return;
            }

            var t = NetReflector.ToNative(claxx);
            if (t == typeof (T) || t == typeof (T?))
            {
                field.DropIndex();
                field.CreateIndex();
            }
        }
    }
}