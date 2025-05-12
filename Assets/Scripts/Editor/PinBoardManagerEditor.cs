// Assets/Editor/PinBoardManagerEditor.cs
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PinBoardManager))]
public class PinBoardManagerEditor : Editor
{
    SerializedProperty socketsProp;

    void OnEnable()
    {
        socketsProp = serializedObject.FindProperty("sockets");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Header
        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Socket", GUILayout.Width(80));
        EditorGUILayout.LabelField("Occupied", GUILayout.Width(110));
        EditorGUILayout.LabelField("Correct", GUILayout.Width(115));
        EditorGUILayout.EndHorizontal();

        // Rows
        for (int i = 0; i < socketsProp.arraySize; i++)
        {
            // Draw alternating background
            var rowRect = GUILayoutUtility.GetRect(
                0, EditorGUIUtility.singleLineHeight + 4,
                GUILayout.ExpandWidth(true)
            );
            Color bg = (i % 2 == 0) ? new Color(0.18f, 0.18f, 0.18f) : new Color(0.16f, 0.16f, 0.16f);
            EditorGUI.DrawRect(rowRect, bg);

            // Now overlay your controls
            float x = rowRect.x;
            float y = rowRect.y + 2;
            EditorGUI.LabelField(new Rect(x, y, 60, rowRect.height), $"Socket {i + 1}");
            x += 90;
            EditorGUI.LabelField(new Rect(x, y, 60, rowRect.height), "Occupied");

            x += 70;
            var correctProp = socketsProp.GetArrayElementAtIndex(i).FindPropertyRelative("Correct");
            var occupiedProp = socketsProp.GetArrayElementAtIndex(i).FindPropertyRelative("Occupied");
            occupiedProp.boolValue = EditorGUI.ToggleLeft(
                new Rect(x, y, 70, rowRect.height),
                "", occupiedProp.boolValue
            );
            x += 40;
            EditorGUI.LabelField(new Rect(x, y, 60, rowRect.height), "Correct");
            x += 70;
            correctProp.boolValue = EditorGUI.ToggleLeft(
                new Rect(x, y, 90, rowRect.height),
                "", correctProp.boolValue
            );
        }

        // ——— Add / Remove buttons ———
        GUILayout.Space(6);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("➕ Add Socket"))
            socketsProp.InsertArrayElementAtIndex(socketsProp.arraySize);
        if (GUILayout.Button("➖ Remove Socket") && socketsProp.arraySize > 0)
            socketsProp.DeleteArrayElementAtIndex(socketsProp.arraySize - 1);
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
