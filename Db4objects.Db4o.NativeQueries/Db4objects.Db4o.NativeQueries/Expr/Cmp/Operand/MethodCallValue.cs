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
using Db4objects.Db4o.Instrumentation.Api;

namespace Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand
{
    public class MethodCallValue : ComparisonOperandDescendant
    {
        private readonly IComparisonOperand[] _args;
        private readonly CallingConvention _callingConvention;
        private readonly IMethodRef _method;

        public MethodCallValue(IMethodRef method, CallingConvention
            callingConvention, IComparisonOperandAnchor parent, IComparisonOperand[] args) :
                base(parent)
        {
            _method = method;
            _args = args;
            _callingConvention = callingConvention;
        }

        public virtual IComparisonOperand[] Args
        {
            get { return _args; }
        }

        public virtual IMethodRef Method
        {
            get { return _method; }
        }

        public virtual CallingConvention CallingConvention
        {
            get { return _callingConvention; }
        }

        public override ITypeRef Type
        {
            get { return _method.ReturnType; }
        }

        public override void Accept(IComparisonOperandVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool Equals(object obj)
        {
            if (!base.Equals(obj))
            {
                return false;
            }
            var casted = (MethodCallValue
                ) obj;
            return _method.Equals(casted._method) && _callingConvention == casted._callingConvention;
        }

        public override int GetHashCode()
        {
            var hc = base.GetHashCode();
            hc *= 29 + _method.GetHashCode();
            hc *= 29 + _args.GetHashCode();
            hc *= 29 + _callingConvention.GetHashCode();
            return hc;
        }

        public override string ToString()
        {
            return base.ToString() + "." + _method.Name + Iterators.Join(Iterators.Iterate(_args
                ), "(", ")", ", ");
        }
    }
}