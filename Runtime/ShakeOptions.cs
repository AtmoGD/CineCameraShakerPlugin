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
        public AnimationCurve shakeFrequency;
        public bool overrideIfAlreadyShaking;

        public void GetAmplitudeAndFrequency(float durationLeft, out float amplitude, out float frequency)
        {
            amplitude = shakeAmplitude.Evaluate(1 - (durationLeft / shakeDuration));
            frequency = shakeFrequency.Evaluate(1 - (durationLeft / shakeDuration));
        }
    }
}