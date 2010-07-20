//-----------------------------------------------------------------------
// <copyright file="IParameterContainer.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.
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
//-----------------------------------------------------------------------
namespace Microsoft.StyleCop.CSharp.CodeModel
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Interface that must be implemented by statements containing a list of parameters.
    /// </summary>
    /// <subcategory>interface</subcategory>
    public interface IParameterContainer
    {
        #region Properties

        /// <summary>
        /// Gets the list of parameters in the container.
        /// </summary>
        IList<Parameter> Parameters
        {
            get;
        }

        #endregion Properties
    }
}
