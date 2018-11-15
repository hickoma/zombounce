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

	// ugly, ugly, UGLY global names
	// need to refactor everything than links here
	public Components.Player m_Player;
}
