
using System.Collections.Generic;
using System.Linq;
using Match3.Auxiliary;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

namespace Match3.General
{
    [CreateAssetMenu(fileName = "GridGeneratorModel", menuName = "Match3/General/Grid/GridGeneratorModel")]
    public class GridGeneratorModel : ScriptableObject
    {
        #region Properties

        [field: SerializeField, Min(3)] public int rowCount { get; private set; }
        [field: SerializeField, Min(3)] public int coloumnCount { get; private set; }
        [field: SerializeField, Min(2)] public int colourCount { get; private set; }
        [field: SerializeField] public List<Color> colours { get; private set; }
        [field: SerializeField] public bool showMockData { get; private set; }

        [field: SerializeField, ShowIf(nameof(showMockData))]
        public string mockGrid { get; private set; }

        [field: SerializeField, ShowIf(nameof(showMockData))]
        public List<int> mockGridElementsValue { get; private set; }

        #endregion

        #region Methods

        [Button, ShowIf(nameof(showMockData))]
        public void ExtractMockGridValues()
        {
            mockGridElementsValue = JsonConvert.DeserializeObject<List<int>>(mockGrid);
        }

        [Button]
        void CreateGrid()
        {
            if (!Application.isPlaying)
            {
                GameLogger.Log("You should be in play mode in order to creat the grid!",GameLogger.Colours.lightRed);
                return;
                
            }
            GridControllerLogic.onCreateGridRequest?.Invoke();
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (colours == default)
                colours = new();
            if (colours.Count > colourCount)
            {
                colours = colours.Take(colourCount).ToList();
                EditorUtility.SetDirty(this);
                return;
            }

            for (int i = colours.Count; i < colourCount; i++)
            {
                var col = Color.white;
                col.a = 1.0f;
                col = UnityEngine.Random.ColorHSV(.00f, 1.0f,.10f,.90f);
                colours.Add(col);
                EditorUtility.SetDirty(this);
                
            }


#endif
        }

        #endregion
    }
}