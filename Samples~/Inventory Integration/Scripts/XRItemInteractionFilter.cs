using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Filtering;
using ToolkitEngine.Inventory;

namespace ToolkitEngine.XR
{
	public class XRItemInteractionFilter : MonoBehaviour, IXRHoverFilter, IXRSelectFilter
	{
		#region Fields

		[SerializeField]
		private List<ItemType> m_includeItems;

		[SerializeField]
		private bool m_autoSocket = false;

		#endregion

		#region Properties

		public bool canProcess => true;

		#endregion

		#region Methods

		private void Start()
		{
			if (m_autoSocket)
			{
				m_includeItems[0].Instantiate((spawnedObject, args) =>
				{
					var interactable = spawnedObject.GetComponent<IXRSelectInteractable>();
					if (interactable == null)
						return;

					var socket = GetComponentInParent<XRSocketInteractor>();
					if (socket != null)
					{
						interactable.transform.SetPositionAndRotation(
							socket.transform.position,
							socket.transform.rotation);
						socket.interactionManager.SelectEnter(socket, interactable);
					}
				});
			}
		}

		public bool Process(IXRHoverInteractor interactor, IXRHoverInteractable interactable)
		{
			return Process(interactable.transform);
		}

		public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
		{
			return Process(interactable.transform);
		}

		private bool Process(Transform transform)
		{
			if (!transform.TryGetComponent(out Item item))
				return false;

			return m_includeItems.Contains(item.itemType);
		}

		#endregion
	}
}