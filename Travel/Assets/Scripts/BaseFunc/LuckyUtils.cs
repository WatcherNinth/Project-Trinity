using UnityEngine;
using System.Collections;

namespace Lucky
{
    public class LuckyUtils
    {
        public const bool debug = true;

        public static T CreatePanelFromResource<T>(string prefabPath, Transform parent) where T : MonoBehaviour
        {
            GameObject prefab = PrefabManager.Instance.GetPrefabs(prefabPath);
            if (prefab == null)
            {
                Lucky.LuckyUtils.Log("load prefab from resource failed: " + prefabPath);
                return null;
            }

            GameObject panelObj = GameObject.Instantiate<GameObject>(prefab);
            panelObj.transform.SetParent(parent);
            MakeIndentity(panelObj.transform);
            panelObj.SetActive(true);
            T panel = panelObj.GetComponent<T>();
            return panel;
        }

        public static void MakeIndentity(Transform trans)
        {
            RectTransform rt = trans as RectTransform;
            rt.localScale = Vector3.one;
            rt.anchoredPosition3D = Vector3.zero;
        }

        public static void MakeFullStretch(RectTransform rt)
        {
            if (rt == null)
                return;

            rt.anchorMax = new Vector2(1, 1);
            rt.anchorMin = new Vector2(0, 0);
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        public static void MakeFullStretch(Transform trans)
        {
            if (trans == null)
                return;

            var rt = trans as RectTransform;
            MakeFullStretch(rt);
            rt.position = Vector3.zero;
        }

        public static void Log(string str)
        {
#if UNITY_EDITOR
            Debug.Log(str);
#endif

#if UNITY_ANDROID
            //Debug.Log(str);
#endif
        }

        public static void Log(int i)
        {
#if UNITY_EDITOR
            Debug.Log(i+"");
#endif

#if UNITY_ANDROID
            //Debug.Log(str);
#endif
        }

        public static void Log(float i)
        {
#if UNITY_EDITOR
            Debug.Log(i + "");
#endif

#if UNITY_ANDROID
            //Debug.Log(str);
#endif
        }

    }
}
