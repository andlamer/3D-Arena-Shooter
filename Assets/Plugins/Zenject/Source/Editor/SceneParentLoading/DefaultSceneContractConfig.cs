using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Zenject.Internal
{
    public class DefaultSceneContractConfig : UnityEngine.ScriptableObject
    {
        public const string ResourcePath = "ZenjectDefaultSceneContractConfig";

        public List<ContractInfo> DefaultContracts;

        [Serializable]
        public class ContractInfo
        {
            public string ContractName;
            public SceneAsset Scene;
        }
    }

}
