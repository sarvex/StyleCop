// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SA1506DocumentationHeaderLineMustNotBeFollowedByABlankLineBulbItem.cs" company="http://stylecop.codeplex.com">
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
//   The s a 1506 documentation header line must not be followed by a blank line bulb item.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StyleCop.ReSharper600.BulbItems.Layout
{
    #region Using Directives

    using JetBrains.ProjectModel;
    using JetBrains.ReSharper.Psi.CSharp.Tree;
    using JetBrains.ReSharper.Psi.Impl.CodeStyle;
    using JetBrains.ReSharper.Psi.Tree;
    using JetBrains.TextControl;

    using StyleCop.ReSharper600.BulbItems.Framework;
    using StyleCop.ReSharper600.Core;

    #endregion

    /// <summary>
    /// The s a 1506 documentation header line must not be followed by a blank line bulb item.
    /// </summary>
    public class SA1506DocumentationHeaderLineMustNotBeFollowedByABlankLineBulbItem : V5BulbItemImpl
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

            var currentNode = (ITreeNode)element;

            var docCommentNode = currentNode as IDocCommentNode;

            var containingElement = docCommentNode.GetContainingNode<IDocCommentBlockNode>(true);

            var rightNode = containingElement.FindFormattingRangeToRight();

            Utils.RemoveNewLineBefore(rightNode);
        }

        #endregion
    }
}