using UnityEngine;
using UnityEngine.Serialization;

namespace FireRingStudio.Surface
{
	[RequireComponent(typeof(Collider))]
	public class Surface : MonoBehaviour
	{
		[FormerlySerializedAs("data")]
		[SerializeField] private SurfaceData _data;

		public SurfaceData Data => _data;
	}
}
