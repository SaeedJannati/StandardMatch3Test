using Match3.Auxiliary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Match3.General.MoveTest
{
    [CreateAssetMenu(fileName = "MoveTestControllerModel",menuName = "Match3/General/MoveTest/MoveTestControllerModel")]
    public class MoveTestControllerModel : ScriptableObject
    {
        #region Properties
        [field: SerializeField] public bool isGraphicalTest { get; private set; } = true;
        [field: SerializeField] public int testMoveCount { get; private set; }
        #endregion

        #region Methods

        [Button]
        void RunTest()
        {
            MoveTestControllerLogic.onTestRun?.Invoke();
        }


        #endregion
    }
}

