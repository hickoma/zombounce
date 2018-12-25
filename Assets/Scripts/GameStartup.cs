using Systems;
using Systems.Game;
using Systems.Physic;
using Systems.PlayerProcessings;
using Systems.Ui;
using Data;
using UnityEngine;

public class GameStartup : MonoBehaviour
{
    [SerializeField]
	private Parameters _parameters = null;

    [SerializeField]
    private Windows.FistStoreWindow m_FistStoreWindow = null;

	[SerializeField]
	private Windows.SettingsWindow m_SettingsWindow = null;

	[Header("Game Systems")]

	[SerializeField]
	private Windows.HUD m_Hud = null;

	[SerializeField]
	private UserInputController m_UserInputController = null;

	[SerializeField]
	private PlayerController m_PlayerController = null;

	[SerializeField]
	private DrawVectorPointerController m_DrawVectorPointerController = null;

	[SerializeField]
	private DistanceBonusController m_DistanceBonusController = null;

	[SerializeField]
	private FieldSpawnController m_FieldSpawnController = null;

	[SerializeField]
	private TurnsController m_TurnsController = null;

	[SerializeField]
	private AddForceController m_AddForceController = null;

	[SerializeField]
	private CameraFollowController m_CameraFollowController = null;

	[SerializeField]
	private DragSimulationController m_DragSimulationController = null;

	[SerializeField]
	private BackBlockerFollowController m_BackBlockFollowController = null;

	[SerializeField]
	private TutorialController m_TutorialController = null;

    [SerializeField]
    private IronSourceController m_IronSourceController = null;

    [SerializeField]
    private PurchaseController m_PurchaseController = null;

	[Space]
	[SerializeField]
	private GameEventsController m_EventsController;

	private Systems.GameState m_GameState = null;

    private void Awake()
    {
        // set quality settings
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 0;

		m_GameState = Systems.GameState.Create();

		m_EventsController.Awake ();

        InitWithParameters();

		// track the very first session
		if (m_GameState.SessionsCount == 1)
		{
			Analytics.SendEventAnalytic (Analytics.PossibleEvents.FirstSession, "1");
		}
    }

    private void Start()
    {
        // level initialize
        Time.timeScale = 1f;

		m_Hud.LateStart ();

		m_UserInputController.LateStart();

		m_PlayerController.LateStart();

		m_DrawVectorPointerController.LateStart();

		m_DistanceBonusController.LateStart();

		m_FieldSpawnController.LateStart();

		m_TurnsController.LateStart();

		m_AddForceController.LateStart();

		m_DragSimulationController.LateStart();

		m_TutorialController.LateStart();

        // windows
        m_FistStoreWindow.LateStart();

		m_SettingsWindow.LateStart();

        // iron source ads
        IronSource.Agent.init (_parameters.IronSourceAppKey, IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL, IronSourceAdUnits.OFFERWALL, IronSourceAdUnits.BANNER);

        m_IronSourceController.LateStart();

        // purchases
        m_PurchaseController.LateStart();
    }

    private void InitWithParameters()
    {
        m_GameState.SessionsCount++;
		m_GameState.m_CoinsDefaultCount = _parameters.CoinsCount;
		m_GameState.m_TurnsDefaultCount = _parameters.TurnsCount;
		m_GameState.AllFists = _parameters.Fists;
		m_GameState.DefaultFist = _parameters.DefaultFist;
		m_GameState.GameOverTimerCount = _parameters.TimerCount;
		m_GameState.SecondLifeTurnsCount = _parameters.SecondLifeTurns;
		m_GameState.PointsToCoinsCoeff = _parameters.PointsToCoinsCoeff;
        m_GameState.AdvertisingCoinsMultiplierCoeff = _parameters.AdvertisingCoinsMultiplierCoeff;
        m_GameState.FreeCoinsAmount = _parameters.FreeCoinsAmount;
		m_GameState.RewardFlyTime = _parameters.RewardFlyTime;
        m_GameState.m_LosesToShowInterstitialCount = _parameters.LosesToShowInterstitialCount;

		m_PlayerController.Multiplier = _parameters.ForceMultiplier;
		m_PlayerController.MinVelocityTolerance = _parameters.MinVelocityTolerance;
		m_PlayerController.MaxForce = _parameters.MaxForce;
		m_PlayerController.MinLength = _parameters.MinLength;

		m_DrawVectorPointerController.MaxForce = _parameters.MaxForce;

		m_FieldSpawnController.FieldPrefabs = _parameters.Fields;
		m_FieldSpawnController.ForwardSpawnCount = _parameters.ForwardSpawnCount;
		m_FieldSpawnController.BackwardSpawnCount = _parameters.BackwardSpawnCount;
		m_FieldSpawnController.InitialPoolSize = _parameters.InitialPoolSize;
		m_FieldSpawnController.ZombiePrefabs = _parameters.Zombies;
		m_FieldSpawnController.CoinPrefab = _parameters.Coin;
		m_FieldSpawnController.EnergySpawnCount = _parameters.EnergySpawnCount;
		m_FieldSpawnController.CoinSpawnCount = _parameters.CoinSpawnCount;

		m_CameraFollowController.CameraSmooth = _parameters.CameraSmooth;
		m_CameraFollowController.CameraMinPositionZ = _parameters.CameraMinPositionZ;

		m_DragSimulationController.Drag = _parameters.Drag;

		m_BackBlockFollowController.DistanceFromCamera = _parameters.BackBlockerDistanceFromCamera;

		m_TutorialController.m_FirstPartLength = _parameters.FirstPartLength;
		m_TutorialController.m_SecondPartLength = _parameters.SecondPartLength;
		m_TutorialController.m_ThirdPartLength = _parameters.ThirdPartLength;
    }

    void OnApplicationPause(bool isPaused)
    {
        IronSource.Agent.onApplicationPause(isPaused);
    }
}
