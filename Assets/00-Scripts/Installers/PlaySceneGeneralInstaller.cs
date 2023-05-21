using Match3.General;
using Match3.General.MoveTest;
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
            InstallMoveTestClasses();
            InstallEventControllers();
        }

        private void InstallMoveTestClasses()
        {
            Container.BindInterfacesAndSelfTo<MoveTestControllerLogic>().AsSingle();
            Container.BindInterfacesAndSelfTo<RandomMoveApplier>().AsSingle();
        }

        private void InstallEventControllers()
        {
            Container.BindInterfacesAndSelfTo<GridControllerEventController>().AsSingle();
            Container.BindInterfacesAndSelfTo<MatchCheckerEventController>().AsSingle();
            Container.BindInterfacesAndSelfTo<MoveTestEventController>().AsSingle();
        }
        void InstallGridClasses()
        {
            Container.BindInterfacesAndSelfTo<GridGenerator>().AsSingle();
            Container.BindInterfacesAndSelfTo<GridControllerLogic>().AsSingle();
            Container.BindInterfacesAndSelfTo<MatchChecker>().AsSingle();
            Container.BindInterfacesAndSelfTo<ElementsDropHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<GridMoveEffectsHandler>().AsSingle();
            Container.BindInterfacesAndSelfTo<GridShuffleController>().AsSingle();
        }

        #endregion

    }
}