using UnityEditor;

using ToolkitEngine.XR;

namespace ToolkitEditor.XR
{
    [CustomEditor(typeof(XRBaseRotationInteractable), true)]
    public class XRBaseRotationInteractableEditor : UnityEditor.XR.Interaction.Toolkit.Interactables.XRBaseInteractableEditor
    {
        #region Fields

        protected SerializedProperty m_isInteractable;
        protected SerializedProperty m_forward;
        protected SerializedProperty m_upward;
        protected SerializedProperty m_range;
        protected SerializedProperty m_rotationDirection;
        protected SerializedProperty m_startingAngle;

        protected SerializedProperty m_useSteps;
        protected SerializedProperty m_stepCount;
        protected SerializedProperty m_updateIndexContinuously;

        protected SerializedProperty m_snapMode;
        protected SerializedProperty m_snap;
        protected SerializedProperty m_snapSpeed;
        protected SerializedProperty m_restingAngle;

        protected SerializedProperty m_useValueChangedHaptics;
        protected SerializedProperty m_valueChangedHaptics;
        protected SerializedProperty m_useIndexChangedHaptics;
        protected SerializedProperty m_indexChangedHaptics;

        protected SerializedProperty m_onValueChanged;
        protected SerializedProperty m_onIndexChanged;

        #endregion

        #region Methods

        protected override void OnEnable()
        {
            base.OnEnable();
            m_isInteractable = serializedObject.FindProperty(nameof(m_isInteractable));
            m_forward = serializedObject.FindProperty(nameof(m_forward));
            m_upward = serializedObject.FindProperty(nameof(m_upward));
            m_range = serializedObject.FindProperty(nameof(m_range));
            m_rotationDirection = serializedObject.FindProperty(nameof(m_rotationDirection));
            m_startingAngle = serializedObject.FindProperty(nameof(m_startingAngle));

            m_useSteps = serializedObject.FindProperty(nameof(m_useSteps));
            m_stepCount = serializedObject.FindProperty(nameof(m_stepCount));
            m_updateIndexContinuously = serializedObject.FindProperty(nameof(m_updateIndexContinuously));

            m_snapMode = serializedObject.FindProperty(nameof(m_snapMode));
            m_snap = serializedObject.FindProperty(nameof(m_snap));
            m_snapSpeed = serializedObject.FindProperty(nameof(m_snapSpeed));
            m_restingAngle = serializedObject.FindProperty(nameof(m_restingAngle));

            m_useValueChangedHaptics = serializedObject.FindProperty(nameof(m_useValueChangedHaptics));
            m_valueChangedHaptics = serializedObject.FindProperty(nameof(m_valueChangedHaptics));
            m_useIndexChangedHaptics = serializedObject.FindProperty(nameof(m_useIndexChangedHaptics));
            m_indexChangedHaptics = serializedObject.FindProperty(nameof(m_indexChangedHaptics));

            m_onValueChanged = serializedObject.FindProperty(nameof(m_onValueChanged));
            m_onIndexChanged = serializedObject.FindProperty(nameof(m_onIndexChanged));
        }

        protected override void DrawProperties()
        {
            DrawCoreConfiguration();
            EditorGUILayout.PropertyField(m_isInteractable);
            EditorGUILayout.Space();

            DrawAngularAxisProperties();
            EditorGUILayout.Space();
        }

        protected virtual void DrawAngularAxisProperties()
        {
            EditorGUILayout.PropertyField(m_forward);
            EditorGUILayout.PropertyField(m_upward);
            EditorGUILayoutUtility.MinMaxSlider(m_range, -180f, 180f);
            EditorGUILayout.PropertyField(m_rotationDirection);
            EditorGUILayout.Slider(m_startingAngle, m_range.vector2Value.x, m_range.vector2Value.y);

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(m_useSteps);
            if (m_useSteps.boolValue)
            {
                ++EditorGUI.indentLevel;
                EditorGUILayout.PropertyField(m_stepCount);
                EditorGUILayout.PropertyField(m_updateIndexContinuously);
                --EditorGUI.indentLevel;
            }

            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(m_snapMode);

            var snapMode = (XRBaseRotationInteractable.SnapMode)m_snapMode.intValue;
            if (snapMode != XRBaseRotationInteractable.SnapMode.None)
            {
                ++EditorGUI.indentLevel;
                EditorGUILayout.PropertyField(m_snap);

                if (!m_snap.boolValue)
                {
                    EditorGUILayout.PropertyField(m_snapSpeed);
                }

                if (snapMode == XRBaseRotationInteractable.SnapMode.Angle)
                {
                    EditorGUILayout.Slider(m_restingAngle, m_range.vector2Value.x, m_range.vector2Value.y);
                }
                --EditorGUI.indentLevel;
            }
        }

        protected override void DrawInteractableEventsNested()
        {
            base.DrawInteractableEventsNested();

            EditorGUILayout.LabelField("Rotator", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_onValueChanged);

            if (m_useSteps.boolValue)
            {
                EditorGUILayout.PropertyField(m_onIndexChanged);
            }
        }

        protected override void DrawEvents()
        {
            base.DrawEvents();

            EditorGUILayout.Separator();
            DrawHapticEvents();
        }

        protected virtual void DrawHapticEvents()
        {
            m_useValueChangedHaptics.isExpanded = EditorGUILayout.Foldout(m_useValueChangedHaptics.isExpanded, EditorGUIUtility.TrTempContent("Haptic Events"), true);
            if (m_useValueChangedHaptics.isExpanded)
            {
                using (new EditorGUI.IndentLevelScope())
                {
                    DrawHapticEventsNested();
                }
            }
        }

        protected virtual void DrawHapticEventsNested()
        {
            EditorGUILayout.PropertyField(m_useValueChangedHaptics);
            if (m_useValueChangedHaptics.boolValue)
            {
                ++EditorGUI.indentLevel;
                EditorGUILayout.PropertyField(m_valueChangedHaptics);
                --EditorGUI.indentLevel;
            }

            if (m_useSteps.boolValue)
            {
                EditorGUILayout.PropertyField(m_useIndexChangedHaptics);
                if (m_useIndexChangedHaptics.boolValue)
                {
                    ++EditorGUI.indentLevel;
                    EditorGUILayout.PropertyField(m_indexChangedHaptics);
                    --EditorGUI.indentLevel;
                }
            }
        }

        #endregion
    }
}