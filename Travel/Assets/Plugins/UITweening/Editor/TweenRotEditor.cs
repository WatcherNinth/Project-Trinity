using UnityEditor;

namespace UnityEngine.UI
{
    [CustomEditor(typeof(TweenRot))]
    [CanEditMultipleObjects]
    public class TweenRotEditor : TweenMainEditor
    {
        private SerializedProperty
            _fromProperty,
            _fromOffetProperty,
            _toProperty,
            _toOffetProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            _fromProperty = serializedObject.FindProperty("from");
            _fromOffetProperty = serializedObject.FindProperty("fromOffset");
            _toProperty = serializedObject.FindProperty("to");
            _toOffetProperty = serializedObject.FindProperty("toOffset");
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
                    var self = (TweenRot) i;
                    self.FromCurrentValue();
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(_fromOffetProperty);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_toProperty);
            if (GUILayout.Button("\u25C0", GUILayout.Width(24f)))
            {
                foreach (var i in targets)
                {
                    var self = (TweenRot) i;
                    self.ToCurrentValue();
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(_toOffetProperty);
            serializedObject.ApplyModifiedProperties();
            
            EditorGUILayout.Separator();
            BaseTweenerProperties();
        }
    }
}
