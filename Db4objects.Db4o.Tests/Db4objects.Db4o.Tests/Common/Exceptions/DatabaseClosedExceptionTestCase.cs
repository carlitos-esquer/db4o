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

using Db4objects.Db4o.Ext;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.Common.Exceptions
{
    public class DatabaseClosedExceptionTestCase : AbstractDb4oTestCase
    {
        public static void Main(string[] args)
        {
            new DatabaseClosedExceptionTestCase().RunAll();
        }

        public virtual void TestRollback()
        {
            Db().Close();
            Assert.Expect(typeof (DatabaseClosedException), new _ICodeBlock_17(this));
        }

        public virtual void TestCommit()
        {
            Db().Close();
            Assert.Expect(typeof (DatabaseClosedException), new _ICodeBlock_26(this));
        }

        public virtual void TestSet()
        {
            Db().Close();
            Assert.Expect(typeof (DatabaseClosedException), new _ICodeBlock_35(this));
        }

        public virtual void TestDelete()
        {
            Db().Close();
            Assert.Expect(typeof (DatabaseClosedException), new _ICodeBlock_44(this));
        }

        public virtual void TestQueryClass()
        {
            Db().Close();
            Assert.Expect(typeof (DatabaseClosedException), new _ICodeBlock_53(this));
        }

        public virtual void TestQuery()
        {
            Db().Close();
            Assert.Expect(typeof (DatabaseClosedException), new _ICodeBlock_62(this));
        }

        public virtual void TestDeactivate()
        {
            Db().Close();
            Assert.Expect(typeof (DatabaseClosedException), new _ICodeBlock_71(this));
        }

        public virtual void TestActivate()
        {
            Db().Close();
            Assert.Expect(typeof (DatabaseClosedException), new _ICodeBlock_80(this));
        }

        public virtual void TestGet()
        {
            Db().Close();
            Assert.Expect(typeof (DatabaseClosedException), new _ICodeBlock_89(this));
        }

        private sealed class _ICodeBlock_17 : ICodeBlock
        {
            private readonly DatabaseClosedExceptionTestCase _enclosing;

            public _ICodeBlock_17(DatabaseClosedExceptionTestCase _enclosing)
            {
                this._enclosing = _enclosing;
            }

            /// <exception cref="System.Exception"></exception>
            public void Run()
            {
                _enclosing.Db().Rollback();
            }
        }

        private sealed class _ICodeBlock_26 : ICodeBlock
        {
            private readonly DatabaseClosedExceptionTestCase _enclosing;

            public _ICodeBlock_26(DatabaseClosedExceptionTestCase _enclosing)
            {
                this._enclosing = _enclosing;
            }

            /// <exception cref="System.Exception"></exception>
            public void Run()
            {
                _enclosing.Db().Commit();
            }
        }

        private sealed class _ICodeBlock_35 : ICodeBlock
        {
            private readonly DatabaseClosedExceptionTestCase _enclosing;

            public _ICodeBlock_35(DatabaseClosedExceptionTestCase _enclosing)
            {
                this._enclosing = _enclosing;
            }

            /// <exception cref="System.Exception"></exception>
            public void Run()
            {
                _enclosing.Db().Store(new Item());
            }
        }

        private sealed class _ICodeBlock_44 : ICodeBlock
        {
            private readonly DatabaseClosedExceptionTestCase _enclosing;

            public _ICodeBlock_44(DatabaseClosedExceptionTestCase _enclosing)
            {
                this._enclosing = _enclosing;
            }

            /// <exception cref="System.Exception"></exception>
            public void Run()
            {
                _enclosing.Db().Delete(new Item());
            }
        }

        private sealed class _ICodeBlock_53 : ICodeBlock
        {
            private readonly DatabaseClosedExceptionTestCase _enclosing;

            public _ICodeBlock_53(DatabaseClosedExceptionTestCase _enclosing)
            {
                this._enclosing = _enclosing;
            }

            /// <exception cref="System.Exception"></exception>
            public void Run()
            {
                _enclosing.Db().Query(GetType());
            }
        }

        private sealed class _ICodeBlock_62 : ICodeBlock
        {
            private readonly DatabaseClosedExceptionTestCase _enclosing;

            public _ICodeBlock_62(DatabaseClosedExceptionTestCase _enclosing)
            {
                this._enclosing = _enclosing;
            }

            /// <exception cref="System.Exception"></exception>
            public void Run()
            {
                _enclosing.Db().Query();
            }
        }

        private sealed class _ICodeBlock_71 : ICodeBlock
        {
            private readonly DatabaseClosedExceptionTestCase _enclosing;

            public _ICodeBlock_71(DatabaseClosedExceptionTestCase _enclosing)
            {
                this._enclosing = _enclosing;
            }

            /// <exception cref="System.Exception"></exception>
            public void Run()
            {
                _enclosing.Db().Deactivate(new Item(), 1);
            }
        }

        private sealed class _ICodeBlock_80 : ICodeBlock
        {
            private readonly DatabaseClosedExceptionTestCase _enclosing;

            public _ICodeBlock_80(DatabaseClosedExceptionTestCase _enclosing)
            {
                this._enclosing = _enclosing;
            }

            /// <exception cref="System.Exception"></exception>
            public void Run()
            {
                _enclosing.Db().Activate(new Item(), 1);
            }
        }

        private sealed class _ICodeBlock_89 : ICodeBlock
        {
            private readonly DatabaseClosedExceptionTestCase _enclosing;

            public _ICodeBlock_89(DatabaseClosedExceptionTestCase _enclosing)
            {
                this._enclosing = _enclosing;
            }

            /// <exception cref="System.Exception"></exception>
            public void Run()
            {
                _enclosing.Db().QueryByExample(new Item());
            }
        }
    }
}