using UnityEngine.Assertions;

namespace UnityEngine.XR.Interaction.Toolkit.Filtering
{
	public class XRSocketInteractableFilter : MonoBehaviour, IXRHoverFilter, IXRSelectFilter
	{
		#region Fields

		[SerializeField]
		private UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable m_interactable;

		[SerializeField]
		private bool m_ifSocketed = true;

		#endregion

		#region Properties

		public bool canProcess => true;

		#endregion

		#region Methods

		private void Awake()
		{
			m_interactable = m_interactable ?? GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
			Assert.IsNotNull(m_interactable);
		}

		public bool Process(UnityEngine.XR.Interaction.Toolkit.Interactors.IXRHoverInteractor interactor, UnityEngine.XR.Interaction.Toolkit.Interactables.IXRHoverInteractable interactable)
		{
			return Process();
		}

		public bool Process(UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor interactor, UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable interactable)
		{
			return Process();
		}

		private bool Process()
		{
			if (m_interactable == null || !m_interactable.enabled)
				return false;

			foreach (var interactor in m_interactable.interactorsSelecting)
			{
				if (interactor is UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor)
					return m_ifSocketed;
			}

			return !m_ifSocketed;
		}

		#endregion
	}
}