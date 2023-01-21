using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Cinemachine;
using System;

namespace CinemachineShaker
{
    [Serializable]
    public class CineShaker : MonoBehaviour
    {

#if UNITY_EDITOR
        [SerializeField] private bool showDebugText = true;
#endif
        public static CineShaker Instance { get; private set; }

        [SerializeField] private bool updateNoiseOnSettingsChanged = true;
        [SerializeField] private CinemachineBrain brain;
        [SerializeField] private ShakeOptions defaultShakeOptions;

        private CinemachineBasicMultiChannelPerlin noise;
        private List<Shake> shakes = new List<Shake>();

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            ResetNoise();

            LoadCameraNoise();
        }

        private void Update()
        {
            if (noise == null) return;

            if (shakes.Count > 0)
            {
                float newAmplitude = 0f;
                float newFrequency = 0f;

                foreach (Shake shake in shakes)
                {
                    float addAmplitude = 0f;
                    float addFrequency = 0f;

                    shake.options.GetAmplitudeAndFrequency(shake.timeLeft, out addAmplitude, out addFrequency);

                    newAmplitude += addAmplitude * (shake.options.useFallOff ? shake.options.fallOff.Evaluate(shake.distance) : 1f);
                    newFrequency += addFrequency * (shake.options.useFallOff ? shake.options.fallOff.Evaluate(shake.distance) : 1f);

                    shake.timeLeft -= Time.deltaTime;

                    if (shake.timeLeft <= 0f)
                        shakes.Remove(shake);
                }
            }
        }

        public void ResetNoise()
        {
            if (noise != null)
            {
                noise.m_FrequencyGain = 0f;
                noise.m_AmplitudeGain = 0f;
            }
        }

        public void Shake()
        {
            Shake(new Shake(defaultShakeOptions));
        }

        public void Shake(Shake _shake)
        {
            if (!_shake.options.overrideIfAlreadyShaking && shakes.Count > 0)
                return;

            if (updateNoiseOnSettingsChanged)
                LoadCameraNoise();

            shakes.Add(_shake);
        }

        private void LoadCinemachineBrain()
        {
            brain = Camera.main.GetComponent<CinemachineBrain>();

#if UNITY_EDITOR
            if (!brain && showDebugText)
                Debug.Log("[CINE_SHAKER]: No brain found in scene. Please add a CinemachineBrain to the main camera.");
#endif

        }

        private void LoadCameraNoise()
        {
            if (!brain)
                LoadCinemachineBrain();

            ICinemachineCamera cam = brain.ActiveVirtualCamera;

            if (cam == null)
            {

#if UNITY_EDITOR
                if (showDebugText)
                    Debug.LogError("[CINE_SHAKER]: No active virtual camera found!");
#endif

                return;
            }

            CinemachineVirtualCamera vcam = cam as CinemachineVirtualCamera;

            if (vcam != null)
            {
                noise = vcam.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();

#if UNITY_EDITOR
                if (noise == null && showDebugText)
                    Debug.LogError("[CINE_SHAKER]: No noise on the camera!");
#endif

            }

            ResetNoise();
        }

        public void SetNewDefaultOptions(ShakeOptions _options)
        {
            defaultShakeOptions = _options;
        }
    }

}

