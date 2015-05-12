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

using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.References;

namespace Db4objects.Db4o.Internal
{
    /// <summary>TODO: Check if all time-consuming stuff is overridden!</summary>
    internal class TransactionObjectCarrier : LocalTransaction
    {
        private readonly ITransactionalIdSystem _idSystem;

        internal TransactionObjectCarrier(ObjectContainerBase container, Transaction parentTransaction
            , ITransactionalIdSystem idSystem, IReferenceSystem referenceSystem) : base(container
                , parentTransaction, idSystem, referenceSystem)
        {
            _idSystem = idSystem;
        }

        public override void Commit()
        {
        }

        // do nothing
        internal override bool SupportsVirtualFields()
        {
            return false;
        }

        public override long VersionForId(int id)
        {
            return 0;
        }

        public override CommitTimestampSupport CommitTimestampSupport
            ()
        {
            return null;
        }
    }
}