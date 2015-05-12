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
using Db4oUnit;

namespace Db4objects.Db4o.Tests.Common.Exceptions.Propagation
{
    public abstract class OneTimeFatalExceptionPropagationFixtureBase : IExceptionPropagationFixture
    {
        public void ThrowShutdownException()
        {
            Assert.Fail();
        }

        public virtual void ThrowCloseException()
        {
        }

        public virtual void AssertExecute(DatabaseContext context, TopLevelOperation op)
        {
            Assert.Expect(ExceptionType(), new _ICodeBlock_16(op, context));
            Assert.IsTrue(context.StorageIsClosed());
        }

        public abstract string Label();
        public abstract void ThrowInitialException();
        protected abstract Type ExceptionType();

        private sealed class _ICodeBlock_16 : ICodeBlock
        {
            private readonly DatabaseContext context;
            private readonly TopLevelOperation op;

            public _ICodeBlock_16(TopLevelOperation op, DatabaseContext context)
            {
                this.op = op;
                this.context = context;
            }

            /// <exception cref="System.Exception"></exception>
            public void Run()
            {
                op.Apply(context);
            }
        }
    }
}