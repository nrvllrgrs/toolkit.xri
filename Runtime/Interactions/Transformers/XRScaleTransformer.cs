using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

namespace UnityEngine.XR.Interaction.Toolkit.Transformers
{
	[AddComponentMenu("XR/Transformers/XR Scale Transformer")]
	public class XRScaleTransformer : XRBaseGrabTransformer
	{
		#region Fields

		[SerializeField, Min(0f), Tooltip("Seconds to change scale of interactable when grabbed.")]
		private float m_duration = 0.25f;

		private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable m_grabInteractable;
		private float m_value;
		private Vector3 m_srcLocalScale = Vector3.one, m_dstLocalScale = Vector3.one;

		private TweenerCore<float, float, FloatOptions> m_tweener = null;

		#endregion

		#region Methods

		private void OnDisable()
		{
			m_value = 1f;
		}

		public void Play(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable, Vector3 targetScale)
		{
			Play(grabInteractable, targetScale, m_duration);
		}

		public void Play(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable, Vector3 targetScale, float duration)
		{
			m_grabInteractable = grabInteractable;
			if (m_grabInteractable == null)
				return;

			Stop();

			m_srcLocalScale = grabInteractable.transform.localScale;
			m_dstLocalScale = targetScale;

			m_value = 0f;
			m_tweener = DOTween.To(() => m_value, x => m_value = x, 1f, duration);
		}

		public void Complete(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable, Vector3 targetScale)
		{
			Play(grabInteractable, targetScale, 0f);
		}

		public void Stop(float value = 0f)
		{
			if (m_tweener != null && m_tweener.IsPlaying())
			{
				m_tweener.Kill();
				m_value = value;
			}

			m_grabInteractable.transform.localScale = m_srcLocalScale;
		}

		public override void Process(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable, XRInteractionUpdateOrder.UpdatePhase updatePhase, ref Pose targetPose, ref Vector3 localScale)
		{
			localScale = Vector3.Lerp(m_srcLocalScale, m_dstLocalScale, m_value);
		}

		#endregion
	}
}