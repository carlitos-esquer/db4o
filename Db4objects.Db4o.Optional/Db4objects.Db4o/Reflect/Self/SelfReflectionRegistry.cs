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

namespace Db4objects.Db4o.Reflect.Self
{
    /// <summary>
    ///     Contains the application-specific reflection information (that would
    ///     be generated by a bytecode enhancer), as opposed to the 'generic'
    ///     functionality contained in SelfReflector.
    /// </summary>
    /// <remarks>
    ///     Contains the application-specific reflection information (that would
    ///     be generated by a bytecode enhancer), as opposed to the 'generic'
    ///     functionality contained in SelfReflector.
    /// </remarks>
    public abstract class SelfReflectionRegistry
    {
        private static readonly Type[] Arraytypes =
        {
            typeof (int[]), typeof (long
                []),
            typeof (short[]), typeof (char[]), typeof (byte[]), typeof (bool[]), typeof (float
                []),
            typeof (double[]), typeof (string[])
        };

        private static readonly Type[] Primitives =
        {
            typeof (int), typeof (long
                ),
            typeof (short), typeof (char), typeof (byte), typeof (bool), typeof (float), typeof (
                double),
            typeof (string)
        };

        public virtual bool IsPrimitive(Type clazz)
        {
            for (var idx = 0; idx < Primitives.Length; idx++)
            {
                if (Primitives[idx].Equals(clazz))
                {
                    return true;
                }
            }
            return false;
        }

        public abstract ClassInfo InfoFor(Type clazz);

        public virtual object ArrayFor(Type clazz, int length)
        {
            if (typeof (int).IsAssignableFrom(clazz) || typeof (int).IsAssignableFrom(clazz))
            {
                return new int[length];
            }
            if (typeof (long).IsAssignableFrom(clazz) || typeof (long).IsAssignableFrom(clazz))
            {
                return new long[length];
            }
            if (typeof (short).IsAssignableFrom(clazz) || typeof (short).IsAssignableFrom(clazz
                ))
            {
                return new short[length];
            }
            if (typeof (bool).IsAssignableFrom(clazz) || typeof (bool).IsAssignableFrom(clazz))
            {
                return new bool[length];
            }
            if (typeof (byte).IsAssignableFrom(clazz) || typeof (byte).IsAssignableFrom(clazz))
            {
                return new byte[length];
            }
            if (typeof (char).IsAssignableFrom(clazz) || typeof (char).IsAssignableFrom(clazz))
            {
                return new char[length];
            }
            if (typeof (float).IsAssignableFrom(clazz) || typeof (float).IsAssignableFrom(clazz
                ))
            {
                return new float[length];
            }
            if (typeof (double).IsAssignableFrom(clazz) || typeof (double).IsAssignableFrom(clazz
                ))
            {
                return new double[length];
            }
            if (typeof (string).IsAssignableFrom(clazz))
            {
                return new string[length];
            }
            return null;
        }

        public virtual Type ComponentType(Type clazz)
        {
            for (var i = 0; i < Arraytypes.Length; i++)
            {
                if (Arraytypes[i].Equals(clazz))
                {
                    return Primitives[i];
                }
            }
            return null;
        }

        public virtual int ArrayLength(object array)
        {
            if (array is bool[])
            {
                return ((bool[]) array).Length;
            }
            if (array is byte[])
            {
                return ((byte[]) array).Length;
            }
            if (array is short[])
            {
                return ((short[]) array).Length;
            }
            if (array is char[])
            {
                return ((char[]) array).Length;
            }
            if (array is int[])
            {
                return ((int[]) array).Length;
            }
            if (array is long[])
            {
                return ((long[]) array).Length;
            }
            if (array is float[])
            {
                return ((float[]) array).Length;
            }
            if (array is double[])
            {
                return ((double[]) array).Length;
            }
            return 0;
        }

        public virtual void SetArray(object array, int index, object element)
        {
            if (array is bool[])
            {
                ((bool[]) array)[index] = ((bool) element);
            }
            if (array is byte[])
            {
                ((byte[]) array)[index] = ((byte) element);
            }
            if (array is short[])
            {
                ((short[]) array)[index] = ((short) element);
            }
            if (array is char[])
            {
                ((char[]) array)[index] = ((char) element);
            }
            if (array is int[])
            {
                ((int[]) array)[index] = ((int) element);
            }
            if (array is long[])
            {
                ((long[]) array)[index] = ((long) element);
            }
            if (array is float[])
            {
                ((float[]) array)[index] = ((float) element);
            }
            if (array is double[])
            {
                ((double[]) array)[index] = ((double) element);
            }
        }

        public virtual object GetArray(object array, int index)
        {
            if (array is bool[])
            {
                return ((bool[]) array)[index];
            }
            if (array is byte[])
            {
                return ((byte[]) array)[index];
            }
            if (array is short[])
            {
                return ((short[]) array)[index];
            }
            if (array is char[])
            {
                return ((char[]) array)[index];
            }
            if (array is int[])
            {
                return ((int[]) array)[index];
            }
            if (array is long[])
            {
                return ((long[]) array)[index];
            }
            if (array is float[])
            {
                return ((float[]) array)[index];
            }
            if (array is double[])
            {
                return ((double[]) array)[index];
            }
            return null;
        }

        public virtual int FlattenArray(object array, object[] a_flat)
        {
            if (array is bool[])
            {
                var shaped = (bool[]) array;
                for (var i = 0; i < shaped.Length; i++)
                {
                    a_flat[i] = shaped[i];
                }
                return shaped.Length;
            }
            if (array is byte[])
            {
                var shaped = (byte[]) array;
                for (var i = 0; i < shaped.Length; i++)
                {
                    a_flat[i] = shaped[i];
                }
                return shaped.Length;
            }
            if (array is short[])
            {
                var shaped = (short[]) array;
                for (var i = 0; i < shaped.Length; i++)
                {
                    a_flat[i] = shaped[i];
                }
                return shaped.Length;
            }
            if (array is char[])
            {
                var shaped = (char[]) array;
                for (var i = 0; i < shaped.Length; i++)
                {
                    a_flat[i] = shaped[i];
                }
                return shaped.Length;
            }
            if (array is int[])
            {
                var shaped = (int[]) array;
                for (var i = 0; i < shaped.Length; i++)
                {
                    a_flat[i] = shaped[i];
                }
                return shaped.Length;
            }
            if (array is long[])
            {
                var shaped = (long[]) array;
                for (var i = 0; i < shaped.Length; i++)
                {
                    a_flat[i] = shaped[i];
                }
                return shaped.Length;
            }
            if (array is float[])
            {
                var shaped = (float[]) array;
                for (var i = 0; i < shaped.Length; i++)
                {
                    a_flat[i] = shaped[i];
                }
                return shaped.Length;
            }
            if (array is double[])
            {
                var shaped = (double[]) array;
                for (var i = 0; i < shaped.Length; i++)
                {
                    a_flat[i] = shaped[i];
                }
                return shaped.Length;
            }
            return 0;
        }

        public virtual int ShapeArray(object[] a_flat, object array)
        {
            if (array is bool[])
            {
                var shaped = (bool[]) array;
                for (var i = 0; i < shaped.Length; i++)
                {
                    shaped[i] = ((bool) a_flat[i]);
                }
                return a_flat.Length;
            }
            if (array is byte[])
            {
                var shaped = (byte[]) array;
                for (var i = 0; i < shaped.Length; i++)
                {
                    shaped[i] = ((byte) a_flat[i]);
                }
                return a_flat.Length;
            }
            if (array is short[])
            {
                var shaped = (short[]) array;
                for (var i = 0; i < shaped.Length; i++)
                {
                    shaped[i] = ((short) a_flat[i]);
                }
                return a_flat.Length;
            }
            if (array is char[])
            {
                var shaped = (char[]) array;
                for (var i = 0; i < shaped.Length; i++)
                {
                    shaped[i] = ((char) a_flat[i]);
                }
                return a_flat.Length;
            }
            if (array is int[])
            {
                var shaped = (int[]) array;
                for (var i = 0; i < shaped.Length; i++)
                {
                    shaped[i] = ((int) a_flat[i]);
                }
                return a_flat.Length;
            }
            if (array is long[])
            {
                var shaped = (long[]) array;
                for (var i = 0; i < shaped.Length; i++)
                {
                    shaped[i] = ((long) a_flat[i]);
                }
                return a_flat.Length;
            }
            if (array is float[])
            {
                var shaped = (float[]) array;
                for (var i = 0; i < shaped.Length; i++)
                {
                    shaped[i] = ((float) a_flat[i]);
                }
                return a_flat.Length;
            }
            if (array is double[])
            {
                var shaped = (double[]) array;
                for (var i = 0; i < shaped.Length; i++)
                {
                    shaped[i] = ((double) a_flat[i]);
                }
                return a_flat.Length;
            }
            return 0;
        }
    }
}