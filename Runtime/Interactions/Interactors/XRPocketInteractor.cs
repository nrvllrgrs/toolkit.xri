using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

namespace UnityEngine.XR.Interaction.Toolkit
{
	[RequireComponent(typeof(XRScaleTransformer))]
    public class XRPocketInteractor : XRSocketInteractor
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

		/// <summary>
		/// Indicates whether interactable can be grabbed by socket
		/// </summary>
		private bool m_canSelect;

		private IXRSelectInteractable m_interactablePocketed;

		#endregion

		#region Properties

		public override XRBaseInteractable.MovementType? selectedInteractableMovementTypeOverride => XRBaseInteractable.MovementType.Instantaneous;
		public override bool isSelectActive => m_canSelect && base.isSelectActive;

		public bool hasPocketed => m_interactablePocketed != null;
		public IXRSelectInteractable interactablePocketed => m_interactablePocketed;

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

			// Something already in socket, skip
			if (hasSelection)
				return;

			var selectInteractable = e.interactableObject as IXRSelectInteractable;

			// Can only be socketed if selected by another interactor
			if (selectInteractable == null || !selectInteractable.isSelected)
			{
				m_canSelect = false;
				return;
			}

			m_canSelect = true;

			// Get bounds of interactable
			var bounds = e.interactableObject.transform.gameObject.GetLocalRendererBounds();

			// Calculate size so hover scale is correct
			var size = m_size * transform.lossyScale;
			size.Scale(bounds.size);

			// Largest component is scale
			interactableHoverScale = size.MaxComponent();

			if (m_autoCenter)
			{
				attachTransform.localPosition = -bounds.center * interactableHoverScale;
			}
		}

		protected override void OnHoverExiting(HoverExitEventArgs e)
		{
			base.OnHoverExiting(e);

			var selectInteractable = e.interactableObject as IXRSelectInteractable;
			if (selectInteractable == null)
				return;

			if (!hasSelection)
			{
				m_canSelect = false;
			}
		}

		protected override void OnSelectEntered(SelectEnterEventArgs e)
		{
			base.OnSelectEntered(e);

			if (!e.interactableObject.transform.TryGetComponent(out XRGrabInteractable grabInteractable))
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
			if (e.interactableObject is IXRHoverInteractable hoverInteractable)
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
		}

		protected override void OnSelectExited(SelectExitEventArgs e)
		{
			if (e.interactableObject.transform.TryGetComponent(out XRGrabInteractable grabInteractable))
			{
				grabInteractable.RemoveSingleGrabTransformer(m_scaleTransformer);
				m_scaleTransformer.Stop();

				if (isActiveAndEnabled)
				{
					m_interactablePocketed = null;
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