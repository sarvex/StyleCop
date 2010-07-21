//-----------------------------------------------------------------------
// <copyright file="Constructor.cs" company="Microsoft">
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
namespace Microsoft.StyleCop.CSharp.CodeModel
{
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Describes a class constructor..
    /// </summary>
    /// <subcategory>element</subcategory>
    public sealed class Constructor : Element, IParameterContainer
    {
        #region Private Fields

        /// <summary>
        /// The constructor's input parameters.
        /// </summary>
        private CodeUnitProperty<IList<Parameter>> parameters;

        /// <summary>
        /// The fully qualified name of the constructor.
        /// </summary>
        private CodeUnitProperty<string> fullyQualifiedName;

        /// <summary>
        /// The variables on the constructor.
        /// </summary>
        private CodeUnitProperty<IList<IVariable>> variables;

        /// <summary>
        /// The optional constructor initializer.
        /// </summary>
        private CodeUnitProperty<ConstructorInitializerStatement> initializer;

        #endregion Private Fields

        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the Constructor class.
        /// </summary>
        /// <param name="proxy">Proxy object for the constructor.</param>
        /// <param name="name">The name of the constructor.</param>
        /// <param name="attributes">The list of attributes attached to this element.</param>
        /// <param name="unsafeCode">Indicates whether the element resides within a block of unsafe code.</param>
        internal Constructor(CodeUnitProxy proxy, string name, ICollection<Attribute> attributes, bool unsafeCode)
            : base(proxy, ElementType.Constructor, name, attributes, unsafeCode)
        {
            Param.AssertNotNull(proxy, "proxy");
            Param.AssertValidString(name, "name");
            Param.Ignore(attributes);
            Param.Ignore(unsafeCode);
        }

        #endregion Internal Constructors

        #region Public Override Properties

        /// <summary>
        /// Gets the fully qualified name of the element.
        /// </summary>
        public override string FullyQualifiedName
        {
            get
            {
                this.ValidateEditVersion();

                if (!this.fullyQualifiedName.Initialized)
                {
                    this.fullyQualifiedName.Value = CodeParser.AddQualifications(this.Parameters, base.FullyQualifiedName);
                }

                return this.fullyQualifiedName.Value;
            }
        }

        /// <summary>
        /// Gets the variables defined within this element.
        /// </summary>
        public override IList<IVariable> Variables
        {
            get
            {
                this.ValidateEditVersion();

                if (!this.variables.Initialized)
                {
                    this.variables.Value = Method.GatherVariablesForElementWithParametersAndChildStatements(this, this.Parameters);
                }

                return this.variables.Value;
            }
        }

        #endregion Public Override Properties

        #region Public Properties

        /// <summary>
        /// Gets the class initializer for the constructor, if any.
        /// </summary>
        public ConstructorInitializerStatement Initializer
        {
            get
            {
                this.ValidateEditVersion();

                if (!this.initializer.Initialized)
                {
                    this.initializer.Value = this.FindFirstChild<ConstructorInitializerStatement>();
                }

                return this.initializer.Value;
            }
        }

        /// <summary>
        /// Gets the list of input parameters in the constructor's declaration.
        /// </summary>
        public IList<Parameter> Parameters
        {
            get
            {
                this.ValidateEditVersion();

                if (!this.parameters.Initialized)
                {
                    this.parameters.Value = this.CollectFormalParameters(this.FirstDeclarationToken, TokenType.CloseParenthesis);
                }

                return this.parameters.Value;
            }
        }

        #endregion Public Properties

        #region Protected Override Properties

        /// <summary>
        /// Gets the collection of modifiers allowed on this element.
        /// </summary>
        protected override IEnumerable<string> AllowedModifiers
        {
            get
            {
                return CodeParser.ConstructorModifiers;
            }
        }

        /// <summary>
        /// Gets the default access modifier for this element.
        /// </summary>
        protected override AccessModifierType DefaultAccessModifierType
        {
            get
            {
                // Static constructors are treated as private and handled as a special case for ordering.
                if (this.ContainsModifier(TokenType.Static))
                {
                    return AccessModifierType.Private;
                }

                return base.DefaultAccessModifierType;
            }
        }

        #endregion Protected Override Properties

        #region Protected Override Methods

        /// <summary>
        /// Gets the name of the element.
        /// </summary>
        /// <returns>The name of the element.</returns>
        protected override string GetElementName()
        {
            for (Token token = this.FindFirstChild<Token>(); token != null; token = token.FindNextSibling<Token>())
            {
                if (token.Is(TokenType.Literal) || token.Is(TokenType.Type))
                {
                    return token.Text;
                }
            }

            throw new SyntaxException(this.Document, this.LineNumber);
        }

        /// <summary>
        /// Resets the contents of the class.
        /// </summary>
        protected override void Reset()
        {
            base.Reset();

            this.parameters.Reset();
            this.fullyQualifiedName.Reset();
            this.variables.Reset();
            this.initializer.Reset();
        }

        #endregion Protected Override Methods
    }
}
