﻿//-----------------------------------------------------------------------
// <copyright file="FileHeader.cs" company="Microsoft">
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
namespace Microsoft.StyleCop.CSharp.CodeModel
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// Represents a file header.
    /// </summary>
    public sealed class FileHeader : CodeUnit
    {
        #region Private Fields

        /// <summary>
        /// Indicates whether this is an auto-generated header.
        /// </summary>
        private CodeUnitProperty<bool> generated;

        /// <summary>
        /// The collection of lines within the header.
        /// </summary>
        private CodeUnitProperty<ICollection<SingleLineComment>> headerLines;

        /// <summary>
        /// Indicates whether the header is empty.
        /// </summary>
        private CodeUnitProperty<bool> empty;

        /// <summary>
        /// The raw header xml.
        /// </summary>
        private CodeUnitProperty<string> headerXml;

        /// <summary>
        /// The header xml with formatting included.
        /// </summary>
        private CodeUnitProperty<string> formattedHeaderXml;

        #endregion Private Fields

        #region Internal Constructors

        /// <summary>
        /// Initializes a new instance of the FileHeader class.
        /// </summary>
        /// <param name="proxy">Proxy object for the header.</param>
        internal FileHeader(CodeUnitProxy proxy) 
            : base(proxy, CodeUnitType.FileHeader)
        {
            Param.AssertNotNull(proxy, "proxy");
        }

        #endregion Internal Constructors

        #region Public Override Properties

        /// <summary>
        /// Gets a value indicating whethre the file header is an auto-generated header.
        /// </summary>
        public override bool Generated
        {
            get
            {
                this.ValidateEditVersion();

                if (!this.generated.Initialized)
                {
                    this.generated.Value = false;

                    // Determine whether this is an auto-generated header.
                    try
                    {
                        string header = this.HeaderXml;

                        if (!string.IsNullOrEmpty(header))
                        {
                            var doc = new XmlDocument();
                            doc.LoadXml(string.Format(CultureInfo.InvariantCulture, "<root>{0}</root>", header));

                            // Check whether the header has the autogenerated tag.
                            XmlNode node = doc.DocumentElement["autogenerated"];
                            if (node != null)
                            {
                                // Set this as generated code.
                                this.generated.Value = true;
                            }
                            else
                            {
                                node = doc.DocumentElement["auto-generated"];
                                if (node != null)
                                {
                                    // Set this as generated code.
                                    this.generated.Value = true;
                                }
                            }
                        }
                    }
                    catch (XmlException)
                    {
                    }
                }

                if (this.generated.Value)
                {
                    return true;
                }

                return base.Generated;
            }
        }

        #endregion Public Override Properties

        #region Public Properties

        /// <summary>
        /// Gets the collection of header lines within the header.
        /// </summary>
        public ICollection<SingleLineComment> HeaderLines
        {
            get
            {
                this.ValidateEditVersion();

                if (!this.headerLines.Initialized)
                {
                    this.headerLines.Value = new List<SingleLineComment>(this.GetChildren<SingleLineComment>()).AsReadOnly();
                }

                return this.headerLines.Value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the header contains anything other than whitespace.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                this.ValidateEditVersion();

                if (!this.empty.Initialized)
                {
                    this.empty.Value = true;

                    for (SingleLineComment headerLine = this.FindFirstChild<SingleLineComment>(); headerLine != null; headerLine = headerLine.FindNextSibling<SingleLineComment>())
                    {
                        if (!this.empty.Value)
                        {
                            break;
                        }
                        
                        string text = headerLine.Text;
                        if (!string.IsNullOrEmpty(text))
                        {
                            // Start searching at index 2 to move past the initial '//' slashes.
                            for (int i = 2; i < text.Length; ++i)
                            {
                                if (!char.IsWhiteSpace(text[i]))
                                {
                                    this.empty.Value = false;
                                    break;
                                }
                            }
                        }
                    }
                }

                return this.empty.Value;
            }
        }

        /// <summary>
        /// Gets the header Xml.
        /// </summary>
        public string HeaderXml
        {
            get
            {
                this.ValidateEditVersion();

                if (!this.headerXml.Initialized)
                {
                    StringBuilder text = new StringBuilder();

                    for (SingleLineComment headerLine = this.FindFirstChild<SingleLineComment>(); headerLine != null; headerLine = headerLine.FindNextSibling<SingleLineComment>())
                    {
                        text.Append(ExtractHeaderLineText(headerLine));
                    }

                    this.headerXml.Value = text.ToString();
                }

                return this.headerXml.Value;
            }
        }

        /// <summary>
        /// Gets the header Xml with original formatting included.
        /// </summary>
        public string FormattedHeaderXml
        {
            get
            {
                this.ValidateEditVersion();

                if (!this.formattedHeaderXml.Initialized)
                {
                    StringBuilder text = new StringBuilder();

                    for (CodeUnit item = this.FindFirstChild<CodeUnit>(); item != null; item = item.FindNextSibling<CodeUnit>())
                    {
                        if (item.Is(CommentType.SingleLineComment))
                        {
                            text.Append(ExtractHeaderLineText((SingleLineComment)item));
                        }
                        else if (item.Is(LexicalElementType.EndOfLine))
                        {
                            text.Append('\n');
                        }
                    }

                    this.formattedHeaderXml.Value = text.ToString();
                }

                return this.formattedHeaderXml.Value;
            }
        }

        #endregion Public Properties

        #region Protected Override Methods

        /// <summary>
        /// Resets the contents of the item.
        /// </summary>
        protected override void Reset()
        {
            base.Reset();

            this.generated.Reset();
            this.headerLines.Reset();
            this.empty.Reset();
            this.headerXml.Reset();
            this.formattedHeaderXml.Reset();
        }

        #endregion Protected Override Methods

        #region Private Static Methods

        /// <summary>
        /// Extracts the raw text from a header line.
        /// </summary>
        /// <param name="headerLine">The header line.</param>
        /// <returns>Returns the raw text.</returns>
        private static string ExtractHeaderLineText(SingleLineComment headerLine)
        {
            Param.AssertNotNull(headerLine, "headerLine");

            string headerLineText = headerLine.Text;
            if (headerLineText.StartsWith("//", StringComparison.Ordinal))
            {
                // Typically, the header line will begin with a single space after the three slashes. We should not
                // consider this space to be part of the documentation, so skip past it.
                int startIndex = 2;
                if (headerLineText.Length > 2 && headerLineText[2] == ' ')
                {
                    startIndex = 3;
                }

                headerLineText = headerLineText.Substring(startIndex, headerLineText.Length - startIndex);
            }

            return headerLineText;
        }

        #endregion Private Static Methods
    }
}
