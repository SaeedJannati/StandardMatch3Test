using UnityEngine;

namespace Match3.General
{
    [CreateAssetMenu(fileName = "GridGeneratorModel", menuName = "Match3/General/Grid/GridGeneratorModel")]
    public class GridGeneratorModel : ScriptableObject
    {
        [field: SerializeField] public int rowCount { get; private set; }
        [field: SerializeField] public int coloumnCount { get; private set; }
        [field: SerializeField,Min(2)] public int colourCount { get; private set; }
    }
}

