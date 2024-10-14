using System;
using System.Collections.Generic;
using FireRingStudio.Patterns;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.UI
{
    [Serializable]
    public struct UserInterface
    {
        public string Id;
        public List<GameObject> GameObjects;
        public bool ActivateOnlyOnce;
        [ShowIf("ActivateOnlyOnce")]
        public string SaveKey;

        
        public UserInterface(string id, List<GameObject> gameObjects, bool activateOnlyOnce = false, string saveKey = null)
        {
            Id = id;
            GameObjects = gameObjects;
            ActivateOnlyOnce = activateOnlyOnce;
            SaveKey = saveKey;
        }
    }
    
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] private List<UserInterface> _userInterfaces;

        private UserInterface _currentUI;
        private readonly LimitedQueue<UserInterface> _history = new(8);

        public UserInterface CurrentUI
        {
            get => _currentUI;
            private set => SetCurrentUI(value);
        }
        
        [Space]
        public UnityEvent<string> OnUIIdChanged;
        public UnityEvent OnClosed;


        private void Start()
        {
            OpenFirstUI();
        }

        [Button]
        public void OpenUI(string id)
        {
            if (_userInterfaces == null)
            {
                return;
            }

            List<GameObject> activatedObjects = new List<GameObject>();
            foreach (UserInterface other in _userInterfaces)
            {
                bool canShow = other.Id == id && CanOpen(other);

                if (other.GameObjects != null)
                {
                    foreach (GameObject gameObject in other.GameObjects)
                    {
                        if (activatedObjects.Contains(gameObject))
                        {
                            continue;
                        }
                        
                        gameObject.SetActive(canShow);
                        if (canShow)
                        {
                            activatedObjects.Add(gameObject);
                        }
                    }
                }
                
                if (canShow && other.ActivateOnlyOnce)
                {
                    PlayerPrefs.SetInt(other.SaveKey, 1);
                    PlayerPrefs.Save();
                }
            }

            SetCurrentUI(new UserInterface(id, activatedObjects));
        }
        
        public void OpenNextUI()
        {
            if (_userInterfaces == null)
            {
                return;
            }

            bool IsCurrentUI(UserInterface info) => info.Id == CurrentUI.Id;
            
            if (!_userInterfaces.Exists(IsCurrentUI))
            {
                return;
            }
            
            int currentIndex = _userInterfaces.FindIndex(IsCurrentUI);
            for (int i = 0; i < _userInterfaces.Count; i++)
            {
                int index = currentIndex + i + 1;
                if (index >= _userInterfaces.Count)
                {
                    index -= _userInterfaces.Count;
                }

                UserInterface userInterface = _userInterfaces[index];
                if (CanOpen(userInterface))
                {
                    OpenUI(userInterface.Id);
                    return;
                }
            }
        }
        
        public void OpenPreviousUI()
        {
            _history.Dequeue();
            for (int i = 0; i < _history.Items.Length; i++)
            {
                UserInterface userInterface = _history.Dequeue();
                if (CanOpen(userInterface))
                {
                    OpenUI(userInterface.Id);
                    return;
                }
            }
            
            CloseUI();
        }
        
        public void CloseUI()
        {
            if (_userInterfaces == null)
            {
                return;
            }
            
            foreach (UserInterface userInterface in _userInterfaces)
            {
                userInterface.GameObjects?.ForEach(gameObject => gameObject.SetActive(false));
            }
            
            CurrentUI = default;
            
            OnClosed?.Invoke();
        }

        private static bool CanOpen(UserInterface userInterface)
        {
            if (!userInterface.ActivateOnlyOnce)
            {
                return true;
            }

            return PlayerPrefs.GetInt(userInterface.SaveKey) == 0;
        }
        
        private void OpenFirstUI()
        {
            if (_userInterfaces != null)
            {
                foreach (UserInterface userInterface in _userInterfaces)
                {
                    if (CanOpen(userInterface))
                    {
                        OpenUI(userInterface.Id);
                        return;
                    }
                }
            }

            Debug.LogWarning("No UI available!", this);
        }
        
        private void SetCurrentUI(UserInterface userInterface)
        {
            if (_currentUI.Id == userInterface.Id)
            {
                _currentUI = userInterface;
                return;
            }
            
            _currentUI = userInterface;
            _history.Enqueue(userInterface);
            
            OnUIIdChanged?.Invoke(userInterface.Id);
        }
    }
}