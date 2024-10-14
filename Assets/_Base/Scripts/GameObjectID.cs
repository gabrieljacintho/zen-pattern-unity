using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FireRingStudio
{
    public class GameObjectID : MonoBehaviour
    {
        public const string PlayerID = "player";
        
        [SerializeField] private string _id;
        
        public string ID => _id;
        
        private static readonly List<GameObjectID> s_gameObjectIDs = new();

        public static Action<GameObject> GameObjectRegistered;
        public static Action Updated;
        
        
        private void Awake()
        {
            Register();
        }

        private void OnDestroy()
        {
            Unregister();
        }
        
        public static GameObject FindGameObjectWithID(string id, bool includeInactive = false)
        {
            List<GameObject> gameObjects = FindGameObjectsWithID(id);

            if (gameObjects.Count == 0 && includeInactive)
            {
                gameObjects = FindGameObjectsWithID(id, true);
            }
            
            return gameObjects.Count > 0 ? gameObjects[0] : null;
        }
        
        public static List<GameObject> FindGameObjectsWithID(string id, bool includeInactive = false)
        {
            if (includeInactive)
            {
                RegisterAllObjects();
            }

            List<GameObject> gameObjects = new List<GameObject>();
            foreach (GameObjectID gameObjectID in s_gameObjectIDs)
            {
                if (MatchGameObjectID(gameObjectID, id, includeInactive))
                {
                    gameObjects.Add(gameObjectID.gameObject);
                }
            }
            
            return gameObjects;
        }

        private static bool MatchGameObjectID(GameObjectID gameObjectID, string id, bool includeInactive = false)
        {
            if (string.IsNullOrEmpty(id))
            {
                if (!string.IsNullOrEmpty(gameObjectID.ID))
                {
                    return false;
                }
            }
            else if (gameObjectID.ID != id)
            {
                return false;
            }

            if (!includeInactive && !gameObjectID.gameObject.activeInHierarchy)
            {
                return false;
            }

            return true;
        }

        public static GameObject FindChildWithID(GameObject parent, string id, bool includeInactive = false)
        {
            List<GameObject> gameObjects = FindGameObjectsWithID(id);

            if (gameObjects.Count == 0 && includeInactive)
            {
                GameObjectID[] gameObjectIDs = parent.GetComponentsInChildren<GameObjectID>(true);
                gameObjectIDs.ForEach(gameObjectID => gameObjectID.Register());

                foreach (GameObjectID gameObjectID in gameObjectIDs)
                {
                    if (MatchGameObjectID(gameObjectID, id, includeInactive))
                    {
                        return gameObjectID.gameObject;
                    }
                }
            }

            return gameObjects.Count > 0 ? gameObjects[0] : null;
        }

        public static T FindComponentInGameObjectWithID<T>(string id, bool includeInactive = false)
        {
            GameObject gameObject = FindGameObjectWithID(id, includeInactive);
            return gameObject != null ? gameObject.GetComponent<T>() : default;
        }
        
        public static T FindComponentInChildrenWithID<T>(string id, bool includeInactive = false)
        {
            GameObject gameObject = FindGameObjectWithID(id, includeInactive);
            return gameObject != null ? gameObject.GetComponentInChildren<T>(includeInactive) : default;
        }

        private void Register()
        {
            if (s_gameObjectIDs.Contains(this))
            {
                return;
            }
            
            s_gameObjectIDs.Add(this);
            
            GameObjectRegistered?.Invoke(gameObject);
            
            Updated?.Invoke();
        }

        private void Unregister()
        {
            if (!s_gameObjectIDs.Contains(this))
            {
                return;
            }
            
            s_gameObjectIDs.Remove(this);
                
            Updated?.Invoke();
        }

        private static void RegisterAllObjects()
        {
            GameObjectID[] gameObjectIDs = FindObjectsOfType<GameObjectID>(true);
            Array.ForEach(gameObjectIDs, gameObjectID => gameObjectID.Register());
        }
    }
}