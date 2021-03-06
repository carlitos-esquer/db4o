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
using Db4objects.Db4o.Query;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.Common.Staging
{
    public class InterfaceQueryTestCase : AbstractDb4oTestCase
    {
        private static readonly string FieldA = "fieldA";
        private static readonly string FieldB = "fieldB";

        /// <exception cref="System.Exception"></exception>
        protected virtual void _configure(IConfiguration config)
        {
            ConfigIndexed(config, typeof (DataA), FieldA);
            ConfigIndexed(config, typeof (DataA), FieldB);
            ConfigIndexed(config, typeof (DataB), FieldA);
            ConfigIndexed(config, typeof (DataB), FieldB);
        }

        private void ConfigIndexed(IConfiguration config, Type clazz, string fieldName)
        {
            config.ObjectClass(clazz).ObjectField(fieldName).Indexed(true);
        }

        /// <exception cref="System.Exception"></exception>
        protected override void Store()
        {
            Store(new DataA(10, 10));
            Store(new DataA(20, 20));
            Store(new DataB(10, 10));
            Store(new DataB(30, 30));
        }

        public virtual void TestExplicitNotQuery()
        {
            var query = NewQuery();
            query.Constrain(typeof (DataA)).And(query.Descend(FieldA).Constrain
                (10).Not()).Or(query.Constrain(typeof (DataB)).And(query.Descend
                    (FieldA).Constrain(10).Not()));
            Assert.AreEqual(2, query.Execute().Count);
        }

        public virtual void TestExplicitNotQuery2()
        {
            var query = NewQuery();
            query.Constrain(typeof (DataA)).Or(query.Constrain(typeof (DataB
                )));
            query.Descend(FieldA).Constrain(10).Not();
            Assert.AreEqual(2, query.Execute().Count);
        }

        public virtual void TestQueryAll()
        {
            AssertQueryResult(4, new _IQueryConstrainer_73());
        }

        public virtual void TestSingleConstraint()
        {
            AssertQueryResult(2, new _IQueryConstrainer_80());
        }

        public virtual void TestAnd()
        {
            AssertQueryResult(2, new _IQueryConstrainer_88());
        }

        public virtual void TestOr()
        {
            AssertQueryResult(2, new _IQueryConstrainer_98());
        }

        public virtual void TestNot()
        {
            AssertQueryResult(2, new _IQueryConstrainer_108());
        }

        public virtual void AssertQueryResult(int expected, IQueryConstrainer
            constrainer)
        {
            var query = NewQuery(typeof (IIData));
            constrainer.Constrain(query);
            Assert.AreEqual(expected, query.Execute().Count);
        }

        public interface IIData
        {
        }

        public class DataA : IIData
        {
            public int fieldA;
            public int fieldB;

            public DataA(int a, int b)
            {
                fieldA = a;
                fieldB = b;
            }
        }

        public class DataB : IIData
        {
            public int fieldA;
            public int fieldB;

            public DataB(int a, int b)
            {
                fieldA = a;
                fieldB = b;
            }
        }

        private sealed class _IQueryConstrainer_73 : IQueryConstrainer
        {
            public void Constrain(IQuery query)
            {
            }
        }

        private sealed class _IQueryConstrainer_80 : IQueryConstrainer
        {
            public void Constrain(IQuery query)
            {
                query.Descend(FieldA).Constrain(10);
            }
        }

        private sealed class _IQueryConstrainer_88 : IQueryConstrainer
        {
            public void Constrain(IQuery query)
            {
                var icon1 = query.Descend(FieldA).Constrain(10);
                var icon2 = query.Descend(FieldB).Constrain(10);
                icon1.And(icon2);
            }
        }

        private sealed class _IQueryConstrainer_98 : IQueryConstrainer
        {
            public void Constrain(IQuery query)
            {
                var icon1 = query.Descend(FieldA).Constrain(10);
                var icon2 = query.Descend(FieldB).Constrain(10);
                icon1.Or(icon2);
            }
        }

        private sealed class _IQueryConstrainer_108 : IQueryConstrainer
        {
            public void Constrain(IQuery query)
            {
                query.Descend(FieldA).Constrain(10).Not();
            }
        }

        public interface IQueryConstrainer
        {
            void Constrain(IQuery query);
        }
    }
}