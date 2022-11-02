using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Gameplay
{
    [CreateAssetMenu(fileName = "New Data Spawner List", menuName = "Cliche/Data Spawner List", order = 0)]
    public class DataSpawnerList : ScriptableObject
    {
        public List<BaseDataBehavior> dataTypes;
    }
}