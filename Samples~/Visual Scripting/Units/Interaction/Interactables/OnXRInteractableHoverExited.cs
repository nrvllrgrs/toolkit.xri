using Unity.VisualScripting;
using System;

namespace ToolkitEngine.XR.VisualScripting
{
    [UnitTitle("On Hover Exited"), UnitSurtitle("XRBaseInteractable")]
    public class OnXRInteractableHoverExited : XRBaseInteractableEventUnit
    {
		#region Fields

		[UnitHeaderInspectable("Last Exited")]
		public bool lastHoverExited;

		#endregion

		#region Properties

		protected override string hookName => lastHoverExited
			? EventHooks.OnXRInteractableLastHoverExited
			: EventHooks.OnXRInteractableHoverExited;

		public override Type MessageListenerType => lastHoverExited
			? typeof(OnXRInteractableLastHoverExitedMessageListener)
			: typeof(OnXRInteractableHoverExitedMessageListener);

		#endregion
	}
}