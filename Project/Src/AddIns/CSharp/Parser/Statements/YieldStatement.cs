//-----------------------------------------------------------------------
// <copyright file="YieldStatement.cs">
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
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// A yield-statement.
    /// </summary>
    /// <subcategory>statement</subcategory>
    public sealed class YieldStatement : Statement
    {
        #region Private Fields

        /// <summary>
        /// The type of the yield statement.
        /// </summary>
        private Type type;

        /// <summary>
        /// The expression being returned, if any.
        /// </summary>
        private Expression returnValue;

        #endregion Private Fields

        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the YieldStatement class.
        /// </summary>
        /// <param name="tokens">The list of tokens that form the statement.</param>
        /// <param name="type">The type of the yield statement.</param>
        /// <param name="returnValue">The yield return value expression.</param>
        internal YieldStatement(CsTokenList tokens, Type type, Expression returnValue)
            : base(StatementType.Yield, tokens)
        {
            Param.AssertNotNull(tokens, "tokens");
            Param.Ignore(type);
            Param.Ignore(returnValue);

            this.type = type;
            this.returnValue = returnValue;

            if (returnValue != null)
            {
                this.AddExpression(returnValue);
            }
        }

        #endregion Internal Constructors

        #region Public Enums

        /// <summary>
        /// The yield statement type.
        /// </summary>
        /// <subcategory>statement</subcategory>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1034:NestedTypesShouldNotBeVisible",
            Justification = "API has already been published and should not be changed.")]
        public enum Type
        {
            /// <summary>
            /// A yield break statement.
            /// </summary>
            Break,

            /// <summary>
            /// A yield return statement.
            /// </summary>
            Return
        }

        #endregion Public Enums

        #region Public Properties

        /// <summary>
        /// Gets the type of the yield statement.
        /// </summary>
        public Type YieldType
        {
            get
            {
                return this.type;
            }
        }

        /// <summary>
        /// Gets the yield return value expression, if there is one.
        /// </summary>
        public Expression ReturnValue
        {
            get
            {
                return this.returnValue;
            }
        }

        #endregion Public Properties
    }
}