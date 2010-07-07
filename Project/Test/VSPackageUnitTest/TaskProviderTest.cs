﻿//-----------------------------------------------------------------------
// <copyright file="TaskProviderTest.cs" company="Microsoft">
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
namespace VSPackageUnitTest
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using Microsoft.Build.BuildEngine;
    using Microsoft.StyleCop.VisualStudio;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using VSPackageUnitTest.Mocks;

    /// <summary>
    ///This is a test class for TaskProviderTest and is intended
    ///to contain all TaskProviderTest Unit Tests
    ///</summary>
    [TestClass()]
    [DeploymentItem("Microsoft.VisualStudio.QualityTools.MockObjectFramework.dll")]
    [DeploymentItem("Microsoft.StyleCop.VSPackage.dll")]
    public class TaskProviderTest : BasicUnitTest
    {
        private MockServiceProvider serviceProvider;

        /// <summary>
        /// Unit Test Case for the constructor.
        /// </summary>
        [TestMethod()]
        public void ConstructorTest()
        {
            // Execute metod under test first time
            TaskProvider_Accessor target = new TaskProvider_Accessor(this.serviceProvider);
            Assert.IsNotNull(target, "Unable to instantiate TaskProvider.");
            Assert.IsNotNull(target.serviceProvider, "TaskProvider.provider returned null");
        }

        /// <summary>
        /// Use TestInitialize to run code before running each test 
        /// </summary>
        [TestInitialize()]
        public void MyTestInitialize()
        {
            this.serviceProvider = new MockServiceProvider();
        }
    }
}
