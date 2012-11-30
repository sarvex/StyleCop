﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SA1604ElementDocumentationMustHaveSummaryBulbItem.cs" company="http://stylecop.codeplex.com">
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
//   BulbItem - SA1604ElementDocumentationMustHaveSummaryBulbItem
//   Also fixes SA1605PartialElementDocumentationMustHaveSummary.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace StyleCop.ReSharper700.BulbItems.Documentation
{
    #region Using Directives

    using JetBrains.ProjectModel;
    using JetBrains.ReSharper.Psi.Tree;
    using JetBrains.TextControl;

    using StyleCop.ReSharper700.BulbItems.Framework;
    using StyleCop.ReSharper700.CodeCleanup.Rules;
    using StyleCop.ReSharper700.Core;

    #endregion

    /// <summary>
    /// BulbItem - SA1604ElementDocumentationMustHaveSummaryBulbItem
    /// Also fixes SA1605PartialElementDocumentationMustHaveSummary.
    /// </summary>
    internal class SA1604ElementDocumentationMustHaveSummaryBulbItem : V5BulbItemImpl
    {
        #region Public Methods and Operators

        /// <inheritdoc />
        public override void ExecuteTransactionInner(ISolution solution, ITextControl textControl)
        {
            IDeclaration declaration = Utils.GetTypeClosestToTextControl<IDeclaration>(solution, textControl);

            // Fixes SA1604, 1605
            new DocumentationRules().InsertMissingSummaryElement(declaration);
        }

        #endregion
    }
}