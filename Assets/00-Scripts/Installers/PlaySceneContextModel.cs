using Match3.General;
using UnityEngine;

namespace Match3.Installers
{
    [CreateAssetMenu(fileName ="PlaySceneContextModel" ,menuName = "Match3/Installers/PlaySceneContextModel")]
    public class PlaySceneContextModel : ScriptableObject
    {
        [field: SerializeField] public GridElement gridElementPrefab { get; private set; }
    }
}
