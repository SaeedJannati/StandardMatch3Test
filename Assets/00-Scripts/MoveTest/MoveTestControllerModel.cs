using System.Collections;
using System.Collections.Generic;
using Match3.Auxiliary;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Match3.General.MoveTest
{
    [CreateAssetMenu(fileName = "MoveTestControllerModel",menuName = "Match3/General/MoveTest/MoveTestControllerModel")]
    public class MoveTestControllerModel : ScriptableObject
    {
    

        #region Properties
        public string stackTrace;
        [field: SerializeField] public bool isGraphicalTest { get; private set; } = true;
        [field: SerializeField] public int testMoveCount { get; private set; }
        


        #endregion

        #region Methods

       

        #endregion
    }
}

