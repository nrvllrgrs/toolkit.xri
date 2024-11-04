using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System.Linq;

namespace ToolkitEditor.XR
{
    public static class MenuItemEditor
    {
        private static List<object> s_clipboard = new();

        [MenuItem("GameObject/Toolkit/XR/Copy Colliders")]
        public static void CopyColliders()
        {
            s_clipboard.Clear();
            s_clipboard.AddRange(Selection.activeGameObject.GetComponents<Collider>());
        }

        [MenuItem("GameObject/Toolkit/XR/Paste Colliders")]
        public static void PasteColliders()
        {
            if (s_clipboard.Count == 0)
                return;

            var colliders = s_clipboard.Where(x => x is Collider)
                .Cast<Collider>();

            foreach (var interactable in Selection.activeGameObject.GetComponents<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>())
            {
                interactable.colliders.Clear();
                interactable.colliders.AddRange(colliders);
            }
        }
    }
}