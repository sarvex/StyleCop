// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StyleCopLocator.cs" company="http://stylecop.codeplex.com">
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
//   The style cop locator.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StyleCop.ReSharper513.Core
{
    #region Using Directives

    using System.IO;
    using System.Reflection;

    using Microsoft.Win32;

    #endregion

    /// <summary>
    /// The style cop locator.
    /// </summary>
    public static class StyleCopLocator
    {
        #region Public Methods

        /// <summary>
        /// Gets the StyleCop assembly path.
        /// </summary>
        /// <returns>
        /// The path to the StyleCop assembly or null if not found.
        /// </returns>
        public static string GetStyleCopPath()
        {
            var directory = StyleCop.Utils.InstallDirFromRegistry() ?? Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            return directory == null ? directory : Path.Combine(directory, Constants.StyleCopAssemblyName);
        }

        #endregion
    }
}