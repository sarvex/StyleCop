﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SuppressQuickFixAttribute.cs" company="http://stylecop.codeplex.com">
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
//   ReSharper Attribute that allows you to define a custom Icon for a Suppress QuickFix.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace StyleCop.ReSharper611.QuickFixes.Framework
{
    #region Using Directives

    using System.Drawing;
    using System.IO;
    using System.Reflection;

    using JetBrains.ReSharper.Feature.Services.Bulbs;

    #endregion

    /// <summary>
    /// ReSharper Attribute that allows you to define a custom Icon for a Suppress QuickFix.
    /// </summary>
    public class SuppressQuickFixAttribute : ConfigureFunctionalGroupAttribute
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns an image that represents an Icon that will be displayed in the QuickFix
        /// Context Menu.
        /// </summary>
        /// <param name="reason">
        /// Availability Reason for the QF.
        /// </param>
        /// <returns>
        /// Image representing the icon.
        /// </returns>
        public override Image IconForReason(ActionAvailabilityReason reason)
        {
            Image image = null;

            Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("StyleCop.ReSharper611.Resources.SuppressQuickFix.png");

            if (resourceStream != null)
            {
                image = Image.FromStream(resourceStream);
            }

            return image;
        }

        #endregion
    }
}