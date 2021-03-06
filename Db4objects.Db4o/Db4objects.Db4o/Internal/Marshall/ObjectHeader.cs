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

namespace Db4objects.Db4o.Internal.Marshall
{
    /// <exclude></exclude>
    public sealed class ObjectHeader
    {
        private readonly ClassMetadata _classMetadata;
        public readonly ObjectHeaderAttributes _headerAttributes;
        public readonly MarshallerFamily _marshallerFamily;
        private int _handlerVersion;

        public ObjectHeader(ObjectContainerBase container, IReadWriteBuffer reader) : this
            (container, null, reader)
        {
        }

        public ObjectHeader(ClassMetadata classMetadata, IReadWriteBuffer
            reader) : this(null, classMetadata, reader)
        {
        }

        private ObjectHeader(ObjectContainerBase container, ClassMetadata
            classMetadata, IReadWriteBuffer reader)
        {
            var classID = reader.ReadInt();
            _marshallerFamily = ReadMarshallerFamily(reader, classID);
            classID = NormalizeID(classID);
            _classMetadata = (classMetadata != null
                ? classMetadata
                : container.ClassMetadataForID
                    (classID));
            // This check has been added to cope with defragment in debug mode: SlotDefragment#setIdentity()
            // will trigger calling this constructor with a source db class metadata and a target db stream,
            // thus _classMetadata==null. There may be a better solution, since this call is just meant to
            // skip the object header.
            _headerAttributes = SlotFormat().ReadHeaderAttributes((ByteArrayBuffer) reader);
        }

        public static ObjectHeader Defrag(DefragmentContextImpl
            context)
        {
            var source = context.SourceBuffer();
            var target = context.TargetBuffer();
            var header = new ObjectHeader
                (context.Services().SystemTrans().Container(), null, source);
            var newID = context.Mapping().StrictMappedID(header.ClassMetadata().GetID());
            var slotFormat = header.SlotFormat();
            slotFormat.WriteObjectClassID(target, newID);
            slotFormat.SkipMarshallerInfo(target);
            slotFormat.ReadHeaderAttributes(target);
            return header;
        }

        private SlotFormat SlotFormat()
        {
            return Marshall.SlotFormat.ForHandlerVersion(HandlerVersion
                ());
        }

        private MarshallerFamily ReadMarshallerFamily(IReadWriteBuffer reader, int classID
            )
        {
            var marshallerAware = MarshallerAware(classID);
            _handlerVersion = 0;
            if (marshallerAware)
            {
                _handlerVersion = reader.ReadByte();
            }
            var marshallerFamily = MarshallerFamily.Version(_handlerVersion);
            return marshallerFamily;
        }

        private bool MarshallerAware(int id)
        {
            return id < 0;
        }

        private int NormalizeID(int id)
        {
            return (id < 0 ? -id : id);
        }

        public ClassMetadata ClassMetadata()
        {
            return _classMetadata;
        }

        public int HandlerVersion()
        {
            return _handlerVersion;
        }

        public static ObjectHeader ScrollBufferToContent
            (LocalObjectContainer container, ByteArrayBuffer buffer)
        {
            return new ObjectHeader(container, buffer);
        }
    }
}