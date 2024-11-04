using UnityEditor;

using UnityEngine;
using ToolkitEngine.XR;

namespace ToolkitEditor.XR
{
    [CustomEditor(typeof(XRBasePositionInteractable), true)]
    public class XRBasePositionInteractableEditor : UnityEditor.XR.Interaction.Toolkit.Interactables.XRBaseInteractableEditor
    {
        #region Fields

        protected SerializedProperty m_isInteractable;
        protected SerializedProperty m_translateTransform;
        protected SerializedProperty m_directionAxis;
        protected SerializedProperty m_normalizedFlowDepths;
        protected SerializedProperty m_maxDepth;
        protected SerializedProperty m_snapMode;
        protected SerializedProperty m_snapSpeed;

        protected SerializedProperty m_useTipflowHaptics;
        protected SerializedProperty m_tipflowHaptics;
        protected SerializedProperty m_useTailflowHaptics;
        protected SerializedProperty m_tailflowHaptics;

        protected SerializedProperty m_onDepthChanged;
        protected SerializedProperty m_onTipflow;
        protected SerializedProperty m_onTailflow;

        private GUIStyle m_previewNormal, m_previewToggled;

        #endregion

        #region Methods

        protected override void OnEnable()
        {
            base.OnEnable();
            m_isInteractable = serializedObject.FindProperty(nameof(m_isInteractable));
			m_translateTransform = serializedObject.FindProperty(nameof(m_translateTransform));
            m_directionAxis = serializedObject.FindProperty(nameof(m_directionAxis));
            m_maxDepth = serializedObject.FindProperty(nameof(m_maxDepth));
            m_normalizedFlowDepths = serializedObject.FindProperty(nameof(m_normalizedFlowDepths));
            m_snapMode = serializedObject.FindProperty(nameof(m_snapMode));
            m_snapSpeed = serializedObject.FindProperty(nameof(m_snapSpeed));

            m_useTipflowHaptics = serializedObject.FindProperty(nameof(m_useTipflowHaptics));
            m_tipflowHaptics = serializedObject.FindProperty(nameof(m_tipflowHaptics));
            m_useTailflowHaptics = serializedObject.FindProperty(nameof(m_useTailflowHaptics));
            m_tailflowHaptics = serializedObject.FindProperty(nameof(m_tailflowHaptics));

            m_onDepthChanged = serializedObject.FindProperty(nameof(m_onDepthChanged));
            m_onTipflow = serializedObject.FindProperty(nameof(m_onTipflow));
            m_onTailflow = serializedObject.FindProperty(nameof(m_onTailflow));

            //m_previewNormal = "Button";
            //m_previewToggled = new GUIStyle(m_previewNormal);
            //m_previewToggled.normal.background = m_previewToggled.active.background;
        }

        protected override void DrawProperties()
        {
            DrawCoreConfiguration();
            EditorGUILayout.PropertyField(m_isInteractable);
            EditorGUILayout.Space();

            DrawLinearAxisProperties();

            EditorGUILayout.Space();

            //DrawPreview();
        }

        protected virtual void DrawLinearAxisProperties()
        {
            EditorGUILayout.PropertyField(m_translateTransform);
            EditorGUILayout.PropertyField(m_directionAxis, new GUIContent("Direction"));
            EditorGUILayout.PropertyField(m_maxDepth);
            EditorGUILayoutUtility.MinMaxSlider(m_normalizedFlowDepths, 0f, 1f);

            EditorGUILayout.PropertyField(m_snapMode);
            if (m_snapMode.intValue != (int)XRBasePositionInteractable.SnapMode.None)
            {
                ++EditorGUI.indentLevel;
                EditorGUILayout.PropertyField(m_snapSpeed);
				--EditorGUI.indentLevel;
			}
        }

        protected void DrawPreview()
        {
            EditorGUILayout.LabelField("Preview", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Tail", m_previewToggled))
            {

            }
            if (GUILayout.Button("Tip", m_previewNormal))
            {

            }
            EditorGUILayout.EndHorizontal();
        }

        protected override void DrawDerivedProperties()
        { }

        protected override void DrawInteractableEventsNested()
        {
            base.DrawInteractableEventsNested();

            EditorGUILayout.LabelField("Flow", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_onDepthChanged);
            EditorGUILayout.PropertyField(m_onTipflow);
            EditorGUILayout.PropertyField(m_onTailflow);
        }

        protected override void DrawEvents()
        {
            base.DrawEvents();

            EditorGUILayout.Separator();
            DrawHapticEvents();
        }

        protected virtual void DrawHapticEvents()
        {
            m_useTipflowHaptics.isExpanded = EditorGUILayout.Foldout(m_useTipflowHaptics.isExpanded, EditorGUIUtility.TrTempContent("Haptic Events"), true);
            if (m_useTipflowHaptics.isExpanded)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    DrawHapticEventsNested();
                }
            }
        }

        protected virtual void DrawHapticEventsNested()
        {
            EditorGUILayout.PropertyField(m_useTipflowHaptics);
            if (m_useTipflowHaptics.boolValue)
            {
                ++EditorGUI.indentLevel;
                EditorGUILayout.PropertyField(m_tipflowHaptics);
                --EditorGUI.indentLevel;
            }

            EditorGUILayout.PropertyField(m_useTailflowHaptics);
            if (m_useTailflowHaptics.boolValue)
            {
                ++EditorGUI.indentLevel;
                EditorGUILayout.PropertyField(m_tailflowHaptics);
                --EditorGUI.indentLevel;
            }
        }

        #endregion
    }
}