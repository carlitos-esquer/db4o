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

#if !SILVERLIGHT
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
    public class InternStringsTestCase : Db4oClientServerTestCase
    {
        public string _name;

        public InternStringsTestCase() : this(null)
        {
        }

        public InternStringsTestCase(string name)
        {
            _name = name;
        }

        public static void Main(string[] args)
        {
            new InternStringsTestCase().RunConcurrency
                ();
        }

        protected override void Configure(IConfiguration config)
        {
            config.InternStrings(true);
        }

        protected override void Store()
        {
            var name = "Foo";
            Store(new InternStringsTestCase(name));
            Store(new InternStringsTestCase(name));
        }

        public virtual void Conc(IExtObjectContainer oc)
        {
            var query = oc.Query();
            query.Constrain(typeof (InternStringsTestCase
                ));
            var result = query.Execute();
            Assert.AreEqual(2, result.Count);
            var first = (InternStringsTestCase
                ) result.Next();
            var second = (InternStringsTestCase
                ) result.Next();
            Assert.AreSame(first._name, second._name);
        }
    }
}

#endif // !SILVERLIGHT