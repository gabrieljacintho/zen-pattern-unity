using System;
using UnityEngine;

namespace FireRingStudio.Helpers
{
    public static class MathHelper
    {
        public static float Lerp3(float a, float b, float c, float t)
        {
            float value;
            if (t <= 0.5f)
            {
                value = Mathf.Lerp(a, b, t);
            }
            else
            {
                t = (t - 0.5f) / 0.5f;
                value = Mathf.Lerp(b, c, t);
            }
            
            return value;
        }
        
        public static int WrapIndex(int length, int index)
        {
            if (index < 0)
            {
                index = length - 1;
            }
            else if (index >= length)
            {
                index = 0;
            }

            return index;
        }

        public static void DelayedAction(Action action, float seconds, ref float time, ref bool running, bool unscaledDeltaTime = false)
        {
            if (!running)
            {
                return;
            }

            if (time >= seconds)
            {
                action.Invoke();
                running = false;
            }

            time += unscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime;
        }
    }
}