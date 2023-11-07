using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR
{
    public class XRHapticInputEventsListener : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private XRBaseInputEvents m_inputEvents;

        [SerializeField]
        private HapticSettings m_haptics = new();

        #endregion

        #region Methods

        private void Awake()
        {
            m_inputEvents = m_inputEvents ?? GetComponent<XRBaseInputEvents>();
            Assert.IsNotNull(m_inputEvents);
        }

        private void OnEnable()
        {
            m_inputEvents.onPerformed.AddListener(InputEvents_Performed);
            m_inputEvents.onCanceled.AddListener(InputEvent_Canceled);
        }

        private void OnDisable()
        {
            m_inputEvents.onPerformed.RemoveListener(InputEvents_Performed);
			m_inputEvents.onCanceled.RemoveListener(InputEvent_Canceled);

            // Be sure to turn off haptics; could be continuous duration
            m_haptics.CancelImpulse(XRHandednessContext.leftController);
			m_haptics.CancelImpulse(XRHandednessContext.rightController);
		}

        private void InputEvents_Performed(InputAction.CallbackContext obj)
        {
            if (m_inputEvents.IsLeftHand(obj))
            {
				m_haptics.SendImpulse(XRHandednessContext.leftController);
			}
            else
            {
                m_haptics.SendImpulse(XRHandednessContext.rightController);
            }
        }

        private void InputEvent_Canceled(InputAction.CallbackContext obj)
        {
			if (m_inputEvents.IsLeftHand(obj))
			{
				m_haptics.CancelImpulse(XRHandednessContext.leftController);
			}
			else
			{
				m_haptics.CancelImpulse(XRHandednessContext.rightController);
			}
		}

		#endregion
	}
}