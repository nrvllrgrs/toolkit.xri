using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

namespace ToolkitEngine.XR
{
	[RequireComponent(typeof(XRScaleTransformer))]
    public class XRPocketInteractor : XRConditionalSocketInteractor
    {
		#region Fields

		[SerializeField]
		private float m_size;

		[SerializeField]
		private bool m_autoCenter = true;

		[SerializeField]
		private bool m_usePocketLayer;

		[SerializeField, Layer]
		private int m_pocketLayer;

		/// <summary>
		/// Layer interactable was on prior to being socketed
		/// </summary>
		private Dictionary<GameObject, int> m_storedLayers = new();

		/// <summary>
		/// Transformer to control scale of socketed interactable
		/// </summary>
		private XRScaleTransformer m_scaleTransformer;

		private UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable m_interactablePocketed;

		#endregion

		#region Events

		[SerializeField]
		private UnityEvent<SelectEnterEventArgs> m_onPocketed;

		[SerializeField]
		private UnityEvent<SelectExitEventArgs> m_onUnpocketed;

		#endregion

		#region Properties

		public override UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable.MovementType? selectedInteractableMovementTypeOverride => UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable.MovementType.Instantaneous;
		public override bool isSelectActive => m_canSelect && base.isSelectActive;

		public bool hasPocketed => m_interactablePocketed != null;
		public UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable interactablePocketed => m_interactablePocketed;
		public UnityEvent<SelectEnterEventArgs> onPocketed => m_onPocketed;
		public UnityEvent<SelectExitEventArgs> onUnpocketed => m_onUnpocketed;

		#endregion

		#region Methods

		protected override void Awake()
		{
			base.Awake();
			m_scaleTransformer = GetComponent<XRScaleTransformer>();
		}

		protected override void OnEnable()
		{
			// Register to interaction manager before trying to re-socket interactable
			base.OnEnable();

			// Show selected object, if exists
			if (m_interactablePocketed != null)
			{
				m_interactablePocketed.transform.gameObject.SetActive(true);

				// Force re-socketing
				m_canSelect = true;
				interactionManager.SelectEnter(this, m_interactablePocketed);

				m_scaleTransformer.Stop(1f);
			}
		}

		protected override void OnDisable()
		{
			// Hide selected object, if exists
			if (m_interactablePocketed != null)
			{
				m_interactablePocketed.transform.gameObject.SetActive(false);
			}

			// Unregister from interactable manager after caching interactable
			base.OnDisable();
		}

		protected override void OnHoverEntering(HoverEnterEventArgs e)
		{
			base.OnHoverEntering(e);

			if (!m_canSelect)
				return;

			// Something already in socket, skip
			if (hasSelection)
				return;

			UpdateInteractableHoverScale(e.interactableObject);
		}

		protected override void OnSelectEntered(SelectEnterEventArgs e)
		{
			base.OnSelectEntered(e);

			if (!e.interactableObject.transform.TryGetComponent(out UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable))
				return;

			// Setup transformer before adding to ensure correct local scale
			m_scaleTransformer.Play(grabInteractable, grabInteractable.transform.localScale * interactableHoverScale);
			grabInteractable.AddSingleGrabTransformer(m_scaleTransformer);

			// Move socketed interactable to another layer to prevent unwanted collisions
			if (m_usePocketLayer)
			{
				foreach (var collider in grabInteractable.colliders)
				{
					if (!m_storedLayers.ContainsKey(collider.gameObject))
					{
						m_storedLayers.Add(collider.gameObject, collider.gameObject.layer);
						collider.gameObject.layer = m_pocketLayer;
					}
				}
			}

			// Stop all other hover interactors
			if (e.interactableObject is UnityEngine.XR.Interaction.Toolkit.Interactables.IXRHoverInteractable hoverInteractable)
			{
				var interactorsHovering = hoverInteractable.interactorsHovering.ToArray();
				foreach (var hoverInteractor in interactorsHovering)
				{
					if (hoverInteractor.Equals(this))
						continue;

					interactionManager.HoverCancel(hoverInteractor, hoverInteractable);
				}
			}

			// Restore layer prior to socketing
			if (m_usePocketLayer)
			{
				foreach (var p in m_storedLayers)
				{
					p.Key.layer = p.Value;
				}
				m_storedLayers.Clear();
			}

			m_interactablePocketed = e.interactableObject;
			m_onPocketed?.Invoke(e);
		}

		protected override void OnSelectExited(SelectExitEventArgs e)
		{
			if (e.interactableObject.transform.TryGetComponent(out UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable))
			{
				grabInteractable.RemoveSingleGrabTransformer(m_scaleTransformer);
				m_scaleTransformer.Stop();

				if (isActiveAndEnabled)
				{
					m_interactablePocketed = null;
					m_onUnpocketed?.Invoke(e);
				}
			}

			base.OnSelectExited(e);
		}

		protected override bool ShouldDrawHoverMesh(MeshFilter meshFilter, Renderer meshRenderer, Camera mainCamera)
		{
			if (!m_canSelect)
				return false;

			return base.ShouldDrawHoverMesh(meshFilter, meshRenderer, mainCamera);
		}

		public void ForceSelectEnter(UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable selectInteractable, bool animateScale = false)
		{
			if (selectInteractable is not UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable)
				return;

			if (!IsHovering(grabInteractable))
			{
				// Force hover enter to recalculate hover scale
				UpdateInteractableHoverScale(grabInteractable);
			}

			m_canSelect = true;
			interactionManager.SelectEnter(this, selectInteractable);

			if (!animateScale)
			{
				m_scaleTransformer.Complete(grabInteractable, grabInteractable.transform.localScale * interactableHoverScale);
			}
		}

		private void UpdateInteractableHoverScale(UnityEngine.XR.Interaction.Toolkit.Interactables.IXRInteractable interactableObject)
		{
			// Get bounds of interactable
			if (!interactableObject.transform.gameObject.TryGetRendererBounds(out Bounds bounds))
				return;

			// Calculate size so hover scale is correct
			var size = m_size * transform.lossyScale;
			size = size.InverseScale(bounds.size);

			// Largest component is scale
			interactableHoverScale = size.MinComponent();

			if (m_autoCenter)
			{
				attachTransform.localPosition = -bounds.center * interactableHoverScale;
			}
		}

		#endregion

		#region Editor-Only
#if UNITY_EDITOR

		private void OnDrawGizmosSelected()
		{
			Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
			Gizmos.DrawWireCube(Vector3.zero, Vector3.one * m_size);
		}

#endif
		#endregion
	}
}