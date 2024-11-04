using Unity.VisualScripting;
using System;

namespace ToolkitEngine.XR.VisualScripting
{
    [UnitTitle("On Select Entered"), UnitSurtitle("XRBaseInteractor")]
    public class OnXRInteractorSelectEntered : XRBaseInteractorEventUnit
    {
        public override Type MessageListenerType => typeof(OnXRInteractorSelectEnteredMessageListener);
    }
}