namespace ToolkitEngine.XR.VisualScripting
{
    public static partial class EventHooks
    {
		// XR Interactables
		public const string OnXRInteractableHoverEntered = nameof(OnXRInteractableHoverEntered);
		public const string OnXRInteractableHoverExited = nameof(OnXRInteractableHoverExited);
        public const string OnXRInteractableFirstHoverEntered = nameof(OnXRInteractableFirstHoverEntered);
        public const string OnXRInteractableLastHoverExited = nameof(OnXRInteractableLastHoverExited);
		public const string OnXRInteractableSelectEntered = nameof(OnXRInteractableSelectEntered);
		public const string OnXRInteractableSelectExited = nameof(OnXRInteractableSelectExited);
		public const string OnXRInteractableActivated = nameof(OnXRInteractableActivated);
		public const string OnXRInteractableDeactivated = nameof(OnXRInteractableDeactivated);
        public const string OnXRInteractablePlugged = nameof(OnXRInteractablePlugged);
        public const string OnXRInteractableUnplugged = nameof(OnXRInteractableUnplugged);
		public const string OnXRInteractableGazed = nameof(OnXRInteractableGazed);
		public const string OnXRInteractableUngazed = nameof(OnXRInteractableUngazed);

        // XR Interactors
        public const string OnXRInteractorHoverEntered = nameof(OnXRInteractorHoverEntered);
        public const string OnXRInteractorHoverExited = nameof(OnXRInteractorHoverExited);
        public const string OnXRSocketPlugged = nameof(OnXRSocketPlugged);
        public const string OnXRSocketUnplugged = nameof(OnXRSocketUnplugged);

        // XR Interactions
        public const string OnXRInputInteractionPerformed = nameof(OnXRInputInteractionPerformed);
        public const string OnXRInputInteractionCanceled = nameof(OnXRInputInteractionCanceled);

        // XR Position Interactables
        public const string OnXRInteractableDepthChanged = nameof(OnXRInteractableDepthChanged);
        public const string OnXRInteractableTipflow = nameof(OnXRInteractableTipflow);
        public const string OnXRInteractableTailflow = nameof(OnXRInteractableTailflow);
    }
}