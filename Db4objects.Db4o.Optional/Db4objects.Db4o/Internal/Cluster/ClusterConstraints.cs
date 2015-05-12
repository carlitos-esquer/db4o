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

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Internal.Cluster
{
    /// <exclude></exclude>
    public class ClusterConstraints : ClusterConstraint, IConstraints
    {
        public ClusterConstraints(Db4o.Cluster.Cluster cluster, IConstraint[]
            constraints) : base(cluster, constraints)
        {
        }

        public virtual IConstraint[] ToArray()
        {
            lock (_cluster)
            {
                var all = new Collection4();
                for (var i = 0; i < _constraints.Length; i++)
                {
                    var c = (ClusterConstraint) _constraints[i];
                    for (var j = 0; j < c._constraints.Length; j++)
                    {
                        all.Add(c._constraints[j]);
                    }
                }
                var res = new IConstraint[all.Size()];
                all.ToArray(res);
                return res;
            }
        }
    }
}