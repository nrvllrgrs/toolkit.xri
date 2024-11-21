using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEditor;
using UnityEditorInternal;
using ToolkitEngine.XR;

namespace ToolkitEditor.XR
{
	[CustomEditor(typeof(XRHandednessContext))]
	public class XRHandednessContextEditor : BaseToolkitEditor
    {
		#region Fields

		protected XRHandednessContext m_context;

		protected SerializedProperty m_pairs;
		private ReorderableList m_pairsList;

		private const string LEFT_PROP_NAME = "left";
		private const string RIGHT_PROP_NAME = "right";
		private const string LEFT_ICON_GUID = "ad7318d4e914fdb48906ea7656fcffec";
		private const string RIGHT_ICON_GUID = "bfcdb7da60bd0b14f9d2e84ff0b40f68";

		#endregion

		#region Methods

		protected virtual void OnEnable()
		{
			m_context = target as XRHandednessContext;
			m_pairs = serializedObject.FindProperty(nameof(m_pairs));
		}

		protected override void DrawProperties()
		{
			base.DrawProperties();

			if (m_pairsList == null)
			{
				m_pairsList = new ReorderableList(m_context.pairs.ToArray(), typeof(XRHandednessContext.InteractorPair), false, true, true, true);
				m_pairsList.drawHeaderCallback += (Rect rect) =>
				{
					EditorGUI.LabelField(rect, "Pairs");
				};
				m_pairsList.drawElementCallback += OnDrawElementCallback;
				m_pairsList.onCanAddCallback += OnCanAddCallback;
				m_pairsList.onAddDropdownCallback += OnAddDropdownCallback;
				m_pairsList.onCanRemoveCallback += OnCanRemoveCallback;
				m_pairsList.onRemoveCallback += OnRemoveCallback;
			}

			// Draw pairs list
			m_pairsList.DoLayoutList();
		}

		#endregion

		#region ReorderableList Methods

		private void OnDrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
		{
			var pairProp = m_pairs.GetArrayElementAtIndex(index);

			Rect iconRect = new Rect(rect);
			iconRect.width = EditorGUIUtility.singleLineHeight;

			Rect dropdownRect = new Rect(rect);
			dropdownRect.x += iconRect.width + EditorGUIUtility.standardVerticalSpacing;
			dropdownRect.width = (rect.width - (EditorGUIUtility.standardVerticalSpacing * 3f) - (iconRect.width * 2f)) / 2f;

			EditorGUI.LabelField(iconRect, new GUIContent(AssetDatabase.LoadAssetAtPath<Texture>(AssetDatabase.GUIDToAssetPath(LEFT_ICON_GUID))));
			DrawDropdownButton(dropdownRect, pairProp.FindPropertyRelative(LEFT_PROP_NAME), InteractorHandedness.Left, (prop) =>
			{
				return prop.FindPropertyRelative(LEFT_PROP_NAME).objectReferenceValue as IXRInteractor;
			});

			iconRect.x = dropdownRect.x + dropdownRect.width + EditorGUIUtility.standardVerticalSpacing;
			dropdownRect.x = iconRect.x + iconRect.width + EditorGUIUtility.standardVerticalSpacing;

			EditorGUI.LabelField(iconRect, new GUIContent(AssetDatabase.LoadAssetAtPath<Texture>(AssetDatabase.GUIDToAssetPath(RIGHT_ICON_GUID))));
			DrawDropdownButton(dropdownRect, pairProp.FindPropertyRelative(RIGHT_PROP_NAME), InteractorHandedness.Right, (prop) =>
			{
				return prop.FindPropertyRelative(RIGHT_PROP_NAME).objectReferenceValue as IXRInteractor;
			});
		}

		private void DrawDropdownButton(Rect position, SerializedProperty prop, InteractorHandedness handedness, Func<SerializedProperty, IXRInteractor> getInteractor)
		{
			var selectedInteractor = prop.objectReferenceValue as IXRInteractor;

			string selectionLabel = string.Empty;
			if (prop.objectReferenceValue != null)
			{
				selectionLabel = GetInteractorLabel(prop.objectReferenceValue as IXRInteractor);
			}

			if (EditorGUI.DropdownButton(position, new GUIContent(selectionLabel), FocusType.Keyboard))
			{
				var interactors = new List<IXRInteractor>();
				foreach (var interactor in m_context.GetComponentsInChildren<IXRInteractor>())
				{
					if (Equals(interactor.handedness, handedness))
					{
						interactors.Add(interactor);
					}
				}

				// Remove interactors already in use
				for (int i = 0; i < m_pairs.arraySize; ++i)
				{
					var interactor = getInteractor(m_pairs.GetArrayElementAtIndex(i));
					if (interactor != selectedInteractor)
					{
						interactors.Remove(interactor);
					}
				}

				var menu = new GenericMenu();
				menu.AddItem(new GUIContent("None"), false, HandleMenuItemClicked, new MenuEventArgs()
				{
					property = prop,
					interactor = null,
				});

				foreach (var interactor in interactors)
				{
					menu.AddItem(new GUIContent(GetInteractorLabel(interactor)), false, HandleMenuItemClicked, new MenuEventArgs()
					{
						property = prop,
						interactor = interactor as XRBaseInteractor
					});
				}

				menu.DropDown(position);
			}
		}

		private string GetInteractorLabel(IXRInteractor interactor) => string.Format("{0} ({1})", interactor.transform.name, interactor.GetType().Name);

		private void HandleMenuItemClicked(object args)
		{
			var menuEventArgs = (MenuEventArgs)args;

			menuEventArgs.property.serializedObject.Update();
			menuEventArgs.property.objectReferenceValue = menuEventArgs.interactor as XRBaseInteractor;
			menuEventArgs.property.serializedObject.ApplyModifiedProperties();
		}

		private bool OnCanAddCallback(ReorderableList list) => true;

		private void OnAddDropdownCallback(Rect buttonRect, ReorderableList list)
		{
			m_context.pairs.Add(new XRHandednessContext.InteractorPair());
			m_pairsList.list = m_context.pairs.ToArray();
		}

		private bool OnCanRemoveCallback(ReorderableList list)
		{
			return m_pairs.arraySize > 0;
		}

		private void OnRemoveCallback(ReorderableList list)
		{
			// Remove item from list
			m_context.pairs.RemoveAt(list.index);
			list.list = m_context.pairs.ToArray();
		}

		#endregion

		#region Structures

		private struct MenuEventArgs
		{
			public SerializedProperty property;
			public IXRInteractor interactor;
		}

		#endregion
	}
}