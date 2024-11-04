using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace ToolkitEngine.XR.Inputs.Haptics
{
	public class HapticManager : Subsystem<HapticManager>
    {
		#region Fields

		private Dictionary<InteractorHandedness, HapticImpulsePlayer> m_map = new();

		#endregion

		#region Methods

		public void Register(HandednessHapticImpulsePlayer hapticImpulsePlayer)
		{
			if (m_map.ContainsKey(hapticImpulsePlayer.handedness))
				return;

			m_map.Add(hapticImpulsePlayer.handedness, hapticImpulsePlayer.hapticImpulsePlayer);
		}

		public void Unregister(HandednessHapticImpulsePlayer hapticImpulsePlayer)
		{
			if (!m_map.ContainsKey(hapticImpulsePlayer.handedness))
				return;

			m_map.Remove(hapticImpulsePlayer.handedness);
		}

		public bool TryGetHapticImpulsePlayer(InteractorHandedness handedness, out HapticImpulsePlayer hapticImpulsePlayer)
		{
			return m_map.TryGetValue(handedness, out hapticImpulsePlayer);
		}

		public void SendImpulse(InteractorHandedness handedness, float amplitude, float duration)
		{
			SendImpulse(handedness, amplitude, duration, 0f);
		}

		public void SendImpulse(InteractorHandedness handedness, float amplitude, float duration, float frequency)
		{
			if (!TryGetHapticImpulsePlayer(handedness, out var hapticImpulsePlayer))
				return;

			hapticImpulsePlayer.SendHapticImpulse(amplitude, duration, frequency);
		}

		public void SendImpulse(InteractorHandedness handedness, HapticSettings settings)
		{
			if (!TryGetHapticImpulsePlayer(handedness, out var hapticImpulsePlayer))
				return;

			// Go through settings because it may continous haptics
			settings.SendImpulse(hapticImpulsePlayer);
		}

		public void CancelImpulse(InteractorHandedness handedness, HapticSettings settings)
		{
			if (!TryGetHapticImpulsePlayer(handedness, out var hapticImpulsePlayer))
				return;

			settings.CancelImpulse(hapticImpulsePlayer);
		}

		#endregion
	}
}