using UnityEngine;
using UnityEngine.UI;

namespace FireRingStudio.SceneManagement
{
    [RequireComponent(typeof(Image))]
    public class SceneLoadingBar : MonoBehaviour
    {
        [SerializeField] private SceneLoader _sceneLoader;
        [SerializeField] private float _smoothTime = 0.1f;
        
        private Image _image;
        
        private float _velocity;

        private float Progress => _sceneLoader != null ? _sceneLoader.AsyncLoadProgress : 0f;


        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void Update()
        {
            float fillAmount = Mathf.SmoothDamp(_image.fillAmount, Progress, ref _velocity, _smoothTime);
            
            _image.fillAmount = fillAmount;
        }
    }
}