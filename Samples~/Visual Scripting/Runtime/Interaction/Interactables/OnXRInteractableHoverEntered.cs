using Unity.VisualScripting;
using System;

namespace ToolkitEngine.XR.VisualScripting
{
	[UnitTitle("On Hover Entered"), UnitSurtitle("XRBaseInteractable")]
    public class OnXRInteractableHoverEntered : XRBaseInteractableEventUnit
    {
		#region Fields

		[UnitHeaderInspectable("First Entered")]
		public bool firstHoverEntered;

		#endregion

		#region Properties

		protected override string hookName => firstHoverEntered
			? EventHooks.OnXRInteractableFirstHoverEntered
			: EventHooks.OnXRInteractableHoverEntered;

		public override Type MessageListenerType => firstHoverEntered
			? typeof(OnXRInteractableFirstHoverEnteredMessageListener)
			: typeof(OnXRInteractableHoverEnteredMessageListener);

		#endregion
	}
}