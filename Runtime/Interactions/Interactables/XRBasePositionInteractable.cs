using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ToolkitEngine.XR
{
	public enum Axis
	{
		Backward,
		Down,
		Forward,
		Left,
		Right,
		Up
	}

    public abstract class XRBasePositionInteractable : XRBaseInteractable
    {
        #region Fields

        [SerializeField]
        protected bool m_isInteractable = true;

        [SerializeField]
        protected Axis m_directionAxis;

        [SerializeField, Min(0f)]
        protected float m_maxDepth;

        [SerializeField, Tooltip("Normalized depths for Tipflow and Tailflow events to trigger (0 = tail; 1 = tip).")]
        protected Vector2 m_normalizedFlowDepths = Vector2.one * 0.5f;

        [SerializeField, Tooltip("Indicates whether object snaps to tail position when interaction stops.")]
        private bool m_tailflowSnap = true;

        [SerializeField, Min(0f), Tooltip("Maximum distance object can move per second towards tail position while not interacting.")]
        private float m_tailflowSpeed;

        [SerializeField]
        private bool m_useTipflowHaptics;

        [SerializeField]
        private HapticSettings m_tipflowHaptics;

        [SerializeField]
        private bool m_useTailflowHaptics;

        [SerializeField]
        private HapticSettings m_tailflowHaptics;

        /// <summary>
        /// Tail position of interactable
        /// </summary>
        private Vector3 m_tailLocalPosition;

        /// <summary>
        /// Position of interactable when interaction starts
        /// </summary>
        private Vector3 m_startLocalPosition;

        /// <summary>
        /// Direction of motion in local space
        /// </summary>
        private Vector3 m_direction;

        private float m_tailflowDepth, m_tipflowDepth;

        private IXRInteractor m_interactor = null;
        private float m_startMagnitude = 0f;

        /// <summary>
        /// Depth of object when interaction starts
        /// </summary>
        private float m_startDepth;

        private bool m_isTipflow;

        private float m_depth = 0f;

        #endregion

        #region Events

        [SerializeField]
        private UnityEvent<InteractionEventArgs> m_onDepthChanged;

		[SerializeField]
		private UnityEvent<InteractionEventArgs> m_onTipflow;

		[SerializeField]
		private UnityEvent<InteractionEventArgs> m_onTailflow;

        #endregion

        #region Properties

        public bool isInteractable
        {
            get => m_isInteractable;
            set
            {
                // No change, skip
                if (m_isInteractable == value)
                    return;

                if (m_isInteractable)
                {
                    EndInteraction(null);
                }
                m_isInteractable = value;
            }
        }

        /// <summary>
        /// Direction of motion in world space
        /// </summary>
        protected Vector3 direction => transform.rotation * m_direction;

        public bool isTipflow
        {
            get => m_isTipflow;
            private set
            {
                // No change, skip
                if (m_isTipflow == value)
                    return;

                var args = new InteractionEventArgs()
                {
                    interactableObject = this,
                    interactorObject = m_interactor
                };

                m_isTipflow = value;
                if (value)
                {
                    m_onTipflow?.Invoke(args);

                    if (m_useTipflowHaptics)
                    {
                        foreach (var interactor in controllerInteractors)
                        {
                            m_tipflowHaptics.SendImpulse(interactor);
                        }
                    }
                }
                else
                {
                    m_onTailflow?.Invoke(args);

                    if (m_useTailflowHaptics)
                    {
                        foreach (var interactor in controllerInteractors)
                        {
                            m_tailflowHaptics.SendImpulse(interactor);
                        }
                    }
                }
            }
        }

        public float depth => m_depth;
        public float normalizedDepth => Mathf.Clamp01(m_depth / m_maxDepth);

        public UnityEvent<InteractionEventArgs> onDepthChanged => m_onDepthChanged;
        public UnityEvent<InteractionEventArgs> onTipflow => m_onTipflow;
        public UnityEvent<InteractionEventArgs> onTailflow => m_onTailflow;

        protected abstract IEnumerable<XRBaseControllerInteractor> controllerInteractors { get; }

        #endregion

        #region Methods

        protected override void Awake()
        {
            base.Awake();

            // Store initial position so it can be restored when released
            m_tailLocalPosition = transform.localPosition;
            m_tailflowDepth = m_maxDepth * m_normalizedFlowDepths.x;
            m_tipflowDepth = m_maxDepth * m_normalizedFlowDepths.y;
            SetDirection();
        }

        protected virtual void BeginInteraction(BaseInteractionEventArgs args)
        {
            if (!m_isInteractable)
                return;

            // Use start position instead of tail position
            // Interactable may be starting along the axis (and NOT necessarily at its tail)
            m_startLocalPosition = transform.localPosition;
            m_startDepth = m_depth;

            // Handle pushing button
            m_interactor = args.interactorObject;
            if (TryGetMagnitudeAlongPath(m_interactor.transform.position, out float magnitude))
            {
                m_startMagnitude = magnitude;
            }
        }

        protected void EndInteraction(BaseInteractionEventArgs args)
        {
            if (m_interactor == null)
                return;

            // Handle releasing button
            m_interactor = null;

            if (m_tailflowSnap)
            {
                // Reset button position
                transform.localPosition = m_tailLocalPosition;

                // Reset button state
                isTipflow = false;
                m_depth = m_startMagnitude = 0f;
                InvokeDepthChanged();
            }
        }

        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            if (m_interactor == null && (m_tailflowSpeed == 0f || m_depth == 0f))
                return;

            // Interacting with object
            if (m_interactor != null)
            {
                if (!TryGetMagnitudeAlongPath(m_interactor.transform.position, out float magnitude, m_startMagnitude))
                    return;

                // Negligible change, skip
                if (Mathf.Approximately(magnitude, 0f))
                    return;

                // Clamp depth
                m_depth = Mathf.Clamp(m_startDepth + magnitude, 0f, m_maxDepth);
            }
            // Not interacting with object, but moving towards tail position
            else if (m_tailflowSpeed > 0f)
            {
                switch (updatePhase)
                {
                    case XRInteractionUpdateOrder.UpdatePhase.Fixed:
                        m_depth -= m_tailflowSpeed * Time.fixedDeltaTime;
                        break;

                    default:
                        m_depth -= m_tailflowSpeed * Time.deltaTime;
                        break;
                }

                // Clamp depth
                m_depth = Mathf.Clamp(m_depth, 0f, m_maxDepth);
            }

            InvokeDepthChanged();

            // Set position from depth
            Vector3 position;
            if (transform.parent == null)
            {
                position = m_tailLocalPosition + direction * m_depth;
            }
            else
            {
                position = transform.parent.position + (transform.parent.rotation * m_tailLocalPosition) + direction * m_depth;
            }
            transform.position = position;

            // Have move to tip...
            if (m_isTipflow)
            {
                // ...and arriving near tail
                if (m_depth.Between(0f, m_tailflowDepth))
                {
                    isTipflow = false;
                }
            }
            // ...(Have moved to tail and) arriving near tip
            else if (m_depth.Between(m_tipflowDepth, m_maxDepth))
            {
                isTipflow = true;
            }       
        }

        protected void InvokeDepthChanged()
        {
            m_onDepthChanged?.Invoke(new InteractionEventArgs()
            {
                interactableObject = this,
                interactorObject = m_interactor
            });
        }

        private void SetDirection()
        {
            switch (m_directionAxis)
            {
                case Axis.Backward:
                    m_direction = Vector3.back;
                    break;

                case Axis.Down:
                    m_direction = Vector3.down;
                    break;

                case Axis.Forward:
                    m_direction = Vector3.forward;
                    break;

                case Axis.Left:
                    m_direction = Vector3.left;
                    break;

                case Axis.Right:
                    m_direction = Vector3.right;
                    break;

                case Axis.Up:
                    m_direction = Vector3.up;
                    break;
            }
        }

        private bool TryGetMagnitudeAlongPath(Vector3 position, out float magnitude, float startPointOffset = 0f)
        {
            // Convert into world space
            var startPoint = transform.parent == null
                ? m_startLocalPosition
                : transform.parent.TransformPoint(m_startLocalPosition);

            if (!Mathf.Approximately(startPointOffset, 0f))
            {
                startPoint += this.direction * startPointOffset;
            }

            // Project position onto world motion path
            var point = Vector3.Project(position - startPoint, this.direction) + startPoint;

            var direction = point - startPoint;
            magnitude = direction.magnitude;

            if (Vector3.Dot(direction, this.direction) < 0)
            {
                magnitude = -magnitude;
            }
            return true;
        }

        #endregion

        #region Editor-Only
#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Vector3 startPoint;
            if (!Application.isPlaying)
            {
                startPoint = transform.position;
                SetDirection();
            }
            else if (transform.parent == null)
            {
                startPoint = m_tailLocalPosition;
            }
            else
            {
                startPoint = transform.parent.TransformPoint(m_tailLocalPosition);
            }

            var scale = HandleUtility.GetHandleSize(startPoint);
            var endPoint = startPoint + direction * m_maxDepth;
            var forward = Quaternion.LookRotation(direction);

            Gizmos.DrawLine(startPoint, endPoint);
            Handles.ArrowHandleCap(0, startPoint, forward, scale, EventType.Repaint);

            Handles.color = Color.green;
            Handles.DrawWireDisc(startPoint, endPoint - startPoint, 0.5f * scale);

            Handles.color = Color.red;
            Handles.DrawWireDisc(endPoint, endPoint - startPoint, 0.5f * scale);

            if (m_interactor != null)
            {
                // Convert into world space
                startPoint = transform.parent == null
                    ? m_startLocalPosition
                    : transform.parent.TransformPoint(m_startLocalPosition);

                Gizmos.color = Color.magenta;
                Gizmos.DrawWireSphere(startPoint, 0.02f);
                Gizmos.DrawWireSphere(m_interactor.transform.position, 0.02f);
                Gizmos.DrawRay(startPoint, direction * m_startMagnitude);
            }
        }

#endif
        #endregion
    }
}
