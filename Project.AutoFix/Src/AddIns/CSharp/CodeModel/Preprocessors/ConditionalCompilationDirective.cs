//-----------------------------------------------------------------------
// <copyright file="ConditionalCompilationDirective.cs">
//     MS-PL
// </copyright>
// <license>
//   This source code is subject to terms and conditions of the Microsoft 
//   Public License. A copy of the license can be found in the License.html 
//   file at the root of this distribution. 
//   By using this source code in any fashion, you are agreeing to be bound 
//   by the terms of the Microsoft Public License. You must not remove this 
//   notice, or any other, from this software.
// </license>
//-----------------------------------------------------------------------
namespace StyleCop.CSharp.CodeModel
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Describes a conditional compilation directive.
    /// </summary>
    /// <subcategory>preprocessor</subcategory>
    public abstract class ConditionalCompilationDirective : PreprocessorDirective
    {
        #region Private Fields

        /// <summary>
        /// The expression that makes up the body of the directive.
        /// </summary>
        private CodeUnitProperty<Expression> body;

        #endregion Private Fields

        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the ConditionalCompilationDirective class.
        /// </summary>
        /// <param name="text">The line text.</param>
        /// <param name="proxy">Proxy object for the directive.</param>
        /// <param name="type">The type of the directive.</param>
        /// <param name="body">The expression that makes up the body of the directive.</param>
        internal ConditionalCompilationDirective(
            string text, CodeUnitProxy proxy, PreprocessorType type, Expression body)
            : base(proxy, type)
        {
            Param.AssertValidString(text, "text");
            Param.Ignore(proxy);
            Param.Ignore(type);
            Param.Ignore(body);

            CsLanguageService.Debug.Assert(
                type == PreprocessorType.If ||
                type == PreprocessorType.Elif ||
                type == PreprocessorType.Else ||
                type == PreprocessorType.Endif,
                "The type must be one of the conditional compilation directive types.");

            this.Text = text;
            this.body.Value = body;
        }

        #endregion Internal Constructors

        #region Public Properties

        /// <summary>
        /// Gets the expression that makes up the body of the directive.
        /// </summary>
        public Expression Body
        {
            get
            {
                this.ValidateEditVersion();

                if (!this.body.Initialized)
                {
                    this.body.Value = this.FindFirstChildExpression();
                }

                return this.body.Value;
            }
        }

        #endregion Public Properties

        #region Protected Override Methods

        /// <summary>
        /// Resets the contents of the item.
        /// </summary>
        protected override void Reset()
        {
            base.Reset();

            this.body.Reset();
        }

        #endregion Protected Override Methods
    }
}
