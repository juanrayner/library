﻿#region License
// Copyright (c) Amos Voron. All rights reserved.
// Licensed under the Apache 2.0 License. See LICENSE in the project root for license information.
#endregion

namespace QueryTalk.Wall
{
    /// <summary>
    /// This class is not intended for public use.
    /// </summary>   
    public sealed class CaseElseChainer : Chainer, IQuery,
        IEndCase
    {
        internal override string Method
        {
            get
            {
                return Text.Method.CaseElse;
            }
        }

        internal CaseElseChainer(Chainer prev, ScalarArgument argument1, ValueScalarArgument argument2, bool equality)
            : this(prev, Expression.EqualitySimplifier(argument1, argument2, equality))
        {
            CheckNullAndThrow(Arg(() => argument1, argument1));
            TryTake(argument1);
            TryTake(argument2); 
        }

        internal CaseElseChainer(Chainer prev, ScalarArgument expression) 
            : base(prev)
        {
            expression = expression ?? Designer.Null;

            Build = (buildContext, buildArgs) =>
            {
                var sql = Text.GenerateSql(20)
                    .NewLineIndent(Text.Else).S()
                    .Append(expression.Build(buildContext, buildArgs))
                    .ToString();

                buildContext.TryTakeException(expression.Exception);
                TryThrow(buildContext);

                return sql;
            };
        }
    }
}
