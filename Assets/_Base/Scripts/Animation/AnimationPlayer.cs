using UnityEngine;

namespace FireRingStudio.Animation
{
    [RequireComponent(typeof(UnityEngine.Animation))]
    public class AnimationPlayer : MonoBehaviour
    {
        private UnityEngine.Animation _animation;

        public UnityEngine.Animation Animation
        {
            get
            {
                if (_animation == null)
                {
                    _animation = GetComponent<UnityEngine.Animation>();
                }

                return _animation;
            } 
        }


        public void Play()
        {
            Animation.Play();
        }

        public void Replay()
        {
            Animation.Stop();
            Animation.Play();
        }
    }
}