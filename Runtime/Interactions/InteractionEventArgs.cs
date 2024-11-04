namespace UnityEngine.XR.Interaction.Toolkit
{
    public class InteractionEventArgs : BaseInteractionEventArgs
    {
        #region Fields

        private XRInteractionManager m_manager;

        #endregion

        #region Properties

        public XRInteractionManager manager
        {
            get
            {
                if (m_manager == null)
                {
                    m_manager = interactableObject?.transform
                        .GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>()
                        ?.interactionManager;
                }
                return m_manager;
            }
        }

        #endregion
    }
}