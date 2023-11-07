using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR
{
    public class XRButtonInteractable : XRBasePositionInteractable
    {
        #region Fields

        [SerializeField, Range(0f, 180f)]
        protected float m_allowedAngle = 90f;

        #endregion

        #region Properties

        protected override IEnumerable<XRBaseControllerInteractor> controllerInteractors
        {
            get
            {
                var list = new List<XRBaseControllerInteractor>();
                foreach (var interactor in interactorsHovering)
                {
                    var controllerInteractor = interactor as XRBaseControllerInteractor;
                    if (controllerInteractor != null)
                    {
                        list.Add(controllerInteractor);
                    }
                }

                return list;
            }
        }

        #endregion

        #region Methods

        protected override void OnHoverEntered(HoverEnterEventArgs args)
        {
            base.OnHoverEntered(args);
            BeginInteraction(args);
        }

        protected override void OnHoverExited(HoverExitEventArgs args)
        {
            base.OnHoverExited(args);
            EndInteraction(args);
        }

        protected override void BeginInteraction(BaseInteractionEventArgs args)
        {
            // Calculate angle button is being pushed
            // Don't want to push button if entered from behind and then pushed in direction
            var entryDir = args.interactorObject.transform.position - transform.position;
            if (Vector3.Angle(direction, entryDir) < m_allowedAngle)
                return;

            base.BeginInteraction(args);
        }

        #endregion
    }
}