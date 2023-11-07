using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR
{
    public class XRKnobInteractable : XRBaseRotationInteractable
    {
        #region Fields

        [SerializeField, Tooltip("Indicates whether interaction is canceled when interactor unhovers.")]
        private bool m_cancelOnUnhover = true;

        #endregion

        #region Methods

        protected override Vector3 GetInteractorRotation()
        {
            return m_interactor.transform.rotation * GetAxisDirection(m_upward);
        }

        protected override void OnHoverExited(HoverExitEventArgs args)
        {
            base.OnHoverExited(args);

            if (m_cancelOnUnhover)
            {
                EndInteraction(args);
            }
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
            BeginInteraction(args);
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);
            EndInteraction(args);
        }

        #endregion
    }
}