using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Unity.VisualScripting;

namespace ToolkitEngine.XR.VisualScripting
{
	[UnitTitle("On Ungazed"), UnitSurtitle("XRBaseInteractable")]
	public class OnXRInteractableUngazed : XRBaseInteractableEventUnit
	{
		public override Type MessageListenerType => typeof(OnXRInteractableUngazedMessageListener);

		protected override bool ShouldTrigger(Flow flow, BaseInteractionEventArgs args)
		{
			return (args.interactorObject is XRGazeInteractor) && base.ShouldTrigger(flow, args);
		}
	}
}