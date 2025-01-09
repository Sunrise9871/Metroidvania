using UnityEngine;
using Zenject;

namespace GameLogic.MainLogic
{
    public class GameStopScenarioInstaller : MonoInstaller
    {
        [SerializeField] private GameStopScenario _gameStopScenario;

        public override void InstallBindings()
        {
            Container.Bind<GameStopScenario>().FromInstance(_gameStopScenario).AsSingle();
        }
    }
}