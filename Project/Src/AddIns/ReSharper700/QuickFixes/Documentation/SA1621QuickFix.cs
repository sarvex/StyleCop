﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SA1621QuickFix.cs" company="http://stylecop.codeplex.com">
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
// <summary>
//   QuickFix - SA1621: GenericTypeParameterDocumentationMustMatchTypeParameters.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace StyleCop.ReSharper700.QuickFixes.Documentation
{
    #region Using Directives

    using System.Collections.Generic;

    using JetBrains.ReSharper.Feature.Services.Bulbs;
    using JetBrains.ReSharper.Intentions.Extensibility;

    using StyleCop.ReSharper700.BulbItems.Documentation;
    using StyleCop.ReSharper700.QuickFixes.Framework;
    using StyleCop.ReSharper700.Violations;

    #endregion

    /// <summary>
    /// QuickFix - SA1621: GenericTypeParameterDocumentationMustMatchTypeParameters.
    /// </summary>
    //// [ShowQuickFix]
    [QuickFix]
    public class SA1621QuickFix : StyleCopQuickFixBase
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the SA1621QuickFix class that can 
        /// handle <see cref="StyleCopHighlightingError"/>.
        /// </summary>
        /// <param name="highlight">
        /// <see cref="StyleCopHighlightingError"/>that has been detected.
        /// </param>
        public SA1621QuickFix(StyleCopHighlightingError highlight)
            : base(highlight)
        {
        }

        /// <summary>
        /// Initializes a new instance of the SA1621QuickFix class that can handle
        /// <see cref="StyleCopHighlightingHint"/>.
        /// </summary>
        /// <param name="highlight">
        /// <see cref="StyleCopHighlightingHint"/>that has been detected.
        /// </param>
        public SA1621QuickFix(StyleCopHighlightingHint highlight)
            : base(highlight)
        {
        }

        /// <summary>
        /// Initializes a new instance of the SA1621QuickFix class that can handle
        /// <see cref="StyleCopHighlightingInfo"/>.
        /// </summary>
        /// <param name="highlight">
        /// <see cref="StyleCopHighlightingInfo"/>that has been detected.
        /// </param>
        public SA1621QuickFix(StyleCopHighlightingInfo highlight)
            : base(highlight)
        {
        }

        /// <summary>
        /// Initializes a new instance of the SA1621QuickFix class that can handle
        /// <see cref="StyleCopHighlightingSuggestion"/>.
        /// </summary>
        /// <param name="highlight">
        /// <see cref="StyleCopHighlightingSuggestion"/>that has been detected.
        /// </param>
        public SA1621QuickFix(StyleCopHighlightingSuggestion highlight)
            : base(highlight)
        {
        }

        /// <summary>
        /// Initializes a new instance of the SA1621QuickFix class that can handle
        /// <see cref="StyleCopHighlightingWarning"/>.
        /// </summary>
        /// <param name="highlight">
        /// <see cref="StyleCopHighlightingWarning"/>that has been detected.
        /// </param>
        public SA1621QuickFix(StyleCopHighlightingWarning highlight)
            : base(highlight)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the QuickFix with all the available BulbItems that can fix the current
        /// StyleCop Violation.
        /// </summary>
        protected override void InitialiseBulbItems()
        {
            this.BulbItems = new List<IBulbAction>
                                 {
                                     new SA1618GenericTypeParametersMustBeDocumentedBulbItem
                                         {
                                             Description =
                                                 "Fix <typeparam> in header : "
                                                 + this.Highlighting.ToolTip
                                         }
                                 };
        }

        #endregion
    }
}