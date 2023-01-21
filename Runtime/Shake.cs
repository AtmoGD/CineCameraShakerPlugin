using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CinemachineShaker
{
    public class Shake
    {
        public Shake(ShakeOptions options, float distance = 0f)
        {
            this.options = options;
            this.timeLeft = options.shakeDuration;
            this.distance = distance;
        }

        public ShakeOptions options = null;
        public float timeLeft = 0f;
        public float distance = 0f;
    }
}