// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SA1633FileMustHaveHeaderBulbItem.cs" company="http://stylecop.codeplex.com">
//   MS-PL
// </copyright>
// <summary>
//   Defines the SA1633FileMustHaveHeaderBulbItem type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace StyleCop.ReSharper610.BulbItems.Documentation
{
    #region Using Directives

    using JetBrains.ProjectModel;
    using JetBrains.ReSharper.Psi.CSharp.Tree;
    using JetBrains.TextControl;

    using StyleCop.ReSharper610.BulbItems.Framework;
    using StyleCop.ReSharper610.CodeCleanup.Rules;
    using StyleCop.ReSharper610.Core;

    #endregion

    /// <summary>
    /// BulbItem for inserting the standard StyleCop File Header.
    /// </summary>
    public class SA1633FileMustHaveHeaderBulbItem : V5BulbItemImpl
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
            ICSharpFile file = Utils.GetCSharpFile(solution, textControl);
            new DocumentationRules().InsertFileHeader(file);
        }

        #endregion
    }
}