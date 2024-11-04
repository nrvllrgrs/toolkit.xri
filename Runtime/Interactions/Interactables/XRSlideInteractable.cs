using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR
{
    public class XRSlideInteractable : XRBasePositionInteractable
    {
        #region Fields

        [SerializeField]
        private bool m_cancelInteractionOnUnhover = true;

        #endregion

        #region Properties

        protected override IEnumerable<UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor> controllerInteractors
        {
            get
            {
                var list = new List<UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor>();
                var controllerInteractor = firstInteractorSelecting as UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor;
                if (controllerInteractor != null)
                {
                    list.Add(controllerInteractor);
                }

                return list;
            }
        }

        #endregion

        #region Methods

        protected override void OnHoverExited(HoverExitEventArgs args)
        {
            base.OnHoverExited(args);

            if (m_cancelInteractionOnUnhover)
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