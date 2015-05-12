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

using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;

namespace Db4objects.Db4o.Internal.Reflect.Generic
{
    public class KnownClassesCollector
    {
        private readonly ObjectContainerBase _container;
        private readonly KnownClassesRepository _repository;

        public KnownClassesCollector(ObjectContainerBase container, KnownClassesRepository
            repository)
        {
            _container = container;
            _repository = repository;
        }

        public virtual IReflectClass[] Collect()
        {
            var classes = new Collection4();
            CollectKnownClasses(classes);
            return (IReflectClass[]) classes.ToArray(new IReflectClass[classes.Size()]);
        }

        private void CollectKnownClasses(Collection4 classes)
        {
            var collectingListener = NewCollectingClassListener(classes);
            _repository.AddListener(collectingListener);
            try
            {
                CollectKnownClasses(classes, Iterators.Copy(_repository.Classes()));
            }
            finally
            {
                _repository.RemoveListener(collectingListener);
            }
        }

        private IListener4 NewCollectingClassListener(Collection4 classes)
        {
            return new _IListener4_37(this, classes);
        }

        private void CollectKnownClasses(Collection4 collector, IEnumerator knownClasses)
        {
            while (knownClasses.MoveNext())
            {
                var clazz = (IReflectClass) knownClasses.Current;
                CollectKnownClass(collector, clazz);
            }
        }

        private void CollectKnownClass(Collection4 classes, IReflectClass clazz)
        {
            if (IsInternalClass(clazz))
            {
                return;
            }
            if (!HasIdentity(clazz))
            {
                return;
            }
            if (clazz.IsArray())
            {
                return;
            }
            classes.Add(clazz);
        }

        private bool IsInternalClass(IReflectClass clazz)
        {
            return _container._handlers.IclassInternal.IsAssignableFrom(clazz);
        }

        private bool HasIdentity(IReflectClass clazz)
        {
            var clazzMeta = _container.ClassMetadataForReflectClass(clazz);
            return clazzMeta == null || clazzMeta.HasIdentity();
        }

        private sealed class _IListener4_37 : IListener4
        {
            private readonly KnownClassesCollector _enclosing;
            private readonly Collection4 classes;

            public _IListener4_37(KnownClassesCollector _enclosing, Collection4 classes)
            {
                this._enclosing = _enclosing;
                this.classes = classes;
            }

            public void OnEvent(object addedClass)
            {
                _enclosing.CollectKnownClass(classes, ((IReflectClass) addedClass));
            }
        }
    }
}