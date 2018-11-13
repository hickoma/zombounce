using Components.Events;
using Data;
using LeopotamGroup.Common;
using LeopotamGroup.Ecs;
using LeopotamGroup.Math;
using UnityEngine;
using UnityEngine.UI;

namespace Systems.Game
{
    [EcsInject]
    public class SettingsProcessing : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _ecsWorld;

        private EcsFilter<SettingsEvent> _settingsEventFilter;

        public GameObject PausePanel;
        public GameObject SettingsPanel;
        
        //debug
        public Parameters Parameters;

        private InputField _forceMultiplier;
        private InputField _drag;
        private InputField _maxForce;

        public void Run()
        {
            for (int i = 0; i < _settingsEventFilter.EntitiesCount; i++)
            {
                var isOpen = _settingsEventFilter.Components1[i].OpenSettings;
                if (isOpen)
                {
                    OpenSettings();
                }
                else
                {
                    CloseSettings();
                }
                _ecsWorld.RemoveEntity(_settingsEventFilter.Entities[i]);
            }
        }

        private void OpenSettings()
        {
            PausePanel.SetActive(false);
            SettingsPanel.SetActive(true);
        }

        private void CloseSettings()
        {
            PausePanel.SetActive(true);
            SettingsPanel.SetActive(false);

            Parameters.ForceMultiplier = _forceMultiplier.text.ToFloat();
            Parameters.Drag = _drag.text.ToFloat();
            Parameters.MaxForce = _maxForce.text.ToFloat();
        }

        public void Initialize()
        {
            _forceMultiplier = SettingsPanel.transform.FindRecursive("ForceMultiplier").GetComponent<InputField>();
            _drag = SettingsPanel.transform.FindRecursive("Drag").GetComponent<InputField>();
            _maxForce = SettingsPanel.transform.FindRecursive("MaxForce").GetComponent<InputField>();

            _forceMultiplier.text = Parameters.ForceMultiplier.ToString();
            _drag.text = Parameters.Drag.ToString();
            _maxForce.text = Parameters.MaxForce.ToString();
        }

        public void Destroy()
        {
            PausePanel = null;
            SettingsPanel = null;
            _forceMultiplier = null;
            _drag = null;
            _maxForce = null;
        }
    }
}