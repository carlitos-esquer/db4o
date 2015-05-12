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
using Db4oUnit.Fixtures;
using Sharpen.Lang;

namespace Db4oUnit.Tests.Fixtures
{
    public class FixtureContextTestCase : ITestCase
    {
        public virtual void Test()
        {
            var f1 = new FixtureVariable();
            var f2 = new FixtureVariable();
            var c1 = new ContextRef();
            var c2 = new ContextRef();
            new FixtureContext().Run(new _IRunnable_19(this, f1, f2, c1, c2));
            AssertNoValue(f1);
            AssertNoValue(f2);
            c1.value.Run(new _IRunnable_41(this, f1, f2));
            c2.value.Run(new _IRunnable_48(this, f1, f2));
        }

        private void AssertNoValue(FixtureVariable f1)
        {
            Assert.Expect(typeof (InvalidOperationException), new _ICodeBlock_57(f1));
        }

        private void AssertValue(string expected, FixtureVariable fixture)
        {
            Assert.AreEqual(expected, fixture.Value);
        }

        public sealed class ContextRef
        {
            public FixtureContext value;
        }

        private sealed class _IRunnable_19 : IRunnable
        {
            private readonly FixtureContextTestCase _enclosing;
            private readonly ContextRef c1;
            private readonly ContextRef c2;
            private readonly FixtureVariable f1;
            private readonly FixtureVariable f2;

            public _IRunnable_19(FixtureContextTestCase _enclosing, FixtureVariable f1, FixtureVariable
                f2, ContextRef c1, ContextRef c2)
            {
                this._enclosing = _enclosing;
                this.f1 = f1;
                this.f2 = f2;
                this.c1 = c1;
                this.c2 = c2;
            }

            public void Run()
            {
                f1.With("foo", new _IRunnable_21(this, f1, f2, c1, c2));
            }

            private sealed class _IRunnable_21 : IRunnable
            {
                private readonly _IRunnable_19 _enclosing;
                private readonly ContextRef c1;
                private readonly ContextRef c2;
                private readonly FixtureVariable f1;
                private readonly FixtureVariable f2;

                public _IRunnable_21(_IRunnable_19 _enclosing, FixtureVariable f1, FixtureVariable
                    f2, ContextRef c1, ContextRef c2)
                {
                    this._enclosing = _enclosing;
                    this.f1 = f1;
                    this.f2 = f2;
                    this.c1 = c1;
                    this.c2 = c2;
                }

                public void Run()
                {
                    _enclosing._enclosing.AssertValue("foo", f1);
                    _enclosing._enclosing.AssertNoValue(f2);
                    c1.value = FixtureContext.Current;
                    f2.With("bar", new _IRunnable_26(this, f1, f2, c2));
                }

                private sealed class _IRunnable_26 : IRunnable
                {
                    private readonly _IRunnable_21 _enclosing;
                    private readonly ContextRef c2;
                    private readonly FixtureVariable f1;
                    private readonly FixtureVariable f2;

                    public _IRunnable_26(_IRunnable_21 _enclosing, FixtureVariable f1, FixtureVariable
                        f2, ContextRef c2)
                    {
                        this._enclosing = _enclosing;
                        this.f1 = f1;
                        this.f2 = f2;
                        this.c2 = c2;
                    }

                    public void Run()
                    {
                        _enclosing._enclosing._enclosing.AssertValue("foo", f1);
                        _enclosing._enclosing._enclosing.AssertValue("bar", f2);
                        c2.value = FixtureContext.Current;
                    }
                }
            }
        }

        private sealed class _IRunnable_41 : IRunnable
        {
            private readonly FixtureContextTestCase _enclosing;
            private readonly FixtureVariable f1;
            private readonly FixtureVariable f2;

            public _IRunnable_41(FixtureContextTestCase _enclosing, FixtureVariable f1, FixtureVariable
                f2)
            {
                this._enclosing = _enclosing;
                this.f1 = f1;
                this.f2 = f2;
            }

            public void Run()
            {
                _enclosing.AssertValue("foo", f1);
                _enclosing.AssertNoValue(f2);
            }
        }

        private sealed class _IRunnable_48 : IRunnable
        {
            private readonly FixtureContextTestCase _enclosing;
            private readonly FixtureVariable f1;
            private readonly FixtureVariable f2;

            public _IRunnable_48(FixtureContextTestCase _enclosing, FixtureVariable f1, FixtureVariable
                f2)
            {
                this._enclosing = _enclosing;
                this.f1 = f1;
                this.f2 = f2;
            }

            public void Run()
            {
                _enclosing.AssertValue("foo", f1);
                _enclosing.AssertValue("bar", f2);
            }
        }

        private sealed class _ICodeBlock_57 : ICodeBlock
        {
            private readonly FixtureVariable f1;

            public _ICodeBlock_57(FixtureVariable f1)
            {
                this.f1 = f1;
            }

            public void Run()
            {
                Use(f1.Value);
            }

            private void Use(object value)
            {
            }
        }
    }
}