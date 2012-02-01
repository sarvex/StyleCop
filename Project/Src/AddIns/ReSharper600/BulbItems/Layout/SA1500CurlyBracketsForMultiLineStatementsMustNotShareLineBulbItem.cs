// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SA1500CurlyBracketsForMultiLineStatementsMustNotShareLineBulbItem.cs" company="http://stylecop.codeplex.com">
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
//   The s a 1500 curly brackets for multi line statements must not share line bulb item.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StyleCop.ReSharper600.BulbItems.Layout
{
    #region Using Directives

    using JetBrains.ProjectModel;
    using JetBrains.ReSharper.Psi.CodeStyle;
    using JetBrains.ReSharper.Psi.CSharp.CodeStyle;
    using JetBrains.ReSharper.Psi.CSharp.Tree;
    using JetBrains.TextControl;

    using StyleCop.ReSharper600.BulbItems.Framework;
    using StyleCop.ReSharper600.CodeCleanup.Rules;
    using StyleCop.ReSharper600.Core;

    #endregion

    /// <summary>
    /// The s a 1500 curly brackets for multi line statements must not share line bulb item.
    /// </summary>
    public class SA1500CurlyBracketsForMultiLineStatementsMustNotShareLineBulbItem : V5BulbItemImpl
    {
        #region Public Methods

        /// <summary>
        /// The execute transaction inner.
        /// </summary>
        /// <param name="solution">
        /// The solution.
        /// </param>
        /// <param name="textControl">
        /// The text control.
        /// </param>
        public override void ExecuteTransactionInner(ISolution solution, ITextControl textControl)
        {
            var element = Utils.GetElementAtCaret(solution, textControl);
            var containingBlock = element.GetContainingNode<IBlock>(true);

            if (containingBlock != null)
            {
                CSharpFormatterHelper.FormatterInstance.Format(containingBlock);
                new LayoutRules().CurlyBracketsForMultiLineStatementsMustNotShareLine(containingBlock);
            }
            else
            {
                var fieldDeclarationNode = element.GetContainingNode<IFieldDeclaration>(true);
                if (fieldDeclarationNode != null)
                {
                    CSharpFormatterHelper.FormatterInstance.Format(fieldDeclarationNode);
                    new LayoutRules().CurlyBracketsForMultiLineStatementsMustNotShareLine(fieldDeclarationNode);
                }
            }
        }

        #endregion
    }
}