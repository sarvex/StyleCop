//-----------------------------------------------------------------------
// <copyright file="BreakStatement.cs" company="Microsoft">
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

    /// <summary>
    /// A break-statement.
    /// </summary>
    /// <subcategory>statement</subcategory>
    public sealed class BreakStatement : Statement
    {
        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the BreakStatement class.
        /// </summary>
        /// <param name="tokens">The list of tokens that form the statement.</param>
        internal BreakStatement(CsTokenList tokens)
            : base(StatementType.Break, tokens)
        {
            Param.Ignore(tokens);
        }

        #endregion Internal Constructors
    }
}
