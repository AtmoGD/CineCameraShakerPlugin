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

        private ShakeOptions currentShakeOptions;
        private CinemachineBasicMultiChannelPerlin noise;
        private float shakeDuration;
        private float shakeAmplitude;
        private float shakeFrequency;

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

            if (shakeDuration > 0f)
            {
                defaultShakeOptions.GetAmplitudeAndFrequency(shakeDuration, out shakeAmplitude, out shakeFrequency);
                noise.m_FrequencyGain = shakeFrequency;
                noise.m_AmplitudeGain = shakeAmplitude;
                shakeDuration -= Time.deltaTime;
            }
            else
            {
                noise.m_FrequencyGain = 0f;
                noise.m_AmplitudeGain = 0f;
            }
        }

        public void ResetNoise()
        {
            shakeDuration = 0f;
            shakeAmplitude = 0f;
            shakeFrequency = 0f;

            if(noise != null)
            {
                noise.m_FrequencyGain = 0f;
                noise.m_AmplitudeGain = 0f;
            }
        }

        public void Shake()
        {
            Shake(defaultShakeOptions);
        }

        public void Shake(ShakeOptions _options)
        {
            if (!_options.overrideIfAlreadyShaking && shakeDuration > 0)
                return;

            if (updateNoiseOnSettingsChanged)
                LoadCameraNoise();

            shakeDuration = _options.shakeDuration;
            currentShakeOptions = _options;
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

