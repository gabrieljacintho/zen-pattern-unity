using FireRingStudio.Character;
using FireRingStudio.Movement;
using UnityEngine;

namespace FireRingStudio.FPS
{
    public class WeaponSway : MonoBehaviour
    {
        public PlayerLook PlayerLook { get; private set; }
        public CharacterGravity CharacterGravity { get; private set; }

        [Header("Settings")]
        [Min(0f)] public float swayMultiplier = 2f;
        [Min(0f)] public float smoothTime = 5f;


        private void Awake()
        {
            PlayerLook = GetComponentInParent<PlayerLook>();
            if (PlayerLook == null)
                Debug.LogNoInParent(nameof(Movement.PlayerLook));
            
            CharacterGravity = GetComponentInParent<CharacterGravity>();
            if (CharacterGravity == null)
                Debug.LogNoInParent(nameof(Character.CharacterGravity));
        }

        private void Update()
        {
            if (GameManager.IsPaused)
            {
                return;
            }

            Vector2 delta = Vector2.zero;

            if (PlayerLook != null)
            {
                delta += PlayerLook.Delta.normalized * swayMultiplier;
            }

            if (CharacterGravity != null)
            {
                float gravity = CharacterGravity.Velocity.y - CharacterGravity.GravityScaleOnGround;
                if (gravity != 0f)
                {
                    gravity = gravity > 0 ? 1f : -1f;
                }
                
                delta.y -= gravity * swayMultiplier;
            }
            
            Quaternion rotationX = Quaternion.AngleAxis(-delta.y, Vector3.right);
            Quaternion rotationY = Quaternion.AngleAxis(delta.x, Vector3.up);
            Quaternion targetRotation = rotationX * rotationY;

            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smoothTime * Time.deltaTime);
        }
    }
}