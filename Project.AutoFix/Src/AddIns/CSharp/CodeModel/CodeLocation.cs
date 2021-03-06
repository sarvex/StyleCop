﻿//-----------------------------------------------------------------------
// <copyright file="CodeLocation.cs">
//   MS-PL
// </copyright>
// <license>
//   This source code is subject to terms and conditions of the Microsoft 
//   Public License. A copy of the license can be found in the License.html 
//   file at the root of this distribution. 
//   By using this source code in any fashion, you are agreeing to be bound 
//   by the terms of the Microsoft Public License. You must not remove this 
//   notice, or any other, from this software.
// </license>
//-----------------------------------------------------------------------
namespace StyleCop.CSharp.CodeModel
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Describes a location within a code document.
    /// </summary>
    /// <subcategory>other</subcategory>
    public sealed class CodeLocation
    {
        #region Public Static Readonly Fields

        /// <summary>
        /// An empty code location.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "A static value.")]
        public static readonly CodeLocation Empty = new CodeLocation();

        #endregion Public Static Readonly Fields

        #region Private Fields

        /// <summary>
        /// The starting position within the code.
        /// </summary>
        private CodePoint startPoint;

        /// <summary>
        /// The ending position within the code.
        /// </summary>
        private CodePoint endPoint;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the CodeLocation class.
        /// </summary>
        public CodeLocation()
        {
            this.startPoint = new CodePoint();
            this.endPoint = new CodePoint();
        }

        /// <summary>
        /// Initializes a new instance of the CodeLocation class.
        /// </summary>
        /// <param name="index">The index of the first character of the item within the file.</param>
        /// <param name="endIndex">The index of the last character of the item within the file.</param>
        /// <param name="indexOnLine">The index of the first character of the item within the line
        /// that it appears on.</param>
        /// <param name="endIndexOnLine">The index of the last character of the item within the line
        /// that it appears on.</param>
        /// <param name="lineNumber">The line number that this item begins on.</param>
        /// <param name="endLineNumber">The line number that this item ends on.</param>
        [SuppressMessage(
            "Microsoft.Naming",
            "CA1702:CompoundWordsShouldBeCasedCorrectly",
            MessageId = "OnLine",
            Justification = "On Line is two words in this context.")]
        public CodeLocation(int index, int endIndex, int indexOnLine, int endIndexOnLine, int lineNumber, int endLineNumber)
        {
            Param.RequireGreaterThanOrEqualToZero(index, "index");
            Param.RequireGreaterThanOrEqualTo(endIndex, index, "endIndex");
            Param.RequireGreaterThanOrEqualToZero(indexOnLine, "indexOnLine");
            Param.RequireGreaterThanOrEqualToZero(endIndexOnLine, "endIndexOnLine");
            Param.RequireGreaterThanZero(lineNumber, "lineNumber");
            Param.RequireGreaterThanOrEqualTo(endLineNumber, lineNumber, "endLineNumber");

#if DEBUG
            // If the entire segment is on the same line, make sure the end index is greater or equal to the start index.
            if (lineNumber == endLineNumber)
            {
                CsLanguageService.Debug.Assert(
                    endIndexOnLine >= indexOnLine,
                    "The end index must be greater than the start index, since they are both on the same line.");
            }
#endif

            this.startPoint = new CodePoint(index, indexOnLine, lineNumber);
            this.endPoint = new CodePoint(endIndex, endIndexOnLine, endLineNumber);
        }

        #endregion Public Constructors

        #region Public Properties

        /// <summary>
        /// Gets the starting point within the code.
        /// </summary>
        public CodePoint StartPoint
        {
            get
            {
                return this.startPoint;
            }
        }

        /// <summary>
        /// Gets the ending point within the code.
        /// </summary>
        [SuppressMessage(
            "Microsoft.Naming",
            "CA1702:CompoundWordsShouldBeCasedCorrectly",
            MessageId = "EndPoint",
            Justification = "EndPoint is two words in this context, to be consistent with StartPoint")]
        public CodePoint EndPoint
        {
            get
            {
                return this.endPoint;
            }
        }

        /// <summary>
        /// Gets the starting line number of this code location.
        /// </summary>
        public int LineNumber
        {
            get
            {
                return this.startPoint.LineNumber;
            }
        }

        /// <summary>
        /// Gets the number of lines spanned by the start and end points.
        /// </summary>
        /// <remarks>The line span will always be at least one.</remarks>
        public int LineSpan
        {
            get
            {
                if (this.startPoint != null && this.endPoint != null)
                {
                    return this.endPoint.LineNumber - this.startPoint.LineNumber + 1;
                }

                return 0;
            }
        }

        #endregion Public Properties

        #region Public Static Methods

        /// <summary>
        /// Joins the two given locations.
        /// </summary>
        /// <param name="location1">The first location to join.</param>
        /// <param name="location2">The second location to join.</param>
        /// <returns>Returns the joined <see cref="CodeLocation"/>.</returns>
        public static CodeLocation Join(CodeLocation location1, CodeLocation location2)
        {
            Param.Ignore(location1, location2);

            if (location1 == null)
            {
                return location2 ?? CodeLocation.Empty;
            }
            else if (location2 == null)
            {
                return location1;
            }
            else
            {
                // Figure out which IndexOnLine and EndIndexOnLine to use.
                int indexOnLine;
                int endIndexOnLine;
                if (location1.StartPoint.LineNumber == location2.StartPoint.LineNumber)
                {
                    indexOnLine = Math.Min(location1.StartPoint.IndexOnLine, location2.StartPoint.IndexOnLine);
                    endIndexOnLine = Math.Max(location1.EndPoint.IndexOnLine, location2.EndPoint.IndexOnLine);
                }
                else if (location1.StartPoint.LineNumber < location2.StartPoint.LineNumber)
                {
                    indexOnLine = location1.StartPoint.IndexOnLine;
                    endIndexOnLine = location2.EndPoint.IndexOnLine;
                }
                else
                {
                    indexOnLine = location2.StartPoint.IndexOnLine;
                    endIndexOnLine = location1.EndPoint.IndexOnLine;
                }

                return new CodeLocation(
                    Math.Min(location1.StartPoint.Index, location2.StartPoint.Index),
                    Math.Max(location1.EndPoint.Index, location2.EndPoint.Index),
                    indexOnLine,
                    endIndexOnLine,
                    Math.Min(location1.StartPoint.LineNumber, location2.StartPoint.LineNumber),
                    Math.Max(location2.EndPoint.LineNumber, location2.EndPoint.LineNumber));
            }
        }

        #endregion Public Static Methods
    }
}
