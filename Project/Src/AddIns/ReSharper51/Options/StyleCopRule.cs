﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StyleCopRule.cs" company="http://stylecop.codeplex.com">
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
//   Class to represent a StyleCop rule.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace StyleCop.ReSharper51.Options
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Xml;

    #endregion

    /// <summary>
    /// Class to represent a StyleCop rule.
    /// </summary>
    public class StyleCopRule
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StyleCopRule"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the rule.
        /// </param>
        /// <param name="ruleID">
        /// The IDof the rule.
        /// </param>
        /// <param name="description">
        /// The rules description.
        /// </param>
        public StyleCopRule(string name, string ruleID, string description)
        {
            this.Name = name;
            this.RuleID = ruleID;
            this.Description = description;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name of the rule.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the rule ID.
        /// </summary>
        /// <value>
        /// The rule ID.
        /// </value>
        public string RuleID { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the rules.
        /// </summary>
        /// <param name="styleCopCore">
        /// The style cop core.
        /// </param>
        /// <returns>
        /// A Dictionary of all the StyleCop rules indexed by their <see cref="SourceAnalyzer"/>.
        /// </returns>
        public static Dictionary<SourceAnalyzer, List<StyleCopRule>> GetRules(StyleCopCore styleCopCore)
        {
            var loadedAnalyzers = GetSourceAnalyzers(styleCopCore);
            var rulesDictionary = GetRules(loadedAnalyzers);

            return rulesDictionary;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the rule.
        /// </summary>
        /// <param name="node">
        /// The <see cref="XmlNode"/> that defines a StyleCop rule.
        /// </param>
        /// <returns>
        /// The StyleCop rule defined in the given <see cref="XmlNode"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Rule Has No Name Attribute.
        /// </exception>
        private static StyleCopRule GetRule(XmlNode node)
        {
            XmlNode ruleNameNode = node.Attributes["Name"];

            if (ruleNameNode == null || string.IsNullOrEmpty(ruleNameNode.Value))
            {
                throw new ArgumentException("Rule Has No Name Attribute");
            }

            XmlNode ruleCheckIDNode = node.Attributes["CheckId"];
            XmlNode ruleDescriptionNode = node["Description"];

            var ruleName = ruleNameNode.Value;
            var ruleCheckID = ruleCheckIDNode.Value;
            var ruleDescription = ruleName;

            if (ruleDescriptionNode != null)
            {
                ruleDescription = TrimXmlContent(ruleDescriptionNode.InnerText);
            }

            return new StyleCopRule(ruleName, ruleCheckID, ruleDescription);
        }

        /// <summary>
        /// Gets the rules from the given RuleGroup XML Node.
        /// </summary>
        /// <param name="node">
        /// The RuleGroup XMLNode.
        /// </param>
        /// <returns>
        /// The found rules.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// RuleGroupHasNoNameAttribute.
        /// </exception>
        private static List<StyleCopRule> GetRuleGroupRules(XmlNode node)
        {
            var ruleGroupNameAttribute = node.Attributes["Name"];

            if (ruleGroupNameAttribute == null || ruleGroupNameAttribute.Value.Length == 0)
            {
                throw new ArgumentException("RuleGroupHasNoNameAttribute");
            }

            var nodeRules = GetRulesFromXml(node);

            return nodeRules;
        }

        /// <summary>
        /// Gets the rules.
        /// </summary>
        /// <param name="analysers">
        /// The analyzers.
        /// </param>
        /// <returns>
        /// A Dictionary of all the StyleCop rules indexed by their <see cref="SourceAnalyzer"/>.
        /// </returns>
        private static Dictionary<SourceAnalyzer, List<StyleCopRule>> GetRules(IEnumerable<SourceAnalyzer> analysers)
        {
            var rules = new Dictionary<SourceAnalyzer, List<StyleCopRule>>();

            foreach (var analyzer in analysers)
            {
                var analyzerRules = GetRules(analyzer);
                rules.Add(analyzer, analyzerRules);
            }

            return rules;
        }

        /// <summary>
        /// Gets the rules.
        /// </summary>
        /// <param name="analyzer">
        /// The analyzer.
        /// </param>
        /// <returns>
        /// A list of all the rules for the <see cref="SourceAnalyzer"/>.
        /// </returns>
        private static List<StyleCopRule> GetRules(SourceAnalyzer analyzer)
        {
            var xmlDocument = StyleCopCore.LoadAddInResourceXml(analyzer.GetType(), null);
            var xmlDefinedRules = GetRulesFromXml(xmlDocument);

            return xmlDefinedRules;
        }

        /// <summary>
        /// Gets the rules from XML.
        /// </summary>
        /// <param name="document">
        /// The document.
        /// </param>
        /// <returns>
        /// A List of the StyleCop rules defined in the XML.
        /// </returns>
        private static List<StyleCopRule> GetRulesFromXml(XmlDocument document)
        {
            var rules = new List<StyleCopRule>();

            Param.RequireNotNull(document, "document");
            Param.RequireNotNull(document.DocumentElement, "xml.DocumentElement");

            if (document.DocumentElement != null)
            {
                foreach (XmlNode node in document.DocumentElement.ChildNodes)
                {
                    if (node.Name == "Rules")
                    {
                        var nodeRules = GetRulesFromXml(node);
                        rules.AddRange(nodeRules);
                    }
                }
            }

            return rules;
        }

        /// <summary>
        /// Gets the rules from XML.
        /// </summary>
        /// <param name="rulesNode">
        /// The rules node.
        /// </param>
        /// <returns>
        /// The StyleCop rules defined in the <see cref="XmlNode"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Rule Has No Name Attribute.
        /// </exception>
        private static List<StyleCopRule> GetRulesFromXml(XmlNode rulesNode)
        {
            var rules = new List<StyleCopRule>();

            foreach (XmlNode node in rulesNode.ChildNodes)
            {
                switch (node.Name)
                {
                    case "RuleGroup":
                        {
                            var groupRules = GetRuleGroupRules(node);
                            rules.AddRange(groupRules);
                        }

                        break;
                    case "Rule":
                        {
                            var rule = GetRule(node);
                            rules.Add(rule);
                        }

                        break;
                }
            }

            return rules;
        }

        /// <summary>
        /// Gets the source analyzers.
        /// </summary>
        /// <param name="styleCopCore">
        /// The style cop core.
        /// </param>
        /// <returns>
        /// The list of <see cref="SourceAnalyzer"/> for StyleCop.
        /// </returns>
        private static List<SourceAnalyzer> GetSourceAnalyzers(StyleCopCore styleCopCore)
        {
            var analyzers = new List<SourceAnalyzer>();

            foreach (var parser in styleCopCore.Parsers)
            {
                analyzers.AddRange(parser.Analyzers);
            }

            return analyzers;
        }

        /// <summary>
        /// Trims the content of the XML.
        /// </summary>
        /// <param name="content">
        /// The content.
        /// </param>
        /// <returns>
        /// The trimmed XML content.
        /// </returns>
        private static string TrimXmlContent(string content)
        {
            if (content == null)
            {
                return null;
            }

            var length = 0;
            var characterArray = new char[content.Length];
            var flag = false;

            for (var i = 0; i < content.Length; i++)
            {
                var c = content[i];

                if (flag)
                {
                    if (char.IsWhiteSpace(c) && (i < (content.Length - 1)))
                    {
                        if (!char.IsWhiteSpace(content[i + 1]))
                        {
                            characterArray[length++] = ' ';
                        }
                    }
                    else
                    {
                        characterArray[length++] = c;
                    }
                }
                else if (!char.IsWhiteSpace(c))
                {
                    characterArray[length++] = c;
                    flag = true;
                }
            }

            if (length == characterArray.Length)
            {
                return content;
            }

            return new string(characterArray, 0, length);
        }

        #endregion
    }
}