//-----------------------------------------------------------------------
// <copyright file="UncheckedStatement.cs">
//   MS-PL
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
namespace StyleCop.CSharp
{
    using System;

    /// <summary>
    /// A unchecked-statement.
    /// </summary>
    /// <subcategory>statement</subcategory>
    public sealed class UncheckedStatement : Statement
    {
        #region Private Fields

        /// <summary>
        /// The statement embedded within this unchecked statement, if any.
        /// </summary>
        private BlockStatement embeddedStatement;

        #endregion Private Fields

        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the UncheckedStatement class.
        /// </summary>
        /// <param name="tokens">The list of tokens that form the statement.</param>
        /// <param name="embeddedStatement">The block statement embedded within this unchecked statement, if any.</param>
        internal UncheckedStatement(CsTokenList tokens, BlockStatement embeddedStatement)
            : base(StatementType.Unchecked, tokens)
        {
            Param.AssertNotNull(tokens, "tokens");
            Param.AssertNotNull(embeddedStatement, "embeddedStatement");

            this.embeddedStatement = embeddedStatement;
            this.AddStatement(embeddedStatement);
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Gets the block statement embedded within this unchecked statement, if any.
        /// </summary>
        public BlockStatement EmbeddedStatement
        {
            get
            {
                return this.embeddedStatement;
            }
        }

        #endregion Public Properties
    }
}