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
using Db4objects.Db4o.Foundation;
using Db4oUnit;

namespace Db4oTool.Tests.Core
{
    internal class VerifyAssemblyTest : ITest
    {
        private readonly string _assemblyPath;

        public VerifyAssemblyTest(string assemblyPath)
        {
            _assemblyPath = assemblyPath;
        }

        public string Label()
        {
            return string.Format("peverify \"{0}\"", Path.GetFileNameWithoutExtension(_assemblyPath));
        }

        public void Run()
        {
            VerifyAssembly();
        }

        public bool IsLeafTest()
        {
            return true;
        }

        public ITest Transmogrify(IFunction4 fun)
        {
            return (ITest) fun.Apply(this);
        }

        private void VerifyAssembly()
        {
            var output = ShellUtilities.shell("peverify.exe", "/nologo", _assemblyPath);
            var stdout = output.ToString();
            if (output.ExitCode != 0)
            {
                Console.WriteLine("Db4oTool.Tests.Core.VerifyAssemblyTest _assemblyPath: " + _assemblyPath);
                Console.WriteLine("Db4oTool.Tests.Core.VerifyAssemblyTest stdout: " + stdout);
                Console.WriteLine("Db4oTool.Tests.Core.VerifyAssemblyTest output.ExitCode: " + output.ExitCode);
            }
            if (stdout.Contains("1.1.4322.573")) return; // ignore older peverify version errors
            if (output.ExitCode == 0 && !stdout.ToUpper().Contains("WARNING")) return;
            Assert.Fail(stdout);
        }
    }
}