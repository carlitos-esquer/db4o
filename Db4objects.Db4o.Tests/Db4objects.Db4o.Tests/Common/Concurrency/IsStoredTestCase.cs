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
using Db4objects.Db4o.Ext;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.Common.Concurrency
{
    public class IsStoredTestCase : Db4oClientServerTestCase
    {
        public string myString;

        public static void Main(string[] args)
        {
            new IsStoredTestCase().RunConcurrency();
        }

        public virtual void Conc(IExtObjectContainer oc)
        {
            var isStored = new IsStoredTestCase();
            isStored.myString = "isStored";
            oc.Store(isStored);
            Assert.IsTrue(oc.IsStored(isStored));
            oc.Commit();
            oc.Delete(isStored);
            Assert.IsFalse(oc.IsStored(isStored));
            oc.Rollback();
            Assert.IsTrue(oc.IsStored(isStored));
            oc.Delete(isStored);
            Assert.IsFalse(oc.IsStored(isStored));
            oc.Commit();
            Assert.IsFalse(oc.IsStored(isStored));
        }

        public virtual void Check(IExtObjectContainer oc)
        {
            AssertOccurrences(oc, typeof (IsStoredTestCase), 0);
        }
    }
}

#endif // !SILVERLIGHT