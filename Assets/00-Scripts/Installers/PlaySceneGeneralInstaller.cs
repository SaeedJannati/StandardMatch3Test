using Match3.General;
using NaughtyAttributes.Test;
using UnityEngine;
using Zenject;

namespace Match3.Installers
{
    public class PlaySceneGeneralInstaller : Installer<PlaySceneGeneralInstaller>
    {
        #region Methods

        public override void InstallBindings()
        {
            InstallGridClasses();
            InstallEventControllers();
        }

        private void InstallEventControllers()
        {
            Container.BindInterfacesAndSelfTo<GridControllerEventController>().AsSingle();
            Container.BindInterfacesAndSelfTo<MatchCheckerEventController>().AsSingle();
        }
        void InstallGridClasses()
        {
            Container.BindInterfacesAndSelfTo<GridGenerator>().AsSingle();
            Container.BindInterfacesAndSelfTo<GridControllerLogic>().AsSingle();
            Container.BindInterfacesAndSelfTo<MatchChecker>().AsSingle();
            Container.BindInterfacesAndSelfTo<ElementsDropHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<GridMoveEffectsHandler>().AsSingle();
        }

        #endregion

    }
}