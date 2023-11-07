using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR
{
    public class XRHandednessEvents : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private XRBaseInteractable m_interactable;

        [SerializeField]
        private UnityCondition m_predicate = new UnityCondition(UnityCondition.ConditionType.All);

        [SerializeField]
        private HandednessAction m_leftHand;

        [SerializeField]
        private HandednessAction m_rightHand;

        [SerializeField]
        private HandednessAction m_anyHand;

        #endregion

        #region Methods

        private void Awake()
        {
            m_interactable = m_interactable ?? GetComponent<XRBaseInteractable>();
            Assert.IsNotNull(m_interactable, "Interactable is undefined!");

            if (m_interactable.isSelected)
            {
                Interactable_Grabbed(new SelectEnterEventArgs()
                {
                    interactableObject = m_interactable,
                    interactorObject = m_interactable.firstInteractorSelecting
                });
            }
            else
            {
                m_leftHand.Ungrab();
                m_rightHand.Ungrab();
                m_anyHand.Ungrab();
            }
        }

        private void OnEnable()
        {
            m_interactable.selectEntered.AddListener(Interactable_Grabbed);
            m_interactable.selectExited.AddListener(Interactable_Ungrabbed);
        }

        private void OnDisable()
        {
            m_interactable.selectEntered.RemoveListener(Interactable_Grabbed);
            m_interactable.selectExited.RemoveListener(Interactable_Ungrabbed);

			m_leftHand.Ungrab();
			m_rightHand.Ungrab();
			m_anyHand.Ungrab();
		}

        private void Interactable_Grabbed(SelectEnterEventArgs e)
        {
            if (XRHandednessContext.IsLeftHand(e.interactorObject) && m_predicate.isTrueAndEnabled)
            {
                m_leftHand.Grab();
                m_anyHand.Grab();
            }
            else if (XRHandednessContext.IsRightHand(e.interactorObject) && m_predicate.isTrueAndEnabled)
            {
                m_rightHand.Grab();
                m_anyHand.Grab();
            }
        }

        private void Interactable_Ungrabbed(SelectExitEventArgs e)
        {
            if (XRHandednessContext.IsLeftHand(e.interactorObject))
            {
                m_leftHand.Ungrab();
                m_anyHand.Ungrab();
            }
            else if (XRHandednessContext.IsRightHand(e.interactorObject))
            {
                m_rightHand.Ungrab();
                m_anyHand.Ungrab();
            }
        }

        #endregion

        #region Structures

        [System.Serializable]
        public class HandednessAction
        {
            #region Fields

            [SerializeField, Tooltip("List of gameObjects that are active while interactable is selected by hand.")]
            private ObjectActivation[] m_objects;

            #endregion

            #region Events

            [SerializeField]
            private UnityEvent m_onGrabbed;

            [SerializeField]
			private UnityEvent m_onUngrabbed;

            #endregion

            #region Properties

            public ObjectActivation[] objects => m_objects;
            public UnityEvent onGrabbed => m_onGrabbed;
            public UnityEvent onUngrabbed => m_onUngrabbed;

			#endregion

			#region Methods

			public void Grab()
            {
				foreach (var obj in objects)
				{
					obj.Set();
				}
				m_onGrabbed?.Invoke();
            }

            public void Ungrab()
            {
				foreach (var obj in objects)
				{
					obj.Invert();
				}
				m_onUngrabbed?.Invoke();
			}

            #endregion
        }

		#endregion
	}
}