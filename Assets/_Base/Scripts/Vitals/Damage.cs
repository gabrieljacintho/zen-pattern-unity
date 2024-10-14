using System;
using UnityEngine;

namespace FireRingStudio.Vitals
{
    public enum DamageType
    {
        Impact,
        Fire,
        Water,
        Poison,
        Explosion
    }

    [Serializable]
    public struct Damage
    {
        public DamageType Type;
        public float Amount;
        public float Duration;
        public Transform Transform;
        
        public Vector3 Position
        {
            get
            {
                if (Transform != null)
                    return Transform.position;
                
                return _position;
            }
            set
            {
                Transform = null;
                _position = value;
            }
        }

        private Vector3 _position;

        
        public Damage(DamageType type, float amount, float duration, Vector3 position, Transform transform = null)
        {
            Type = type;
            Amount = amount;
            Duration = duration;
            _position = position;
            Transform = transform;
        }
        
        public Damage(DamageType type, float amount, Vector3 position) : this(type, amount, 0f, position)
        {
            
        }

        public Damage(DamageType type, float amount, float duration, Transform transform) : this(type, amount, duration, transform != null ? transform.position : Vector3.zero, transform)
        {
            
        }
        
        public Damage(DamageType type, float amount, Transform transform) : this(type, amount, 0f, transform)
        {
            
        }
        
        public Damage(DamageType type, float amount) : this(type, amount, Vector3.zero)
        {
            
        }

        public override string ToString()
        {
            string value = "Type: " + Type + ", Amount: " + Amount.ToString("F1");

            if (Duration > 0f)
            {
                value += ", Duration: " + Duration.ToString("F1");
            }

            if (Transform != null)
            {
                value += ", Transform: \"" + Transform.name + "\"";
            }

            return value;
        }
    }
}