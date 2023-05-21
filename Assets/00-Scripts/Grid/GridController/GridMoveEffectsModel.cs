using UnityEngine;

namespace Match3.General
{
    [CreateAssetMenu(fileName = "GridMoveEffectsModel",menuName = "Match3/General/Grid/GridMoveEffectsModel")]
    public class GridMoveEffectsModel:ScriptableObject
    {
        [field: SerializeField] public float spawnPeriod { get; private set; }=.4f;
        [field: SerializeField] public float tileSwipePeriod { get; private set; } = .3f;
        [field: SerializeField] public float tileFadePeriod { get; private set; } = .3f;
        [field: SerializeField] public float dropPeriod { get; private set; } = .5f;
        [field: SerializeField] public float shuffleFadePeriod { get; private set; } = .3f;
        [field:SerializeField]public float shuffleDelayPeriod { get; private set; } = .3f;
    }
}