using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using ToolkitEngine.Inventory;
using NaughtyAttributes;
using System;

namespace ToolkitEngine.XR
{
	[RequireComponent(typeof(XRPocketInteractor))]
    public class XRInventoryPocketInteractor : MonoBehaviour
    {
		#region Fields

		[SerializeField, Required]
		private Inventory.InventoryList m_inventory;

		/// <summary>
		/// Indicates whether selecting interactable should add item to inventoy. Used to prevent adding extra item when spawning item from inventory.
		/// </summary>
		private bool m_ignoreAdding = false;

		/// <summary>
		/// Interactable object that is currently hovering over pocket that has another interactable socketed
		/// </summary>
		private UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable m_registeredInteractable;

		private int m_slotIndex;
		private XRPocketInteractor m_pocket;

		#endregion

		#region Events

		public event EventHandler<int> SlotIndexChanged;

		#endregion

		#region Properties

		public Inventory.InventoryList inventory => m_inventory;

		/// <summary>
		/// Index of item slot currently in pocket
		/// </summary>
		public int slotIndex
		{
			get => m_slotIndex;
			private set
			{
				// No change, skip
				if (m_slotIndex ==  value)
					return;

				m_slotIndex = value;
				SlotIndexChanged?.Invoke(this, value);
			}
		}

		#endregion

		#region Methods

		private void Awake()
		{
			m_inventory ??= GetComponent<Inventory.InventoryList>();
			m_pocket = GetComponent<XRPocketInteractor>();
		}

		private void Start()
		{
			InstantiateNext();
		}

		private void OnEnable()
		{
			m_inventory.onItemSlotChanged.AddListener(Inventory_ItemSlotChanged);
			m_pocket.hoverEntered.AddListener(Pocket_HoverEntered);
			m_pocket.hoverExited.AddListener(Pocket_HoverExited);
			m_pocket.onPocketed.AddListener(Pocketed);
			m_pocket.onUnpocketed.AddListener(Unpocketed);
		}

		private void OnDisable()
		{
			m_inventory.onItemSlotChanged.RemoveListener(Inventory_ItemSlotChanged);
			m_pocket.hoverEntered.RemoveListener(Pocket_HoverEntered);
			m_pocket.hoverExited.RemoveListener(Pocket_HoverExited);
			m_pocket.onPocketed.RemoveListener(Pocketed);
			m_pocket.onUnpocketed.RemoveListener(Unpocketed);
		}

		private void InstantiateNext()
		{
			if (slotIndex < 0)
				return;

			int count = m_inventory.items.Length;
			for (int i = 0; i < count; ++i)
			{
				int pendingIndex = (i + slotIndex) % count;

				var slot = m_inventory.items[pendingIndex];
				if (slot.amount > 0)
				{
					slotIndex = pendingIndex;
					m_ignoreAdding = true;
					slot.slotType.Instantiate(m_pocket.transform.position, m_pocket.transform.rotation, ItemSpawned);
					return;
				}
			}

			// No item in inventory to pocket
			slotIndex = -1;
		}

		private void ItemSpawned(GameObject obj, params object[] args)
		{
			var grabInteractable = obj.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
			if (grabInteractable != null)
			{
				// Add default grab transformer, if necessary
				grabInteractable.ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase.Dynamic);
			}

			m_pocket.ForceSelectEnter(obj.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable>(), false);
			m_ignoreAdding = false;
		}

		private void Inventory_ItemSlotChanged(ItemEventArgs e)
		{
			// Item added to inventory but item not in pocket...
			if (!m_pocket.hasPocketed)
			{
				if (e.delta > 0)
				{
					// Reset slot index and look for slot
					m_slotIndex = 0;
					InstantiateNext();
				}
			}
			// Item removed from inventory and is now empty...
			else if (e.delta < 0 && m_inventory.GetItemTotal() == 0)
			{
				m_pocket.socketActive = false;

				// Remember pocketed interactable before destroying
				var obj = m_pocket.interactablePocketed.transform.gameObject;
				m_pocket.ForceSelectExit();
				PoolItem.Destroy(obj);

				m_pocket.socketActive = true;

				// Reset slot index; wait for next interactable
				m_slotIndex = -1;
			}
		}

		private void Pocket_HoverEntered(HoverEnterEventArgs e)
		{
			if (!m_pocket.hasPocketed)
				return;

			if (e.interactableObject is not UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable selectInteractable)
				return;

			selectInteractable.selectExited.AddListener(Item_Dropped);
			m_registeredInteractable = selectInteractable;
		}

		private void Pocket_HoverExited(HoverExitEventArgs e)
		{
			if (!m_pocket.hasPocketed)
				return;

			if (e.interactableObject is not UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable selectInteractable || selectInteractable != m_registeredInteractable)
				return;

			UnregisterDropInteractable();
		}

		private void UnregisterDropInteractable()
		{
			if (m_registeredInteractable == null)
				return;

			m_registeredInteractable.selectExited.RemoveListener(Item_Dropped);
			m_registeredInteractable = null;
		}

		private void Item_Dropped(SelectExitEventArgs e)
		{
			if (!TryGetItem(e.interactableObject, out var item))
				return;

			UnregisterDropInteractable();

			if (e.interactorObject == (UnityEngine.XR.Interaction.Toolkit.Interactors.IXRHoverInteractor)m_pocket)
				return;

			if (m_inventory.AddItem(item, out int overflow))
			{
				PoolItem.Destroy(item.gameObject);
			}
		}

		private void Pocketed(SelectEnterEventArgs e)
		{
			if (m_ignoreAdding)
				return;

			if (!TryGetItem(e.interactableObject, out var item))
				return;

			m_inventory.AddItem(item, out int overflow);
		
			if (slotIndex < 0)
			{
				slotIndex = m_inventory.IndexOf(item.itemType, true);
			}
        }

		private void Unpocketed(SelectExitEventArgs e)
		{
			if (!TryGetItem(e.interactableObject, out var item))
				return;

			m_inventory.TryRemoveItem(item);
			InstantiateNext();
		}

		private bool TryGetItem(UnityEngine.XR.Interaction.Toolkit.Interactables.IXRInteractable interactable, out Item item)
		{
			item = interactable.transform.GetComponent<Item>();
			return item != null;
		}

		#endregion
	}
}