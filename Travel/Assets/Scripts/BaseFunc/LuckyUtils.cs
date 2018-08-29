using UnityEngine;
using System.Collections;

namespace Lucky
{
    public class LuckyUtils
    {
        public static T CreatePanelFromResource<T>(string prefabPath, Transform parent) where T : MonoBehaviour
        {
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            if (prefab == null)
            {
                Debug.Log("load prefab from resource failed: " + prefabPath);
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
            rt.position = Vector3.zero;
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
        }

    }
}
