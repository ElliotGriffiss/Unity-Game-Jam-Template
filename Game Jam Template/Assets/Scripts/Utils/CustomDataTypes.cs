using System;
using UnityEngine;

namespace CustomDataTypes
{
    [Serializable]
    public struct LevelData
    {
        public GameObject LevelParent;
        public Vector3Int SpawnPoint;
    }
}
