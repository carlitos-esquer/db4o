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
using System.Collections.Generic;
using Sharpen.Lang;

namespace Db4objects.Db4o.Internal.Threading
{
    public class ThreadPool4Impl : IThreadPool4
    {
        private readonly IList<Thread> _activeThreads = new List<Thread>();
        private EventHandler<UncaughtExceptionEventArgs> _uncaughtException;

        /// <exception cref="System.Exception"></exception>
        public virtual void Join(int timeoutMilliseconds)
        {
            foreach (var thread in ActiveThreads())
            {
                thread.Join(timeoutMilliseconds);
            }
        }

        public virtual void StartLowPriority(string taskName, IRunnable task)
        {
            var thread = ThreadFor(taskName, task);
            ActivateThread(thread);
        }

        public virtual void Start(string taskName, IRunnable task)
        {
            var thread = ThreadFor(taskName, task);
            ActivateThread(thread);
        }

        public virtual event EventHandler<UncaughtExceptionEventArgs> UncaughtException
        {
            add
            {
                _uncaughtException = (EventHandler<UncaughtExceptionEventArgs>) Delegate.Combine
                    (_uncaughtException, value);
            }
            remove
            {
                _uncaughtException = (EventHandler<UncaughtExceptionEventArgs>) Delegate.Remove
                    (_uncaughtException, value);
            }
        }

        private Thread ThreadFor(string threadName, IRunnable task)
        {
            var thread = new Thread(new _IRunnable_41(this, task), threadName);
            thread.SetDaemon(true);
            return thread;
        }

        private void ActivateThread(Thread thread)
        {
            AddActiveThread(thread);
            thread.Start();
        }

        private Thread[] ActiveThreads()
        {
            lock (_activeThreads)
            {
                return Sharpen.Collections.ToArray(_activeThreads, new Thread[_activeThreads.Count
                    ]);
            }
        }

        private void AddActiveThread(Thread thread)
        {
            lock (_activeThreads)
            {
                _activeThreads.Add(thread);
            }
        }

        protected virtual void Dispose(Thread thread)
        {
            lock (_activeThreads)
            {
                _activeThreads.Remove(thread);
            }
        }

        protected virtual void TriggerUncaughtExceptionEvent(Exception e)
        {
            if (null != _uncaughtException)
                _uncaughtException(null, new UncaughtExceptionEventArgs
                    (e));
        }

        private sealed class _IRunnable_41 : IRunnable
        {
            private readonly ThreadPool4Impl _enclosing;
            private readonly IRunnable task;

            public _IRunnable_41(ThreadPool4Impl _enclosing, IRunnable task)
            {
                this._enclosing = _enclosing;
                this.task = task;
            }

            public void Run()
            {
                try
                {
                    task.Run();
                }
                catch (Exception e)
                {
                    _enclosing.TriggerUncaughtExceptionEvent(e);
                }
                finally
                {
                    _enclosing.Dispose(Thread.CurrentThread());
                }
            }
        }
    }
}