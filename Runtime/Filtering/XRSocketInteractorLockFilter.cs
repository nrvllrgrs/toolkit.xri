using UnityEngine.Assertions;

namespace UnityEngine.XR.Interaction.Toolkit.Filtering
{
	public class XRSocketInteractorLockFilter : MonoBehaviour, IXRHoverFilter, IXRSelectFilter
	{
		#region Fields

		[SerializeField]
		private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor m_interactor;

		[SerializeField, Tooltip("Indicates whether socketed interactable cannot be removed with another interactor.")]
		private bool m_locked = false;

		[SerializeField, Tooltip("Indicates whether socket is unlocked if interactable is forcibly removed.")]
		private bool m_autoUnlock = false;

		#endregion

		#region Properties

		public bool locked { get => m_locked; set => m_locked = value; }

		public bool canProcess => true;

		#endregion

		#region Methods

		private void Awake()
		{
			m_interactor = m_interactor ?? GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
			Assert.IsNotNull(m_interactor);
		}

		private void OnEnable()
		{
			m_interactor.selectEntered.AddListener(Socket_SelectEntered);
			m_interactor.selectExited.AddListener(Socket_SelectExited);
		}

		private void OnDisable()
		{
			m_interactor.selectEntered.RemoveListener(Socket_SelectEntered);
			m_interactor.selectExited.RemoveListener(Socket_SelectExited);
		}

		private void Socket_SelectEntered(SelectEnterEventArgs e)
		{
			var interactable = e.interactableObject.transform.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
			if (interactable == null)
				return;

			// Add filter to interactable
			interactable.hoverFilters.Add(this);
			interactable.selectFilters.Add(this);
		}

		private void Socket_SelectExited(SelectExitEventArgs e)
		{
			if (m_autoUnlock)
			{
				m_locked = false;
			}

			var interactable = e.interactableObject.transform.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
			if (interactable == null)
				return;

			// Remove filter from interactable
			interactable.hoverFilters.Remove(this);
			interactable.selectFilters.Remove(this);
		}

		public bool Process(UnityEngine.XR.Interaction.Toolkit.Interactors.IXRHoverInteractor interactor, UnityEngine.XR.Interaction.Toolkit.Interactables.IXRHoverInteractable interactable)
		{
			return Process(interactor);
		}

		public bool Process(UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor interactor, UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable interactable)
		{
			return Process(interactor);
		}

		private bool Process(UnityEngine.XR.Interaction.Toolkit.Interactors.IXRInteractor interactor)
		{
			// Socketed to interactor, skip
			if (Equals(interactor, m_interactor))
				return true;

			return !m_locked;
		}

		#endregion
	}
}