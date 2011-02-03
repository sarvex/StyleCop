//-----------------------------------------------------------------------
// <copyright file="FinallyStatement.cs">
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
    /// A finally-statement.
    /// </summary>
    /// <subcategory>statement</subcategory>
    public sealed class FinallyStatement : Statement
    {
        #region Private Fields

        /// <summary>
        /// The try-statement that this finally-statement is attached to.
        /// </summary>
        private TryStatement tryStatement;

        /// <summary>
        /// The statement embedded within the catch-statement.
        /// </summary>
        private BlockStatement embeddedStatement;

        #endregion Private Fields

        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the FinallyStatement class.
        /// </summary>
        /// <param name="tokens">The list of tokens that form the statement.</param>
        /// <param name="tryStatement">The try-statement that this finally-statement is embedded to.</param>
        /// <param name="embeddedStatement">The statement embedded within the finally-statement.</param>
        internal FinallyStatement(CsTokenList tokens, TryStatement tryStatement, BlockStatement embeddedStatement)
            : base(StatementType.Finally, tokens)
        {
            Param.AssertNotNull(tokens, "tokens");
            Param.AssertNotNull(tryStatement, "tryStatement");
            Param.AssertNotNull(embeddedStatement, "embeddedStatement");

            this.tryStatement = tryStatement;
            this.embeddedStatement = embeddedStatement;

            this.AddStatement(embeddedStatement);
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Gets the try-statement that this finally-statement is attached to.
        /// </summary>
        public TryStatement TryStatement
        {
            get
            {
                return this.tryStatement;
            }
        }

        /// <summary>
        /// Gets the statement embedded within the finally-statement.
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
