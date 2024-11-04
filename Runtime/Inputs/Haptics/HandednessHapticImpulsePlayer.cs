using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace ToolkitEngine.XR.Inputs.Haptics
{
	[AddComponentMenu("XR/Haptics/Handedness Haptic Impulse Player", 21)]
	[RequireComponent(typeof(HapticImpulsePlayer))]
	public class HandednessHapticImpulsePlayer : MonoBehaviour
	{
		#region Fields

		[SerializeField]
		private InteractorHandedness m_handedness = InteractorHandedness.None;

		private HapticImpulsePlayer m_hapticImpulsePlayer;

		#endregion

		#region Properties

		public InteractorHandedness handedness => m_handedness;
		public HapticImpulsePlayer hapticImpulsePlayer => m_hapticImpulsePlayer;

		#endregion

		#region Methods

		private void Awake()
		{
			m_hapticImpulsePlayer = GetComponent<HapticImpulsePlayer>();
		}

		private void OnEnable()
		{
			HapticManager.CastInstance.Register(this);
		}

		private void OnDisable()
		{
			HapticManager.CastInstance.Unregister(this);
		}

		#endregion
	}
}