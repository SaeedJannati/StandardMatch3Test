using Match3.General;
using NaughtyAttributes.Test;
using UnityEngine;
using Zenject;

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
    }

    void InstallGridClasses()
    {
        Container.BindInterfacesAndSelfTo<GridGenerator>().AsSingle();
        Container.BindInterfacesAndSelfTo<GridControllerLogic>().AsSingle();
    }

    #endregion
  
}