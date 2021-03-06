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

using Db4objects.Db4o.Config;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.IO;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.Common.Config
{
    public class ObjectContainerCustomNameTestCase : ITestCase
    {
        private static readonly string FileName = "foo.db4o";
        protected static readonly string CustomName = "custom";

        public virtual void TestDefault()
        {
            AssertName(Config(), FileName);
        }

        public virtual void TestCustom()
        {
            var config = Config();
            config.Common.NameProvider(new CustomNameProvider
                ());
            AssertName(config, CustomName);
        }

        public virtual void TestNameIsAvailableAtConfigurationItemApplication()
        {
            var config = Config();
            config.Common.NameProvider(new CustomNameProvider
                ());
            config.Common.Add(new _IConfigurationItem_35());
            AssertName(config, CustomName);
        }

        private void AssertName(IEmbeddedConfiguration config, string expected)
        {
            var db = Db4oEmbedded.OpenFile(config, FileName);
            Assert.AreEqual(expected, db.ToString());
            db.Close();
        }

        private IEmbeddedConfiguration Config()
        {
            var config = Db4oEmbedded.NewConfiguration();
            config.File.Storage = new MemoryStorage();
            return config;
        }

        private class CustomNameProvider : INameProvider
        {
            public virtual string Name(IObjectContainer db)
            {
                return CustomName;
            }
        }

        private sealed class _IConfigurationItem_35 : IConfigurationItem
        {
            public void Apply(IInternalObjectContainer container)
            {
                Assert.AreEqual(CustomName, container.ToString(
                    ));
            }

            public void Prepare(IConfiguration configuration)
            {
            }
        }
    }
}