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
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Query.Processor;

namespace Db4objects.Db4o.Internal.Fieldindex
{
    public abstract class IndexedNodeBase : IIndexedNode
    {
        protected readonly QConObject _constraint;

        public IndexedNodeBase(QConObject qcon)
        {
            if (null == qcon)
            {
                throw new ArgumentNullException();
            }
            if (null == qcon.GetField())
            {
                throw new ArgumentException();
            }
            _constraint = qcon;
        }

        public virtual TreeInt ToTreeInt()
        {
            return AddToTree(null, this);
        }

        public BTree GetIndex()
        {
            return GetYapField().GetIndex(Transaction());
        }

        public virtual bool IsResolved()
        {
            var parent = Constraint().Parent();
            return null == parent || !parent.HasParent();
        }

        public virtual IIndexedNode Resolve()
        {
            if (IsResolved())
            {
                return null;
            }
            return IndexedPath.NewParentPath(this, Constraint());
        }

        public abstract IEnumerator GetEnumerator();
        public abstract void MarkAsBestIndex();
        public abstract int ResultSize();

        private FieldMetadata GetYapField()
        {
            return _constraint.GetField().GetFieldMetadata();
        }

        public virtual QCon Constraint()
        {
            return _constraint;
        }

        public virtual IBTreeRange Search(object value)
        {
            return GetYapField().Search(Transaction(), value);
        }

        public static TreeInt AddToTree(TreeInt tree, IIndexedNode node)
        {
            var i = node.GetEnumerator();
            while (i.MoveNext())
            {
                var composite = (IFieldIndexKey) i.Current;
                tree = (TreeInt) Tree.Add(tree, new TreeInt(composite.ParentID()));
            }
            return tree;
        }

        private Transaction Transaction()
        {
            return Constraint().Transaction();
        }
    }
}