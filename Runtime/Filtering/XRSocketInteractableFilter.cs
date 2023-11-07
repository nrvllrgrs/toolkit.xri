using UnityEngine.Assertions;

namespace UnityEngine.XR.Interaction.Toolkit.Filtering
{
	public class XRSocketInteractableFilter : MonoBehaviour, IXRHoverFilter, IXRSelectFilter
	{
		#region Fields

		[SerializeField]
		private XRBaseInteractable m_interactable;

		[SerializeField]
		private bool m_ifSocketed = true;

		#endregion

		#region Properties

		public bool canProcess => true;

		#endregion

		#region Methods

		private void Awake()
		{
			m_interactable = m_interactable ?? GetComponent<XRBaseInteractable>();
			Assert.IsNotNull(m_interactable);
		}

		public bool Process(IXRHoverInteractor interactor, IXRHoverInteractable interactable)
		{
			return Process();
		}

		public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
		{
			return Process();
		}

		private bool Process()
		{
			if (m_interactable == null || !m_interactable.enabled)
				return false;

			foreach (var interactor in m_interactable.interactorsSelecting)
			{
				if (interactor is XRSocketInteractor)
					return m_ifSocketed;
			}

			return !m_ifSocketed;
		}

		#endregion
	}
}