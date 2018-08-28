using UnityEditor;

namespace UnityEngine.UI
{
    [CustomEditor(typeof(TweenColor))]
    [CanEditMultipleObjects]
    public class TweenColorEditor : TweenMainEditor
    {
        private SerializedProperty
            _fromProperty,
            _toProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            _fromProperty = serializedObject.FindProperty("from");
            _toProperty = serializedObject.FindProperty("to");
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
                    var self = (TweenColor) i;
                    self.FromCurrentValue();
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_toProperty);
            if (GUILayout.Button("\u25C0", GUILayout.Width(24f)))
            {
                foreach (var i in targets)
                {
                    var self = (TweenColor) i;
                    self.ToCurrentValue();
                }
            }
            EditorGUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Separator();
            BaseTweenerProperties();
        }
    }
}
