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

public class GameController : MonoBehaviour
{
    [SerializeField]
	private Parameters _parameters = null;

	[Header("Game Systems")]

	[SerializeField]
	private UserInputController m_UserInputController = null;

	[SerializeField]
	private PlayerController m_PlayerController = null;

	[SerializeField]
	private DrawVectorPointerController m_DrawVectorPointerController = null;

//	[SerializeField]
//	private DistanceBonusController m_DistanceBonuseController = null;

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

    private void Awake()
    {
		m_EventsController.Awake ();

        AddProcessings();
    }

    private void Start()
    {
//        _startInit.Initialize();
        // level initialize
        Time.timeScale = 1f;

		m_UserInputController.LateStart();

		m_PlayerController.LateStart();

		m_DrawVectorPointerController.LateStart();

//		m_DistanceBonuseController.LateStart();

		m_AddForceController.LateStart();

		m_DragSimulationController.LateStart();
    }

    private void AddProcessings()
    {
		m_PlayerController.Multiplier = _parameters.ForceMultiplier;
		m_PlayerController.MinVelocityTolerance = _parameters.MinVelocityTolerance;
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
}
