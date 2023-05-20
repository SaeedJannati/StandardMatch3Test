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
        #endregion

        #region Methods
        public override void InstallBindings()
        {
            Container.Bind<GridGeneratorModel>().FromScriptableObject(gridGeneratorModel).AsSingle();
            Container.BindInterfacesAndSelfTo<GridGeneratorViewModel>().FromInstance(gridGeneratorViewModel);
        }
    

        #endregion

    }
}
