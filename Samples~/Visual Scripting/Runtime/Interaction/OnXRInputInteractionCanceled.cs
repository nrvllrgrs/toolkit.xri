using System;
using Unity.VisualScripting;

namespace ToolkitEngine.XR.VisualScripting
{
	[UnitTitle("On Canceled")]
	public class OnXRInputInteractionCanceled : XRBaseInputInteractionEventUnit
	{
		public override Type MessageListenerType => typeof(OnXRInputInteractionCanceledMessageListener);
	}
}