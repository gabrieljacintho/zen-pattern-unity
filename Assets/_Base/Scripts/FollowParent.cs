using UnityEngine;

namespace FireRingStudio
{
    public class FollowParent : Follow
    {
        protected override Vector3 GetTargetPosition()
        {
            return transform.parent != null ? transform.parent.position : transform.position;
        }

        protected override Quaternion GetTargetRotation()
        {
            return transform.parent != null ? transform.parent.rotation : transform.rotation;
        }
    }
}