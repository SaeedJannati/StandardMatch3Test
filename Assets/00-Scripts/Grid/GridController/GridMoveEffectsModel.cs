using UnityEngine;

namespace Match3.General
{
    [CreateAssetMenu(fileName = "GridMoveEffectsModel",menuName = "Match3/General/Grid/GridMoveEffectsModel")]
    public class GridMoveEffectsModel:ScriptableObject
    {
        [field: SerializeField] public float tileSwipePeriod { get; private set; } = .3f;
        [field: SerializeField] public float tileFadePeriod { get; private set; } = .3f;
        [field: SerializeField] public float dropPeriod { get; private set; } = .5f;
    }
}