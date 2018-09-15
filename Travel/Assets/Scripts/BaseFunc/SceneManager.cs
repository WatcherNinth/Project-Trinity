using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Lucky
{

    /// <summary>
    /// jony
    /// 场景跳转
    /// </summary>
    public class MySceneManager : MonoBehaviour
    {
        private const string GLOBAL_ASSET = "GLOBAL_ASSET";
        private bool _isLoadingScene = false;
        private const int RAM_THRESHOLD = 1024;//MB

        #region 单例
        private static MySceneManager _instance = null;
        public static MySceneManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameSystem.instance.gameObject.AddComponent<MySceneManager>();
                    Lucky.LuckyUtils.LogError("不应该走到这里");
                }
                return _instance;
            }
        }
        void Awake()
        {
            _instance = this;
        }
        #endregion

        #region 缓存相关
        private bool hasInitMessageBus = false;
        /// <summary>
        /// 当前场景加载了的prefab，可供Instantiate，跨场景时，先清空，再从preloadDic中加载
        /// </summary>
        private Dictionary<string, GameObject> loadedPrefabDic = new Dictionary<string, GameObject>();
        /// <summary>
        /// 全局加载了的preafab，可供Instantiate，不清空
        /// </summary>
        private Dictionary<string, GameObject> globalLoadedPrefabDic = new Dictionary<string, GameObject>();
        /// <summary>
        /// 当前场景加载了的sprite，跨场景时清空
        /// </summary>
        private Dictionary<string, Sprite> loadedImageDic = new Dictionary<string, Sprite>();
        /// <summary>
        /// 全局加载了的sprite，不清空
        /// </summary>
        private Dictionary<string, Sprite> globalLoadedImageDic = new Dictionary<string, Sprite>();


        /// <summary>
        /// 预加载Prefab
        /// </summary>
        private Dictionary<string, List<string>> preloadPrefabDic = new Dictionary<string, List<string>>()
        {

        };









        /// <summary>
        /// 从 loadedPrefabDic,globalLoadedPrefabDic 中找
        /// </summary>
        public GameObject getPreloadedGameObject(string sourceName)
        {
            if (loadedPrefabDic != null && loadedPrefabDic.ContainsKey(sourceName))
            {
                return loadedPrefabDic[sourceName];
            }
            else if (globalLoadedPrefabDic != null && globalLoadedPrefabDic.ContainsKey(sourceName))
            {
                return globalLoadedPrefabDic[sourceName];
            }
            else
            {
                return null;
            }
        }

        public Sprite getPreloadedSprite(string sourceName)
        {
            if (loadedImageDic != null && loadedImageDic.ContainsKey(sourceName))
            {
                return loadedImageDic[sourceName];
            }
            else if (globalLoadedImageDic != null && globalLoadedImageDic.ContainsKey(sourceName))
            {
                return globalLoadedImageDic[sourceName];
            }
            else
            {
                return null;
            }
        }

        #endregion

    }
}
