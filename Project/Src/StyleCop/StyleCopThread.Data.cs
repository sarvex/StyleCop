//-----------------------------------------------------------------------
// <copyright file="StyleCopThread.Data.cs">
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
//-----------------------------------------------------------------------
namespace StyleCop
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;

    /// <content>
    /// StyleCop thread.
    /// </content>
    internal partial class StyleCopThread
    {
        /// <summary>
        /// Class that contains data used by analysis worker threads.
        /// </summary>
        public class Data
        {
            #region Private Fields

            /// <summary>
            /// The number of threads running.
            /// </summary>
            private int threads;

            /// <summary>
            /// The index of the current project.
            /// </summary>
            private int projectIndex;

            /// <summary>
            /// The index of the current source code index within the current project.
            /// </summary>
            private int sourceCodeInstanceIndex = -1;

            /// <summary>
            /// True if the results cache should be ignored.
            /// </summary>
            private bool ignoreResultsCache;

            /// <summary>
            /// The path to the settings to use during analysis.
            /// </summary>
            private string settingsPath;

            /// <summary>
            /// The list of projectss to analyze.
            /// </summary>
            private IList<CodeProject> projects;

            /// <summary>
            /// The StyleCop core instance.
            /// </summary>
            private StyleCopCore core;

            /// <summary>
            /// The results cache manager.
            /// </summary>
            private ResultsCache cache;

            /// <summary>
            /// The pass number.
            /// </summary>
            private int passNumber;

            /// <summary>
            /// Stores the settings for each project.
            /// </summary>
            private Dictionary<int, Settings> settings;

            /// <summary>
            /// The current analysis status of each source code document.
            /// </summary>
            private Dictionary<SourceCode, DocumentAnalysisStatus> sourceCodeInstanceStatus = new Dictionary<SourceCode, DocumentAnalysisStatus>();

            /// <summary>
            /// The current analysis status of each project.
            /// </summary>
            private Dictionary<CodeProject, ProjectStatus> projectStatus = new Dictionary<CodeProject, ProjectStatus>(); 

            #endregion Private Fields

            #region Public Constructors

            /// <summary>
            /// Initializes a new instance of the Data class.
            /// </summary>
            /// <param name="core">The StyleCop core instance.</param>
            /// <param name="codeProjects">The list of code projects to analyze.</param>
            /// <param name="resultsCache">The results cache.</param>
            /// <param name="ignoreResultsCache">True to ignore the results cache.</param>
            /// <param name="settingsPath">The path to the settings to use during analysis.</param>
            public Data(
                StyleCopCore core, 
                IList<CodeProject> codeProjects,
                ResultsCache resultsCache, 
                bool ignoreResultsCache, 
                string settingsPath)
            {
                Param.AssertNotNull(core, "core");
                Param.AssertNotNull(codeProjects, "codeProjects");
                Param.Ignore(resultsCache);
                Param.Ignore(ignoreResultsCache);
                Param.Ignore(settingsPath);

                this.core = core;
                this.projects = codeProjects;
                this.cache = resultsCache;
                this.ignoreResultsCache = ignoreResultsCache;
                this.settingsPath = settingsPath;
            }
            #endregion Public Constructors

            #region Public Properties

            /// <summary>
            /// Gets the StyleCop core instance.
            /// </summary>
            public StyleCopCore Core
            {
                get 
                { 
                    return this.core; 
                }
            }

            /// <summary>
            /// Gets the results cache handler.
            /// </summary>
            public ResultsCache ResultsCache
            {
                get 
                { 
                    return this.cache; 
                }
            }

            /// <summary>
            /// Gets a value indicating whether to ignore the cached results from the last run.
            /// </summary>
            public bool IgnoreResultsCache
            {
                get 
                { 
                    return this.ignoreResultsCache; 
                }
            }

            /// <summary>
            /// Gets or sets the current pass number.
            /// </summary>
            public int PassNumber
            {
                get
                {
                    return this.passNumber;
                }

                set
                {
                    Param.RequireGreaterThanOrEqualToZero(value, "PassNumber");
                    this.passNumber = value;
                }
            }

            #endregion Public Properties

            #region Public Methods

            /// <summary>
            /// Gets the next source code document to analyze.
            /// </summary>
            /// <returns>Returns the source code document to analyze or null if none.</returns>
            public SourceCode GetNextSourceCodeDocument()
            {
                // Keep looping until we find a file that is not marked as excluded.
                while (true)
                {
                    SourceCode sourceCode = this.ExtractNextSourceCodeDocument();
                    if (sourceCode == null)
                    {
                        return null;
                    }

                    sourceCode.Settings = sourceCode.Project.Settings.GetCustomSettingsForFile(sourceCode.Name);

                    return sourceCode;
                }
            }

            /// <summary>
            /// Increments the active thread count.
            /// </summary>
            /// <returns>Returns the new thread count.</returns>
            public int IncrementThreadCount()
            {
                return ++this.threads;
            }

            /// <summary>
            /// Decrements the active thread count. If all threads have ended,
            /// wakes the parent thread.
            /// </summary>
            /// <returns>Returns the new thread count.</returns>
            public int DecrementThreadCount()
            {
                return --this.threads;
            }

            /// <summary>
            /// Resets the source code document index.
            /// </summary>
            public void ResetEmumerator()
            {
                this.sourceCodeInstanceIndex = -1;
                this.projectIndex = 0;
            }

            /// <summary>
            /// Gets the settings for the given project.
            /// </summary>
            /// <param name="project">The project containing the settings.</param>
            /// <returns>Returns the settings or null if the settings could not be loaded.</returns>
            /// <remarks>If a settings path has been provided by the host, the project is ignored and
            /// the alternate settings provided by the host are loaded instead.</remarks>
            public Settings GetSettings(CodeProject project)
            {
                Param.AssertNotNull(project, "project");

                // Create the dictionary key based on the path to the settings being loaded.
                int key = project.Key;
                if (this.settingsPath != null)
                {
                    key = this.settingsPath.GetHashCode();
                }

                Settings loadedSettings = null;

                // Try to load this from the cache.
                if (this.settings != null)
                {
                    this.settings.TryGetValue(key, out loadedSettings);
                }

                // If the settings were not loaded from the cache, load them from scratch.
                if (loadedSettings == null)
                {
                    // Check whether custom settings have been specified.
                    if (this.settingsPath != null)
                    {
                        loadedSettings = this.core.Environment.GetSettings(this.settingsPath, true);
                    }
                    else
                    {
                        loadedSettings = this.core.Environment.GetProjectSettings(project, true);
                    }

                    // Save the settings in the cache.
                    if (loadedSettings != null)
                    {
                        // Create the dictionary if needed.
                        if (this.settings == null)
                        {
                            this.settings = new Dictionary<int, Settings>();
                        }

                        // Add the settings to the dictionary.
                        this.settings.Add(key, loadedSettings);
                    }
                }

                return loadedSettings;
            }

            /// <summary>
            /// Gets the analysis status for the given source code document.
            /// </summary>
            /// <param name="sourceCode">The source code to retrieve status for.</param>
            /// <returns>Returns the analysis status for the source code.</returns>
            public DocumentAnalysisStatus GetDocumentStatus(SourceCode sourceCode)
            {
                Param.AssertNotNull(sourceCode, "sourceCode");

                DocumentAnalysisStatus status;
                if (!this.sourceCodeInstanceStatus.TryGetValue(sourceCode, out status))
                {
                    // Create a new status object and add add it to the dictionary.
                    status = new DocumentAnalysisStatus();
                    this.sourceCodeInstanceStatus.Add(sourceCode, status);
                }

                return status;
            }

            /// <summary>
            /// Gets the analysis status for the given project.
            /// </summary>
            /// <param name="project">The project.</param>
            /// <returns>Returns the analysis status for the project.</returns>
            public ProjectStatus GetProjectStatus(CodeProject project)
            {
                Param.AssertNotNull(project, "project");

                ProjectStatus status;
                if (!this.projectStatus.TryGetValue(project, out status))
                {
                    // Create a new status object and add add it to the dictionary.
                    status = new ProjectStatus();
                    this.projectStatus.Add(project, status);
                }

                return status;
            }

            #endregion Public Methods

            #region Private Methods

            /// <summary>
            /// Pulls out the next source code document.
            /// </summary>
            /// <returns>Returns the source code document to analyze or null if none.</returns>
            private SourceCode ExtractNextSourceCodeDocument()
            {
                SourceCode sourceCode = null;
                while (this.projectIndex < this.projects.Count)
                {
                    CodeProject project = this.projects[this.projectIndex];

                    ++this.sourceCodeInstanceIndex;
                    if (this.sourceCodeInstanceIndex >= project.SourceCodeInstances.Count)
                    {
                        ++this.projectIndex;
                        this.sourceCodeInstanceIndex = -1;
                    }
                    else
                    {
                        sourceCode = project.SourceCodeInstances[this.sourceCodeInstanceIndex];
                        break;
                    }
                }

                return sourceCode;
            }

            #endregion Private Methods
        }
    }
}