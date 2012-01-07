﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyInfo.cs" company="http://stylecop.codeplex.com">
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
//   AssemblyInfo.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#region Using Directives

using System;
using System.Reflection;
using System.Runtime.InteropServices;

using StyleCop.ReSharper51.Properties;

#endregion

[assembly: AssemblyTitle("StyleCop R# 5.1 Plugin")]
[assembly: AssemblyDescription(Constants.DescriptionShort)]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct(Constants.ProductName)]
[assembly: AssemblyCopyright("MS-PL")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]

// Many of the R# base types are not CLS compliant so we can't be.
[assembly: CLSCompliant(false)]

[assembly: Guid("8c180a9c-bd4d-45eb-becf-02f78e57ed91")]