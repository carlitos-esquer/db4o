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
using Sharpen;
using Sharpen.Lang;

namespace Db4objects.Db4o.Internal.Weakref
{
    internal class EnabledWeakReferenceSupport : IWeakReferenceSupport
    {
        private readonly ObjectContainerBase _container;
        private readonly object _queue;
        private SimpleTimer _timer;

        internal EnabledWeakReferenceSupport(ObjectContainerBase container)
        {
            _container = container;
            _queue = Platform4.CreateReferenceQueue();
        }

        public virtual object NewWeakReference(ObjectReference referent, object obj)
        {
            return Platform4.CreateActiveObjectReference(_queue, referent, obj);
        }

        public virtual void Purge()
        {
            Platform4.PollReferenceQueue(_container, _queue);
        }

        public virtual void Start()
        {
            if (_timer != null)
            {
                return;
            }
            if (!_container.ConfigImpl.WeakReferences())
            {
                return;
            }
            if (_container.ConfigImpl.WeakReferenceCollectionInterval() <= 0)
            {
                return;
            }
            _timer = new SimpleTimer(new Collector(this), _container
                .ConfigImpl.WeakReferenceCollectionInterval());
            _container.ThreadPool().Start("db4o WeakReference collector", _timer);
        }

        public virtual void Stop()
        {
            if (_timer == null)
            {
                return;
            }
            _timer.Stop();
            _timer = null;
        }

        private sealed class Collector : IRunnable
        {
            private readonly EnabledWeakReferenceSupport _enclosing;

            internal Collector(EnabledWeakReferenceSupport _enclosing)
            {
                this._enclosing = _enclosing;
            }

            public void Run()
            {
                try
                {
                    _enclosing.Purge();
                }
                catch (DatabaseClosedException)
                {
                }
                catch (Exception e)
                {
                    // can happen, no stack trace
                    // don't bring down the thread
                    Runtime.PrintStackTrace(e);
                }
            }
        }
    }
}