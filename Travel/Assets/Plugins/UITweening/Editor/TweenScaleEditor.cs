using UnityEditor;

namespace UnityEngine.UI
{
    [CustomEditor(typeof(TweenScale))]
    [CanEditMultipleObjects]
    public class TweenScaleEditor : TweenMainEditor
    {
        private SerializedProperty
            _fromProperty,
            _fromOffsetProperty,
            _toProperty,
            _toOffsetProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            _fromProperty = serializedObject.FindProperty("from");
            _fromOffsetProperty = serializedObject.FindProperty("fromOffset");
            _toProperty = serializedObject.FindProperty("to");
            _toOffsetProperty = serializedObject.FindProperty("toOffset");
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
                    var self = (TweenScale) i;
                    self.FromCurrentValue();
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(_fromOffsetProperty);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_toProperty);
            if (GUILayout.Button("\u25C0", GUILayout.Width(24f)))
            {
                foreach (var i in targets)
                {
                    var self = (TweenScale) i;
                    self.ToCurrentValue();
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(_toOffsetProperty);
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Separator();
            BaseTweenerProperties();
        }
    }
}
