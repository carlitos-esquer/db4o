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
using System.Collections;
using System.Collections.Generic;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Internal
{
    /// <exclude></exclude>
    public partial class ObjectContainerSession
    {
        void IDisposable.Dispose()
        {
            Close();
        }

        public IObjectSet Query(Predicate match, IComparer comparer)
        {
            return _server.Query(_transaction, match, new ComparerAdaptor(comparer));
        }

        public IList<Extent> Query<Extent>(Predicate<Extent> match)
        {
            return _server.Query(_transaction, match);
        }

        public IList<Extent> Query<Extent>(Predicate<Extent> match, IComparer<Extent> comparer)
        {
            return _server.Query(_transaction, match, comparer);
        }

        public IList<Extent> Query<Extent>(Predicate<Extent> match, Comparison<Extent> comparison)
        {
            return _server.Query(_transaction, match, comparison);
        }

        public IList<ElementType> Query<ElementType>(Type extent)
        {
            return _server.Query<ElementType>(_transaction, extent, null);
        }

        public IList<Extent> Query<Extent>()
        {
            return Query<Extent>(typeof (Extent));
        }

        public IList<Extent> Query<Extent>(IComparer<Extent> comparer)
        {
            return Query(typeof (Extent), comparer);
        }

        public void WithEnvironment(Action4 action)
        {
            _server.WithEnvironment(new RunnableAction(action));
        }

        public IList<ElementType> Query<ElementType>(Type extent, IComparer<ElementType> comparer)
        {
            return _server.Query(_transaction, extent, comparer);
        }
    }
}