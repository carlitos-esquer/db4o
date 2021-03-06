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

using Db4objects.Db4o.Foundation;

namespace Db4oUnit.Fixtures
{
    internal sealed class FixtureDecorator : ITestDecorator
    {
        private readonly object _fixture;
        private readonly int _fixtureIndex;
        private readonly FixtureVariable _provider;

        internal FixtureDecorator(FixtureVariable provider, object fixture, int fixtureIndex
            )
        {
            _fixture = fixture;
            _provider = provider;
            _fixtureIndex = fixtureIndex;
        }

        public ITest Decorate(ITest test)
        {
            var label = Label();
            return test.Transmogrify(new _IFunction4_22(this, label));
        }

        private string Label()
        {
            var label = _provider.Label + "[" + _fixtureIndex + "]";
            if (_fixture is ILabeled)
            {
                label += ":" + ((ILabeled) _fixture).Label();
            }
            return label;
        }

        private sealed class _IFunction4_22 : IFunction4
        {
            private readonly FixtureDecorator _enclosing;
            private readonly string label;

            public _IFunction4_22(FixtureDecorator _enclosing, string label)
            {
                this._enclosing = _enclosing;
                this.label = label;
            }

            public object Apply(object innerTest)
            {
                return new TestWithFixture(((ITest) innerTest), label, _enclosing._provider,
                    _enclosing._fixture);
            }
        }
    }
}