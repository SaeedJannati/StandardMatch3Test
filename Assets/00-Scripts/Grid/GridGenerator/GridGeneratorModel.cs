using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Match3.General
{
    [CreateAssetMenu(fileName = "GridGeneratorModel", menuName = "Match3/General/Grid/GridGeneratorModel")]
    public class GridGeneratorModel : ScriptableObject
    {
        #region Properties
        [field: SerializeField,Min(3)] public int rowCount { get; private set; }
        [field: SerializeField,Min(3)] public int coloumnCount { get; private set; }
        [field: SerializeField,Min(2)] public int colourCount { get; private set; }
        [field: SerializeField] public string mockGrid { get; private set; }
        [field: SerializeField] public List<int> mockGridElementsValue { get; private set; }
        #endregion

        #region Methods

        [Button]
        public void ExtractMockGridValues()
        {
            mockGridElementsValue = JsonConvert.DeserializeObject<List<int>>(mockGrid);
        }


        #endregion
    }
}

