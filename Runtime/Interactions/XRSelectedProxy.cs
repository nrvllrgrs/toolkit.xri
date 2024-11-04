using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

namespace ToolkitEngine.XR
{
	public class XRSelectedProxy : MonoBehaviour
	{
		#region Fields

		[SerializeField]
		private XRBaseInteractor m_interactor;

		private IXRSelectInteractable m_selectInteractable;

		#endregion

		#region Events

		[SerializeField, Foldout("Events")]
		private UnityEvent<GameObject> m_onSelectEntered;

		[SerializeField, Foldout("Events")]
		private UnityEvent<GameObject> m_onSelectExited;

		#endregion

		#region Properties

		public IXRSelectInteractable selectedInteractable => m_selectInteractable;
		public UnityEvent<GameObject> onSelectEntered => m_onSelectEntered;
		public UnityEvent<GameObject> onSelectExited => m_onSelectExited;

		#endregion

		#region Methods

		private void Awake()
		{
			m_interactor = m_interactor ?? GetComponent<XRBaseInteractor>();
			Assert.IsNotNull(m_interactor);
		}

		private void OnEnable()
		{
			m_interactor.selectEntered.AddListener(SelectEntered);
			m_interactor.selectExited.AddListener(SelectExited);

			if (m_interactor.hasSelection)
			{
				SelectEntered(new SelectEnterEventArgs()
				{
					interactableObject = m_interactor.firstInteractableSelected
				});
			}
		}

		private void OnDisable()
		{
			m_interactor.selectEntered.RemoveListener(SelectEntered);
			m_interactor.selectExited.RemoveListener(SelectExited);
		}

		#endregion

		#region Interactor Callback Methods

		private void SelectEntered(SelectEnterEventArgs e)
		{
			if (m_selectInteractable != null)
				return;

			m_selectInteractable = e.interactableObject;
			m_onSelectEntered?.Invoke(e.interactableObject.transform.gameObject);
		}

		private void SelectExited(SelectExitEventArgs e)
		{
			if (m_selectInteractable == null)
				return;

			m_selectInteractable = null;
			m_onSelectExited?.Invoke(null);
		}

		#endregion
	}
}