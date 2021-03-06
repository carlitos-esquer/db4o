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
using System.IO;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.Common.Api
{
    public class Db4oEmbeddedTestCase : TestWithTempFile
    {
        public virtual void TestOpenFile()
        {
            IObjectContainer container = Db4oEmbedded.OpenFile(Db4oEmbedded.NewConfiguration(
                ), TempFile());
            try
            {
                Assert.IsTrue(File.Exists(TempFile()));
            }
            finally
            {
                container.Close();
            }
        }

        public virtual void TestOpenFileWithNullConfiguration()
        {
            Assert.Expect(typeof (ArgumentNullException), new _ICodeBlock_23(this));
        }

        private sealed class _ICodeBlock_23 : ICodeBlock
        {
            private readonly Db4oEmbeddedTestCase _enclosing;

            public _ICodeBlock_23(Db4oEmbeddedTestCase _enclosing)
            {
                this._enclosing = _enclosing;
            }

            /// <exception cref="System.Exception"></exception>
            public void Run()
            {
                Db4oEmbedded.OpenFile(null, _enclosing.TempFile());
            }
        }
    }
}