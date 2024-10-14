using System;
using System.Collections.Generic;
using FireRingStudio.Helpers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FireRingStudio
{
    public class CameraSwitcher : MonoBehaviour
    {
        [Serializable]
        public struct CameraInfo
        {
            public GameObject CameraObject;
            public InputActionReference InputReference;
        }
        
        [SerializeField] private List<CameraInfo> _cameras;   
        [SerializeField] private InputActionReference _previousCameraInput;
        [SerializeField] private InputActionReference _nextCameraInput;
        [Tooltip("Switch to the closest camera to the main or current camera. If no cameras are found, the component position is used as the origin.")]
        [SerializeField] private InputActionReference _closestCameraInput;
        [SerializeField] private InputActionReference _defaultCameraInput;
        [SerializeField] private string _defaultCameraObjectsId = "game-camera";
        [SerializeField] private bool _developmentOnly;

        private GameObject _currentCameraObject;
        
        
        private void OnEnable()
        {
            if (_developmentOnly && !SymbolsHelper.IsInDevelopment())
            {
                return;
            }
            
            RegisterListeners();
        }

        private void OnDisable()
        {
            if (_developmentOnly && !SymbolsHelper.IsInDevelopment())
            {
                return;
            }
            
            UnregisterListeners();
        }

        public void SwitchCamera(int index)
        {
            if (ListHelper.IsNullOrEmpty(_cameras) || index >= _cameras.Count)
            {
                return;
            }

            GameObject cameraObject = _cameras[index].CameraObject;
            if (cameraObject != null)
            {
                SwitchCamera(cameraObject);
            }
        }
        
        public void SwitchToPreviousCamera()
        {
            if (ListHelper.IsNullOrEmpty(_cameras))
            {
                return;
            }

            int index = MathHelper.WrapIndex(_cameras.Count, GetCurrentCameraIndex() - 1);
            GameObject cameraObject = _cameras[index].CameraObject;
            if (cameraObject == null)
            {
                return;
            }
            
            SwitchCamera(cameraObject);
        }

        private void SwitchToPreviousCamera(InputAction.CallbackContext context)
        {
            SwitchToPreviousCamera();
        }

        public void SwitchToNextCamera()
        {
            if (ListHelper.IsNullOrEmpty(_cameras))
            {
                return;
            }

            int index = MathHelper.WrapIndex(_cameras.Count, GetCurrentCameraIndex() + 1);
            GameObject cameraObject = _cameras[index].CameraObject;
            if (cameraObject == null)
            {
                return;
            }
            
            SwitchCamera(cameraObject);
        }
        
        private void SwitchToNextCamera(InputAction.CallbackContext context)
        {
            SwitchToNextCamera();
        }

        public void SwitchToClosestCamera()
        {
            Camera camera = Camera.main;
            if (camera == null)
            {
                camera = Camera.current;
            }

            Vector3 origin = camera != null ? camera.transform.position : transform.position;
            GameObject closestCameraObject = null;
            if (_cameras != null)
            {
                foreach (CameraInfo cameraInfo in _cameras)
                {
                    GameObject otherCameraObject = cameraInfo.CameraObject;
                    if (otherCameraObject == null)
                    {
                        continue;
                    }
                    
                    if (closestCameraObject == null)
                    {
                        closestCameraObject = otherCameraObject;
                        continue;
                    }

                    float distanceA = Vector3.Distance(origin, closestCameraObject.transform.position);
                    float distanceB = Vector3.Distance(origin, otherCameraObject.transform.position);
                    if (distanceB < distanceA)
                    {
                        closestCameraObject = otherCameraObject;
                    }
                }
            }

            if (closestCameraObject == null)
            {
                return;
            }

            SwitchCamera(closestCameraObject);
        }
        
        private void SwitchToClosestCamera(InputAction.CallbackContext context)
        {
            SwitchToClosestCamera();
        }
        
        public void SwitchToDefaultCameras()
        {
            List<GameObject> defaultCameraObjects = GameObjectID.FindGameObjectsWithID(_defaultCameraObjectsId, true);
            if (defaultCameraObjects.Count == 0)
            {
                return;
            }

            if (_cameras != null)
            {
                foreach (CameraInfo cameraInfo in _cameras)
                {
                    GameObject otherCameraObject = cameraInfo.CameraObject;
                    if (otherCameraObject != null)
                    {
                        otherCameraObject.SetActive(false);
                    }
                }
            }

            foreach (GameObject cameraObject in defaultCameraObjects)
            {
                cameraObject.SetActive(true);
            }

            _currentCameraObject = null;
        }
        
        private void SwitchToDefaultCameras(InputAction.CallbackContext context)
        {
            SwitchToDefaultCameras();
        }

        private void SwitchCamera(InputAction.CallbackContext context)
        {
            if (ListHelper.IsNullOrEmpty(_cameras))
            {
                return;
            }
            
            GameObject cameraObject = _cameras.Find(info => info.InputReference != null && info.InputReference.action == context.action).CameraObject;
            if (cameraObject == null)
            {
                return;
            }

            if (cameraObject == _currentCameraObject)
            {
                SwitchToDefaultCameras();
                return;
            }
            
            SwitchCamera(cameraObject);
        }
        
        private void SwitchCamera(GameObject cameraObject)
        {
            if (ListHelper.IsNullOrEmpty(_cameras))
            {
                return;
            }
            
            List<GameObject> defaultCameraObjects = GameObjectID.FindGameObjectsWithID(_defaultCameraObjectsId, true);
            foreach (GameObject otherCameraObject in defaultCameraObjects)
            {
                if (otherCameraObject != null)
                {
                    otherCameraObject.SetActive(otherCameraObject == cameraObject);
                }
            }
            
            foreach (CameraInfo cameraInfo in _cameras)
            {
                GameObject otherCameraObject = cameraInfo.CameraObject;
                if (otherCameraObject != null)
                {
                    otherCameraObject.SetActive(otherCameraObject == cameraObject);
                }
            }

            _currentCameraObject = cameraObject;
        }
        
        private int GetCurrentCameraIndex()
        {
            int index = 0;

            bool Match(CameraInfo info) => info.CameraObject != null && info.CameraObject == _currentCameraObject;
            if (_currentCameraObject != null && _cameras.Exists(Match))
            {
                index = _cameras.FindIndex(Match);
            }

            return index;
        }

        private void RegisterListeners()
        {
            if (_cameras != null)
            {
                foreach (CameraInfo cameraInfo in _cameras)
                {
                    if (cameraInfo.InputReference != null)
                    {
                        cameraInfo.InputReference.action.performed += SwitchCamera;
                    }
                }
            }

            if (_previousCameraInput != null)
            {
                _previousCameraInput.action.performed += SwitchToPreviousCamera;
            }
            
            if (_nextCameraInput != null)
            {
                _nextCameraInput.action.performed += SwitchToNextCamera;
            }
            
            if (_closestCameraInput != null)
            {
                _closestCameraInput.action.performed += SwitchToClosestCamera;
            }
            
            if (_defaultCameraInput != null)
            {
                _defaultCameraInput.action.performed += SwitchToDefaultCameras;
            }
        }

        private void UnregisterListeners()
        {
            if (_cameras != null)
            {
                foreach (CameraInfo cameraInfo in _cameras)
                {
                    if (cameraInfo.InputReference != null)
                    {
                        cameraInfo.InputReference.action.performed -= SwitchCamera;
                    }
                }
            }
            
            if (_previousCameraInput != null)
            {
                _previousCameraInput.action.performed -= SwitchToPreviousCamera;
            }
            
            if (_nextCameraInput != null)
            {
                _nextCameraInput.action.performed -= SwitchToNextCamera;
            }
            
            if (_closestCameraInput != null)
            {
                _closestCameraInput.action.performed -= SwitchToClosestCamera;
            }
            
            if (_defaultCameraInput != null)
            {
                _defaultCameraInput.action.performed -= SwitchToDefaultCameras;
            }
        }
    }
}