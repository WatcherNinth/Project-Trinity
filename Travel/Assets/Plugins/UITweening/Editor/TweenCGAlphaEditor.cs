using UnityEditor;

namespace UnityEngine.UI
{
    [CustomEditor(typeof(TweenCGAlpha))]
    [CanEditMultipleObjects]
    public class TweenCGAlphaEditor : TweenMainEditor
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
            EditorGUILayout.Slider(_fromProperty, 0f, 1f);
            if (GUILayout.Button("\u25C0", GUILayout.Width(24f)))
            {
                foreach (var i in targets)
                {
                    var self = (TweenCGAlpha) i;
                    self.FromCurrentValue();
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(_fromOffsetProperty);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.Slider(_toProperty, 0f, 1f);
            if (GUILayout.Button("\u25C0", GUILayout.Width(24f)))
            {
                foreach (var i in targets)
                {
                    var self = (TweenCGAlpha) i;
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
