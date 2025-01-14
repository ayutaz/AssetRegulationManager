﻿// --------------------------------------------------------------
// Copyright 2022 CyberAgent, Inc.
// --------------------------------------------------------------

using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetFilterImpl;
using NUnit.Framework;

namespace AssetRegulationManager.Tests.Editor.AssetFilterImpl
{
    internal sealed class ExtensionBasedAssetFilterTest
    {
        [Test]
        public void IsMatch_RegisterMatchedExtension_ReturnTrue()
        {
            var filter = new ExtensionBasedAssetFilter();
            filter.Extension.Value = "test";
            filter.SetupForMatching();
            Assert.That(filter.IsMatch("Test.test", null), Is.True);
        }

        [Test]
        public void IsMatch_RegisterMatchedExtensionWithDot_ReturnTrue()
        {
            var filter = new ExtensionBasedAssetFilter();
            filter.Extension.Value = ".test";
            filter.SetupForMatching();
            Assert.That(filter.IsMatch("Test.test", null), Is.True);
        }

        [Test]
        public void IsMatch_RegisterNotMatchedExtension_ReturnFalse()
        {
            var filter = new ExtensionBasedAssetFilter();
            filter.Extension.Value = "test2";
            filter.SetupForMatching();
            Assert.That(filter.IsMatch("Test.test", null), Is.False);
        }

        [Test]
        public void IsMatch_RegisterExtensionsAndContainsMatched_ReturnTrue()
        {
            var filter = new ExtensionBasedAssetFilter();
            filter.Extension.IsListMode = true;
            filter.Extension.AddValue("test");
            filter.Extension.AddValue("test2");
            filter.SetupForMatching();
            Assert.That(filter.IsMatch("Test.test", null), Is.True);
        }

        [Test]
        public void IsMatch_RegisterExtensionsAndNotContainsMatched_ReturnFalse()
        {
            var filter = new ExtensionBasedAssetFilter();
            filter.Extension.IsListMode = true;
            filter.Extension.AddValue("test2");
            filter.Extension.AddValue("test3");
            filter.SetupForMatching();
            Assert.That(filter.IsMatch("Test.test", null), Is.False);
        }
    }
}
