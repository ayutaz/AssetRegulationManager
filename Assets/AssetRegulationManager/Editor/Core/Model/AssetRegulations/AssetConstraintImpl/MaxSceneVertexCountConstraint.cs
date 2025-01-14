﻿using System;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace AssetRegulationManager.Editor.Core.Model.AssetRegulations.AssetConstraintImpl
{
    /// <summary>
    ///     Base class of the Limitation of the maximum number of vertices in a Scene.
    /// </summary>
    [Serializable]
    [AssetConstraint("Mesh/Max Vertex Count in Scene", "Max Vertex Count in Scene")]
    public class MaxSceneVertexCountConstraint : AssetConstraint<SceneAsset>
    {
        [SerializeField] private int _maxCount;
        [SerializeField] private bool _excludeInactive;
        [SerializeField] private bool _allowDuplicateCount;
        private int _latestValue;

        public int MaxCount
        {
            get => _maxCount;
            set => _maxCount = value;
        }

        public bool ExcludeInactive
        {
            get => _excludeInactive;
            set => _excludeInactive = value;
        }

        public bool AllowDuplicateCount
        {
            get => _allowDuplicateCount;
            set => _allowDuplicateCount = value;
        }

        public override string GetDescription()
        {
            var optionsDescription = new StringBuilder();
            optionsDescription.Append("(");
            optionsDescription.Append(_excludeInactive ? "Exclude Inactive" : "Include Inactive");
            optionsDescription.Append(", ");
            optionsDescription.Append(_allowDuplicateCount ? "Allow Duplicate Count" : "Disallow Duplicate Count");
            optionsDescription.Append(")");
            var desc = $"Max Vertex Count in Scene: {_maxCount} {optionsDescription}";
            return desc;
        }

        public override string GetLatestValueAsText()
        {
            return _latestValue.ToString();
        }

        protected override bool CheckInternal(SceneAsset asset)
        {
            Assert.IsNotNull(asset);

            var sceneAssetPath = AssetDatabase.GetAssetPath(asset);
            if (!AssetConstraintUtility.OpenScene(sceneAssetPath))
            {
                throw new Exception("The process was canceled by user operation.");
            }

            var rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            var meshes = rootGameObjects.SelectMany(x =>
                AssetConstraintUtility.GetMeshesFromGameObject(x, true, !_excludeInactive));
            if (!_allowDuplicateCount)
            {
                meshes = meshes.Distinct();
            }

            var vertexCount = meshes.Aggregate(0, (x, m) => x + m.vertexCount);

            _latestValue = vertexCount;
            return vertexCount <= _maxCount;
        }
    }
}
