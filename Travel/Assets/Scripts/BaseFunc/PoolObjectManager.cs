using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Lucky
{
    /// <summary>
    /// 简单对象池，
    /// </summary>
    public class PoolObjectManager : MonoBehaviour
    {

        public GameObject perfab;
        private List<GameObject> pools;
        public GameObject GetGameObject(Transform parent)
        {
            if (pools == null)
            {
                pools = new List<GameObject>();
            }
            for (int i = 0; i < pools.Count; i++)
            {
                if (!pools[i].activeSelf)
                {
                    pools[i].SetActive(true);
                    return pools[i];
                }
            }
            GameObject go = Instantiate(perfab, Vector3.zero, Quaternion.identity) as GameObject;
            Debug.Log("go width " + go.GetComponent<RectTransform>().sizeDelta);
            go.transform.SetParent(parent,false);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            go.SetActive(true);
            pools.Add(go);
            Debug.Log("go width " + go.GetComponent<RectTransform>().sizeDelta);
            return go;
        }

        public void ClearPool()
        {
            if (pools != null)
            {
                pools.Clear();
            }
        }

        public void SetAllFree()
        {
            if (pools != null)
            {
                for (int i = 0; i < pools.Count; i++)
                {
                    pools[i].SetActive(false);
                }
            }
        }

        public void DestroyItem(GameObject go)
        {
            if (pools != null)
            {
                for (int i = 0; i < pools.Count; i++)
                {
                    if (pools[i] == go)
                    {
                        pools.RemoveAt(i);
                        break;
                    }
                }
            }
            Destroy(go);
        }

        public void OnDestroy()
        {
            if (pools != null)
            {
                pools.Clear();
                pools = null;
            }
        }
    }
}

