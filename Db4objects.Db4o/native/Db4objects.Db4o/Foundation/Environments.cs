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

namespace Db4objects.Db4o.Foundation
{
    public partial class Environments
    {
        public static string DefaultImplementationFor(Type type)
        {
            var implName = ("." + type.Name.Substring(1) + "Impl");
            if (type.Namespace.IndexOf(".Internal.") > 0)
                return type.Namespace + implName + ", " + AssemblyNameFor(type);

            var lastDot = type.Namespace.LastIndexOf('.');
            var typeName = type.Namespace.Substring(0, lastDot) + ".Internal." + type.Namespace.Substring(lastDot + 1) +
                           implName;
            return typeName + ", " + AssemblyNameFor(type);
        }

        private static string AssemblyNameFor(Type type)
        {
#if SILVERLIGHT
			string fullyQualifiedTypeName = type.AssemblyQualifiedName;
			int assemblyNameSeparator = fullyQualifiedTypeName.IndexOf(',');
			return fullyQualifiedTypeName.Substring(assemblyNameSeparator + 1);
#else
            return type.Assembly.GetName().Name;
#endif
        }
    }
}