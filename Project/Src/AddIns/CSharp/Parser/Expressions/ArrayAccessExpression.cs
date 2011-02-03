//-----------------------------------------------------------------------
// <copyright file="ArrayAccessExpression.cs">
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
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// An expression representing an array access operation.
    /// </summary>
    /// <subcategory>expression</subcategory>
    public sealed class ArrayAccessExpression : Expression
    {
        #region Private Fields

        /// <summary>
        /// Represents the item being indexed.
        /// </summary>
        private Expression array;

        /// <summary>
        /// The array access arguments.
        /// </summary>
        private ICollection<Argument> arguments;

        #endregion Private Fields

        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the ArrayAccessExpression class.
        /// </summary>
        /// <param name="tokens">The list of tokens that form the expression.</param>
        /// <param name="array">Represents the item being indexed.</param>
        /// <param name="arguments">The array access arguments.</param>
        internal ArrayAccessExpression(CsTokenList tokens, Expression array, ICollection<Argument> arguments)
            : base(ExpressionType.ArrayAccess, tokens)
        {
            Param.AssertNotNull(tokens, "tokens");
            Param.AssertNotNull(array, "array");
            Param.AssertNotNull(arguments, "arguments");

            this.array = array;
            this.arguments = arguments;
            Debug.Assert(arguments.IsReadOnly, "The arguments collection should be read-only.");

            this.AddExpression(array);

            foreach (Argument argument in arguments)
            {
                this.AddExpression(argument.Expression);
            }
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Gets the item being indexed.
        /// </summary>
        public Expression Array
        {
            get
            {
                return this.array;
            }
        }

        /// <summary>
        /// Gets the array access arguments.
        /// </summary>
        public ICollection<Argument> Arguments
        {
            get
            {
                return this.arguments;
            }
        }

        #endregion Public Properties
    }
}
