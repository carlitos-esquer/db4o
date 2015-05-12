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
    public class ClassInfo
    {
        private readonly FieldInfo[] _fieldInfo;
        private readonly bool _isAbstract;
        private readonly Type _superClass;

        public ClassInfo(bool isAbstract, Type superClass, FieldInfo
            [] fieldInfo)
        {
            _isAbstract = isAbstract;
            _superClass = superClass;
            _fieldInfo = fieldInfo;
        }

        public virtual bool IsAbstract()
        {
            return _isAbstract;
        }

        public virtual Type SuperClass()
        {
            return _superClass;
        }

        public virtual FieldInfo[] FieldInfo()
        {
            return _fieldInfo;
        }

        public virtual FieldInfo FieldByName(string name)
        {
            if (!(_fieldInfo.Length == 0))
            {
                for (var i = 0; i < _fieldInfo.Length; i++)
                {
                    if (_fieldInfo[i].Name().Equals(name))
                    {
                        return _fieldInfo[i];
                    }
                }
            }
            return null;
        }
    }
}