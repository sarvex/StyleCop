//-----------------------------------------------------------------------
// <copyright file="Method.cs" company="Microsoft">
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
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Describes a method element.
    /// </summary>
    /// <subcategory>element</subcategory>
    public sealed class Method : Element, IParameterContainer, ITypeConstraintContainer
    {
        #region Private Fields

        /// <summary>
        /// The method's return type.
        /// </summary>
        private CodeUnitProperty<TypeToken> returnType;

        /// <summary>
        /// The list if type constraints on the item, if any.
        /// </summary>
        private CodeUnitProperty<ICollection<TypeParameterConstraintClause>> typeConstraints;

        /// <summary>
        /// Indicates whether this is an extension method. 
        /// </summary>
        private CodeUnitProperty<bool> extensionMethod;

        /// <summary>
        /// The parameters within the methods formal parameter list.
        /// </summary>
        private CodeUnitProperty<IList<Parameter>> parameters;

        /// <summary>
        /// The fully qualified name of the method.
        /// </summary>
        private CodeUnitProperty<string> fullyQualifiedName;

        /// <summary>
        /// The variables on the method.
        /// </summary>
        private CodeUnitProperty<IList<IVariable>> variables;

        #endregion Private Fields

        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the Method class.
        /// </summary>
        /// <param name="proxy">Proxy object for the method.</param>
        /// <param name="name">The name of the method.</param>
        /// <param name="attributes">The list of attributes attached to this element.</param>
        /// <param name="returnType">The method's return type.</param>
        /// <param name="typeConstraints">The list of type constraints on the element.</param>
        /// <param name="unsafeCode">Indicates whether the element resides within a block of unsafe code.</param>
        internal Method(CodeUnitProxy proxy, string name, ICollection<Attribute> attributes, TypeToken returnType, ICollection<TypeParameterConstraintClause> typeConstraints, bool unsafeCode)
            : base(proxy, ElementType.Method, name, attributes, unsafeCode)
        {
            Param.AssertNotNull(proxy, "proxy");
            Param.AssertValidString(name, "name");
            Param.Ignore(attributes);
            Param.Ignore(returnType);
            Param.Ignore(typeConstraints);
            Param.Ignore(unsafeCode);

            Debug.Assert(
                returnType != null || this.ContainsModifier(TokenType.Explicit, TokenType.Implicit),
                "A method's return type can only be null in an explicit or implicit operator overload method.");

            this.returnType.Value = returnType;

            this.typeConstraints.Value = typeConstraints ?? TypeParameterConstraintClause.EmptyTypeParameterConstraintClause;
            Debug.Assert(typeConstraints == null || typeConstraints.IsReadOnly, "Must be a read-only collection.");
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
                    this.variables.Value = GatherVariablesForElementWithParametersAndChildStatements(this, this.Parameters);
                    Debug.Assert(this.variables.Value != null && this.variables.Value.IsReadOnly, "Invalid");
                }

                return this.variables.Value;
            }
        }

        #endregion Public Override Properties

        #region Public Properties

        /// <summary>
        /// Gets the method's return type.
        /// </summary>
        public TypeToken ReturnType
        {
            get
            {
                this.ValidateEditVersion();

                if (!this.returnType.Initialized)
                {
                    this.returnType.Value = null;

                    if (!this.ContainsModifier(TokenType.Explicit, TokenType.Implicit))
                    {
                        this.returnType.Value = this.FindFirstChild<TypeToken>();
                    }
                }

                return this.returnType.Value;
            }
        }

        /// <summary>
        /// Gets the list of input parameters in the method's declaration.
        /// </summary>
        public IList<Parameter> Parameters
        {
            get
            {
                this.ValidateEditVersion();

                if (!this.parameters.Initialized)
                {
                    this.parameters.Value = this.CollectFormalParameters(this.FirstDeclarationToken, TokenType.CloseParenthesis);
                    Debug.Assert(this.parameters.Value != null, "Must return a non-null value.");
                }

                return this.parameters.Value;
            }
        }

        /// <summary>
        /// Gets the list of type constraints on the method, if any.
        /// </summary>
        public ICollection<TypeParameterConstraintClause> TypeConstraints
        {
            get
            {
                this.ValidateEditVersion();

                if (!this.typeConstraints.Initialized)
                {
                    this.typeConstraints.Value = new List<TypeParameterConstraintClause>(this.GetChildren<TypeParameterConstraintClause>()).AsReadOnly();
                }

                return this.typeConstraints.Value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this is an extension method. 
        /// </summary>
        public bool IsExtensionMethod
        {
            get
            {
                this.ValidateEditVersion();

                if (!this.extensionMethod.Initialized)
                {
                    this.extensionMethod.Value = false;

                    // An extension method must be static.
                    if (this.Parameters.Count > 0 && this.ContainsModifier(TokenType.Static))
                    {
                        // Look at this first parameter to see if it contains the 'this' modifier.
                        if ((this.Parameters[0].Modifiers & ParameterModifiers.This) != 0)
                        {
                            this.extensionMethod.Value = true;
                        }
                    }
                }

                return this.extensionMethod.Value;
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
                return CodeParser.MethodModifiers;
            }
        }

        /// <summary>
        /// Gets the default access modifier for this type.
        /// </summary>
        protected override AccessModifierType DefaultAccessModifierType
        {
            get
            {
                string name = this.Name;

                // If this is an explicit interface member implementation and our access modifier
                // is currently set to private because we don't have one, then it should be public instead.
                if (name.IndexOf(".", StringComparison.Ordinal) > -1 && !name.StartsWith("this.", StringComparison.Ordinal))
                {
                    return AccessModifierType.Public;
                }

                return base.DefaultAccessModifierType;
            }
        }

        #endregion Protected Override Properties

        #region Internal Static Methods

        /// <summary>
        /// Gets the variables defined by an element with parameters and child statements, such as a method or a constructor.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="parameters">The element's parameters.</param>
        /// <returns>Returns the variables.</returns>
        internal static IList<IVariable> GatherVariablesForElementWithParametersAndChildStatements(Element element, IList<Parameter> parameters)
        {
            Param.AssertNotNull(element, "element");
            Param.Ignore(parameters);

            var variables = new List<IVariable>();

            if (parameters != null && parameters.Count > 0)
            {
                for (int i = 0; i < parameters.Count; ++i)
                {
                    variables.Add(parameters[i]);
                }
            }

            for (VariableDeclarationStatement variableStatement = element.FindFirstChild<VariableDeclarationStatement>();
                variableStatement != null;
                variableStatement = variableStatement.FindNextSibling<VariableDeclarationStatement>())
            {
                variables.AddRange(variableStatement.Variables);
            }

            return variables.AsReadOnly();
        }

        #endregion Internal Static Methods

        #region Protected Override Methods

        /// <summary>
        /// Gets the name of the element.
        /// </summary>
        /// <returns>The name of the element.</returns>
        protected override string GetElementName()
        {
            CodeUnit start = this.Children.First;
            if (!this.ContainsModifier(TokenType.Implicit, TokenType.Explicit))
            {
                // Get the return type.
                start = this.FindFirstChild<TypeToken>();
            }

            if (start != null)
            {
                Token next = start.FindNextSibling<Token>();
                if (next != null)
                {
                    if (next.Is(TokenType.Operator))
                    {
                        // The next token is the operator name.
                        Token operatorName = next.FindNextSibling<Token>();
                        if (operatorName != null)
                        {
                            return "operator " + operatorName.Text;
                        }
                    }
                    else
                    {
                        // This is a regular method. 
                        return next.Text;
                    }
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

            this.returnType.Reset();
            this.typeConstraints.Reset();
            this.extensionMethod.Reset();
            this.parameters.Reset();
            this.fullyQualifiedName.Reset();
            this.variables.Reset();
        }

        #endregion Protected Override Methods
    }
}
