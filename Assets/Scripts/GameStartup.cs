using Systems;
using Systems.Game;
using Systems.Physic;
using Systems.PlayerProcessings;
using Systems.Service;
using Systems.Ui;
using Systems._DEBUG;
using Data;
using LeopotamGroup.Ecs;
using UnityEngine;

public class GameStartup : MonoBehaviour
{
    [SerializeField] private Parameters _parameters;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _startGamePanel;

#if UNITY_EDITOR
    private GameObject _worldObserver;
#endif

    EcsWorld _world;
    EcsSystems _update;
    EcsSystems _fixedUpdate;
    EcsSystems _startInit;

    private void Awake()
    {
        _world = new EcsWorld();
#if UNITY_EDITOR
        _worldObserver = LeopotamGroup.Ecs.UnityIntegration.EcsWorldObserver.Create(_world);
#endif
        _startInit = new EcsSystems(_world);
        _update = new EcsSystems(_world);
        _fixedUpdate = new EcsSystems(_world);

        AddProcessings();

        _update.Initialize();
        _fixedUpdate.Initialize();
#if UNITY_EDITOR
        LeopotamGroup.Ecs.UnityIntegration.EcsSystemsObserver.Create(_update);
#endif
    }

    private void Start()
    {
        _startInit.Initialize();
    }

    private void AddProcessings()
    {
        _startInit
            .Add(new LevelInitializeProcessor());

        _update
            .Add(new CatchClickEventProcessing())
            .Add(new TurnCounterProcessing
            {
                InitTurnCounter = _parameters.TurnCount,
                MinVelocityTolerace = _parameters.MinVelocityTolerance
            })
            .Add(new GameOverProcessing
            {
                GameOverPanel = _gameOverPanel,
                TimerCount = _parameters.TimerCount
            })
            .Add(new PlayMoreProcessing
            {
                GameOverPanel = _gameOverPanel
            })
            .Add(new HideTimerProcessing
            {
                GameOverPanel = _gameOverPanel,
                RescaleSpeed = _parameters.RescaleSpeed
            })
            .Add(new StartGameProcessing
            {
                StartGamePanel = _startGamePanel
            })
            .Add(new TimerProcessing
            {
                GameOverPanel = _gameOverPanel
            })
            .Add(new RestartProcessing())
            .Add(new UserInputProcessing())
            .Add(new PlayerProcessing
            {
                Multiplier = _parameters.ForceMultiplier,
                MaxForce = _parameters.MaxForce,
                AliveSprite = _parameters.AliveSprite,
                DeathSprite = _parameters.DeadSprite,
                MinLength = _parameters.MinLength
            })
            .Add(new BonusProcessing())
            .Add(new DrawVectorPointerProcessing
            {
                MaxForce = _parameters.MaxForce
            })
            .Add(new DistanceBonusProcessing())
            .Add(new FieldsSpawnProcessing
            {
                Prefabs = _parameters.Fields,
                SpawnCount = _parameters.SpawnCount,
                InitialPoolSize = _parameters.InitialPoolSize,
                EnergyPrefab = _parameters.Energy,
                EnergySpawnCount = _parameters.EnergySpawnCount
            })
#if DEBUG
            .Add(new DebugProcessingUpdate()) //debug
#endif
            .Add(new TimeScaleProcessing())
            .Add(new ClearEventsProcessing());

        _fixedUpdate
            .Add(new AddForceProcessing())
            .Add(new CameraFollowProcessing
            {
                CameraSmooth = _parameters.CameraSmooth
            })
            .Add(new DragSimulation
            {
                Drag = _parameters.Drag
            })
#if DEBUG
            .Add(new DebugProcessingFixedUpdate()) //debug
#endif
            ;
    }

    private void Update()
    {
        _update.Run();
    }

    private void FixedUpdate()
    {
        _fixedUpdate.Run();
    }

    void OnDestroy()
    {
        _update.Destroy();
        _fixedUpdate.Destroy();
        _world.Dispose();
        _world = null;
#if UNITY_EDITOR
        Destroy(_worldObserver);
#endif
    }
}