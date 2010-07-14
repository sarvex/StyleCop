//-----------------------------------------------------------------------
// <copyright file="UncheckedExpression.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.
// </copyright>
// <license>
//   This source code is subject to terms and conditions of the Microsoft 
//   Public License. A copy of the license can be found in the License.html 
//   file at the root of this distribution. If you cannot locate the  
//   Microsoft Public License, please send an email to dlr@microsoft.com. 
//   By using this source code in any fashion, you are agreeing to be bound 
//   by the terms of the Microsoft Public License. You must not remove this 
//   notice, or any other, from this software.
// </license>
//-----------------------------------------------------------------------
namespace Microsoft.StyleCop.CSharp_old
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// An expression representing an unchecked operation.
    /// </summary>
    /// <subcategory>expression</subcategory>
    public sealed class UncheckedExpression : Expression
    {
        #region Private Fields

        /// <summary>
        /// The expression wrapped within the unchecked expression.
        /// </summary>
        private Expression internalExpression;

        #endregion Private Fields

        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the UncheckedExpression class.
        /// </summary>
        /// <param name="tokens">The list of tokens that form the expression.</param>
        /// <param name="internalExpression">The expression wrapped within the unchecked expression.</param>
        internal UncheckedExpression(CsTokenList tokens, Expression internalExpression)
            : base(ExpressionType.Unchecked, tokens)
        {
            Param.AssertNotNull(tokens, "tokens");
            Param.AssertNotNull(internalExpression, "internalExpression");

            this.internalExpression = internalExpression;
            this.AddExpression(internalExpression);
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Gets the expression wrapped within this unchecked expression.
        /// </summary>
        public Expression InternalExpression
        {
            get
            {
                return this.internalExpression;
            }
        }

        #endregion Public Properties
    }
}
