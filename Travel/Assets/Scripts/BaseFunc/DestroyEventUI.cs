using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Lucky
{
    /// <summary>
    /// 如果物体需要在销毁时有事件，则挂此组件
    /// 
    /// @authro bughuang
    /// </summary>
    public class DestroyEventUI : MonoBehaviour
    {
        public delegate void DestroyHandler(GameObject traget);

        public event DestroyHandler onDestroy;

        public Transform parent;

        void Start()
        {
            parent = transform.parent;
        }

        void OnDestroy()
        {
            if (onDestroy != null)
                onDestroy(this.gameObject);
            parent = null;
        }
    }
}
