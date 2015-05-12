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

#if !CF && !SILVERLIGHT

using Db4objects.Db4o.CS.Foundation;

namespace Db4objects.Db4o.CS.Monitoring
{
    public class MonitoredClientSocket4 : MonitoredSocket4Base
    {
        private NetworkingCounters _counters;

        public MonitoredClientSocket4(ISocket4 socket) : base(socket)
        {
        }

        public override void Close()
        {
            base.Close();
            _counters.Close();
        }

        protected override NetworkingCounters Counters()
        {
            if (null == _counters)
            {
                _counters = new NetworkingCounters();
            }
            return _counters;
        }
    }
}

#endif