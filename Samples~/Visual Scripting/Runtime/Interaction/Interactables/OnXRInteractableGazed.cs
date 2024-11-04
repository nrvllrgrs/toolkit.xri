using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.VisualScripting;

namespace ToolkitEngine.XR.VisualScripting
{
	[UnitTitle("On Gazed"), UnitSurtitle("XRBaseInteractable")]
	public class OnXRInteractableGazed : XRBaseInteractableEventUnit
    {
        public override Type MessageListenerType => typeof(OnXRInteractableGazedMessageListener);

        protected override bool ShouldTrigger(Flow flow, BaseInteractionEventArgs args)
        {
            return (args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRGazeInteractor) && base.ShouldTrigger(flow, args);
        }
    }
}