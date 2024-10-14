using System.Collections.Generic;
using FireRingStudio.Helpers;
using UnityEngine;

namespace FireRingStudio.Extensions
{
    public static class AnimatorExtensions
    {
        public static AnimatorClipInfo GetAnimatorClipInfo(this Animator animator, string clipName, int layerIndex = 0)
        {
            List<AnimatorClipInfo> animatorClipInfos = new List<AnimatorClipInfo>();
            animator.GetCurrentAnimatorClipInfo(layerIndex, animatorClipInfos);
            
            return animatorClipInfos.Find(x => x.clip != null && x.clip.name == clipName);
        }

        public static void ResetAllTriggers(this Animator animator)
        {
            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Trigger)
                {
                    animator.ResetTrigger(parameter.name);
                }
            }
        }
    }
}