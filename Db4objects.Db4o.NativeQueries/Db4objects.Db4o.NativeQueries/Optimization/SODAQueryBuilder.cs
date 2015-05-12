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
using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Instrumentation.Api;
using Db4objects.Db4o.NativeQueries.Expr;
using Db4objects.Db4o.NativeQueries.Expr.Cmp;
using Db4objects.Db4o.NativeQueries.Expr.Cmp.Operand;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.NativeQueries.Optimization
{
    public class SODAQueryBuilder
    {
        public virtual void OptimizeQuery(IExpression expr, IQuery query, object predicate
            , INativeClassFactory classSource, IReferenceResolver referenceResolver)
        {
            expr.Accept(new SODAQueryVisitor(query, predicate, classSource,
                referenceResolver));
        }

        private class SODAQueryVisitor : IExpressionVisitor
        {
            private readonly INativeClassFactory _classSource;
            private readonly object _predicate;
            private readonly IQuery _query;
            private readonly IReferenceResolver _referenceResolver;
            private IConstraint _constraint;

            internal SODAQueryVisitor(IQuery query, object predicate, INativeClassFactory classSource
                , IReferenceResolver referenceResolver)
            {
                _query = query;
                _predicate = predicate;
                _classSource = classSource;
                _referenceResolver = referenceResolver;
            }

            public virtual void Visit(AndExpression expression)
            {
                expression.Left().Accept(this);
                var left = _constraint;
                expression.Right().Accept(this);
                left.And(_constraint);
                _constraint = left;
            }

            public virtual void Visit(BoolConstExpression expression)
            {
            }

            public virtual void Visit(OrExpression expression)
            {
                expression.Left().Accept(this);
                var left = _constraint;
                expression.Right().Accept(this);
                left.Or(_constraint);
                _constraint = left;
            }

            public virtual void Visit(ComparisonExpression expression)
            {
                var subQuery = Descend(expression.Left());
                var visitor = new ComparisonQueryGeneratingVisitor(_predicate
                    , _classSource, _referenceResolver);
                expression.Right().Accept(visitor);
                _constraint = subQuery.Constrain(visitor.Value());
                var op = expression.Op();
                if (op.Equals(ComparisonOperator.ValueEquality))
                {
                    return;
                }
                if (op.Equals(ComparisonOperator.ReferenceEquality))
                {
                    _constraint.Identity();
                    return;
                }
                if (op.Equals(ComparisonOperator.Greater))
                {
                    _constraint.Greater();
                    return;
                }
                if (op.Equals(ComparisonOperator.Smaller))
                {
                    _constraint.Smaller();
                    return;
                }
                if (op.Equals(ComparisonOperator.Contains))
                {
                    _constraint.Contains();
                    return;
                }
                if (op.Equals(ComparisonOperator.StartsWith))
                {
                    _constraint.StartsWith(true);
                    return;
                }
                if (op.Equals(ComparisonOperator.EndsWith))
                {
                    _constraint.EndsWith(true);
                    return;
                }
                throw new Exception("Can't handle constraint: " + op);
            }

            public virtual void Visit(NotExpression expression)
            {
                expression.Expr().Accept(this);
                _constraint.Not();
            }

            private IQuery Descend(FieldValue left)
            {
                var subQuery = _query;
                var fieldNameIterator = FieldNames(left);
                while (fieldNameIterator.MoveNext())
                {
                    subQuery = subQuery.Descend((string) fieldNameIterator.Current);
                }
                return subQuery;
            }

            private IEnumerator FieldNames(FieldValue fieldValue)
            {
                var coll = new Collection4();
                IComparisonOperand curOp = fieldValue;
                while (curOp is FieldValue)
                {
                    var curField = (FieldValue) curOp;
                    coll.Prepend(curField.FieldName());
                    curOp = curField.Parent();
                }
                return coll.GetEnumerator();
            }
        }
    }
}