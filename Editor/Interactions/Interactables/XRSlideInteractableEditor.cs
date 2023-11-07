using UnityEditor;
using ToolkitEngine.XR;

namespace ToolkitEditor.XR
{
    [CustomEditor(typeof(XRSlideInteractable))]
    public class XRSlideInteractableEditor : XRBasePositionInteractableEditor
    {
        #region Fields

        protected SerializedProperty m_cancelInteractionOnUnhover;

        #endregion

        #region Methods

        protected override void OnEnable()
        {
            base.OnEnable();
            m_cancelInteractionOnUnhover = serializedObject.FindProperty(nameof(m_cancelInteractionOnUnhover));
        }

        protected override void DrawLinearAxisProperties()
        {
            base.DrawLinearAxisProperties();
            EditorGUILayout.PropertyField(m_cancelInteractionOnUnhover);
        }

        #endregion
    }
}