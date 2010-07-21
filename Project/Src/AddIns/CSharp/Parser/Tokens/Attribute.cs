//-----------------------------------------------------------------------
// <copyright file="Attribute.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    /// <summary>
    /// Describes an attribute declared on an element.
    /// </summary>
    /// <subcategory>token</subcategory>
    [SuppressMessage("Microsoft.Naming", "CA1711:IdentifiersShouldNotHaveIncorrectSuffix", Justification = "The class describes a C# attribute.")]
    public sealed class Attribute : CsToken, ITokenContainer
    {
        #region Private Fields

        /// <summary>
        /// The list of child tokens within this attribute.
        /// </summary>
        private MasterList<CsToken> childTokens;

        /// <summary>
        /// The element that this attribute is attached to.
        /// </summary>
        private CsElement element;

        /// <summary>
        /// Gets the list of attribute expressions within this attribute.
        /// </summary>
        private CodeUnitCollection<AttributeExpression> attributeExpressions;

        #endregion Private Fields

        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the Attribute class.
        /// </summary>
        /// <param name="childTokens">The list of child tokens for the attribute.</param>
        /// <param name="location">The location of the attribute.</param>
        /// <param name="parent">The parent of the attribute.</param>
        /// <param name="attributeExpressions">The list of attribute expressions within this attribute.</param>
        /// <param name="generated">Indicates whether the attribute resides within a block of generated code.</param>
        internal Attribute(
            MasterList<CsToken> childTokens, 
            CodeLocation location, 
            Reference<ICodePart> parent,
            IEnumerable<AttributeExpression> attributeExpressions, 
            bool generated)
            : base(CsTokenType.Attribute, CsTokenClass.Attribute, location, parent, generated)
        {
            Param.AssertNotNull(childTokens, "childTokens");
            Param.AssertGreaterThanOrEqualTo(childTokens.Count, 2, "childTokens");
            Param.AssertNotNull(location, "location");
            Param.AssertNotNull(parent, "parent");
            Param.AssertNotNull(attributeExpressions, "attributeExpressions");
            Param.Ignore(generated);

            this.childTokens = childTokens;

            this.attributeExpressions = new CodeUnitCollection<AttributeExpression>(this);
            this.attributeExpressions.AddRange(attributeExpressions);
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Gets the list of child tokens within this attribute.
        /// </summary>
        public MasterList<CsToken> ChildTokens
        {
            get
            {
                return this.childTokens.AsReadOnly;
            }
        }

        /// <summary>
        /// Gets the list of attribute expressions within this attribute.
        /// </summary>
        public ICollection<AttributeExpression> AttributeExpressions
        {
            get
            {
                return this.attributeExpressions;
            }
        }

        /// <summary>
        /// Gets the element that this attribute is attached to, if any.
        /// </summary>
        public CsElement Element
        {
            get
            {
                return this.element;
            }

            internal set
            {
                this.element = value;
            }
        }

        #endregion Public Properties

        #region ITokenContainer Interface Properties

        /// <summary>
        /// Gets the list of child tokens contained within this object.
        /// </summary>
        ICollection<CsToken> ITokenContainer.Tokens
        {
            get
            {
                return this.childTokens.AsReadOnly;
            }
        }

        #endregion ITokenContainer Interface Properties

        #region Protected Override Methods

        /// <summary>
        /// Creates a text string based on the child tokens in the attribute.
        /// </summary>
        protected override void CreateTextString()
        {
            StringBuilder text = new StringBuilder();
            foreach (CsToken token in this.childTokens)
            {
                if (token.CsTokenType != CsTokenType.SingleLineComment &&
                    token.CsTokenType != CsTokenType.MultiLineComment &&
                    token.CsTokenType != CsTokenType.PreprocessorDirective)
                {
                    text.Append(token.Text);
                }
            }

            this.Text = text.ToString();
        }

        #endregion Protected Override Methods
    }
}
