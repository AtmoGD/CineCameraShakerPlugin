using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CinemachineShaker
{
    [Serializable]
    [CreateAssetMenu(fileName = "ShakeSettings", menuName = "CineShaker/New shake settings")]
    public class ShakeOptions : ScriptableObject
    {
        public float shakeDuration;
        public AnimationCurve shakeAmplitude;
        public float amplitudeMultiplier = 1f;
        public AnimationCurve shakeFrequency;
        public float frequencyMultiplier = 1f;
        public bool useFallOff;
        public AnimationCurve fallOff;
        public bool overrideIfAlreadyShaking;

        public void GetAmplitudeAndFrequency(float durationLeft, out float amplitude, out float frequency)
        {
            amplitude = shakeAmplitude.Evaluate(1 - (durationLeft / shakeDuration)) * amplitudeMultiplier;
            frequency = shakeFrequency.Evaluate(1 - (durationLeft / shakeDuration)) * frequencyMultiplier;
        }
    }

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