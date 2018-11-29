﻿using System.Collections;
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
			PLAY
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

		// data
		// coins
		bool m_AreCoinsInitialized = false;
		// is inited from Parameters
		public int m_CoinsDefaultCount = 500;
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
	}
}
