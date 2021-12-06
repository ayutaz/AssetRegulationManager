// --------------------------------------------------------------
// Copyright 2021 CyberAgent, Inc.
// --------------------------------------------------------------

using System;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations
{
    /// <summary>
    ///     The base generic class of the Asset Regulation Entry.
    /// </summary>
    public abstract class AssetRegulationEntry<TAsset> : IAssetRegulationEntry where TAsset : Object
    {
        public abstract string Label { get; }
        public abstract string Description { get; }
        public abstract void DrawGUI();

        bool IAssetRegulationEntry.RunTest(Object obj)
        {
            var asset = obj as TAsset;
            Assert.IsFalse(asset == null);

            return RunTest(asset);
        }

        /// <summary>
        ///     Determine if you are following the regulations.
        ///     <param name="asset"></param>
        /// </summary>
        protected abstract bool RunTest(TAsset asset);
    }
}