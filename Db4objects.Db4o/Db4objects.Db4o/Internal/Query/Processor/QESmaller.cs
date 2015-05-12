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

namespace Db4objects.Db4o.Internal.Query.Processor
{
    /// <exclude></exclude>
    public class QESmaller : QEAbstract
    {
        internal override bool Evaluate(QConObject constraint, QCandidate candidate, object
            obj)
        {
            if (obj == null)
            {
                return false;
            }
            var preparedComparison = constraint.PrepareComparison(candidate);
            if (preparedComparison is PreparedArrayContainsComparison)
            {
                return ((PreparedArrayContainsComparison) preparedComparison).IsGreaterThan(obj);
            }
            return preparedComparison.CompareTo(obj) > 0;
        }

        public override void IndexBitMap(bool[] bits)
        {
            bits[Smaller] = true;
        }
    }
}