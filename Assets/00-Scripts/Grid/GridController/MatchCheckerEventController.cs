﻿using Match3.EventController;
using UnityEngine.Rendering;

namespace Match3.General
{
    public class MatchCheckerEventController:BaseEventController
    {
        public readonly ListFuncEvent<TileGridElement, bool> onElementMatchCheck = new();
    }
}