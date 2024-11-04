using Unity.VisualScripting;
using System;

namespace ToolkitEngine.XR.VisualScripting
{
    [UnitTitle("On Unplugged"), UnitSurtitle("XRBaseInteractor")]
    public class OnXRSocketUnplugged : XRBaseInteractorEventUnit
    {
        public override Type MessageListenerType => typeof(OnXRSocketUnpluggedMessageListener);
    }
}