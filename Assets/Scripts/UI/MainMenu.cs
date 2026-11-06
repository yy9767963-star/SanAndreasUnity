using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SanAndreasUnity.Behaviours;
using UGameCore.Utilities;

namespace SanAndreasUnity.UI
{
	
	public class MainMenu : MonoBehaviour {

		public static MainMenu Instance { get; private set; }

		public MenuBar menuBar;

		public Color openedWindowTextColor = Color.green;
		public Color ClosedWindowTextColor => this.menuBar.DefaultMenuEntryTextColor;

		public Canvas canvas;



		void Awake()
		{
			if (null == Instance)
				Instance = this;

			// add Exit button
			this.menuBar.RegisterMenuEntry("Exit", int.MaxValue, () => GameManager.ExitApplication());
		}

		int m_lastEntryCount = 0;
		GameObject m_autoSelected;

		void Update()
		{
			if (!this.canvas.enabled)
			{
				m_autoSelected = null;
				m_lastEntryCount = 0;
				return;
			}

			var eventSystem = UnityEngine.EventSystems.EventSystem.current;
			if (null == eventSystem)
				return;

			var first = this.menuBar.MenuBarEntries.FirstOrDefault();
			if (null == first)
				return;

			int entryCount = this.menuBar.MenuBarEntries.Count();
			GameObject current = eventSystem.currentSelectedGameObject;
			bool nothingSelected = null == current || !current.activeInHierarchy;
			bool onlyAutoSelected = null != current && current == m_autoSelected;

			// select first button when nothing is selected, and again when new buttons
			// were registered while the selection was still the automatic one
			if (nothingSelected || (onlyAutoSelected && entryCount != m_lastEntryCount))
			{
				eventSystem.SetSelectedGameObject(first.gameObject);
				m_autoSelected = first.gameObject;
			}

			m_lastEntryCount = entryCount;
		}

		void OnSceneChanged(SceneChangedMessage sceneChangedMessage)
		{
			this.canvas.enabled = GameManager.IsInStartupScene;
		}

	}

}
