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

namespace Db4objects.Db4o.Internal.Fileheader
{
    /// <exclude></exclude>
    public class FileHeaderVariablePart3 : FileHeaderVariablePart2
    {
        public FileHeaderVariablePart3(LocalObjectContainer container) : base(container)
        {
        }

        public override int OwnLength()
        {
            return base.OwnLength() + Const4.IntLength*2;
        }

        protected override void ReadBuffer(ByteArrayBuffer buffer, bool versionsAreConsistent
            )
        {
            base.ReadBuffer(buffer, versionsAreConsistent);
            var systemData = SystemData();
            systemData.IdToTimestampIndexId(buffer.ReadInt());
            systemData.TimestampToIdIndexId(buffer.ReadInt());
        }

        protected override void WriteBuffer(ByteArrayBuffer buffer, bool shuttingDown)
        {
            base.WriteBuffer(buffer, shuttingDown);
            var systemData = SystemData();
            buffer.WriteInt(systemData.IdToTimestampIndexId());
            buffer.WriteInt(systemData.TimestampToIdIndexId());
        }
    }
}