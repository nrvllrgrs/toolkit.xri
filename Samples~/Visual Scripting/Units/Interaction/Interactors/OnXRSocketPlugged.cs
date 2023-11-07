using Unity.VisualScripting;
using System;

namespace ToolkitEngine.XR.VisualScripting
{
    [UnitTitle("On Plugged"), UnitSurtitle("XRBaseInteractor")]
    public class OnXRSocketPlugged : XRBaseInteractorEventUnit
    {
        public override Type MessageListenerType => typeof(OnXRSocketPluggedMessageListener);
    }
}