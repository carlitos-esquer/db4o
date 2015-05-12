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
using Db4objects.Db4o.Constraints;
using Db4oUnit;
using Db4oUnit.Extensions;

namespace Db4objects.Db4o.Tests.Common.Constraints
{
    public class UniqueFieldIndexTestCase : AbstractDb4oTestCase
    {
        public static void Main(string[] arguments)
        {
            new UniqueFieldIndexTestCase().RunAll();
        }

        /// <exception cref="System.Exception"></exception>
        protected override void Configure(IConfiguration config)
        {
            base.Configure(config);
            IndexField(config, typeof (Item), "_str");
            config.Add(new UniqueFieldValueConstraint(typeof (Item),
                "_str"));
            config.ObjectClass(typeof (IHavaNothingToDoWithItemInstances
                )).CallConstructor(true);
        }

        /// <exception cref="System.Exception"></exception>
        protected override void Store()
        {
            AddItem("1");
            AddItem("2");
            AddItem("3");
        }

        public virtual void TestNewViolates()
        {
            AddItem("2");
            CommitExpectingViolation();
        }

        public virtual void TestUpdateViolates()
        {
            UpdateItem("2", "3");
            CommitExpectingViolation();
        }

        public virtual void TestUpdateDoesNotViolate()
        {
            UpdateItem("2", "4");
            Db().Commit();
        }

        public virtual void TestUpdatingSameObjectDoesNotViolate()
        {
            UpdateItem("2", "2");
            Db().Commit();
        }

        public virtual void TestNewAfterDeleteDoesNotViolate()
        {
            DeleteItem("2");
            AddItem("2");
            Db().Commit();
        }

        public virtual void TestDeleteAfterNewDoesNotViolate()
        {
            var existing = QueryItem("2");
            AddItem("2");
            Db().Delete(existing);
            Db().Commit();
        }

        public virtual void TestObjectsAreNotReadUnnecessarily()
        {
            AddItem("5");
            Store(new IHavaNothingToDoWithItemInstances(unchecked(0xdb40)));
            Db().Commit();
            Assert.AreEqual(ExpectedConstructorsCalls(), IHavaNothingToDoWithItemInstances
                ._constructorCallsCounter);
        }

        private int ExpectedConstructorsCalls()
        {
            return IsNetworkClientServer() ? 3 : 1;
        }

        // Account for constructor validations 
        private bool IsNetworkClientServer()
        {
            return IsMultiSession() && !IsEmbedded();
        }

        private void DeleteItem(string value)
        {
            Db().Delete(QueryItem(value));
        }

        private void CommitExpectingViolation()
        {
            Assert.Expect(typeof (UniqueFieldValueConstraintViolationException), new _ICodeBlock_109
                (this));
            Db().Rollback();
        }

        private Item QueryItem(string str)
        {
            var q = NewQuery(typeof (Item));
            q.Descend("_str").Constrain(str);
            return (Item) q.Execute().Next();
        }

        private void AddItem(string value)
        {
            Store(new Item(value));
        }

        private void UpdateItem(string existing, string newValue)
        {
            var item = QueryItem(existing);
            item._str = newValue;
            Store(item);
        }

        public class Item
        {
            public string _str;

            public Item()
            {
            }

            public Item(string str)
            {
                _str = str;
            }
        }

        public class IHavaNothingToDoWithItemInstances
        {
            public static int _constructorCallsCounter;

            public IHavaNothingToDoWithItemInstances(int value)
            {
                _constructorCallsCounter = value == unchecked(0xdb40)
                    ? 0
                    : _constructorCallsCounter
                      + 1;
            }
        }

        private sealed class _ICodeBlock_109 : ICodeBlock
        {
            private readonly UniqueFieldIndexTestCase _enclosing;

            public _ICodeBlock_109(UniqueFieldIndexTestCase _enclosing)
            {
                this._enclosing = _enclosing;
            }

            /// <exception cref="System.Exception"></exception>
            public void Run()
            {
                _enclosing.Db().Commit();
            }
        }
    }
}