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
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.Common.Querying
{
    /// <exclude></exclude>
    public class IndexOnParentFieldTestCase : AbstractDb4oTestCase
    {
        protected override void Store()
        {
            Store(new Parent("one"));
            Store(new Child("one"));
        }

        /// <exception cref="System.Exception"></exception>
        protected override void Configure(IConfiguration config)
        {
            config.ObjectClass(typeof (Parent)).ObjectField("_name"
                ).Indexed(true);
        }

        public virtual void Test()
        {
            var q = NewQuery();
            q.Constrain(typeof (Child));
            q.Descend("_name").Constrain("one");
            Assert.AreEqual(1, q.Execute().Count);
        }

        public class Parent
        {
            public string _name;

            public Parent(string name)
            {
                _name = name;
            }
        }

        public class Child : Parent
        {
            public Child(string name) : base(name)
            {
            }
        }
    }
}