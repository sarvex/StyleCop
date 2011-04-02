//-----------------------------------------------------------------------
// <copyright file="ResultsCache.cs">
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
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Security;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// Handles saving and loading file data from the cache.
    /// </summary>
    internal class ResultsCache
    {
        #region Internal Constants

        /// <summary>
        /// The current file cache version.
        /// </summary>
        internal const string Version = "11";

        #endregion Internal Constants

        #region Private Fields

        /// <summary>
        /// The StyleCop core instance.
        /// </summary>
        private StyleCopCore core;

        /// <summary>
        /// The hashtable to store the cached documents in.
        /// </summary>
        private Dictionary<string, XmlDocument> documentHash = new Dictionary<string, XmlDocument>();

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the ResultsCache class.
        /// </summary>
        /// <param name="core">The StyleCop core instance.</param>
        public ResultsCache(StyleCopCore core)
        {
            Param.AssertNotNull(core, "core");
            this.core = core;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Loads results for the given source code document from the cache.
        /// </summary>
        /// <param name="sourceCode">The source code to load.</param>
        /// <param name="parser">The parser that created this document.</param>
        /// <param name="writeTime">The last write time of the document.</param>
        /// <param name="settingsTimeStamp">The time when the settings were last updated.</param>
        /// <returns>Returns true if the results were loaded from the cache.</returns>
        public bool LoadResults(SourceCode sourceCode, SourceParser parser, DateTime writeTime, DateTime settingsTimeStamp)
        {
            Param.AssertNotNull(sourceCode, "sourceCode");
            Param.AssertNotNull(parser, "parser");
            Param.Ignore(writeTime);
            Param.Ignore(settingsTimeStamp);

            bool success = false;

            lock (this)
            {
                XmlNode item = null;
                XmlDocument doc = this.OpenResultsCache(sourceCode, parser, out item);

                if (doc != null && item != null)
                {
                    try
                    {
                        // Check the settings file timestamp.
                        XmlElement settingsNode = item["settings"];
                        if (settingsNode != null && IsNodeUpToDate(settingsNode, settingsTimeStamp))
                        {
                            // Get the timestamp and make sure the file has not been changed
                            // since this cache was written.
                            if (IsNodeUpToDate(item, writeTime))
                            {
                                XmlNode violations = item.SelectSingleNode("violations");
                                if (violations != null)
                                {
                                    if (parser.ImportViolations(sourceCode, violations))
                                    {
                                        success = true;
                                    }
                                }
                            }
                        }
                    }
                    catch (XmlException)
                    {
                    }

                    if (!this.documentHash.ContainsKey(sourceCode.Project.Location))
                    {
                        this.documentHash.Add(sourceCode.Project.Location, doc);
                    }
                }
            }

            return success;
        }

        /// <summary>
        /// Saves the given code document results into a cache document.
        /// </summary>
        /// <param name="document">The document to save.</param>
        /// <param name="parser">The parser that created the document.</param>
        /// <param name="settingsTimeStamp">The time when the settings were last updated.</param>
        /// <returns>Returns true if the document was saved.</returns>
        public bool SaveDocumentResults(CodeDocument document, SourceParser parser, DateTime settingsTimeStamp)
        {
            Param.AssertNotNull(document, "document");
            Param.AssertNotNull(parser, "parser");
            Param.Ignore(settingsTimeStamp);

            bool success = false;

            lock (this)
            {
                XmlDocument xml = null;

                try
                {
                    if (!this.documentHash.ContainsKey(document.SourceCode.Project.Location))
                    {
                        XmlNode temp;
                        xml = this.OpenResultsCache(document.SourceCode, parser, out temp);
                        if (xml != null)
                        {
                            this.documentHash.Add(document.SourceCode.Project.Location, xml);
                        }
                    }
                    else
                    {
                        xml = this.documentHash[document.SourceCode.Project.Location];
                    }

                    if (xml != null)
                    {
                        XmlNode remove = xml.DocumentElement.SelectSingleNode(
                            string.Format(CultureInfo.InvariantCulture, "sourcecode[@name=\"{0}\"][@parser=\"{1}\"]", document.SourceCode.Name, parser.Id));
                        if (remove != null)
                        {
                            xml.DocumentElement.RemoveChild(remove);
                        }
                    }
                    else
                    {
                        xml = new XmlDocument();

                        // Create the document node.
                        xml.AppendChild(xml.CreateElement("stylecopresultscache"));

                        // Add the version.
                        XmlNode versionNode = xml.CreateElement("version");
                        xml.DocumentElement.AppendChild(versionNode);
                        versionNode.InnerText = ResultsCache.Version;

                        if (this.documentHash.ContainsKey(document.SourceCode.Project.Location))
                        {
                            this.documentHash.Remove(document.SourceCode.Project.Location);
                        }

                        this.documentHash.Add(document.SourceCode.Project.Location, xml);
                    }

                    XmlNode root = xml.CreateElement("sourcecode");
                    XmlAttribute name = xml.CreateAttribute("name");
                    name.Value = document.SourceCode.Name;
                    root.Attributes.Append(name);
                    xml.DocumentElement.AppendChild(root);

                    // Save the last write time of the settings.
                    XmlNode settingsNode = xml.CreateElement("settings");
                    root.AppendChild(settingsNode);

                    XmlNode node = xml.CreateElement("timestamp");
                    settingsNode.AppendChild(node);
                    node.InnerText = settingsTimeStamp.ToString(CultureInfo.InvariantCulture);

                    node = xml.CreateElement("milliseconds");
                    settingsNode.AppendChild(node);
                    node.InnerText = settingsTimeStamp.Millisecond.ToString(CultureInfo.InvariantCulture);

                    // Get the last write time of the source code.
                    DateTime writeTime = document.SourceCode.TimeStamp;

                    // Add the timestamp.
                    node = xml.CreateElement("timestamp");
                    root.AppendChild(node);
                    node.InnerText = writeTime.ToString(CultureInfo.InvariantCulture);

                    node = xml.CreateElement("milliseconds");
                    root.AppendChild(node);
                    node.InnerText = writeTime.Millisecond.ToString(CultureInfo.InvariantCulture);

                    // Add the parser ID attribute.
                    if (document.SourceCode.Parser != null)
                    {
                        XmlAttribute attribute = xml.CreateAttribute("parser");
                        root.Attributes.Append(attribute);
                        attribute.Value = document.SourceCode.Parser.Id;
                    }

                    // Create the violations node.
                    node = xml.CreateElement("violations");
                    root.AppendChild(node);

                    // Add the violations.
                    SourceParser.ExportViolations(document, xml, node);

                    success = true;
                }
                catch (XmlException)
                {
                }
            }

            return success;
        }

        /// <summary>
        /// Loads the given project from the cache.
        /// </summary>
        /// <param name="project">The project to load.</param>
        /// <returns>Returns the project configuration or null if the 
        /// project does not exist in the cache.</returns>
        public string LoadProject(CodeProject project)
        {
            Param.AssertNotNull(project, "project");

            lock (this)
            {
                XmlNode item = null;
                XmlDocument doc = this.OpenCacheProject(project, out item);

                if (doc != null && item != null)
                {
                    try
                    {
                        // Get the configuration string.
                        XmlElement node = item["configuration"];
                        if (node != null)
                        {
                            return node.InnerText;
                        }
                    }
                    catch (XmlException)
                    {
                    }
                    catch (NullReferenceException)
                    {
                    }

                    if (!this.documentHash.ContainsKey(project.Location))
                    {
                        this.documentHash.Add(project.Location, doc);
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Saves the given code project configuration into a cache document.
        /// </summary>
        /// <param name="project">The project to save.</param>
        /// <returns>Returns true if the configuration was saved.</returns>
        public bool SaveProject(CodeProject project)
        {
            Param.AssertNotNull(project, "project");

            bool success = false;

            lock (this)
            {
                XmlDocument xml = null;

                try
                {
                    if (!this.documentHash.TryGetValue(project.Location, out xml))
                    {
                        XmlNode temp;
                        xml = this.OpenCacheProject(project, out temp);
                        if (xml != null)
                        {
                            this.documentHash.Add(project.Location, xml);
                        }
                    }

                    if (xml != null)
                    {
                        XmlNode remove = xml.DocumentElement.SelectSingleNode(
                            string.Format(CultureInfo.InvariantCulture, "project[@key=\"{0}\"]", project.Key));
                        if (remove != null)
                        {
                            xml.DocumentElement.RemoveChild(remove);
                        }
                    }
                    else
                    {
                        xml = new XmlDocument();

                        // Create the document node.
                        xml.AppendChild(xml.CreateElement("stylecopresultscache"));

                        // Add the version.
                        XmlNode versionNode = xml.CreateElement("version");
                        xml.DocumentElement.AppendChild(versionNode);
                        versionNode.InnerText = ResultsCache.Version;

                        if (this.documentHash.ContainsKey(project.Location))
                        {
                            this.documentHash.Remove(project.Location);
                        }

                        this.documentHash.Add(project.Location, xml);
                    }

                    XmlNode root = xml.CreateElement("project");
                    XmlAttribute key = xml.CreateAttribute("key");
                    key.Value = project.Key.ToString(CultureInfo.InvariantCulture);
                    root.Attributes.Append(key);
                    xml.DocumentElement.AppendChild(root);

                    // Add the configuration details.
                    StringBuilder configuration = new StringBuilder();
                    if (project.Configuration != null)
                    {
                        bool first = true;
                        foreach (string field in project.Configuration.Flags)
                        {
                            if (first)
                            {
                                first = false;
                                configuration.Append(field);
                            }
                            else
                            {
                                configuration.AppendFormat(";{0}", field);
                            }
                        }
                    }

                    XmlNode node = xml.CreateElement("configuration");
                    root.AppendChild(node);
                    node.InnerText = configuration.ToString();

                    success = true;
                }
                catch (XmlException)
                {
                }

                return success;
            }
        }

        /// <summary>
        /// Saves the cached results.
        /// </summary>
        public void Flush()
        {
            lock (this)
            {
                if (this.core.WriteResultsCache && this.core.Environment.SupportsResultsCache)
                {
                    foreach (KeyValuePair<string, XmlDocument> item in this.documentHash)
                    {
                        this.core.Environment.SaveResultsCache(item.Key, item.Value);
                    }
                }
            }
        }

        #endregion Public Methods

        #region Private Static Methods

        /// <summary>
        /// Determines whether the timestamp information contained in the given
        /// node is equal to the given timestamp.
        /// </summary>
        /// <param name="timeStampNode">The node containing the timestamp information.</param>
        /// <param name="timeStamp">The time to match again.</param>
        /// <returns>Returns true if the node matchines the given timestamp.</returns>
        private static bool IsNodeUpToDate(XmlNode timeStampNode, DateTime timeStamp)
        {
            Param.AssertNotNull(timeStampNode, "timeStampNode");
            Param.AssertNotNull(timeStamp, "timeStamp");

            // If the timestamp is empty, then we consider the values equal.
            if (timeStamp.Year == 0 && timeStamp.Month == 0 && timeStamp.Second == 0 && timeStamp.Millisecond == 0)
            {
                return true;
            }

            // Check the values in the node against the timestamp.
            XmlNode node = timeStampNode["timestamp"];
            if (node != null && node.InnerText == timeStamp.ToString(CultureInfo.InvariantCulture))
            {
                node = timeStampNode["milliseconds"];
                if (node != null && node.InnerText == timeStamp.Millisecond.ToString(CultureInfo.InvariantCulture))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion Private Static Methods

        #region Private Methods

        /// <summary>
        /// Opens the results cache for the given source code document.
        /// </summary>
        /// <param name="sourceCode">The source code document.</param>
        /// <param name="parser">The parser that created the document.</param>
        /// <param name="item">Returns the node from the results cache for this code document.</param>
        /// <returns>Returns the results cache.</returns>
        private XmlDocument OpenResultsCache(SourceCode sourceCode, SourceParser parser, out XmlNode item)
        {
            Param.AssertNotNull(sourceCode, "sourceCode");
            Param.AssertNotNull(parser, "parser");

            item = null;

            XmlDocument doc = null;

            try
            {
                lock (this)
                {
                    // Determine whether this results cache is already in our list.
                    if (this.documentHash.TryGetValue(sourceCode.Project.Location, out doc))
                    {
                        // Now pull out the section for this source code document.
                        item = doc.DocumentElement.SelectSingleNode(
                            string.Format(CultureInfo.InvariantCulture, "sourcecode[@name=\"{0}\"][@parser=\"{1}\"]", sourceCode.Name, parser.Id));
                    }
                    else
                    {
                        doc = this.core.Environment.LoadResultsCache(sourceCode.Project.Location);
                        if (doc != null)
                        {
                            // Get the version and make sure it matches.
                            XmlElement node = doc["stylecopresultscache"]["version"];
                            if (node.InnerText == ResultsCache.Version)
                            {
                                // Now pull out the section for this source code document.
                                item = doc.DocumentElement.SelectSingleNode(
                                    string.Format(CultureInfo.InvariantCulture, "sourcecode[@name=\"{0}\"][@parser=\"{1}\"]", sourceCode.Name, parser.Id));
                            }
                            else
                            {
                                // Since the version does not match, ignore this document.
                                doc = null;
                            }
                        }
                    }
                }
            }
            catch (XmlException)
            {
                doc = null;
            }
            catch (NullReferenceException)
            {
                doc = null;
            }

            return doc;
        }

        /// <summary>
        /// Opens the results cache for the given code project.
        /// </summary>
        /// <param name="project">The code project.</param>
        /// <param name="projectNode">Returns the node from the results cache for this code project.</param>
        /// <returns>Returns the results cache.</returns>
        private XmlDocument OpenCacheProject(CodeProject project, out XmlNode projectNode)
        {
            Param.AssertNotNull(project, "project");

            projectNode = null;

            XmlDocument doc = null;

            try
            {
                lock (this)
                {
                    // Determine whether this project is already in our list.
                    if (this.documentHash.TryGetValue(project.Location, out doc))
                    {
                        // Now pull out the section for this project.
                        projectNode = doc.DocumentElement.SelectSingleNode(
                            string.Format(CultureInfo.InvariantCulture, "project[@key=\"{0}\"]", project.Key.ToString(CultureInfo.InvariantCulture)));
                    }
                    else
                    {
                        // Load the document if it exists and add it to the hashtable.
                        doc = this.core.Environment.LoadResultsCache(project.Location);
                        if (doc != null)
                        {
                            // Get the version and make sure it matches.
                            XmlElement node = doc["stylecopresultscache"]["version"];
                            if (node.InnerText == ResultsCache.Version)
                            {
                                // Now pull out the section for this project.
                                projectNode = doc.DocumentElement.SelectSingleNode(
                                    string.Format(CultureInfo.InvariantCulture, "project[@key=\"{0}\"]", project.Key.ToString(CultureInfo.InvariantCulture)));
                            }
                            else
                            {
                                // Since the version does not match, ignore this document.
                                doc = null;
                            }
                        }
                    }
                }
            }
            catch (XmlException)
            {
                doc = null;
            }
            catch (NullReferenceException)
            {
                doc = null;
            }

            return doc;
        }

        #endregion Private Methods
    }
}