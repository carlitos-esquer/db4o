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

using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.CS.Internal.Messages
{
    public sealed class MPrefetchIDs : MsgD, IMessageWithResponse
    {
        public Msg ReplyFromServer()
        {
            var prefetchIDCount = ReadInt();
            var reply = IdList.GetWriterForLength(Transaction(), Const4.IntLength*prefetchIDCount
                );
            lock (ContainerLock())
            {
                var idSystem = Transaction().IdSystem();
                for (var i = 0; i < prefetchIDCount; i++)
                {
                    reply.WriteInt(idSystem.PrefetchID());
                }
            }
            return reply;
        }
    }
}