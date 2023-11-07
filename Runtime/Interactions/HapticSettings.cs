using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR
{
    [System.Serializable]
    public class HapticSettings
    {
        #region Fields

        [SerializeField, Range(0f, 1f), Tooltip("Intensity of haptic impulse.")]
        private float m_intensity;

        [SerializeField, Min(0f), Tooltip("Seconds of haptic impulse.")]
        private float m_duration = 0.1f;

        [SerializeField, Tooltip("Indicates whether haptic is continously triggered.")]
        private bool m_continuous;

        [SerializeField, Min(0f), Tooltip("Seconds to wait between haptic impulses.")]
        private float m_delay = 0f;

        private Dictionary<XRBaseControllerInteractor, Coroutine> m_coroutineMap = new();

        #endregion

        #region Properties

        public float intensity { get => m_intensity; set => m_intensity = value; }
        public float duration { get => m_duration; set => m_duration = value; }
        public bool continuous => m_continuous;

        #endregion

        #region Methods

        public void SendImpulse(XRBaseControllerInteractor controllerInteractor)
        {
            if (controllerInteractor?.xrController == null)
                return;

            if (!m_continuous)
            {
                controllerInteractor.SendHapticImpulse(m_intensity, m_duration);
            }
            else
            {
                CancelImpulse(controllerInteractor);

                var routinue = controllerInteractor.StartCoroutine(AsyncSendImpulse(controllerInteractor));
                m_coroutineMap.Add(controllerInteractor, routinue);
            }
        }

        public void CancelImpulse(XRBaseControllerInteractor controllerInteractor)
        {
            if (controllerInteractor == null)
                return;

            if (continuous && m_coroutineMap.TryGetValue(controllerInteractor, out var routinue))
            {
                controllerInteractor.StopCoroutine(routinue);
                m_coroutineMap.Remove(controllerInteractor);
            }
        }

        private IEnumerator AsyncSendImpulse(XRBaseControllerInteractor controllerInteractor)
        {
            while (true)
            {
                controllerInteractor.SendHapticImpulse(m_intensity, m_duration);
                yield return new WaitForSeconds(m_duration + m_delay);
            }
        }

        #endregion
    }
}