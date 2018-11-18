using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsController : MonoBehaviour
{
	// Singleton stuff
	static GameEventsController m_Instance = null;

	public static GameEventsController Instance
	{
		get
		{
			return m_Instance;
		}
	}

	public void Awake()
	{
		m_Instance = this;
	}

	// events and invokers
	// Player Controller
	public event Action<bool> OnSetPlayerSprite;

	public void SetPlayerSprite(bool isLive)
	{
		if (OnSetPlayerSprite != null)
		{
			OnSetPlayerSprite (isLive);
		}
	}

	public event Action<bool, Vector3, Vector3> OnPointerUpDown;

	public void PointerUpDown(bool isDown, Vector3 downPointerPosition, Vector3 upPointerPosition)
	{
		if (OnPointerUpDown != null)
		{
			OnPointerUpDown (isDown, downPointerPosition, upPointerPosition);
		}
	}

	public event Action<Components.Events.GameState> OnGameStateChanged;

	public void ChangeGameState(Components.Events.GameState newState)
	{
		if (OnGameStateChanged != null)
		{
			OnGameStateChanged (newState);
		}
	}

	// Draw Vector Pointer Processing
	public event Action<Vector3, Vector3, bool> OnDrawVectorPointer;

	public void DrawVectorPointer(Vector3 downVector, Vector3 forceVector, bool release)
	{
		if (OnDrawVectorPointer != null)
		{
			OnDrawVectorPointer (downVector, forceVector, release);
		}
	}

	// Distance Bonus Processing
	public event Action<float> OnDistanceChanged;

	public void ChangeDistance(float distance)
	{
		if (OnDistanceChanged != null)
		{
			OnDistanceChanged (distance);
		}
	}

	// Add Force Controller
	public event Action<Vector3> OnForceAdded;

	public void AddForce(Vector3 forceVector)
	{
		if (OnForceAdded != null)
		{
			OnForceAdded (forceVector);
		}
	}

	// Turn Counter Controller
	public event Action<int> OnTurnsChanged;

	public void ChangeTurns(int delta)
	{
		if (OnTurnsChanged != null)
		{
			OnTurnsChanged (delta);
		}
	}

	// Bonus Controller
	public event Action<int> OnPointsAdded;

	public void AddPoints(int points)
	{
		if (OnPointsAdded != null)
		{
			OnPointsAdded (points);
		}
	}

    public event Action OnScoreSaved;

    public void SaveScore()
    {
        if (OnScoreSaved != null)
        {
            OnScoreSaved();
        }
    }

	// Start Game Window
	public event Action OnGameStartClick;

	public void StartGame()
	{
		if (OnGameStartClick != null)
		{
			OnGameStartClick ();
		}

		LeopotamGroup.Ecs.EcsWorld _world = LeopotamGroup.Ecs.EcsWorld.Active;
		GameEventsController.Instance.ChangeGameState (Components.Events.GameState.PLAY);
		_world.CreateEntityWith<Components.Events.GameStateEvent>().State = Components.Events.GameState.PLAY;
	}

	// Restart Game Window
	public event Action OnGameRestartClick;

	public void RestartGame()
	{
		if (OnGameStartClick != null)
		{
			OnGameStartClick ();
		}

		LeopotamGroup.Ecs.EcsWorld _world = LeopotamGroup.Ecs.EcsWorld.Active;
		_world.CreateEntityWith<Components.Events.RestartEvent>();
	}

	// HUD
	public event Action OnGamePauseClick;

	public void PauseGame()
	{
		if (OnGamePauseClick != null)
		{
			OnGamePauseClick ();
		}

		LeopotamGroup.Ecs.EcsWorld _world = LeopotamGroup.Ecs.EcsWorld.Active;
		GameEventsController.Instance.ChangeGameState (Components.Events.GameState.PAUSE);
		_world.CreateEntityWith<Components.Events.GameStateEvent>().State = Components.Events.GameState.PAUSE;
	}

	public event Action OnSettingsClick;

	public void OpenSettings()
	{
		if (OnSettingsClick != null)
		{
			OnSettingsClick ();
		}

		LeopotamGroup.Ecs.EcsWorld _world = LeopotamGroup.Ecs.EcsWorld.Active;

		if (_world != null)
        {
			_world.CreateEntityWith<Components.Events.SettingsEvent>().OpenSettings = true;
        }
	}

	// ugly, ugly, UGLY global names
	// need to refactor everything than links here
	public Components.Player m_Player;
}
