﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.IO;
using System.Collections.ObjectModel;

namespace UnitTests
{
    /// <summary>
    /// Summary description for CredentialStorageUnitTest
    /// </summary>
    [TestClass]
    public class CredentialStorageUnitTest
    {
        public const string ModuleName = "Erwine.Leonard.T.CredentialStorage";
        public const string RelativeModulePath = @"CredentialStorage\CredentialStorage";

        public CredentialStorageUnitTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void ImportCredentialStorageTestMethod()
        {
            PowerShellHelper.TestLoadModule(TestContext, ModuleName, RelativeModulePath, ".psm1", Path.GetFullPath(@"..\..\..\IOUtility\IOUtility\Erwine.Leonard.T.IOUtility.psd1"), Path.GetFullPath(@"..\..\..\XmlUtility\XmlUtility\Erwine.Leonard.T.XmlUtility.psd1"));
        }
    }
}
