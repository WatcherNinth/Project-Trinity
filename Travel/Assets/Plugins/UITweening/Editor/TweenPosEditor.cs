using UnityEditor;

namespace UnityEngine.UI
{
    [CustomEditor(typeof(TweenPos))]
    [CanEditMultipleObjects]
    public class TweenPosEditor : TweenMainEditor
    {
        private SerializedProperty
            _fromProperty,
            _fromOffsetProperty,
            _toProperty,
            _toOffsetProperty,
            _cSpaceProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            _fromProperty = serializedObject.FindProperty("from");
            _fromOffsetProperty = serializedObject.FindProperty("fromOffset");
            _toProperty = serializedObject.FindProperty("to");
            _toOffsetProperty = serializedObject.FindProperty("toOffset");
            _cSpaceProperty = serializedObject.FindProperty("CSpace");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_fromProperty);
            if (GUILayout.Button("\u25C0", GUILayout.Width(24f)))
            {
                foreach (var i in targets)
                {
                    var self = (TweenPos) i;
                    self.FromCurrentValue();
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(_fromOffsetProperty, new GUIContent("Offset"));

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_toProperty);
            if (GUILayout.Button("\u25C0", GUILayout.Width(24f)))
            {
                foreach (var i in targets)
                {
                    var self = (TweenPos) i;
                    self.ToCurrentValue();
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(_toOffsetProperty, new GUIContent("Offset"));
            EditorGUILayout.PropertyField(_cSpaceProperty, new GUIContent("Coordinate Space"));
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Separator();
            BaseTweenerProperties();
        }
    }
}
