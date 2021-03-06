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
using Db4objects.Db4o.Config;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.CLI1
{
    public class CsCustomTransientAttribute : AbstractDb4oTestCase
    {
        public string myPersistent;

        [CustomTransient] public string myTransient;

        protected override void Configure(IConfiguration config)
        {
            base.Configure(config);
            config.MarkTransient(typeof (CustomTransientAttribute).FullName);
        }

        protected override void Store()
        {
            var item = new CsCustomTransientAttribute();
            item.myTransient = "trans";
            item.myPersistent = "pers";
            Store(item);
        }

        public void Test()
        {
            var instance = (CsCustomTransientAttribute) RetrieveOnlyInstance(GetType());
            Assert.IsNull(instance.myTransient);
            Assert.AreEqual("pers", instance.myPersistent);
        }
    }

    public class CustomTransientAttribute : Attribute
    {
    }
}