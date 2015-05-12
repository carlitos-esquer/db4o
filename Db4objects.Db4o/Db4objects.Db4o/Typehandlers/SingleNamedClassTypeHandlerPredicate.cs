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

using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Typehandlers
{
    /// <summary>allows installing a Typehandler for a single classname.</summary>
    /// <remarks>allows installing a Typehandler for a single classname.</remarks>
    public sealed class SingleNamedClassTypeHandlerPredicate : ITypeHandlerPredicate
    {
        private readonly string _className;

        public SingleNamedClassTypeHandlerPredicate(string className)
        {
            _className = className;
        }

        public bool Match(IReflectClass candidate)
        {
            return candidate.GetName().Equals(_className);
        }
    }
}