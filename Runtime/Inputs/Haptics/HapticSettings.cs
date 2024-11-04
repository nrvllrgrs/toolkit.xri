using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

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

		private Dictionary<HapticImpulsePlayer, Coroutine> m_coroutineMap = new();
		private Dictionary<GameObject, HapticImpulsePlayer> m_hapticMap = new();

		#endregion

		#region Properties

		public float intensity { get => m_intensity; set => m_intensity = value; }
        public float duration { get => m_duration; set => m_duration = value; }
        public bool continuous => m_continuous;
        public float delay { get => m_delay; set => m_delay = value; }

		#endregion

		#region HapticImpulsePlayer Methods

		public void SendImpulse(HapticImpulsePlayer hapticImpulsePlayer)
		{
			if (!m_continuous)
			{
				hapticImpulsePlayer.SendHapticImpulse(m_intensity, m_duration);
			}
			else
			{
				CancelImpulse(hapticImpulsePlayer);

				var routinue = hapticImpulsePlayer.StartCoroutine(AsyncSendImpulse(hapticImpulsePlayer));
				m_coroutineMap.Add(hapticImpulsePlayer, routinue);
			}
		}

		public void CancelImpulse(HapticImpulsePlayer hapticImpulsePlayer)
		{
			if (hapticImpulsePlayer == null)
				return;

			if (continuous && m_coroutineMap.TryGetValue(hapticImpulsePlayer, out var routinue))
			{
				hapticImpulsePlayer.StopCoroutine(routinue);
				m_coroutineMap.Remove(hapticImpulsePlayer);
			}
		}

		private IEnumerator AsyncSendImpulse(HapticImpulsePlayer hapticImpulsePlayer)
		{
			while (true)
			{
				hapticImpulsePlayer.SendHapticImpulse(m_intensity, m_duration);
				yield return new WaitForSeconds(m_duration + m_delay);
			}
		}

		#endregion

		#region XRBaseInputInteractor Methods

		private HapticImpulsePlayer GetOrCreateHapticImpulsePlayer(GameObject obj)
		{
			HapticImpulsePlayer hapticImpulsePlayer;
			if (m_hapticMap.TryGetValue(obj, out hapticImpulsePlayer))
				return hapticImpulsePlayer;

			hapticImpulsePlayer = obj.GetComponentInParent<HapticImpulsePlayer>(true);
			if (hapticImpulsePlayer == null)
			{
				// Try to add the component in the hierarchy where it can be found and shared by other interactors.
				// Otherwise, just add it to this GameObject.
				var impulseProvider = obj.GetComponentInParent<IXRHapticImpulseProvider>(true);
				var impulseProviderComponent = impulseProvider as Component;

				hapticImpulsePlayer = impulseProviderComponent != null
					? impulseProviderComponent.gameObject.AddComponent<HapticImpulsePlayer>()
					: obj.AddComponent<HapticImpulsePlayer>();
			}

			m_hapticMap.Add(obj, hapticImpulsePlayer);
			return hapticImpulsePlayer;
		}

		public void SendImpulse(XRBaseInputInteractor controllerInteractor)
        {
            if (controllerInteractor == null)
                return;

			SendImpulse(GetOrCreateHapticImpulsePlayer(controllerInteractor.gameObject));
        }

		public void CancelImpulse(XRBaseInputInteractor controllerInteractor)
        {
            if (controllerInteractor == null)
                return;

			CancelImpulse(GetOrCreateHapticImpulsePlayer(controllerInteractor.gameObject));
		}

		#endregion
	}
}