#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

// Tell Unity this drawer is for your socket struct
[CustomPropertyDrawer(typeof(socket))]
public class SocketDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Wrap in Begin/EndProperty for prefab overrides, context menus, animated values, etc.
        EditorGUI.BeginProperty(position, label, property);

        // Turn off any indent so we're flush to the left edge of `position`
        int oldIndent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // === Manually draw the label ===
        // Reserve the standard labelWidth for the “Sockets” title
        float labelWidth = EditorGUIUtility.labelWidth;
        Rect labelRect = new Rect(position.x, position.y, labelWidth, position.height);
        Rect contentRect = new Rect(position.x + labelWidth,
                                   position.y,
                                   position.width - labelWidth,
                                   position.height);

        // Draw the “Sockets” label
        EditorGUI.LabelField(labelRect, label);

        // === Split the remaining space in half for your two bools ===
        float halfWidth = (contentRect.width / 2f) - 2;
        Rect leftRect = new Rect(contentRect.x,
                                  contentRect.y,
                                  halfWidth,
                                  contentRect.height);
        Rect rightRect = new Rect(contentRect.x + halfWidth + 4,
                                  contentRect.y,
                                  halfWidth,
                                  contentRect.height);

        // Fetch the two bool properties by their exact field names
        var correctProp = property.FindPropertyRelative("Correct");
        var occupiedProp = property.FindPropertyRelative("Occupied");

        if (correctProp == null || occupiedProp == null)
        {
            // If one is null, you’ll see this error text in-editor immediately
            EditorGUI.LabelField(position, "[SocketDrawer] field names mismatch");
        }
        else
        {
            // Draw them side by side, with your custom labels
            EditorGUI.PropertyField(leftRect, correctProp, new GUIContent("Correct"));
            EditorGUI.PropertyField(rightRect, occupiedProp, new GUIContent("Occupied"));
        }

        // Restore indent and finish up
        EditorGUI.indentLevel = oldIndent;
        EditorGUI.EndProperty();
    }

    // Tell Unity this drawer is exactly one line tall
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }
}
#endif
