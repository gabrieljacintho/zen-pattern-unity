using BehaviorDesigner.Runtime;
using System.Collections.Generic;
using UnityEngine;

namespace FireRingStudio.BehaviourTree
{
    [System.Serializable]
    public class SharedColliderList : SharedVariable<List<Collider>>
    {
        public SharedColliderList()
        {
            mValue = new List<Collider>();
        }

        public static implicit operator SharedColliderList(List<Collider> value) { return new SharedColliderList { mValue = value }; }
    }
}