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
    public event Action<Sprite> OnSetFist;

    public void SetFist(Sprite sprite)
    {
        if (OnSetFist != null)
        {
            OnSetFist (sprite);
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

	public event Action<Systems.GameState.State> OnGameStateChanged;

	public void ChangeGameState(Systems.GameState.State newState)
	{
		if (OnGameStateChanged != null)
		{
			OnGameStateChanged (newState);
		}
	}

	public event Action<int> OnShowHitAnimation;

	public void ShowHitAnimation(int deltaTurns)
	{
		if (OnShowHitAnimation != null)
		{
			OnShowHitAnimation (deltaTurns);
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

	// Start Game Window
	public event Action OnGameStartClick;

	public void StartGame()
	{
		if (OnGameStartClick != null)
		{
			OnGameStartClick ();
		}

		// track session start
		Analytics.SendEventAnalytic (Analytics.PossibleEvents.SessionStart, Systems.GameState.Instance.SessionsCount.ToString());

		GameEventsController.Instance.ChangeGameState (Systems.GameState.State.PLAY);
	}

    public event Action OnStoreWindowOpen;

    public void OpenStore()
    {
        if (OnStoreWindowOpen != null)
        {
            OnStoreWindowOpen ();
        }
    }

	public event Action OnSettingsClick;

	public void OpenSettings()
	{
		if (OnSettingsClick != null)
		{
			OnSettingsClick ();
		}
	}

	public event Action OnPurchaseNoAds;

	public void PurchaseNoAds()
	{
		if (OnPurchaseNoAds != null)
		{
			OnPurchaseNoAds ();
		}
	}

    // Pause Game Window
    public event Action OnGameResumeClick;

    public void ResumeGame()
    {
        if (OnGameResumeClick != null)
        {
            OnGameResumeClick ();
        }

        GameEventsController.Instance.ChangeGameState (Systems.GameState.State.PLAY);
    }

	// Restart Game Window
	public event Action OnGameRestartClick;

	public void RestartGame()
	{
		if (OnGameStartClick != null)
		{
			OnGameStartClick ();
		}

        if (Tween.instance != null)
        {
            Tween.instance.Clear();
        }
	}

	// Game Over Window
    public event Action<Action> OnShowAdvertising;

    public void ShowRewardVideo(Action onAdvertisingClose)
    {
        if (OnShowAdvertising != null)
        {
            OnShowAdvertising(onAdvertisingClose);
        }
    }

    public event Action<Action> OnShowInterstitial;

    public void ShowInterstitial(Action onInterstitialClose)
    {
        if (OnShowInterstitial != null)
        {
            OnShowInterstitial(onInterstitialClose);
        }
    }

	public event Action OnPlayMoreClick;

	public void PlayMore()
	{
		if (OnPlayMoreClick != null)
		{
			OnPlayMoreClick ();
		}

		// track session second half start
		Analytics.SendEventAnalytic (Analytics.PossibleEvents.SessionHalfStart, Systems.GameState.Instance.SessionsCount.ToString());

		Systems.GameState.Instance.TurnsCount = Systems.GameState.Instance.SecondLifeTurnsCount;
		GameEventsController.Instance.ChangeGameState (Systems.GameState.State.PLAY);
	}

	public event Action OnStartRewarding;

	public void StartRewarding()
	{
		if (OnStartRewarding != null)
		{
			OnStartRewarding ();
		}

		GameEventsController.Instance.ChangeGameState (Systems.GameState.State.REWARDING);
	}

	// HUD
	public event Action OnGamePauseClick;

	public void PauseGame()
	{
		if (OnGamePauseClick != null)
		{
			OnGamePauseClick ();
		}

		GameEventsController.Instance.ChangeGameState (Systems.GameState.State.PAUSE);
	}

	public event Action<Vector3, int> OnCreateRewardTurn;

	public void CreateRewardTurn(Vector3 startPosition, int count)
	{
		if (OnCreateRewardTurn != null)
		{
			OnCreateRewardTurn (startPosition, count);
		}
	}

	public event Action<Vector3, int> OnCreateRewardCoin;

	public void CreateRewardCoin(Vector3 startPosition, int count)
	{
		if (OnCreateRewardCoin != null)
		{
			OnCreateRewardCoin (startPosition, count);
		}
	}

	// Field Spawn Processing
	public event Action<float> OnFieldEntered;

	public void EnterField(float zPosition)
	{
		if (OnFieldEntered != null)
		{
			OnFieldEntered (zPosition);
		}
	}

	public event Action<LeopotamGroup.Pooling.IPoolObject> OnEnergyGathered;

	public void GatherEnergy(LeopotamGroup.Pooling.IPoolObject energy)
	{
		if (OnEnergyGathered != null)
		{
			OnEnergyGathered (energy);
		}
	}

	public event Action<LeopotamGroup.Pooling.IPoolObject> OnCoinGathered;

	public void GatherCoin(LeopotamGroup.Pooling.IPoolObject coin)
	{
		if (OnCoinGathered != null)
		{
			OnCoinGathered (coin);
		}
	}

	public event Action<float> OnPlayerStopped;

	public void PlayerStopped(float zPosition)
	{
		if (OnPlayerStopped != null)
		{
			OnPlayerStopped (zPosition);
		}
	}

	// Game State (debug)
	public void DropPurchases()
	{
		Systems.GameState.Instance.DropPurchases ();
	}

    public event Action<string> OnWriteToLog;

    public void WriteToLog(string error)
    {
        if (OnWriteToLog != null)
        {
            OnWriteToLog(error);
        }
    }
}
