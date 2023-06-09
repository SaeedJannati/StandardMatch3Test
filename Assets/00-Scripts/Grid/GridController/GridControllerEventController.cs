﻿using Match3.EventController;

namespace Match3.General
{
    public class GridControllerEventController:BaseEventController
    {
        public readonly SimpleEvent onCreateGridRequest = new();
        public readonly ListFuncEvent<TilesGrid> onGridRequest = new();
        public readonly SimpleEvent onShuffleRequest=new();
        public readonly ListEvent<(int row, int col, Direction dir)> onSwipeRequest = new();
        public readonly SimpleEvent onGridCreated = new();
        public readonly ListEvent<(int row,int col,int value)> onElementValueChange=new();
        public readonly ListEvent<bool> onInputEnable = new();
        public readonly SimpleEvent onAfterMatch=new();
        public readonly SimpleEvent onAfterDrop = new();
        public readonly SimpleEvent onFillEmptySlotsRequest=new();
        public readonly ListFuncEvent<TileGridElement, GridElement> onRequestTileView = new();
        public readonly ListEvent<TileGridElement> onTileViewFadeRequest = new();
        public readonly SimpleEvent onCreateMockGridRequest=new();
        public readonly ListEvent<(TileGridElement elemnetToDrop,TileGridElement destElement)>onDropEffectRequest=new();
        public readonly SimpleEvent onAfterShuffle = new();
        public readonly SimpleEvent onShuffleEffectRequest = new();
        public readonly ListEvent<(bool fade,float period)> onFadeGridRequest= new();
        public readonly SimpleEvent onRandomMoveRequest=new();
        public readonly  ListFuncEvent<TileGridElement,(bool possible,Direction swipeDirection)> onCheckForPossibleMove=new();
        public readonly SimpleEvent onUpdateTilesColours=new();
    }
}