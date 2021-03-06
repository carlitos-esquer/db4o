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
using System.Collections;
using Db4oUnit.Fixtures;

namespace Db4objects.Db4o.Tests.Jre5.Collections.Typehandler
{
    public sealed class ListTypeHandlerTestVariables
    {
        public static readonly FixtureVariable ListImplementation = new FixtureVariable("list"
            );

        public static readonly FixtureVariable ElementsSpec = new FixtureVariable("elements"
            );

        public static readonly FixtureVariable ListTypehander = new FixtureVariable("typehandler"
            );

        public static readonly IFixtureProvider ListFixtureProvider = new SimpleFixtureProvider
            (ListImplementation, new object[]
            {
                new ArrayListItemFactory
                    (),
                new LinkedListItemFactory(), new ListItemFactory
                    (),
                new NamedArrayListItemFactory()
            });

        public static readonly IFixtureProvider TypehandlerFixtureProvider = new SimpleFixtureProvider
            (ListTypehander, new object[] {null});

        public static readonly ListTypeHandlerTestElementsSpec StringElementsSpec = new ListTypeHandlerTestElementsSpec
            (new object[] {"zero", "one"}, "two", "zzz");

        public static readonly ListTypeHandlerTestElementsSpec IntElementsSpec = new ListTypeHandlerTestElementsSpec
            (new object[] {0, 1}, 2, int.MaxValue);

        public static readonly ListTypeHandlerTestElementsSpec ObjectElementsSpec = new ListTypeHandlerTestElementsSpec
            (new object[]
            {
                new ReferenceElement(0), new ReferenceElement
                    (1)
            }, new ReferenceElement(2), null);

        private ListTypeHandlerTestVariables()
        {
        }

        public class ReferenceElement
        {
            public int _id;

            public ReferenceElement(int id)
            {
                _id = id;
            }

            public override bool Equals(object obj)
            {
                if (this == obj)
                {
                    return true;
                }
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }
                var other = (ReferenceElement
                    ) obj;
                return _id == other._id;
            }

            public override int GetHashCode()
            {
                return _id;
            }

            public override string ToString()
            {
                return "FCE#" + _id;
            }
        }

        private class ArrayListItemFactory : AbstractListItemFactory, ILabeled
        {
            public virtual string Label()
            {
                return "ArrayList";
            }

            public override object NewItem()
            {
                return new Item();
            }

            public override Type ItemClass()
            {
                return typeof (Item);
            }

            public override Type ContainerClass()
            {
                return typeof (ArrayList);
            }

            private class Item
            {
                public ArrayList _list = new ArrayList();
            }
        }

        private class LinkedListItemFactory : AbstractListItemFactory, ILabeled
        {
            public virtual string Label()
            {
                return "LinkedList";
            }

            public override object NewItem()
            {
                return new Item();
            }

            public override Type ItemClass()
            {
                return typeof (Item);
            }

            public override Type ContainerClass()
            {
                return typeof (ArrayList);
            }

            private class Item
            {
                public ArrayList _list = new ArrayList();
            }
        }

        private class ListItemFactory : AbstractListItemFactory, ILabeled
        {
            public virtual string Label()
            {
                return "[Linked]List";
            }

            public override object NewItem()
            {
                return new Item();
            }

            public override Type ItemClass()
            {
                return typeof (Item);
            }

            public override Type ContainerClass()
            {
                return typeof (ArrayList);
            }

            private class Item
            {
                public IList _list = new ArrayList();
            }
        }

        private class NamedArrayListItemFactory : AbstractListItemFactory, ILabeled
        {
            public virtual string Label()
            {
                return "NamedArrayList";
            }

            public override object NewItem()
            {
                return new Item();
            }

            public override Type ItemClass()
            {
                return typeof (Item);
            }

            public override Type ContainerClass()
            {
                return typeof (NamedArrayList);
            }

            private class Item
            {
                public ArrayList _list = new NamedArrayList();
            }
        }
    }
}