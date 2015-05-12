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
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.Common.Staging
{
    public class LazyQueryDeleteTestCase : AbstractDb4oTestCase
    {
        private const int Count = 3;

        protected override void Configure(IConfiguration config)
        {
            config.Queries().EvaluationMode(QueryEvaluationMode.Lazy);
        }

        /// <exception cref="System.Exception"></exception>
        protected override void Store()
        {
            for (var i = 0; i < Count; i++)
            {
                Store(new Item(i.ToString()));
                Db().Commit();
            }
        }

        public virtual void Test()
        {
            var objectSet = NewQuery(typeof (Item)).Execute();
            for (var i = 0; i < Count; i++)
            {
                Db().Delete(objectSet.Next());
                Db().Commit();
            }
        }

        public static void Main(string[] arguments)
        {
            new LazyQueryDeleteTestCase().RunSolo();
        }

        public class Item
        {
            public string _name;

            public Item(string name)
            {
                _name = name;
            }
        }
    }
}