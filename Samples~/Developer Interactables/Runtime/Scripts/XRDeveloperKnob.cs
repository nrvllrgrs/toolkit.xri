using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using ToolkitEngine.XR;
using TMPro;

public class XRDeveloperKnob : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private XRKnobInteractable m_knob;

    [SerializeField, TextArea]
    private string[] m_labels;

    [SerializeField]
    private TextMeshProUGUI m_textMesh;

    private string m_selected = null;

    #endregion

    #region Properties

    public XRKnobInteractable knob => m_knob;

    public string selected
    {
        get => m_selected;
        private set
        {
            // No change, skip
            if (m_selected == value)
                return;

            m_selected = value;
        }
    }

    public int index => m_knob.index;

    #endregion

    #region Events

    [SerializeField]
    private UnityEvent m_onIndexChanged;

	#endregion

	#region Methods

	private void Awake()
    {
        m_knob.stepCount = m_labels.Length;
    }

    private void OnEnable()
    {
        m_knob.onIndexChanged.AddListener(Knob_IndexChanged);
        Knob_IndexChanged(null);
    }

    private void OnDisable()
    {
        m_knob?.onIndexChanged.RemoveListener(Knob_IndexChanged);
    }

    private void Knob_IndexChanged(InteractionEventArgs e)
    {
        m_textMesh.text = selected = m_knob.index.Between(0, m_labels.Length - 1)
            ? m_labels[m_knob.index]
            : string.Empty;

        m_onIndexChanged?.Invoke();
	}

    #endregion
}
