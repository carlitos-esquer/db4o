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
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;

namespace Db4oUnit.Extensions.Dbmock
{
    public partial class MockServer : IObjectServer
    {
        public virtual bool Close()
        {
            throw new NotImplementedException();
        }

        public virtual IExtObjectServer Ext()
        {
            throw new NotImplementedException();
        }

        public virtual void GrantAccess(string userName, string password)
        {
            throw new NotImplementedException();
        }

        public virtual IObjectContainer OpenClient()
        {
            throw new NotImplementedException();
        }
    }
}