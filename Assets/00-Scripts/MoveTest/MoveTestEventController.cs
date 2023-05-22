using System.Collections;
using System.Collections.Generic;
using Match3.EventController;
using UnityEngine;

namespace Match3.General.MoveTest
{
    public class MoveTestEventController : BaseEventController
    {
        public readonly ListEvent<bool> onTestEnable=new();
        public readonly SimpleEvent onTestBegin = new();
        public readonly SimpleEvent onNextMovePossible = new();
        public readonly SimpleEvent onTestFinish=new();
        public ListFuncEvent<bool> onNonGraphicalTestRunningRequest = new();
    }
}
