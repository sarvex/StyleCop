// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SA1121UseBuiltInTypeAliasBulbItem.cs" company="http://stylecop.codeplex.com">
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
//   The s a 1121 use built in type alias bulb item.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace StyleCop.ReSharper513.BulbItems.Readability
{
    #region Using Directives

    using System.Collections.Generic;

    using JetBrains.ProjectModel;
    using JetBrains.ReSharper.Psi.CSharp.Tree;
    using JetBrains.ReSharper.Psi.Tree;
    using JetBrains.TextControl;

    using StyleCop.ReSharper513.BulbItems.Framework;
    using StyleCop.ReSharper513.CodeCleanup.Rules;
    using StyleCop.ReSharper513.Core;

    #endregion

    /// <summary>
    /// The s a 1121 use built in type alias bulb item.
    /// </summary>
    public class SA1121UseBuiltInTypeAliasBulbItem : V5BulbItemImpl
    {
        #region Public Methods and Operators

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
            IList<ITokenNode> tokensForLine = Utils.GetTokensForLineFromTextControl(solution, textControl);

            foreach (ITokenNode tokenNode in tokensForLine)
            {
                ITypeArgumentListNode typeArgumentListNode = tokenNode.GetContainingElement<ITypeArgumentListNode>(true);

                if (typeArgumentListNode != null)
                {
                    ReadabilityRules.SwapToBuiltInTypeAlias(typeArgumentListNode);
                }

                IObjectCreationExpressionNode objectCreationExpressionNode = tokenNode.GetContainingElement<IObjectCreationExpressionNode>(true);

                if (objectCreationExpressionNode != null)
                {
                    ReadabilityRules.SwapToBuiltInTypeAlias(objectCreationExpressionNode);
                }

                IArrayCreationExpressionNode arrayCreationExpressionNode = tokenNode.GetContainingElement<IArrayCreationExpressionNode>(true);

                if (arrayCreationExpressionNode != null)
                {
                    ReadabilityRules.SwapToBuiltInTypeAlias(arrayCreationExpressionNode);
                }

                IMethodDeclaration methodDeclaration = tokenNode.GetContainingElement<IMethodDeclaration>(true);

                if (methodDeclaration != null)
                {
                    ReadabilityRules.SwapToBuiltInTypeAlias((ITreeNode)methodDeclaration);
                }

                IVariableDeclaration variableDeclaration = tokenNode.GetContainingElement<IVariableDeclaration>(true);

                if (variableDeclaration != null)
                {
                    ReadabilityRules.SwapToBuiltInTypeAlias((ITreeNode)variableDeclaration);
                }

                IMultipleDeclarationNode multipleDeclarationNode = tokenNode.GetContainingElement<IMultipleDeclarationNode>(true);

                if (multipleDeclarationNode != null)
                {
                    ReadabilityRules.SwapToBuiltInTypeAlias(multipleDeclarationNode);
                }
            }
        }

        #endregion
    }
}