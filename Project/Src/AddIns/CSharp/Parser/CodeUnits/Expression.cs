//-----------------------------------------------------------------------
// <copyright file="ICodeUnit.cs" company="Microsoft">
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
namespace Microsoft.StyleCop.CSharp
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    /// <summary>
    /// A single expression.
    /// </summary>
    /// <subcategory>expression</subcategory>
    public abstract class Expression : CodeUnit
    {
        #region Internal Static Fields

        /// <summary>
        /// An empty array of expressions.
        /// </summary>
        internal static readonly Expression[] EmptyExpressionArray = new Expression[] { };

        #endregion Internal Static Fields

        #region Private Fields

        /// <summary>
        /// Stores a text representation of the expression. This is created on demand.
        /// </summary>
        ////private string text;

        #endregion Private Fields

        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the Expression class.
        /// </summary>
        /// <param name="proxy">Proxy object for the expression.</param>
        /// <param name="type">The type of the expression.</param>
        internal Expression(CodeUnitProxy proxy, ExpressionType type) 
            : base(proxy, (int)type)
        {
            Param.AssertNotNull(proxy, "proxy");
            Param.Ignore(type);
            Debug.Assert(System.Enum.IsDefined(typeof(ExpressionType), this.ExpressionType), "The type is invalid.");
        }

        /*
        /// <summary>
        /// Initializes a new instance of the Expression class.
        /// </summary>
        /// <param name="type">The type of the expression.</param>
        /// <param name="tokens">The list of tokens that form the expression.</param>
        [SuppressMessage(
            "Microsoft.Usage", 
            "CA2214:DoNotCallOverridableMethodsInConstructors", 
            Justification = "The tokens property is virtual but it this is safe as expressions are sealed.")]
        internal Expression(ExpressionType type, MasterList<CodeUnit> children)
            : base(CodeUnitType.Expression)
        {
            Param.Ignore(type);
            Param.AssertNotNull(children, "children");

            this.type = type;

            this.Children = children;
            ////this.Tokens.Trim();

            Debug.Assert(this.Tokens.First != null, "The tokens list should not be empty");
        }
        */

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Gets the type of the expression.
        /// </summary>
        public ExpressionType ExpressionType
        {
            get
            {
                return (ExpressionType)(this.FundamentalType & (int)FundamentalTypeMasks.Expression);
            }
        }

        /////// <summary>
        /////// Gets a text representation of the expression.
        /////// </summary>
        ////public string Text
        ////{
        ////    get
        ////    {
        ////        if (this.text == null)
        ////        {
        ////            this.CreateTextString();
        ////        }

        ////        return this.text;
        ////    }
        ////}

        #endregion Public Properties

        #region Protected Methods

        /// <summary>
        /// Iterates through the tokens in the expression and extracts the arguments, if any.
        /// </summary>
        /// <returns>Returns the arguments.</returns>
        protected IList<Argument> CollectArguments()
        {
            // Find the argument list.
            ArgumentList argumentList = this.FindFirstChild<ArgumentList>();

            if (argumentList != null)
            {
                List<Argument> list = new List<Argument>();

                for (Argument argument = argumentList.FindFirstChild<Argument>(); argument != null; argument = argument.FindNextSibling<Argument>())
                {
                    list.Add(argument);
                }
                
                return list.ToArray();
            }

            return Argument.EmptyArgumentArray;
        }

        #endregion Protected Methods

        #region Private Methods

        /////// <summary>
        /////// Creates a text string based on the child tokens in the attribute.
        /////// </summary>
        ////private void CreateTextString()
        ////{
        ////    var tokenText = new StringBuilder();
        ////    this.AddChildTexts(this, tokenText);

        ////    this.text = tokenText.ToString();
        ////}

        ////private void AddChildTexts(CodeUnit codeUnit, StringBuilder textBuilder)
        ////{
        ////    if (codeUnit.Children != null && codeUnit.Children.Count > 0)
        ////    {
        ////        foreach (CodeUnit item in codeUnit.Children)
        ////        {
        ////            this.AddChildTexts(item, textBuilder);
        ////        }
        ////    }
        ////    else
        ////    {
        ////        // Leaf nodes should always be tokens.
        ////        Token token = (Token)codeUnit;

        ////        // Strip out comments and preprocessor directives.
        ////        if (token.TokenType != TokenType.SingleLineComment &&
        ////            token.TokenType != TokenType.MultiLineComment &&
        ////            token.TokenType != TokenType.PreprocessorDirective)
        ////        {
        ////            textBuilder.Append(token.Text);
        ////        }
        ////    }
        ////}

        #endregion Private Methods
    }
}
