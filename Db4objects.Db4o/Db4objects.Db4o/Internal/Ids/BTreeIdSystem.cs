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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Marshall;

namespace Db4objects.Db4o.Internal.Ids
{
    /// <exclude></exclude>
    public class BTreeIdSystem : IStackableIdSystem
    {
        private const int BtreeIdIndex = 0;
        private const int IdGeneratorIndex = 1;
        private const int ChildIdIndex = 2;
        private readonly LocalObjectContainer _container;
        private readonly SequentialIdGenerator _idGenerator;
        private readonly IStackableIdSystem _parentIdSystem;
        private readonly ITransactionalIdSystem _transactionalIdSystem;
        private BTree _bTree;
        private PersistentIntegerArray _persistentState;

        public BTreeIdSystem(LocalObjectContainer container, IStackableIdSystem parentIdSystem
            , int maxValidId)
        {
            _container = container;
            _parentIdSystem = parentIdSystem;
            _transactionalIdSystem = container.NewTransactionalIdSystem(null, new _IClosure4_40
                (parentIdSystem));
            var persistentArrayId = parentIdSystem.ChildId();
            if (persistentArrayId == 0)
            {
                InitializeNew();
            }
            else
            {
                InitializeExisting(persistentArrayId);
            }
            _idGenerator = new SequentialIdGenerator(new _IFunction4_52(this), IdGeneratorValue
                (), _container.Handlers.LowestValidId(), maxValidId);
        }

        public BTreeIdSystem(LocalObjectContainer container, IStackableIdSystem idSystem)
            : this(container, idSystem, int.MaxValue)
        {
        }

        public virtual void Close()
        {
        }

        public virtual Slot CommittedSlot(int id)
        {
            var mapping = (IdSlotMapping) _bTree.Search(Transaction(), new IdSlotMapping
                (id, 0, 0));
            if (mapping == null)
            {
                throw new InvalidIDException(id);
            }
            return mapping.Slot();
        }

        public virtual void CompleteInterruptedTransaction(int transactionId1, int transactionId2
            )
        {
        }

        // do nothing
        public virtual int NewId()
        {
            var id = _idGenerator.NewId();
            _bTree.Add(Transaction(), new IdSlotMapping(id, 0, 0));
            return id;
        }

        public virtual void Commit(IVisitable slotChanges, FreespaceCommitter freespaceCommitter
            )
        {
            _container.FreespaceManager().BeginCommit();
            slotChanges.Accept(new _IVisitor4_129(this));
            // TODO: Maybe we want a BTree that doesn't allow duplicates.
            // Then we could do the following in one step without removing first.
            _bTree.Commit(Transaction());
            IdGeneratorValue(_idGenerator.PersistentGeneratorValue());
            if (_idGenerator.IsDirty())
            {
                _idGenerator.SetClean();
                _persistentState.SetStateDirty();
            }
            if (_persistentState.IsDirty())
            {
                _persistentState.Write(Transaction());
            }
            _container.FreespaceManager().EndCommit();
            _transactionalIdSystem.Commit(freespaceCommitter);
            _transactionalIdSystem.Clear();
        }

        public virtual void ReturnUnusedIds(IVisitable visitable)
        {
            visitable.Accept(new _IVisitor4_167(this));
        }

        public virtual int ChildId()
        {
            return _persistentState.Array()[ChildIdIndex];
        }

        public virtual void ChildId(int id)
        {
            _persistentState.Array()[ChildIdIndex] = id;
            _persistentState.SetStateDirty();
        }

        public virtual void TraverseOwnSlots(IProcedure4 block)
        {
            _parentIdSystem.TraverseOwnSlots(block);
            block.Apply(OwnSlotInfo(_persistentState.GetID()));
            block.Apply(OwnSlotInfo(_bTree.GetID()));
            var nodeIds = _bTree.AllNodeIds(_container.SystemTransaction());
            while (nodeIds.MoveNext())
            {
                block.Apply(OwnSlotInfo((((int) nodeIds.Current))));
            }
        }

        private void InitializeExisting(int persistentArrayId)
        {
            _persistentState = new PersistentIntegerArray(SlotChangeFactory.IdSystem, _transactionalIdSystem
                , persistentArrayId);
            _persistentState.Read(Transaction());
            _bTree = new BTree(Transaction(), BTreeConfiguration(), BTreeId(), new IdSlotMappingHandler
                ());
        }

        private BTreeConfiguration BTreeConfiguration()
        {
            return new BTreeConfiguration(_transactionalIdSystem
                , SlotChangeFactory.IdSystem, 64, false);
        }

        private int IdGeneratorValue()
        {
            return _persistentState.Array()[IdGeneratorIndex];
        }

        private void IdGeneratorValue(int value)
        {
            _persistentState.Array()[IdGeneratorIndex] = value;
        }

        private int BTreeId()
        {
            return _persistentState.Array()[BtreeIdIndex];
        }

        private void InitializeNew()
        {
            _bTree = new BTree(Transaction(), BTreeConfiguration(), new IdSlotMappingHandler
                ());
            var idGeneratorValue = _container.Handlers.LowestValidId() - 1;
            _persistentState = new PersistentIntegerArray(SlotChangeFactory.IdSystem, _transactionalIdSystem
                , new[] {_bTree.GetID(), idGeneratorValue, 0});
            _persistentState.Write(Transaction());
            _parentIdSystem.ChildId(_persistentState.GetID());
        }

        private int FindFreeId(int start)
        {
            throw new NotImplementedException();
        }

        private Transaction Transaction()
        {
            return _container.SystemTransaction();
        }

        public virtual ITransactionalIdSystem FreespaceIdSystem()
        {
            return _transactionalIdSystem;
        }

        public virtual void TraverseIds(IVisitor4 visitor)
        {
            _bTree.TraverseKeys(_container.SystemTransaction(), visitor);
        }

        private Pair OwnSlotInfo(int id)
        {
            return Pair.Of(id, _parentIdSystem.CommittedSlot(id));
        }

        private sealed class _IClosure4_40 : IClosure4
        {
            private readonly IStackableIdSystem parentIdSystem;

            public _IClosure4_40(IStackableIdSystem parentIdSystem)
            {
                this.parentIdSystem = parentIdSystem;
            }

            public object Run()
            {
                return parentIdSystem;
            }
        }

        private sealed class _IFunction4_52 : IFunction4
        {
            private readonly BTreeIdSystem _enclosing;

            public _IFunction4_52(BTreeIdSystem _enclosing)
            {
                this._enclosing = _enclosing;
            }

            public object Apply(object start)
            {
                return _enclosing.FindFreeId((((int) start)));
            }
        }

        private sealed class _IVisitor4_129 : IVisitor4
        {
            private readonly BTreeIdSystem _enclosing;

            public _IVisitor4_129(BTreeIdSystem _enclosing)
            {
                this._enclosing = _enclosing;
            }

            public void Visit(object slotChange)
            {
                if (!((SlotChange) slotChange).SlotModified())
                {
                    return;
                }
                _enclosing._bTree.Remove(_enclosing.Transaction(), new IdSlotMapping(((
                    TreeInt) slotChange)._key, 0, 0));
                if (((SlotChange) slotChange).RemoveId())
                {
                    return;
                }
                _enclosing._bTree.Add(_enclosing.Transaction(), new IdSlotMapping(((TreeInt
                    ) slotChange)._key, ((SlotChange) slotChange).NewSlot()));
                if (DTrace.enabled)
                {
                    DTrace.SlotMapped.LogLength(((TreeInt) slotChange)._key, ((SlotChange) slotChange).
                        NewSlot());
                }
            }
        }

        private sealed class _IVisitor4_167 : IVisitor4
        {
            private readonly BTreeIdSystem _enclosing;

            public _IVisitor4_167(BTreeIdSystem _enclosing)
            {
                this._enclosing = _enclosing;
            }

            public void Visit(object id)
            {
                _enclosing._bTree.Remove(_enclosing.Transaction(), new IdSlotMapping(((
                    (int) id)), 0, 0));
            }
        }

        public class IdSlotMappingHandler : IIndexable4
        {
            public virtual void DefragIndexEntry(DefragmentContextImpl context)
            {
                throw new NotImplementedException();
            }

            public virtual object ReadIndexEntry(IContext context, ByteArrayBuffer buffer)
            {
                return IdSlotMapping.Read(buffer);
            }

            public virtual void WriteIndexEntry(IContext context, ByteArrayBuffer buffer, object
                mapping)
            {
                ((IdSlotMapping) mapping).Write(buffer);
            }

            public virtual IPreparedComparison PrepareComparison(IContext context, object sourceMapping
                )
            {
                return new _IPreparedComparison_190(sourceMapping);
            }

            public int LinkLength()
            {
                return Const4.IntLength*3;
            }

            private sealed class _IPreparedComparison_190 : IPreparedComparison
            {
                private readonly object sourceMapping;

                public _IPreparedComparison_190(object sourceMapping)
                {
                    this.sourceMapping = sourceMapping;
                }

                public int CompareTo(object targetMapping)
                {
                    return ((IdSlotMapping) sourceMapping)._id == ((IdSlotMapping) targetMapping)._id
                        ? 0
                        : (((IdSlotMapping) sourceMapping)._id < ((IdSlotMapping) targetMapping)._id
                            ? -
                                1
                            : 1);
                }
            }
        }
    }
}