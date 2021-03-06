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

using Db4objects.Db4o.TA;

namespace Db4objects.Db4o.Internal.Activation
{
    public class TPFixedUpdateDepth : FixedUpdateDepth
    {
        private readonly IModifiedObjectQuery _query;

        public TPFixedUpdateDepth(int depth, IModifiedObjectQuery query) : base(depth)
        {
            _query = query;
        }

        public override bool CanSkip(ObjectReference @ref)
        {
            var clazz = @ref.ClassMetadata();
            return clazz.Reflector().ForClass(typeof (IActivatable)).IsAssignableFrom(clazz.ClassReflector
                ()) && !_query.IsModified(@ref.GetObject());
        }

        protected override FixedUpdateDepth ForDepth(int depth)
        {
            return new TPFixedUpdateDepth(depth, _query);
        }
    }
}