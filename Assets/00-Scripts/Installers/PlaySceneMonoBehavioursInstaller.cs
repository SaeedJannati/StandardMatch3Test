using Match3.General;
using Match3.General.MoveTest;
using UnityEngine;
using Zenject;

namespace Match3.Installers
{
    public class PlaySceneMonoBehavioursInstaller : MonoInstaller
    {
        #region Fields
        [SerializeField] private PlaySceneContextModel _model;
        [SerializeField] private GridControllerView _gridControllerView;
        [SerializeField] private MoveTestControllerView _moveTestControllerView;
     
        #endregion

        #region Methods

        public override void InstallBindings()
        {
            BindInstallers();
            BindMonoBehaviours();
            BindFactories();
        }

        private void BindFactories()
        {
            Container.BindFactory
                    <int, int, GridControllerEventController,GridGeneratorViewModel, GridElement, GridElement.Factory>()
                .FromComponentInNewPrefab(_model.gridElementPrefab);
        }

        void BindMonoBehaviours()
        {
            Container.Bind<GridControllerView>().FromInstance(_gridControllerView);
            Container.Bind<MoveTestControllerView>().FromInstance(_moveTestControllerView);
        }

        void BindInstallers()
        {
            PlaySceneGeneralInstaller.Install(Container);
        }

        #endregion
    
    }
}
