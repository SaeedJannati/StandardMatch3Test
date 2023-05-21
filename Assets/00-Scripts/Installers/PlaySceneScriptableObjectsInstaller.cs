using System;
using Match3.General;
using NaughtyAttributes;
using UnityEngine;
using Zenject;

namespace Match3.Installers
{
    [CreateAssetMenu(fileName = "PlaySceneScriptableObjectsInstaller", menuName = "Match3/Installers/PlaySceneScriptableObjectsInstaller")]
    public class PlaySceneScriptableObjectsInstaller : ScriptableObjectInstaller<PlaySceneScriptableObjectsInstaller>
    {
        #region Fields

        [SerializeField, Expandable] private GridGeneratorModel gridGeneratorModel;
        [SerializeField, Expandable] private GridGeneratorViewModel gridGeneratorViewModel;
        [SerializeField, Expandable] private GridMoveEffectsModel _gridMoveEffectsModel;
        #endregion

        #region Methods
        public override void InstallBindings()
        {
            Container.Bind<GridGeneratorModel>().FromScriptableObject(gridGeneratorModel).AsSingle();
            Container.Bind<GridGeneratorViewModel>().FromScriptableObject(gridGeneratorViewModel).AsSingle();
            Container.BindInterfacesAndSelfTo<GridMoveEffectsModel>().FromScriptableObject(_gridMoveEffectsModel)
                .AsSingle();
        }
    

        #endregion

    }
}
