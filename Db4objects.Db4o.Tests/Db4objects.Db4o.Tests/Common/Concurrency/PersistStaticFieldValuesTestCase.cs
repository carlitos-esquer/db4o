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
    public class PersistStaticFieldValuesTestCase : Db4oClientServerTestCase
    {
        public static readonly PsfvHelper One = new PsfvHelper
            ();

        public static readonly PsfvHelper Two = new PsfvHelper
            ();

        public static readonly PsfvHelper Three = new PsfvHelper
            ();

        public PsfvHelper one;
        public PsfvHelper three;
        public PsfvHelper two;

        public static void Main(string[] args)
        {
            new PersistStaticFieldValuesTestCase().RunConcurrency();
        }

        protected override void Configure(IConfiguration config)
        {
            config.ObjectClass(typeof (PersistStaticFieldValuesTestCase)).PersistStaticFieldValues
                ();
        }

        protected override void Store()
        {
            var psfv = new PersistStaticFieldValuesTestCase();
            psfv.one = One;
            psfv.two = Two;
            psfv.three = Three;
            Store(psfv);
        }

        public virtual void Conc(IExtObjectContainer oc)
        {
            var psfv = (PersistStaticFieldValuesTestCase
                ) RetrieveOnlyInstance(oc, typeof (PersistStaticFieldValuesTestCase));
            Assert.AreSame(One, psfv.one);
            Assert.AreSame(Two, psfv.two);
            Assert.AreSame(Three, psfv.three);
        }

        public class PsfvHelper
        {
        }
    }
}

#endif // !SILVERLIGHT