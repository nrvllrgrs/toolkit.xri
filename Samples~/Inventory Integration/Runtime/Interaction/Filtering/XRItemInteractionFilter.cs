using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.Interaction.Toolkit.Filtering;
using ToolkitEngine.Inventory;

namespace ToolkitEngine.XR
{
	public class XRItemInteractionFilter : MonoBehaviour, IXRHoverFilter, IXRSelectFilter
	{
		#region Fields

		[SerializeField]
		private List<ItemType> m_includeItems;

		[SerializeField, Tooltip("Indicates whether item is spawned in socket at initialization.")]
		private bool m_spawnOnStart = false;

		#endregion

		#region Properties

		public ItemType firstIncludedItem => m_includeItems.Count > 0 ? m_includeItems[0] : null;
		public ItemType[] includedItems => m_includeItems.ToArray();
		public bool canProcess => true;

		#endregion

		#region Methods

		private void Start()
		{
			if (m_spawnOnStart)
			{
				Spawn();
			}
		}

		public bool Process(UnityEngine.XR.Interaction.Toolkit.Interactors.IXRHoverInteractor interactor, UnityEngine.XR.Interaction.Toolkit.Interactables.IXRHoverInteractable interactable)
		{
			return Process(interactable.transform);
		}

		public bool Process(UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor interactor, UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable interactable)
		{
			return Process(interactable.transform);
		}

		private bool Process(Transform transform)
		{
			if (!transform.TryGetComponent(out Item item))
				return false;

			return m_includeItems.Contains(item.itemType);
		}

		public void Spawn()
		{
			var socket = GetComponentInParent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
			if (socket == null)
				return;

			firstIncludedItem.Instantiate(socket.transform.position, socket.transform.rotation, (spawnedObject, args) =>
			{
				var interactable = spawnedObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable>();
				if (interactable == null)
					return;

				var socket = args[0] as UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor;
				if (socket is XRConditionalSocketInteractor conditionalSocket)
				{
					conditionalSocket.ForceSelectEnter(interactable);
				}
				else
				{
					socket.interactionManager.SelectEnter(socket, interactable);
				}
			}, socket);
		}

		#endregion
	}
}