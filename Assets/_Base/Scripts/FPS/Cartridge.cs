using UnityEngine;
using UnityEngine.Serialization;

namespace FireRingStudio.FPS
{
    public class Cartridge : MonoBehaviour
    {
        [FormerlySerializedAs("casingMesh")]
        [SerializeField] private MeshRenderer _casingMesh;
        [FormerlySerializedAs("emptyCasingMesh")]
        [SerializeField] private MeshRenderer _emptyCasingMesh;
        [FormerlySerializedAs("keepEmptyCasing")]
        [SerializeField] private bool _keepEmptyCasing;

        private bool _filled;
        

        public void Fill()
        {
            if (_emptyCasingMesh != null)
            {
                _emptyCasingMesh.enabled = _keepEmptyCasing;
            }
            
            if (_casingMesh != null)
            {
                _casingMesh.enabled = true;
            }
        }

        public void EmptyOut()
        {
            if (_emptyCasingMesh != null)
            {
                _emptyCasingMesh.enabled = _casingMesh.enabled;
            }
            
            if (_casingMesh != null)
            {
                _casingMesh.enabled = false;
            }
        }
        
        public void SetEnabled(bool value)
        {
            if (_casingMesh != null)
            {
                _casingMesh.enabled = value;
            }

            if (_emptyCasingMesh != null)
            {
                _emptyCasingMesh.enabled = value;
            }
        }
    }
}
