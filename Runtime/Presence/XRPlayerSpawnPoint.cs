using UnityEngine;
using ToolkitEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRPlayerSpawnPoint : MonoBehaviour
{
	#region Fields

	[SerializeField, Tag]
	private string m_tag = "Player";

	#endregion

	#region Methods

	private void Start()
	{
		var obj = GameObject.FindGameObjectWithTag(m_tag);
		if (obj == null)
			return;

		var provider = obj.GetComponentInChildren<TeleportationProvider>();
		if (provider == null)
			return;

		provider.QueueTeleportRequest(new TeleportRequest()
		{
			destinationPosition = transform.position,
			destinationRotation = transform.rotation,
			matchOrientation = MatchOrientation.TargetUpAndForward,
			requestTime = 0,
		});
	}

	private void OnDrawGizmos()
	{
		GizmosUtil.DrawArrow(transform, Color.cyan);
	}

	#endregion
}
