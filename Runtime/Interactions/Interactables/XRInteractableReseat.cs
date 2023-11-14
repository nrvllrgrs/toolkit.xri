using DG.Tweening;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace ToolkitEngine.XR
{
	[RequireComponent(typeof(XRBaseInteractable))]
	public class XRInteractableReseat : MonoBehaviour
	{
		#region Fields

		[SerializeField, Min(0f)]
		private float m_easeTime = 1f;

		private XRBaseInteractable m_interactable;
		private Sequence m_sequence;
		private Vector3 m_position, m_eulerAngles;

		#endregion

		#region Methods

		private void Awake()
		{
			m_interactable = GetComponent<XRBaseInteractable>();
			m_position = transform.position;
			m_eulerAngles = transform.eulerAngles;
		}

		private void OnEnable()
		{
			m_interactable.selectEntered.AddListener(Grabbed);
			m_interactable.selectExited.AddListener(Dropped);
		}

		private void OnDisable()
		{
			m_interactable.selectEntered.RemoveListener(Grabbed);
			m_interactable.selectExited.RemoveListener(Dropped);
		}

		private void Grabbed(SelectEnterEventArgs e)
		{
			m_sequence?.Kill();
		}

		private void Dropped(SelectExitEventArgs e)
		{
			m_sequence?.Kill();

			m_sequence = DOTween.Sequence();
			m_sequence.Append(transform.DOMove(m_position, m_easeTime));
			m_sequence.Join(transform.DORotate(m_eulerAngles, m_easeTime));
		}

		#endregion
	}
}