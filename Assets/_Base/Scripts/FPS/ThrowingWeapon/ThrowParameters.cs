using System;
using UnityEngine;

namespace FireRingStudio.FPS.ThrowingWeapon
{
    [Serializable]
    public struct ThrowParameters
    {
        public float Force;
        public float TorqueRange;
        public float Distance;
        public LayerMask ObstacleLayers;


        public ThrowParameters(float force, float torqueRange, float distance, LayerMask obstacleLayers = default)
        {
            Force = force;
            TorqueRange = torqueRange;
            Distance = distance;
            ObstacleLayers = obstacleLayers;
        }
    }
}