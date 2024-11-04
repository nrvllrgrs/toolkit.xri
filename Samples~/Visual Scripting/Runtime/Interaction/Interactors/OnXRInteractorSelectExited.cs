using Unity.VisualScripting;
using System;

namespace ToolkitEngine.XR.VisualScripting
{
    [UnitTitle("On Select Exited"), UnitSurtitle("XRBaseInteractor")]
    public class OnXRInteractorSelectExited : XRBaseInteractorEventUnit
    {
        public override Type MessageListenerType => typeof(OnXRInteractorSelectExitedMessageListener);
    }
}