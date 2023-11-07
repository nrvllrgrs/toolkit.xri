using Unity.VisualScripting;
using System;

namespace ToolkitEngine.XR.VisualScripting
{
    [UnitTitle("On Hover Exited"), UnitSurtitle("XRBaseInteractor")]
    public class OnXRInteractorHoverExited : XRBaseInteractorEventUnit
    {
        public override Type MessageListenerType => typeof(OnXRInteractorHoverExitedMessageListener);
    }
}