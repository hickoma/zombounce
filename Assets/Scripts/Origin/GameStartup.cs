using Systems;
using Systems.Game;
using Systems.Physic;
using Systems.PlayerProcessings;
using Systems.Service;
using Systems.Ui;
using Systems._DEBUG;
using Components.Events;
using Data;
using LeopotamGroup.Ecs;
using UnityEngine;

public class GameStartup : MonoBehaviour
{
    [SerializeField]
	private Parameters _parameters = null;

    [SerializeField]
	private GameObject _gameOverPanel = null;

    [SerializeField]
	private GameObject _pausePanel = null;

    [SerializeField]
	private GameObject _settingsPanel = null;

    [SerializeField]
	private GameObject _startGamePanel = null;

	[Header("Game Systems")]

	[SerializeField]
	private PlayerController m_PlayerController = null;

	[SerializeField]
	private DrawVectorPointerController m_DrawVectorPointerController = null;

	[SerializeField]
	private DistanceBonusController m_DistanceBonuseController = null;

	[SerializeField]
	private AddForceController m_AddForceController = null;

	[SerializeField]
	private CameraFollowController m_CameraFollowController = null;

	[SerializeField]
	private DragSimulationController m_DragSimulationController = null;

	[SerializeField]
	private BackBlockerFollowController m_BackBlockFollowController = null;

	// ugly, ugly, UGLY! need to be removed
	// needed just to ensure that it's initialized before everything else
	[Space]
	[SerializeField]
	private GameEventsController m_EventsController;

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
		m_EventsController.Awake ();
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

		m_PlayerController.LateStart();

		m_DrawVectorPointerController.LateStart();

		m_DistanceBonuseController.LateStart();

		m_AddForceController.LateStart();

		m_DragSimulationController.LateStart();
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
            .Add(new CoinsCounterProcessing())
            .Add(new PlayMoreProcessing
            {
                GameOverPanel = _gameOverPanel
            })
            .Add(new SettingsProcessing
            {
                PausePanel = _pausePanel,
                SettingsPanel = _settingsPanel,
                Parameters = _parameters // debug
            })
            .Add(new GameOverProcessing
            {
                GameOverPanel = _gameOverPanel,
                TimerCount = _parameters.TimerCount
            })
            .Add(new PauseMenuProcessing
            {
                PausePanel = _pausePanel
            })
            .Add(new HideTimerProcessing
            {
                GameOverPanel = _gameOverPanel,
                RescaleSpeed = _parameters.RescaleSpeed
            })
            .Add(new ShowTakeCoinsProcessing
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
//            .Add(new PlayerController
//            {
//                Multiplier = _parameters.ForceMultiplier,
//                MaxForce = _parameters.MaxForce,
//                AliveSprite = _parameters.AliveSprite,
//                DeathSprite = _parameters.DeadSprite,
//                MinLength = _parameters.MinLength
//            })
            .Add(new BonusProcessing())
            .Add(new PauseButtonStateProcessing())
//            .Add(new DrawVectorPointerProcessing
//            {
//                MaxForce = _parameters.MaxForce
//            })
//            .Add(new DistanceBonusController())
            .Add(new FieldsSpawnProcessing
            {
                Prefabs = _parameters.Fields,
                ForwardSpawnCount = _parameters.ForwardSpawnCount,
				BackwardSpawnCount = _parameters.BackwardSpawnCount,
                InitialPoolSize = _parameters.InitialPoolSize,
                EnergyPrefab = _parameters.Energy,
                CoinPrefab = _parameters.Coin,
                EnergySpawnCount = _parameters.EnergySpawnCount,
                CoinSpawnCount = _parameters.CoinSpawnCount
            })
#if DEBUG
            .Add(new DebugProcessingUpdate()) //debug
#endif
            .Add(new TimeScaleProcessing())
            .Add(new ClearEventsProcessing());

        _fixedUpdate
//            .Add(new AddForceController())
//            .Add(new CameraFollowController
//            {
//                CameraSmooth = _parameters.CameraSmooth,
//                CameraMinPositionZ = _parameters.CameraMinPositionZ
//            })
//            .Add(new DragSimulationController
//            {
//                Drag = _parameters.Drag
//            })
#if DEBUG
            .Add(new DebugProcessingFixedUpdate()) //debug
#endif
            ;

		m_PlayerController.Multiplier = _parameters.ForceMultiplier;
		m_PlayerController.MaxForce = _parameters.MaxForce;
		m_PlayerController.AliveSprite = _parameters.AliveSprite;
		m_PlayerController.DeathSprite = _parameters.DeadSprite;
		m_PlayerController.MinLength = _parameters.MinLength;

		m_DrawVectorPointerController.MaxForce = _parameters.MaxForce;

		m_CameraFollowController.CameraSmooth = _parameters.CameraSmooth;
		m_CameraFollowController.CameraMinPositionZ = _parameters.CameraMinPositionZ;

		m_DragSimulationController.Drag = _parameters.Drag;

		m_BackBlockFollowController.DistanceFromCamera = _parameters.BackBlockerDistanceFromCamera;
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