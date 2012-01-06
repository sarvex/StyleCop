//-----------------------------------------------------------------------
// <copyright file="StringProperty.cs">
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
namespace StyleCop
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    /// <summary>
    /// A string property.
    /// </summary>
    public class StringProperty : PropertyValue<string>
    {
        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the StringProperty class.
        /// </summary>
        /// <param name="propertyDescriptor">The property descriptor that this value represents.</param>
        /// <param name="value">The value of the property.</param>
        public StringProperty(PropertyDescriptor<string> propertyDescriptor, string value)
            : base(propertyDescriptor, value)
        {
            Param.RequireNotNull(propertyDescriptor, "propertyDescriptor");
            Param.Ignore(value);
        }

        /// <summary>
        /// Initializes a new instance of the StringProperty class.
        /// </summary>
        /// <param name="propertyContainer">The container of this property.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="value">The value of the property.</param>
        public StringProperty(IPropertyContainer propertyContainer, string propertyName, string value)
            : base(propertyContainer, propertyName, value)
        {
            Param.RequireNotNull(propertyContainer, "propertyContainer");
            Param.RequireValidString(propertyName, "propertyName");
            Param.Ignore(value);
        }

        #endregion Public Constructors

        #region Public Override Methods

        /// <summary>
        /// Clones the contents of the property.
        /// </summary>
        /// <returns>Returns the cloned property.</returns>
        public override PropertyValue Clone()
        {
            return new StringProperty((PropertyDescriptor<string>)this.PropertyDescriptor, this.Value);
        }

        #endregion Public Override Methods
    }
}