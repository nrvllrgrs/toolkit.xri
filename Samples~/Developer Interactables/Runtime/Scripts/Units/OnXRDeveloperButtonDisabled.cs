using System;
using Unity.VisualScripting;

namespace ToolkitEngine.VisualScripting
{
    [UnitCategory("Events/Developer")]
    [UnitTitle("On Button Disabled")]
    public class OnXRDeveloperButtonDisabled : BaseEventUnit<EmptyEventArgs>
    {
        protected override bool showEventArgs => false;
        public override Type MessageListenerType => typeof(OnXRDeveloperButtonDisabledMessageListener);
    }
}