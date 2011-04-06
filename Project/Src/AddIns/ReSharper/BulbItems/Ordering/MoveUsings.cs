﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MoveUsings.cs" company="http://stylecop.codeplex.com">
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
//   BulbItem - MoveUsings : Moves Using statements inside the clsoest namespace.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StyleCop.ReSharper.BulbItems.Ordering
{
    #region Using Directives

    using JetBrains.ProjectModel;
    using JetBrains.ReSharper.Psi.CSharp.Tree;
    using JetBrains.TextControl;

    using StyleCop.ReSharper.BulbItems.Framework;
    using StyleCop.ReSharper.Core;

    #endregion

    /// <summary>
    /// BulbItem - MoveUsings : Moves Using statements inside the clsoest namespace.
    /// </summary>
    internal class MoveUsings : V5BulbItemImpl
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

            if (element == null)
            {
                return;
            }

            var usingList = element.GetContainingElement(typeof(IUsingListNode), false) as IUsingListNode;

            if (usingList == null)
            {
                return;
            }

            var file = Utils.GetCSharpFile(solution, textControl);

            // This violation will only run if there are some using statements and definately at least 1 namespace
            // so [0] index will always be OK.
            var firstNamespace = file.NamespaceDeclarations[0];

            foreach (var usingDirectiveNode in usingList.Imports)
            {
                firstNamespace.AddImportBefore(usingDirectiveNode, null);

                file.RemoveImport(usingDirectiveNode);
            }
        }

        #endregion
    }
}