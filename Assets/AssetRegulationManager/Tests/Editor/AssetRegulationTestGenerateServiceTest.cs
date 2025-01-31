﻿// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using AssetRegulationManager.Editor.Core.Data;
using AssetRegulationManager.Editor.Core.Model;
using AssetRegulationManager.Editor.Core.Model.Adapters;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations;
using AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetFilterImpl;
using NUnit.Framework;

namespace AssetRegulationManager.Tests.Editor
{
    public class AssetRegulationTestGenerateServiceTest
    {
        private const string AssetPath01 = "Assets/Tests/Test01.asset";
        private const string AssetPath02 = "Assets/Tests/Test02.asset";

        [Test]
        public void RunWithAssetFilter_GenerateSuccessfully()
        {
            var store = CreateFakeTestStore();
            var service = CreateFakeTestGenerateService(store);
            service.Run("Test");

            var test01 = store.FilteredTests.First(x => x.AssetPath.Equals(AssetPath01));
            var test02 = store.FilteredTests.First(x => x.AssetPath.Equals(AssetPath02));
            Assert.That(store.Tests.Count, Is.EqualTo(2));
            Assert.That(test01.Entries.Count, Is.EqualTo(1));
            Assert.That(test01.Entries.Values.Count(x => x.Constraint is FakeAssetConstraint), Is.EqualTo(1));
            Assert.That(test02.Entries.Count, Is.EqualTo(0));
        }

        [Test]
        public void RunWithAssetFilter_PassEmpty_NoTestsAreGenerated()
        {
            var store = CreateFakeTestStore();
            var service = CreateFakeTestGenerateService(store);
            service.Run("");

            Assert.That(store.Tests.Count, Is.EqualTo(0));
        }

        [Test]
        public void RunWithAssetFilter_PassNonExistentAssetName_NoTestsAreGenerated()
        {
            var store = CreateFakeTestStore();
            var service = CreateFakeTestGenerateService(store);
            service.Run("NotExist");

            Assert.That(store.Tests.Count, Is.EqualTo(0));
        }

        [Test]
        public void RunWithAssetFilter_AddValidDescriptionFilter_GenerateSuccessfully()
        {
            var store = CreateFakeTestStore();
            var service = CreateFakeTestGenerateService(store);
            var descriptionFilters = new List<string> { "Regulation1" };
            service.Run("Test", descriptionFilters);

            var test01 = store.FilteredTests.First(x => x.AssetPath.Equals(AssetPath01));
            var test02 = store.FilteredTests.First(x => x.AssetPath.Equals(AssetPath02));
            Assert.That(store.Tests.Count, Is.EqualTo(2));
            Assert.That(test01.Entries.Count, Is.EqualTo(1));
            Assert.That(test01.Entries.Values.Count(x => x.Constraint is FakeAssetConstraint), Is.EqualTo(1));
            Assert.That(test02.Entries.Count, Is.EqualTo(0));
        }

        [Test]
        public void RunWithAssetFilter_AddInvalidDescriptionFilter_GenerateSuccessfully()
        {
            var store = CreateFakeTestStore();
            var service = CreateFakeTestGenerateService(store);
            var descriptionFilters = new List<string> { "Invalid" };
            service.Run("Test", descriptionFilters);

            var test01 = store.FilteredTests.First(x => x.AssetPath.Equals(AssetPath01));
            var test02 = store.FilteredTests.First(x => x.AssetPath.Equals(AssetPath02));
            Assert.That(store.Tests.Count, Is.EqualTo(2));
            Assert.That(test01.Entries.Count, Is.EqualTo(0));
            Assert.That(test02.Entries.Count, Is.EqualTo(0));
        }

        [Test]
        public void RunWithAssetPathFilters_GenerateSuccessfully()
        {
            var store = CreateFakeTestStore();
            var service = CreateFakeTestGenerateService(store);
            service.Run(new List<string> { AssetPath01, AssetPath02 });

            var test01 = store.FilteredTests.First(x => x.AssetPath.Equals(AssetPath01));
            var test02 = store.FilteredTests.First(x => x.AssetPath.Equals(AssetPath02));
            Assert.That(store.Tests.Count, Is.EqualTo(2));
            Assert.That(test01.Entries.Count, Is.EqualTo(1));
            Assert.That(test01.Entries.Values.Count(x => x.Constraint is FakeAssetConstraint), Is.EqualTo(1));
            Assert.That(test02.Entries.Count, Is.EqualTo(0));
        }

        [Test]
        public void RunWithAssetPathFilters_PassEmpty_AllTestsAreGenerated()
        {
            var store = CreateFakeTestStore();
            var service = CreateFakeTestGenerateService(store);
            service.Run(new List<string>());

            var test01 = store.FilteredTests.First(x => x.AssetPath.Equals(AssetPath01));
            var test02 = store.FilteredTests.First(x => x.AssetPath.Equals(AssetPath02));
            Assert.That(store.Tests.Count, Is.EqualTo(2));
            Assert.That(test01.Entries.Count, Is.EqualTo(1));
            Assert.That(test01.Entries.Values.Count(x => x.Constraint is FakeAssetConstraint), Is.EqualTo(1));
            Assert.That(test02.Entries.Count, Is.EqualTo(0));
        }

        [Test]
        public void RunWithAssetPathFilters_PassNonExistentAssetName_NoTestsAreGenerated()
        {
            var store = CreateFakeTestStore();
            var service = CreateFakeTestGenerateService(store);
            service.Run(new List<string> { "Assets/NotExist.asset" });

            Assert.That(store.Tests.Count, Is.EqualTo(0));
        }

        [Test]
        public void RunWithAssetPathFilters_AddValidDescriptionFilter_GenerateSuccessfully()
        {
            var store = CreateFakeTestStore();
            var service = CreateFakeTestGenerateService(store);
            var descriptionFilters = new List<string> { "Regulation1" };
            service.Run(new List<string> { AssetPath01, AssetPath02 }, descriptionFilters);

            var test01 = store.FilteredTests.First(x => x.AssetPath.Equals(AssetPath01));
            var test02 = store.FilteredTests.First(x => x.AssetPath.Equals(AssetPath02));
            Assert.That(store.Tests.Count, Is.EqualTo(2));
            Assert.That(test01.Entries.Count, Is.EqualTo(1));
            Assert.That(test01.Entries.Values.Count(x => x.Constraint is FakeAssetConstraint), Is.EqualTo(1));
            Assert.That(test02.Entries.Count, Is.EqualTo(0));
        }

        [Test]
        public void RunWithAssetPathFilters_AddInvalidDescriptionFilter_GenerateSuccessfully()
        {
            var store = CreateFakeTestStore();
            var service = CreateFakeTestGenerateService(store);
            var descriptionFilters = new List<string> { "Invalid" };
            service.Run(new List<string> { AssetPath01, AssetPath02 }, descriptionFilters);

            var test01 = store.FilteredTests.First(x => x.AssetPath.Equals(AssetPath01));
            var test02 = store.FilteredTests.First(x => x.AssetPath.Equals(AssetPath02));
            Assert.That(store.Tests.Count, Is.EqualTo(2));
            Assert.That(test01.Entries.Count, Is.EqualTo(0));
            Assert.That(test02.Entries.Count, Is.EqualTo(0));
        }

        private static AssetRegulationTestStore CreateFakeTestStore()
        {
            var testStore = new AssetRegulationTestStore();
            return testStore;
        }

        private static AssetRegulationTestGenerateService CreateFakeTestGenerateService(AssetRegulationTestStore store)
        {
            var assetDatabaseAdapter = new FakeAssetDatabaseAdapter();
            var regulation = CreateFakeAssetRegulation(AssetPath01, true);
            regulation.Name.Value = "Regulation1";
            var repository = new InMemoryAssetRegulationRepository();
            repository.Regulations.Add(regulation);
            return new AssetRegulationTestGenerateService(repository, store, assetDatabaseAdapter);
        }

        private static AssetRegulation CreateFakeAssetRegulation(string assetPathRegex, bool result)
        {
            var assetRegulation = new AssetRegulation();
            var assetFilter = new RegexBasedAssetFilter();
            assetFilter.AssetPathRegex.Value = assetPathRegex;
            var constraint = new FakeAssetConstraint(result);
            var group = assetRegulation.AddAssetGroup();
            group.AddFilter(assetFilter);
            assetRegulation.AddConstraint(constraint);
            return assetRegulation;
        }

        private class FakeAssetDatabaseAdapter : IAssetDatabaseAdapter
        {
            public IEnumerable<string> FindAssetPaths(string filter)
            {
                if (AssetPath01.Contains(filter)) yield return AssetPath01;

                if (AssetPath02.Contains(filter)) yield return AssetPath02;
            }

            TAsset IAssetDatabaseAdapter.LoadAssetAtPath<TAsset>(string assetPath)
            {
                throw new NotImplementedException();
            }

            public string[] GetAllAssetPaths()
            {
                return new[] { AssetPath01, AssetPath02 };
            }
        }
    }
}
