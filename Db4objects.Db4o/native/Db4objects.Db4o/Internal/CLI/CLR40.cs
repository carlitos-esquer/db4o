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

using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace Db4objects.Db4o.Internal.CLI
{
    internal class CLR40 : ICLIFacade
    {
        [SecuritySafeCritical]
        public void Flush(FileStream stream)
        {
            stream.Flush(true);
            FlushFileBuffers(stream.SafeFileHandle); // We still need to call FlushFileBuffer due to bug 
            // http://connect.microsoft.com/VisualStudio/feedback/details/634385/filestream-flush-flushtodisk-true-call-does-not-flush-the-buffers-to-disk#details
        }

        [SecuritySafeCritical]
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int FlushFileBuffers(SafeFileHandle fileHandle);
    }
}