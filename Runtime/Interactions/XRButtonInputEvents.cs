namespace ToolkitEngine.XR
{
	public class XRButtonInputEvents : XRBaseInputEvents
    {
		#region Editor-Only
#if UNITY_EDITOR

		[UnityEngine.ContextMenu("Perform - Click")]
		protected void EditorPerformClick()
		{
			SetPerforming(true, default);
			SetPerforming(false, default);
		}

		[UnityEngine.ContextMenu("Perform - Press")]
		protected void EditorPerformPress()
		{
			SetPerforming(true, default);
		}

		[UnityEngine.ContextMenu("Cancel")]
		protected void EditorCanel()
		{
			SetPerforming(false, default);
		}

#endif
		#endregion
	}
}