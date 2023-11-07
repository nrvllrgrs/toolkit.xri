using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ToolkitEngine.XR
{
    public class RotationInteractionEvent : InteractionEventArgs
    {
        public int index { get; set; }
        public float value { get; set; }
        public float delta { get; set; }
    }

    public abstract class XRBaseRotationInteractable : XRBaseInteractable
    {
        #region Enumerators

        [Flags]
        public enum RotationMode
        {
            Clockwise = 1 << 1,
            Counterclockwise = 1 << 2,
        }

        public enum SnapMode
        {
            None,
            Angle,
            Step,
        }

        #endregion

        #region Fields

        [SerializeField]
        protected bool m_isInteractable = true;

        [SerializeField]
        protected Axis m_forward = Axis.Forward;

        [SerializeField]
        protected Axis m_upward = Axis.Up;

        [SerializeField]
        protected Vector2 m_range = new Vector2(-180f, 180f);

        [SerializeField]
        protected RotationMode m_rotationDirection = RotationMode.Clockwise | RotationMode.Counterclockwise;

        [SerializeField]
        protected float m_startingAngle;

        [SerializeField]
        protected bool m_useSteps = true;

        [SerializeField, Min(2)]
        protected int m_stepCount = 2;

        [SerializeField, Tooltip("Indicates whether step index is continuously updated during interaction; otherwise index is only updated when interaction ends.")]
        protected bool m_updateIndexContinuously = true;

        [SerializeField]
        private SnapMode m_snapMode = SnapMode.None;

        [SerializeField, Tooltip("Indicates whether object snaps to final angle when interaction stops.")]
        private bool m_snap = true;

        [SerializeField, Min(0f), Tooltip("Maximum degrees object can rotate per second towards final angle while not interacting.")]
        private float m_snapSpeed;

        [SerializeField]
        private float m_restingAngle;

        [SerializeField]
        private bool m_useValueChangedHaptics;

        [SerializeField]
        private HapticSettings m_valueChangedHaptics;

        [SerializeField]
        private bool m_useIndexChangedHaptics;

        [SerializeField]
        private HapticSettings m_indexChangedHaptics;

        /// <summary>
        /// Default rotation of interactable
        /// </summary>
        private Quaternion m_defaultLocalRotation;

        /// <summary>
        /// Interactor currently manipulating interactable
        /// </summary>
        protected IXRInteractor m_interactor = null;

        /// <summary>
        /// Rotation of interactor relative to Upward when interaction starts
        /// </summary>
        private float m_interactorStartAngle;

        /// <summary>
        /// Rotation of interactable relative to Upward when interaction starts
        /// </summary>
        private float m_startAngle;

        private float m_angle = float.NaN, m_delta, m_theta;
        private int m_closestStepIndex, m_index, m_pendingIndex;

#if UNITY_EDITOR
        private const float SCREEN_SPACE_SIZE = 2f;
#endif
        #endregion

        #region Events

        [SerializeField]
        private UnityEvent<RotationInteractionEvent> m_onValueChanged;

		[SerializeField]
		private UnityEvent<RotationInteractionEvent> m_onIndexChanged;

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

        protected Vector3 forward => GetDirection(m_forward);
        protected Vector3 upward => GetDirection(m_upward);

        /// <summary>
        /// Indicates whether object rotatation wraps at its limits.
        /// </summary>
        protected bool wrapping => minAngle == -180f && maxAngle == 180f;

        /// <summary>
        /// Minimum angle of interactable
        /// </summary>
        public float minAngle => m_range.x;

        /// <summary>
        /// Maximum angle of interactable
        /// </summary>
        public float maxAngle => m_range.y;

        /// <summary>
        /// Angle of interactable
        /// </summary>
        public float value
        {
            get => m_angle;
            private set
            {
                // No change, skip
                if (m_angle == value)
                    return;

                m_angle = value;
                m_onValueChanged?.Invoke(GetInteractionEventArgs());

                if (m_useValueChangedHaptics)
                {
                    m_valueChangedHaptics.SendImpulse(firstInteractorSelecting as XRBaseControllerInteractor);
                }

                if (m_useSteps)
                {
                    m_closestStepIndex = Mathf.FloorToInt((m_angle - minAngle + (m_theta * 0.5f)) / m_theta);
                    if (!wrapping)
                    {
                        m_pendingIndex = m_closestStepIndex = Mathf.Clamp(m_closestStepIndex, 0, m_stepCount - 1);
                    }
                    else
                    {
                        m_pendingIndex = m_closestStepIndex.Mod(m_stepCount);
                    }

                    if (m_updateIndexContinuously)
                    {
                        index = m_pendingIndex;
                    }
                }
            }
        }

        public float normalizedValue => MathUtil.GetPercent(m_angle, minAngle, maxAngle);

        public float finalAngle
        {
            get
            {
                switch (m_snapMode)
                {
                    case SnapMode.None:
                        return m_angle;

                    case SnapMode.Angle:
                        return m_restingAngle;

                    case SnapMode.Step:
                        if (m_useSteps)
                        {
                            // Need to use an overflow index for wrapping
                            return minAngle + (m_theta * m_closestStepIndex);
                        }
                        break;
                }
                return m_angle;
            }
        }

        public int stepCount
        {
            get => m_stepCount;
            set
            {
                if (!m_useSteps)
                {
					Debug.LogWarningFormat("{0} is set to not use steps! Cannot set step count!", name);
					return;
                }

                value = Mathf.Max(1, value);
				m_stepCount = value;
				m_theta = (maxAngle - minAngle) / (wrapping ? m_stepCount : m_stepCount - 1);
			}
        }

        /// <summary>
        /// Index of selected step
        /// </summary>
        public int index
        {
            get => m_index;
            private set
            {
                // No change, skip
                if (m_index == value)
                    return;

                m_index = value;
                m_onIndexChanged?.Invoke(GetInteractionEventArgs());

                if (m_useIndexChangedHaptics)
                {
                    m_indexChangedHaptics.SendImpulse(firstInteractorSelecting as XRBaseControllerInteractor);
                }
            }
        }

        public UnityEvent<RotationInteractionEvent> onValueChanged => m_onValueChanged;
        public UnityEvent<RotationInteractionEvent> onIndexChanged => m_onIndexChanged;

        #endregion

        #region Methods

        protected override void Awake()
        {
            base.Awake();

            // Store initial rotation for math
            m_defaultLocalRotation = transform.localRotation;
            m_theta = (maxAngle - minAngle) / (wrapping ? m_stepCount : m_stepCount - 1);

            // Set starting angle of interactable
            // Will automatically update index, if applicable
            value = m_startingAngle;

            // Set interactable rotation
            SetRotation(finalAngle);
        }

        protected virtual void BeginInteraction(BaseInteractionEventArgs args)
        {
            if (!m_isInteractable)
                return;

            m_startAngle = m_angle;

            // Remember interator and its initial angle relative to Upwards property
            // This angle is used as an offset during rotation
            m_interactor = args.interactorObject;
            if (TryGetAngleOnPlane(out float angle))
            {
                m_interactorStartAngle = angle;
            }
        }

        protected void EndInteraction(BaseInteractionEventArgs args)
        {
            if (m_interactor == null)
                return;

            // Handle releasing button
            m_interactor = null;

            if (!m_updateIndexContinuously)
            {
                index = m_pendingIndex;
            }

            if (m_snapMode != SnapMode.None && m_snap)
            {
                // Reset interactable rotation
                SetRotation(finalAngle);
                value = finalAngle;
            }
        }

        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            if (m_interactor == null && (m_snapMode == SnapMode.None || m_snap || m_snapSpeed == 0f || Mathf.Approximately(m_angle, finalAngle)))
                return;

            // Interacting with object
            if (m_interactor != null)
            {
                if (!TryGetAngleOnPlane(out float deltaAngle, m_interactorStartAngle))
                    return;

                // Negligible change, skip
                if (Mathf.Approximately(deltaAngle, 0f))
                    return;

                var angle = Mathf.Clamp((m_startAngle + deltaAngle).WrapEulerAngle(), minAngle, maxAngle);

                // Cannot rotate interactable in direction, skip
                m_delta = (angle - m_angle).WrapEulerAngle();
                if ((m_delta > 0f && (m_rotationDirection & RotationMode.Clockwise) == 0)
                    || (m_delta < 0f && (m_rotationDirection & RotationMode.Counterclockwise) == 0))
                    return;

                // Clamp angle
                value = angle;
                SetRotation(m_angle);
            }
            // Not interacting with object, but rotate towards closest step
            else if (m_snapSpeed > 0f)
            {
                var closetStepAngle = finalAngle;
                var angle = m_angle;

                if (m_angle < closetStepAngle)
                {
                    switch (updatePhase)
                    {
                        case XRInteractionUpdateOrder.UpdatePhase.Fixed:
                            angle += m_snapSpeed * Time.fixedDeltaTime;
                            break;

                        default:
                            angle += m_snapSpeed * Time.deltaTime;
                            break;
                    }
                    angle = Mathf.Min(angle, closetStepAngle);
                }
                else
                {
                    switch (updatePhase)
                    {
                        case XRInteractionUpdateOrder.UpdatePhase.Fixed:
                            angle -= m_snapSpeed * Time.fixedDeltaTime;
                            break;

                        default:
                            angle -= m_snapSpeed * Time.deltaTime;
                            break;
                    }

                    angle = Mathf.Max(angle, closetStepAngle);
                }

                value = angle;
                SetRotation(angle);
            }
        }

        private bool TryGetAngleOnPlane(out float angle, float startAngleOffset = 0f)
        {
            if (m_interactor == null)
            {
                angle = 0f;
                return false;
            }

            // Project vector onto rotation plane
            var v = Vector3.ProjectOnPlane(GetInteractorRotation(), forward);

            // Calculate angle between default and interactor up vectors
            angle = Vector3.SignedAngle(upward, v, forward) - startAngleOffset;
            return true;
        }

        protected abstract Vector3 GetInteractorRotation();

        private void SetRotation(float angle)
        {
            transform.rotation = (transform.parent == null
                ? m_defaultLocalRotation
                : transform.parent.rotation * m_defaultLocalRotation) * Quaternion.AngleAxis(angle, GetAxisDirection(m_forward));
        }

        private Vector3 GetDirection(Axis axis)
        {
            if (!Application.isPlaying)
                return transform.rotation * GetAxisDirection(axis);

            return (transform.parent == null
                ? m_defaultLocalRotation
                : transform.parent.rotation * m_defaultLocalRotation) * GetAxisDirection(axis);
        }

        protected Vector3 GetAxisDirection(Axis axis)
        {
            switch (axis)
            {
                case Axis.Backward:
                    return Vector3.back;

                case Axis.Down:
                    return Vector3.down;

                case Axis.Forward:
                    return Vector3.forward;

                case Axis.Left:
                    return Vector3.left;

                case Axis.Right:
                    return Vector3.right;

                case Axis.Up:
                    return Vector3.up;
            }
            return Vector3.zero;
        }

        protected RotationInteractionEvent GetInteractionEventArgs()
        {
            return new RotationInteractionEvent()
            {
                interactableObject = this,
                interactorObject = m_interactor,
                value = m_angle,
                delta = m_delta,
                index = m_index
            };
        }

        #endregion

        #region Editor-Only
#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            var point = transform.position;
            float range = maxAngle - minAngle;

            if (m_useSteps)
            {
                float theta = range / m_stepCount;
                int count = m_stepCount * 2;

                if (!wrapping)
                {
                    theta = range / (m_stepCount - 1);
                    --count;
                }

                // Cut theta in half to draw step boundaries
                theta *= 0.5f;

                for (int i = 0; i < count; ++i)
                {
                    if (i % 2 == 0)
                    {
                        if (m_snap || m_snapSpeed > 0f)
                        {
                            Handles.color = Color.white;
                            Handles.DrawLine(point, point + Quaternion.AngleAxis(minAngle + (theta * i), forward) * upward * 0.5f, SCREEN_SPACE_SIZE);
                        }
                        else if (!wrapping && (i == 0 || i == count - 1))
                        {
                            Handles.color = Color.green;
                            Handles.DrawLine(point, point + Quaternion.AngleAxis(minAngle + (theta * i), forward) * upward * 0.5f, SCREEN_SPACE_SIZE);
                        }
                    }
                    else
                    {
                        Handles.color = Color.green;
                        Handles.DrawDottedLine(point, point + Quaternion.AngleAxis(minAngle + (theta * i), forward) * upward * 0.5f, SCREEN_SPACE_SIZE * 2f);
                    }
                }
            }
            else if (!wrapping)
            {
                Handles.color = Color.green;
                Handles.DrawLine(point, point + Quaternion.AngleAxis(minAngle, forward) * upward * 0.5f, SCREEN_SPACE_SIZE);
                Handles.DrawLine(point, point + Quaternion.AngleAxis(maxAngle, forward) * upward * 0.5f, SCREEN_SPACE_SIZE);
            }

            Handles.color = Color.green;
            Handles.DrawWireArc(point, forward, Quaternion.AngleAxis(minAngle, forward) * upward * 0.5f, range, 0.5f, SCREEN_SPACE_SIZE);

            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, forward * 0.5f);

            if (m_interactor != null)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawRay(m_interactor.transform.position, m_interactor.transform.up * 0.5f);
            }
        }

#endif
        #endregion
    }
}