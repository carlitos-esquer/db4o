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

using Db4objects.Db4o.CS.Internal.Messages;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.References;

namespace Db4objects.Db4o.CS.Internal
{
    public sealed class ClientTransaction : Transaction
    {
        private readonly ClientObjectContainer _client;
        protected Tree _objectRefrencesToGC;

        internal ClientTransaction(ClientObjectContainer container, Transaction parentTransaction
            , IReferenceSystem referenceSystem) : base(container, parentTransaction, referenceSystem
                )
        {
            _client = container;
        }

        public override void Commit()
        {
            PreCommit();
            if (IsSystemTransaction())
            {
                _client.Write(Msg.CommitSystemtrans);
            }
            else
            {
                _client.Write(Msg.Commit.GetWriter(this));
                _client.ExpectedResponse(Msg.Ok);
            }
        }

        public void PreCommit()
        {
            CommitTransactionListeners();
            ClearAll();
        }

        protected override void Clear()
        {
            RemoveObjectReferences();
        }

        private void RemoveObjectReferences()
        {
            if (_objectRefrencesToGC != null)
            {
                _objectRefrencesToGC.Traverse(new _IVisitor4_43(this));
            }
            _objectRefrencesToGC = null;
        }

        public override bool Delete(ObjectReference @ref, int id, int cascade)
        {
            if (!base.Delete(@ref, id, cascade))
            {
                return false;
            }
            var msg = Msg.TaDelete.GetWriterForInts(this, new[] {id, cascade});
            _client.WriteBatchedMessage(msg);
            return true;
        }

        public override void ProcessDeletes()
        {
            IVisitor4 deleteVisitor = new _IVisitor4_63(this);
            TraverseDelete(deleteVisitor);
            _client.WriteBatchedMessage(Msg.ProcessDeletes);
        }

        public override void Rollback()
        {
            lock (Container().Lock())
            {
                _objectRefrencesToGC = null;
                RollBackTransactionListeners();
                ClearAll();
            }
        }

        public override void WriteUpdateAdjustIndexes(int id, ClassMetadata classMetadata
            , ArrayType arrayType)
        {
        }

        // do nothing
        public override ITransactionalIdSystem IdSystem()
        {
            return null;
        }

        public override long VersionForId(int id)
        {
            var msg = Msg.VersionForId.GetWriterForInt(SystemTransaction(), id);
            _client.Write(msg);
            return _client.ExpectedBufferResponse(Msg.VersionForId).ReadLong();
        }

        public override long GenerateTransactionTimestamp(long forcedTimeStamp)
        {
            _client.WriteMsg(Msg.GenerateTransactionTimestamp.GetWriterForLong(this, forcedTimeStamp
                ), true);
            return _client.ExpectedBufferResponse(Msg.GenerateTransactionTimestamp).ReadLong(
                );
        }

        public override void UseDefaultTransactionTimestamp()
        {
            _client.WriteMsg(Msg.UseDefaultTransactionTimestamp, true);
        }

        private sealed class _IVisitor4_43 : IVisitor4
        {
            private readonly ClientTransaction _enclosing;

            public _IVisitor4_43(ClientTransaction _enclosing)
            {
                this._enclosing = _enclosing;
            }

            public void Visit(object a_object)
            {
                var yo = (ObjectReference) ((TreeIntObject) a_object)._object;
                _enclosing.RemoveReference(yo);
            }
        }

        private sealed class _IVisitor4_63 : IVisitor4
        {
            private readonly ClientTransaction _enclosing;

            public _IVisitor4_63(ClientTransaction _enclosing)
            {
                this._enclosing = _enclosing;
            }

            public void Visit(object a_object)
            {
                var info = (DeleteInfo) a_object;
                if (info._reference != null)
                {
                    _enclosing._objectRefrencesToGC = Tree.Add(_enclosing._objectRefrencesToGC
                        , new TreeIntObject(info._key, info._reference));
                }
            }
        }
    }
}