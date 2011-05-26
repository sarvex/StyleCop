//-----------------------------------------------------------------------
// <copyright file="ConstructorConstraint.cs">
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
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// A token describing a constructor constraint.
    /// </summary>
    /// <subcategory>token</subcategory>
    public sealed class ConstructorConstraint : CsToken, ITokenContainer
    {
        #region Private Fields

        /// <summary>
        /// The list of child tokens within this token.
        /// </summary>
        private MasterList<CsToken> childTokens;

        #endregion Private Fields

        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the ConstructorConstraint class.
        /// </summary>
        /// <param name="childTokens">The list of child tokens that form the token.</param>
        /// <param name="location">The location of the token in the code.</param>
        /// <param name="parent">The parent of the token.</param>
        /// <param name="generated">True if the token is inside of a block of generated code.</param>
        internal ConstructorConstraint(MasterList<CsToken> childTokens, CodeLocation location, Reference<ICodePart> parent, bool generated)
            : base(CsTokenType.Other, CsTokenClass.ConstructorConstraint, location, parent, generated)
        {
            Param.AssertNotNull(childTokens, "childTokens");
            Param.AssertNotNull(location, "location");
            Param.AssertNotNull(parent, "parent");
            Param.Ignore(generated);

            this.childTokens = childTokens;
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Gets the list of child tokens within this token.
        /// </summary>
        public MasterList<CsToken> ChildTokens
        {
            get
            {
                return this.childTokens.AsReadOnly;
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
        /// Creates a text string based on the child tokens in the token.
        /// </summary>
        protected override void CreateTextString()
        {
            StringBuilder text = new StringBuilder();
            foreach (CsToken token in this.childTokens)
            {
                // Strip out comments.
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