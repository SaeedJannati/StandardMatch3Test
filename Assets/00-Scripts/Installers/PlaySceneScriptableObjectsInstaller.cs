using System;
using Match3.General;
using Match3.General.MoveTest;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Match3.Installers
{
    [CreateAssetMenu(fileName = "PlaySceneScriptableObjectsInstaller",
        menuName = "Match3/Installers/PlaySceneScriptableObjectsInstaller")]
    public class PlaySceneScriptableObjectsInstaller : ScriptableObjectInstaller<PlaySceneScriptableObjectsInstaller>
    {
        #region Fields

        [SerializeField, Expandable] private GridGeneratorModel gridGeneratorModel;
        [SerializeField, Expandable] private GridMoveEffectsModel _gridMoveEffectsModel;
        [SerializeField, Expandable] private MoveTestControllerModel _moveTestControllerModel;

        #endregion

        #region Methods

        public override void InstallBindings()
        {
            Container.Bind<GridGeneratorModel>().FromScriptableObject(gridGeneratorModel).AsSingle();
            Container.BindInterfacesAndSelfTo<GridMoveEffectsModel>().FromScriptableObject(_gridMoveEffectsModel)
                .AsSingle();
            Container.Bind<MoveTestControllerModel>().FromScriptableObject(_moveTestControllerModel).AsSingle();
        }

        #endregion
    }
}