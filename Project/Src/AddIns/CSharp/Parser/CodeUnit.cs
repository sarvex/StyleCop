//-----------------------------------------------------------------------
// <copyright file="CodeUnit.cs" company="Microsoft">
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

    /// <summary>
    /// A basic code unit, either an expression or a statement.
    /// </summary>
    public class CodeUnit : IWriteableCodeUnit
    {
        #region Private Static Fields

        /// <summary>
        /// An empty array of expressions.
        /// </summary>
        private static Expression[] emptyExpressionArray = new Expression[0];

        /// <summary>
        /// An empty array of statements.
        /// </summary>
        private static Statement[] emptyStatementArray = new Statement[0];

        /// <summary>
        /// An empty master list of tokens.
        /// </summary>
        private static CsTokenList emptyTokenList;

        #endregion Private Static Fields

        #region Private Fields

        /// <summary>
        /// The friendly name of the type.
        /// </summary>
        private string friendlyTypeName;

        /// <summary>
        /// The friendly name of the type, in plural form.
        /// </summary>
        private string friendlyPluralTypeName;

        /// <summary>
        /// The token list for this code unit.
        /// </summary>
        private CsTokenList tokens;

        /// <summary>
        /// The location of this expression.
        /// </summary>
        private CodeLocation location;

        /// <summary>
        /// The collection of variables defined by this code unit.
        /// </summary>
        private VariableCollection variables = new VariableCollection();

        /// <summary>
        /// The collection of expressions beneath this code unit.
        /// </summary>
        private CodeUnitCollection<Expression> expressions;

        /// <summary>
        /// The collection of statements beneath this code unit.
        /// </summary>
        private CodeUnitCollection<Statement> statements;

        /// <summary>
        /// The parent of this code unit.
        /// </summary>
        private ICodePart parent;

        /// <summary>
        /// The type of the code unit.
        /// </summary>
        private CodePartType codeUnitType;

        /// <summary>
        /// Indicates whether to automatically trim down the token list whenever it is set.
        /// </summary>
        private bool trimTokens = true;

        #endregion Private Fields

        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the CodeUnit class.
        /// </summary>
        /// <param name="codeUnitType">The type of the code unit.</param>
        internal CodeUnit(CodePartType codeUnitType)
        {
            Param.Ignore(codeUnitType);
            this.codeUnitType = codeUnitType;
        }

        /// <summary>
        /// Initializes a new instance of the CodeUnit class.
        /// </summary>
        /// <param name="codeUnitType">The type of the code unit.</param>
        /// <param name="tokens">The list of tokens that form the code unit.</param>
        internal CodeUnit(CodePartType codeUnitType, CsTokenList tokens) : this(codeUnitType)
        {
            Param.Ignore(codeUnitType);
            Param.AssertNotNull(tokens, "tokens");

            this.tokens = tokens;
            this.tokens.Trim();

            Debug.Assert(this.tokens.First != null, "The tokens should not be empty");
        }

        #endregion Internal Constructors

        #region Public Virtual Properties

        /// <summary>
        /// Gets the list of tokens within this code unit.
        /// </summary>
        public virtual CsTokenList Tokens
        {
            get
            {
                if (this.tokens == null)
                {
                    if (emptyTokenList == null)
                    {
                        emptyTokenList = new CsTokenList(new MasterList<CsToken>());
                    }

                    return emptyTokenList;
                }

                return this.tokens;
            }

            internal set
            {
                this.tokens = value;

                if (this.trimTokens)
                {
                    this.tokens.Trim();
                }

                Debug.Assert(this.tokens.First != null, "The token list should not be empty");
            }
        }

        /// <summary>
        /// Gets the location of this code unit within the document.
        /// </summary>
        public virtual CodeLocation Location
        {
            get
            {
                if (this.location == null)
                {
                    Debug.Assert(this.tokens.First != null, "The token list should not be empty");
                    this.location = CsToken.JoinLocations(this.tokens.First, this.tokens.Last);
                }

                return this.location;
            }

            internal set
            {
                this.location = value;
            }
        }

        /// <summary>
        /// Gets the line number that this code unit appears on in the document.
        /// </summary>
        public virtual int LineNumber
        {
            get
            {
                return this.Location.LineNumber;
            }
        }

        #endregion Public Virtual Properties

        #region Public Properties

        /// <summary>
        /// Gets the friendly name of the code unit type, which can be used in user output.
        /// </summary>
        public string FriendlyTypeText
        {
            get
            {
                string text = this.GetFriendlyTypeText(null);
                if (text != null)
                {
                    return text;
                }

                text = this.GetFriendlyTypeText(this.GetType().Name);
                Debug.Assert(!string.IsNullOrEmpty(text), "The text should not be empty");

                return text;
            }
        }

        /// <summary>
        /// Gets the friendly name of the code unit type as a plural noun, which can be used in user output.
        /// </summary>
        public string FriendlyPluralTypeText
        {
            get
            {
                string text = this.GetFriendlyPluralTypeText(null);
                if (text != null)
                {
                    return text;
                }

                text = this.GetFriendlyPluralTypeText(this.GetType().Name);
                Debug.Assert(!string.IsNullOrEmpty(text), "The text should not be empty");

                return text;
            }
        }

        /// <summary>
        /// Gets the type of the code unit.
        /// </summary>
        public CodePartType CodePartType
        {
            get
            {
                return this.codeUnitType;
            }
        }

        /// <summary>
        /// Gets the list of variables and constants defined by this code unit.
        /// </summary>
        public VariableCollection Variables
        {
            get
            {
                return this.variables;
            }
        }

        /// <summary>
        /// Gets the collection of expressions beneath this code unit.
        /// </summary>
        public ICollection<Expression> ChildExpressions
        {
            get
            {
                if (this.expressions == null)
                {
                    return emptyExpressionArray;
                }

                return this.expressions;
            }
        }

        /// <summary>
        /// Gets the collection of statements beneath this code unit.
        /// </summary>
        public ICollection<Statement> ChildStatements
        {
            get
            {
                if (this.statements == null)
                {
                    return emptyStatementArray;
                }

                return this.statements;
            }
        }

        /// <summary>
        /// Gets the parent of this code unit.
        /// </summary>
        public ICodePart Parent
        {
            get
            {
                return this.parent;
            }
        }

        #endregion Public Properties

        #region Internal Properties

        /// <summary>
        /// Gets or sets a value indicating whether to automatically trim down the 
        /// token list whenever it is set.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "A property should always have a get accessor.")]
        internal bool TrimTokens
        {
            get
            {
                return this.trimTokens;
            }

            set
            {
                Param.Ignore(value);
                this.trimTokens = value;
            }
        }

        #endregion Internal Properties

        #region IWriteableCodeUnit Methods

        /// <summary>
        /// Adds a child expression.
        /// </summary>
        /// <param name="expression">The expression to add.</param>
        void IWriteableCodeUnit.AddExpression(Expression expression)
        {
            Param.Ignore(expression);
            this.AddExpression(expression);
        }

        /// <summary>
        /// Adds a range of child expressions.
        /// </summary>
        /// <param name="items">The expressions to add.</param>
        void IWriteableCodeUnit.AddExpressions(IEnumerable<Expression> items)
        {
            Param.Ignore(items);
            this.AddExpressions(items);
        }

        /// <summary>
        /// Adds a child statement.
        /// </summary>
        /// <param name="statement">The statement to add.</param>
        void IWriteableCodeUnit.AddStatement(Statement statement)
        {
            Param.Ignore(statement);
            this.AddStatement(statement);
        }

        /// <summary>
        /// Adds a range of child statements.
        /// </summary>
        /// <param name="items">The statements to add.</param>
        void IWriteableCodeUnit.AddStatements(IEnumerable<Statement> items)
        {
            Param.Ignore(items);
            this.AddStatements(items);
        }

        /// <summary>
        /// Sets the parent of this code unit.
        /// </summary>
        /// <param name="parentCodeUnit">The parent.</param>
        void IWriteableCodeUnit.SetParent(ICodePart parentCodeUnit)
        {
            Param.Ignore(parentCodeUnit);
            this.parent = parentCodeUnit;
        }

        #endregion IWriteableCodeUnit Methods

        #region Internal Methods

        /// <summary>
        /// Adds a child expression.
        /// </summary>
        /// <param name="expression">The expression to add.</param>
        internal void AddExpression(Expression expression)
        {
            Param.AssertNotNull(expression, "expression");

            if (this.expressions == null)
            {
                this.expressions = new CodeUnitCollection<Expression>(this);
            }

            this.expressions.Add(expression);
        }

        /// <summary>
        /// Adds a range of child expressions.
        /// </summary>
        /// <param name="items">The expressions to add.</param>
        internal void AddExpressions(IEnumerable<Expression> items)
        {
            Param.AssertNotNull(items, "items");

            if (this.expressions == null)
            {
                this.expressions = new CodeUnitCollection<Expression>(this);
            }

            this.expressions.AddRange(items);
        }

        /// <summary>
        /// Adds a child statement.
        /// </summary>
        /// <param name="statement">The statement to add.</param>
        internal void AddStatement(Statement statement)
        {
            Param.AssertNotNull(statement, "statement");

            if (this.statements == null)
            {
                this.statements = new CodeUnitCollection<Statement>(this);
            }

            this.statements.Add(statement);
        }

        /// <summary>
        /// Adds a range of child statements.
        /// </summary>
        /// <param name="items">The statements to add.</param>
        internal void AddStatements(IEnumerable<Statement> items)
        {
            Param.AssertNotNull(items, "items");

            if (this.statements == null)
            {
                this.statements = new CodeUnitCollection<Statement>(this);
            }

            this.statements.AddRange(items);
        }

        /// <summary>
        /// Gets the friendly name of the code unit type, which can be used in user output.
        /// </summary>
        /// <param name="typeName">The name of the type.</param>
        /// <returns>Returns the friendly name text.</returns>
        internal string GetFriendlyTypeText(string typeName)
        {
            Param.Ignore(typeName);

            if (this.friendlyTypeName == null && typeName != null)
            {
                this.friendlyTypeName = TypeNames.ResourceManager.GetString(typeName, TypeNames.Culture);
            }

            return this.friendlyTypeName;
        }

        /// <summary>
        /// Gets the friendly name of the code unit type as a plural noun, which can be used in user output.
        /// </summary>
        /// <param name="typeName">The name of the type.</param>
        /// <returns>Returns the plural friendly name text.</returns>
        internal string GetFriendlyPluralTypeText(string typeName)
        {
            Param.Ignore(typeName);

            if (this.friendlyPluralTypeName == null && typeName != null)
            {
                this.friendlyPluralTypeName = TypeNames.ResourceManager.GetString(typeName + "Plural", TypeNames.Culture);
            }

            return this.friendlyPluralTypeName;
        }

        #endregion Internal Methods
    }
}
