using Unity.VisualScripting;
using System;

namespace ToolkitEngine.XR.VisualScripting
{
    [UnitTitle("On Hover Entered"), UnitSurtitle("XRBaseInteractor")]
    public class OnXRInteractorHover : XRBaseInteractorEventUnit
    {
        public override Type MessageListenerType => typeof(OnXRInteractorHoverMessageListener);
    }
}