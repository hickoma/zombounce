using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Components;

namespace Systems
{
	public class GameState
	{
		public enum State
		{
			PAUSE,
			GAME_OVER,
			PLAY,
			REWARDING
		}

		// interface
		static GameState m_Instance;

		public static GameState Instance
		{
			get
			{
				return m_Instance;
			}
		}

		public static GameState Create()
		{
			m_Instance = new GameState();

			return m_Instance;
		}

		public event System.Action<int> OnCoinsChanged;
		public event System.Action<int> OnTurnsChanged;
		public event System.Action<int> OnPointsChanged;
		public event System.Action<int> OnBestScoreChanged;

		// data
		// coins
		bool m_AreCoinsInitialized = false;
		// is inited from Parameters
		public int m_CoinsDefaultCount = 0;
		int m_CoinsCount = -1;

		public int CoinsCount
		{
			get
			{
				if (!m_AreCoinsInitialized)
				{
					m_CoinsCount = PlayerPrefs.GetInt (Data.PrefKeys.CoinsKey, m_CoinsDefaultCount);
					m_AreCoinsInitialized = true;
				}

				return m_CoinsCount;
			}

			set
			{
				m_CoinsCount = value;
				PlayerPrefs.SetInt(Data.PrefKeys.CoinsKey, m_CoinsCount);
				PlayerPrefs.Save();

				// notify
				if (OnCoinsChanged != null)
				{
					OnCoinsChanged (m_CoinsCount);
				}
			}
		}

		// energy
		// coins
		bool m_AreTurnsInitialized = false;
		// is inited from Parameters
		public int m_TurnsDefaultCount = 10;
		int m_TurnsCount = -1;

		public int TurnsCount
		{
			get
			{
				if (!m_AreTurnsInitialized)
				{
					m_TurnsCount = m_TurnsDefaultCount;
					m_AreTurnsInitialized = true;
				}

				return m_TurnsCount;
			}

			set
			{
				m_TurnsCount = value;

				// notify
				if (OnTurnsChanged != null)
				{
					OnTurnsChanged (m_TurnsCount);
				}
			}
		}

		// fists
		// all fists
		private Fist[] m_AllFists = null;

		public Fist[] AllFists
		{
			get
			{
				return m_AllFists;
			}

			set
			{
				m_AllFists = value;
			}
		}

		private Fist m_DefaultFist = null;

		public Fist DefaultFist
		{
			get
			{
				return m_DefaultFist;
			}

			set
			{
				m_DefaultFist = value;
			}
		}

		//selected fist
		string m_SelectedFistName = "";

		public string SelectedFistName
		{
			get
			{
				if (string.IsNullOrEmpty (m_SelectedFistName))
				{
					// load from prefs or choose default fist
					m_SelectedFistName = PlayerPrefs.GetString (Data.PrefKeys.SelectedFistKey, m_DefaultFist.m_Id);
				}

				return m_SelectedFistName;
			}

			set
			{
				m_SelectedFistName = value;
				PlayerPrefs.SetString (Data.PrefKeys.SelectedFistKey, m_SelectedFistName);
				PlayerPrefs.Save();
			}
		}

		public Fist SelectedFist
		{
			get
			{
				Fist selectedFist = System.Array.Find (m_AllFists, (f) => f.m_Id == SelectedFistName);

				return selectedFist;
			}
		}

		// purchased fists
		private string[] m_PurchasedFists = null;

		public string[] PurchasedFists
		{
			get
			{
				if (m_PurchasedFists == null)
				{
					m_PurchasedFists = PlayerPrefsExtensions.GetStringArray (Data.PrefKeys.FistsKey);

					// add default fist to inventory
					if (m_PurchasedFists.Length == 0)
					{
						m_PurchasedFists = new string[]{ DefaultFist.m_Id };
						PurchasedFists = m_PurchasedFists;
					}
				}

				return m_PurchasedFists;
			}

			set
			{
				m_PurchasedFists = value;
				PlayerPrefsExtensions.SetStringArray (Data.PrefKeys.FistsKey, m_PurchasedFists);
				PlayerPrefs.Save();
			}
		}

		public void DropPurchases()
		{
			// drop selected fist
			SelectedFistName = m_DefaultFist.m_Id;

			// drop purchased fists
			PurchasedFists = new string[]{ DefaultFist.m_Id };

			// drop coins
			CoinsCount = m_CoinsDefaultCount;
		}

		// game over seconds before aborting
		private int m_GameOverTimerCount = 5;

		public int GameOverTimerCount
		{
			get
			{
				return m_GameOverTimerCount;
			}

			set
			{
				m_GameOverTimerCount = value;
			}
		}

		// turns added after first death
		private int m_SecondLifeTurnsCount = 5;

		public int SecondLifeTurnsCount
		{
			get
			{
				return m_SecondLifeTurnsCount;
			}

			set
			{
				m_SecondLifeTurnsCount = value;
			}
		}

		// current points
		private int m_CurrentPointsCount = 0;

		public int CurrentPointsCount
		{
			get
			{
				return m_CurrentPointsCount;
			}

			set
			{
				m_CurrentPointsCount = value;

				// notify
				if (OnPointsChanged != null)
				{
					OnPointsChanged (m_CurrentPointsCount);
				}
			}
		}

		// best score points
		private bool m_IsBestScoreInitialized = false;
		private int m_BestScorePointsCount = 0;

		public int BestScorePointsCount
		{
			get
			{
				if (!m_IsBestScoreInitialized)
				{
					m_BestScorePointsCount = PlayerPrefs.GetInt (Data.PrefKeys.BestScoreKey, 0);
					m_IsBestScoreInitialized = true;
				}

				return m_BestScorePointsCount;
			}

			set
			{
				m_BestScorePointsCount = value;
				PlayerPrefs.SetInt (Data.PrefKeys.BestScoreKey, m_BestScorePointsCount);
				PlayerPrefs.Save();

				// notify
				if (OnBestScoreChanged != null)
				{
					OnBestScoreChanged (m_BestScorePointsCount);
				}
			}
		}

		public void AddPoints(int points)
		{
			int oldPoints = CurrentPointsCount;
			int newPoints = oldPoints + points;

			if (oldPoints != newPoints)
			{
				Systems.GameState.Instance.CurrentPointsCount = newPoints;
			}
		}

		public void UpdateBestScore()
		{
			if (CurrentPointsCount > BestScorePointsCount)
			{
				BestScorePointsCount = CurrentPointsCount;
			}
		}

		// points to coins ratio
		private int m_PointsToCoinsCoeff = 3;

		public int PointsToCoinsCoeff
		{
			get
			{
				return m_PointsToCoinsCoeff;
			}

			set
			{
				m_PointsToCoinsCoeff = value;
			}
		}

        // coins will be multiplied by this coeff after watching ads
        private int m_AdvertisingCoinsMultiplierCoeff = 3;

        public int AdvertisingCoinsMultiplierCoeff
        {
            get
            {
                return m_AdvertisingCoinsMultiplierCoeff;
            }

            set
            {
                m_AdvertisingCoinsMultiplierCoeff = value;
            }
        }

        // free coins given for watching ads
        private int m_FreeCoinsAmount = 100;

        public int FreeCoinsAmount
        {
            get
            {
                return m_FreeCoinsAmount;
            }

            set
            {
                m_FreeCoinsAmount = value;
            }
        }

		// Player is dead only when m_FlyingRewardsExist is false
		private bool m_FlyingRewardsExist = false;

		public bool FlyingRewardsExist
		{
			get
			{
				return m_FlyingRewardsExist;
			}

			set
			{
				m_FlyingRewardsExist = value;
			}
		}
	}
}
