

namespace ToolkitEngine.XR
{
	public static class XRBaseInteractorExt
    {
        public static void ForceSelectExit(this UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInteractor interactor)
        {
            if (!interactor.hasSelection)
                return;

            interactor.interactionManager.SelectExit(interactor, interactor.firstInteractableSelected);
        }
    }
}