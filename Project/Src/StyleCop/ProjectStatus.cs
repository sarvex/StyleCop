//-----------------------------------------------------------------------
// <copyright file="ProjectStatus.cs" company="Microsoft">
//   Copyright (c) Microsoft Corporation.
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
namespace Microsoft.StyleCop
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Keeps track of the analysis status for a single code project.
    /// </summary>
    internal class ProjectStatus
    {
        #region Private Fields

        /// <summary>
        /// Indicates whether to ignore the cached results for all files in this project.
        /// </summary>
        private bool ignoreResultsCache;

        /// <summary>
        /// The list of analyzers for this project for each file type.
        /// </summary>
        private Dictionary<string, ICollection<SourceAnalyzer>> analyzerLists = new Dictionary<string, ICollection<SourceAnalyzer>>();

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the ProjectStatus class.
        /// </summary>
        public ProjectStatus()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether to ignore the cached results for all source code 
        /// documents within this project.
        /// </summary>
        public bool IgnoreResultsCache
        {
            get
            {
                return this.ignoreResultsCache;
            }

            set
            {
                Param.Ignore(value);
                this.ignoreResultsCache = value;
            }
        }

        /// <summary>
        /// Gets the analyzer lists for each file type in this project.
        /// </summary>
        internal Dictionary<string, ICollection<SourceAnalyzer>> AnalyzerLists
        {
            get
            {
                return this.analyzerLists;
            }
        }

        #endregion Public Properties
    }
}
