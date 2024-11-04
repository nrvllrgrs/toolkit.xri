using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using ToolkitEngine;
using ToolkitEngine.XR;
using NaughtyAttributes;

public class XRDeveloperButton : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private XRButtonInteractable m_interactable;

    [SerializeField]
    private StateInfo m_off;

    [SerializeField]
    private StateInfo m_on;

    [SerializeField]
    private StateInfo m_disabled;

    [SerializeField]
    private bool m_startOn;

    [SerializeField]
    private bool m_singleUse;

    [SerializeField, Min(0f)]
    private float m_duration;

    [Header("UI Settings")]

    [SerializeField]
    private CanvasGroup m_canvasGroup;

    [SerializeField]
    private TextMeshProUGUI m_textMesh;

    [SerializeField]
    private TimedCurve m_timedCurve;

    private bool m_isOn;
    private bool m_isDisabled;
    private Coroutine m_turnOffThread = null;

    #endregion

    #region Events

    [Foldout("Events")]
    public UnityEvent<bool> onValueChanged;

	[Foldout("Events")]
	public UnityEvent onDisabled;

    #endregion

    #region Properties

    /// <summary>
    /// Indicates whether button is on
    /// </summary>
    public bool isOn
    {
        get => m_isOn;
        set
        {
            // No change, skip
            if (m_isOn == value)
                return;

			// Button disabled, skip
			if (isDisabled)
				return;

			// On and turn off timer is running...
			if (isOn && m_turnOffThread != null)
			{
				ResetTurnOffTimer();
				return;
			}

			SetState(!isOn);
		}
    }

    /// <summary>
    /// Indicates whether button is disabled
    /// </summary>
    public bool isDisabled
    {
        get => m_isDisabled;
        set
        {
            // No change, skip
            if (m_isDisabled == value)
                return;

            m_isDisabled = value;
            if (value)
            {
                onDisabled?.Invoke();
            }

            UpdateState();
        }
    }

    #endregion

    #region Methods

    private void Start()
    {
        m_isOn = m_startOn;
        SetState(m_isOn, true, true);
    }

    private void OnEnable()
    {
        if (isDisabled)
            return;

        m_interactable.onTipflow.AddListener(Interactable_Pressed);
        m_timedCurve.OnTimeChanged.AddListener(TimedCurve_TimeChanged);
    }

    private void OnDisable()
    {
        m_interactable.onTipflow.RemoveListener(Interactable_Pressed);
        m_timedCurve.OnTimeChanged.RemoveListener(TimedCurve_TimeChanged);
    }

    private void Interactable_Pressed(InteractionEventArgs e)
    {
        isOn = !m_isOn;
    }

    private void SetState(bool value, bool force = false, bool silent = false)
    {
        // No change, skip
        if (m_isOn == value && !force)
            return;

        m_isOn = value;

        if (value)
        {
            // Start reset timer, if necessary
            if (m_duration > 0f)
            {
                ResetTurnOffTimer();
            }
        }

        if (!silent)
        {
            onValueChanged.Invoke(value);
        }

        // Disable if single-use button
        if (m_singleUse && !m_isDisabled)
        {
            isDisabled = true;
        }
        else
        {
			UpdateState();
		}
    }

    private void UpdateState()
    {
        StateInfo state;
        if (m_isDisabled)
        {
            state = m_disabled;
        }
        else if (m_isOn)
        {
            state = m_on;
        }
        else
        {
            state = m_off;
        }

        // Change button color
        var renderer = m_interactable.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = state.color;
        }

        // Show / hide label
        if (!string.IsNullOrWhiteSpace(state.label))
        {
            m_textMesh.text = state.label;
            m_timedCurve.PlayForward();
        }
        else
        {
            m_textMesh.text = string.Empty;
            m_timedCurve.PlayBackwards();
        }
    }

    private void ResetTurnOffTimer()
    {
        if (m_turnOffThread != null)
        {
            StopCoroutine(m_turnOffThread);
        }
        m_turnOffThread = StartCoroutine(AsyncTurnOff());
    }

    private IEnumerator AsyncTurnOff()
    {
        yield return new WaitForSeconds(m_duration);

        SetState(false);
        m_turnOffThread = null;
    }

    private void TimedCurve_TimeChanged(TimedCurve timedCurve)
    {
        m_canvasGroup.alpha = timedCurve.value;
    }

    #endregion

    #region Editor-Only
#if UNITY_EDITOR

    [ContextMenu("Toggle")]
    private void Toggle()
    {
        SetState(!m_isOn);
    }

#endif
    #endregion

    #region Structures

    [System.Serializable]
    public struct StateInfo
    {
        [TextArea]
        public string label;

        public Color color;
    }

    #endregion
}
