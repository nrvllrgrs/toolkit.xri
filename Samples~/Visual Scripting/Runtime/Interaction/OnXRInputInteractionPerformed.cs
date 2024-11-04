using System;
using Unity.VisualScripting;

namespace ToolkitEngine.XR.VisualScripting
{
    [UnitTitle("On Performed")]
    public class OnXRInputInteractionPerformed : XRBaseInputInteractionEventUnit
	{
        public override Type MessageListenerType => typeof(OnXRInputInteractionPerformedMessageListener);
    }
}