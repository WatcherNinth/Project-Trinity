using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lucky
{
    class PopPanelPathConst
    {
        
    }

    

    public class PopUpManager
    {
        public enum PopCanvasLayer
        {
            E_UILayer = 0,
            E_PopLayer
        };

        public enum PopupShowPriority
        {
            E_Low = -1,
            E_Normal = 0,
            E_Middle = 5,
            E_High = 9,
            E_High1,
            E_High2,
            E_High3
        };


        /// <summary>
        /// PopupInfo:表示一个弹窗的信息：gameObj是该弹窗，childObjs是显示在该弹窗上的子弹窗,showPriority是显示优先级，strId是该弹窗的id
        /// </summary>
        public class PopupInfo
        {
            public GameObject gameObj;
            public List<PopupInfo> childObjs = new List<PopupInfo>();
            public int showPriority;
            public string strId;

            public void SetPopupInfoVisible(bool isVisible)
            {
                //gameObj.SetActive(isVisible);
                gameObj.GetComponent<BaseScene>().SetVisable(isVisible);
                for (int idx = 0; idx < childObjs.Count; ++idx)
                {
                    //childObjs[idx].gameObj.SetActive(isVisible);
                    childObjs[idx].gameObj.GetComponent<BaseScene>().SetVisable(isVisible);
                }
            }
        };

        private static PopUpManager m_Instance;
        public static PopUpManager Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new PopUpManager();
                }
                return m_Instance;
            }
        }


        //uiLayerPopUps:UI层的canvas上弹窗的列表
        private List<PopupInfo> uiLayerPopUps = new List<PopupInfo>();
        //popLayerPopUps:Pop层的canvas上弹窗的列表
        private List<PopupInfo> popLayerPopUps = new List<PopupInfo>();
        //currentShowPopupInfo：当前正在显示的弹窗
        private PopupInfo currentShowPopupInfo = null;
        //下一次update的时候是否要检查一下弹面板的逻辑:从两个队列里取优先级最高的一个弹窗，并将之弹出来
        private bool needCheckToShowPopup = false;

        //重登陆失败的弹框永远不要自动消失
        public const string connectFailedDialogId = "LOGIN_RECONNECT_Error";
        //不能移除的游戏内通知
        public const string noRemoveGameNoticeId = "NOREMOVE_NOTICE";
        //不自动移除的弹窗
        public const string FriendRoomAddChipPanel = "FriendRoomAddChipPanel";
        //不自动移除消息中心的弹窗
        public const string FRMessageCenterPanel = "FRMessageCenterPanel";

        #region add
        // for lua
        public void AddUiLayerPopup2(GameObject popUp)
        {
            //判定预设是否存在，否则返回null
            Canvas canvas = null;

            // added by jackmo at 2017-2-6，扫描bug，popUp判空
            if (popUp == null)
            {
                Lucky.LuckyUtils.Log("PopupManager, AddPopupNew, Cannot Instantiate the prefab!");
                return;
            }
            // added end

            //GetCanvasByLayer：获取对应的canvas：UICanvas or PopCanvas：如果有则直接取值，如果没有则会执行创建过程
            canvas = GetCanvasByLayer(PopCanvasLayer.E_UILayer);

            //设置popUp位置：居中显示在屏幕上
            popUp.transform.SetParent(canvas.transform, false);
            popUp.transform.localPosition = Vector3.zero;
            popUp.transform.localScale = Vector3.one;
            (popUp.transform as RectTransform).SetAsLastSibling();

            //给所有的物件都挂上DestroyEventUI：该脚本的OnDestory函数里面会通知到PopUpMgr.OnDestroy
            //所以实现了PopUpMgr监听弹窗的销毁事件
            DestroyEventUI destroyUi = popUp.GetComponent<DestroyEventUI>();
            if (destroyUi == null)
            {
                destroyUi = popUp.AddComponent<DestroyEventUI>();
            }
            destroyUi.onDestroy += OnDestroy;

            //创建popInfo，准备插入到显示队列里面去
            PopupInfo popInfo = new PopupInfo();
            popInfo.gameObj = popUp;
            popInfo.showPriority = 0;

            popInfo.strId = "";

            //插入过程：根据showPriority从队列尾部遍历插入
            bool insertSuc = false;
            for (int idx = uiLayerPopUps.Count - 1; idx >= 0; --idx)
            {
                if (uiLayerPopUps[idx].showPriority <= 0)
                {
                    uiLayerPopUps.Insert(idx + 1, popInfo);
                    insertSuc = true;
                    break;
                }
            }
            //没找到合适的位置插入，说明该弹窗的showPriority是最小的，所以往顶部插
            if (!insertSuc)
            {
                uiLayerPopUps.Insert(0, popInfo);
            }

            //刚插入进去，等下一帧update里面会执行checkshow逻辑，如果该弹窗优先级是最高的，则她会在下一帧弹出来
            popUp.GetComponent<BaseScene>().SetVisable(false);
            needCheckToShowPopup = true;
        }

        //for lua
        public void AddPopupLayerPopup2(GameObject popUp)
        {
            //判定预设是否存在，否则返回null
            Canvas canvas = null;

            // added by jackmo at 2017-2-6，扫描bug，popUp判空
            if (popUp == null)
            {
                Lucky.LuckyUtils.Log("PopupManager, AddPopupNew, Cannot Instantiate the prefab!");
                return;
            }
            // added end

            //GetCanvasByLayer：获取对应的canvas：UICanvas or PopCanvas：如果有则直接取值，如果没有则会执行创建过程
            canvas = GetCanvasByLayer(PopCanvasLayer.E_PopLayer);

            //设置popUp位置：居中显示在屏幕上
            popUp.transform.SetParent(canvas.transform, false);
            popUp.transform.localPosition = Vector3.zero;
            popUp.transform.localScale = Vector3.one;
            (popUp.transform as RectTransform).SetAsLastSibling();

            //给所有的物件都挂上DestroyEventUI：该脚本的OnDestory函数里面会通知到PopUpMgr.OnDestroy
            //所以实现了PopUpMgr监听弹窗的销毁事件
            DestroyEventUI destroyUi = popUp.GetComponent<DestroyEventUI>();
            if (destroyUi == null)
            {
                destroyUi = popUp.AddComponent<DestroyEventUI>();
            }
            destroyUi.onDestroy += OnDestroy;

            //创建popInfo，准备插入到显示队列里面去
            PopupInfo popInfo = new PopupInfo();
            popInfo.gameObj = popUp;
            popInfo.showPriority = 0;

            popInfo.strId = "";

            //插入过程：根据showPriority从队列尾部遍历插入
            bool insertSuc = false;
            for (int idx = popLayerPopUps.Count - 1; idx >= 0; --idx)
            {
                if (popLayerPopUps[idx].showPriority <= 0)
                {
                    popLayerPopUps.Insert(idx + 1, popInfo);
                    insertSuc = true;
                    break;
                }
            }
            //没找到合适的位置插入，说明该弹窗的showPriority是最小的，所以往顶部插
            if (!insertSuc)
            {
                popLayerPopUps.Insert(0, popInfo);
            }

            //刚插入进去，等下一帧update里面会执行checkshow逻辑，如果该弹窗优先级是最高的，则她会在下一帧弹出来
            popUp.GetComponent<BaseScene>().SetVisable(false);
            needCheckToShowPopup = true;
        }


        //往UI层Canvas上加一个弹窗：
        public GameObject AddUiLayerPopUp(string prefabName, int showPriority = (int)PopupShowPriority.E_Normal, string id = "")
        {
            AudioManager.Instance.PlayMusic(Audios.PopupClip);
            return AddPopupNew(prefabName, PopCanvasLayer.E_UILayer, ref uiLayerPopUps, showPriority, id);
        }

        //往Pop层Canvas上加一个弹窗：
        public GameObject AddPopLayerPopUp(string prefabName, int showPriority = (int)PopupShowPriority.E_Normal, string id = "")
        {
            return AddPopupNew(prefabName, PopCanvasLayer.E_PopLayer, ref popLayerPopUps, showPriority, id);
        }

        //往某个弹窗上加一个子弹窗：
        public GameObject AddPopupInParent(string prefabName, GameObject parent, string id = "")
        {
            if (parent == null)
            {
                Lucky.LuckyUtils.Log("AddPopupInParent, parent is null");
                return null;
            }

            return AddPopUpInParent(prefabName, parent, 0, id);
        }

        [Obsolete("过时的方法")]
        public GameObject AddPopUpWithBlur(string prefabName)
        {
            return AddPopLayerPopUp(prefabName);
        }

        public GameObject AddPopupInNewCanvas(string prefabName)
        {
            //jony add
            //GameObject prefab = MySceneManager.instance.getPreloadedGameObject(prefabName);
            GameObject prefab = null;
            if (prefab == null)
            {
                prefab = (GameObject)Resources.Load(prefabName);
                if (prefab == null)
                {
                    Lucky.LuckyUtils.Log("AddPopupInNewCanvas, Cannot find prefab!");
                    return null;
                }
            }
            //add end

            Canvas canvas = null;
            GameObject popUp = null;

            //创建PopUp
            popUp = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
            //CreateStoreCanvas：获取对应的canvas:如果有则直接取值，如果没有则会执行创建过程
            canvas = CreatePopCanvas(prefab.name);

            //设置popUp位置：居中显示在屏幕上
            popUp.transform.SetParent(canvas.transform, false);
            popUp.transform.localPosition = Vector3.zero;
            popUp.transform.localScale = Vector3.one;
            (popUp.transform as RectTransform).SetAsLastSibling();

            BaseSceneEaseInOut baseEIO = popUp.GetComponent<BaseSceneEaseInOut>();

            if (baseEIO != null)
            {
                baseEIO.clickBlankAutoClose = true;
            }

            return popUp;
        }

        public void RemovePopupAndParentCanvas(BaseSceneEaseInOut panel)
        {
            if (panel == null)
                return;

            panel.Dispose();
            Canvas canvas = panel.transform.parent.GetComponent<Canvas>();
            if(canvas != null)
                GameObject.Destroy(canvas.gameObject);
        }

        private Canvas CreatePopCanvas(string popName)
        {
            GameObject tStandardCanvs = GameObject.Find("Canvas");//用来赋值

            //依据不同的层来确定Canvas的名称
            string strCanvasName = popName + "Canvas";

            //依据名称去查找Canvas是否已经被创建了，如果没有被创建，则创建之
            GameObject tPopCanvs = GameObject.Find(strCanvasName);
            if (tPopCanvs == null)
            {
                //创建Canvas并设置摄像机信息，Layer信息，位置信息，RenderMode信息，planeDistance信息，scaler信息
                tPopCanvs = new GameObject();
                Canvas tCanvas = tPopCanvs.AddComponent<Canvas>();
                tPopCanvs.name = strCanvasName;

                tCanvas.sortingLayerName = "Share";  //放在Share层上面，刚好在主canvas和UIPOP之间
                tCanvas.worldCamera = GameObject.Find("UICamera").GetComponent<Camera>();
                tCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                // added by jackmo at 2016-12-22，美术要求
                tCanvas.planeDistance = 400;
                // added end

                CanvasScaler tCanvasScaler = tPopCanvs.AddComponent<CanvasScaler>();
                tCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                tCanvasScaler.referenceResolution = tStandardCanvs.GetComponent<CanvasScaler>().referenceResolution;
                tCanvasScaler.matchWidthOrHeight = tStandardCanvs.GetComponent<CanvasScaler>().matchWidthOrHeight;

                tPopCanvs.AddComponent<GraphicRaycaster>();
            }

            return tPopCanvs.GetComponent<Canvas>();
        }
        #endregion

        #region remove
        //移除所有的弹窗
        public void RemoveAllPopupPanel()
        {
            //移除所有的弹窗
            //要把uiLayerPopUps队列以及popLayerPopUps队列里面的所有弹窗都删掉，并且删掉这些弹窗的所有子弹窗
            //网络错误的弹窗不要自动移除掉

            List<GameObject> objs = new List<GameObject>();
            //uiLayerPopUps弹窗组件以及这些弹窗组件的子弹窗
            for (int idx = 0; idx < uiLayerPopUps.Count; ++idx)
            {
                if (!CanClearDialog(uiLayerPopUps[idx].strId))
                {
                    Lucky.LuckyUtils.Log("Try Remove All uiLayerPopUps stop dialogId:" + uiLayerPopUps[idx].strId);
                    return;
                }

                objs.Add(uiLayerPopUps[idx].gameObj);

                for (int idx2 = 0; idx2 < uiLayerPopUps[idx].childObjs.Count; ++idx2)
                {
                    objs.Add(uiLayerPopUps[idx].childObjs[idx2].gameObj);
                }
            }
            //popLayerPopUps弹窗组件以及这些弹窗组件的子弹窗
            for (int idx = 0; idx < popLayerPopUps.Count; ++idx)
            {
                if (!CanClearDialog(popLayerPopUps[idx].strId))
                {
                    Lucky.LuckyUtils.Log("Try Remove All popLayerPopUps stop dialogId:" + popLayerPopUps[idx].strId);
                    return;
                }

                objs.Add(popLayerPopUps[idx].gameObj);

                for (int idx2 = 0; idx2 < popLayerPopUps[idx].childObjs.Count; ++idx2)
                {
                    objs.Add(popLayerPopUps[idx].childObjs[idx2].gameObj);
                }
            }

            //销毁掉所有弹窗
            for (int idx = 0; idx < objs.Count; ++idx)
            {
                Lucky.LuckyUtils.Log("Destroy !!!" + objs[idx].gameObject.name);
                GameObject.Destroy(objs[idx]);
            }
            //队列数据清理
            uiLayerPopUps.Clear();
            popLayerPopUps.Clear();
        }

        /// <summary>
        /// 返回找到的第一个popup
        /// </summary>
        /// <param name="strId"></param>
        /// <returns></returns>
        public GameObject getPopup(string strId)
        {
            //找到所有ID为StrId的弹窗
            List<GameObject> objs = new List<GameObject>();
            //在uiLayerPopUps队列以及uiLayerPopUps里面GameObj的子弹窗里面找
            for (int idx = 0; idx < uiLayerPopUps.Count; ++idx)
            {
                if (uiLayerPopUps[idx].strId == strId)
                {
                    objs.Add(uiLayerPopUps[idx].gameObj);

                    for (int idx2 = 0; idx2 < uiLayerPopUps[idx].childObjs.Count; ++idx2)
                    {
                        if (uiLayerPopUps[idx].childObjs[idx2].strId == strId)
                        {
                            objs.Add(uiLayerPopUps[idx].childObjs[idx2].gameObj);
                        }
                    }
                }
            }

            //在popLayerPopUps队列以及popLayerPopUps里面GameObj的子弹窗里面找
            for (int idx = 0; idx < popLayerPopUps.Count; ++idx)
            {
                if (popLayerPopUps[idx].strId == strId)
                {
                    objs.Add(popLayerPopUps[idx].gameObj);

                    for (int idx2 = 0; idx2 < popLayerPopUps[idx].childObjs.Count; ++idx2)
                    {
                        if (popLayerPopUps[idx].childObjs[idx2].strId == strId)
                        {
                            objs.Add(popLayerPopUps[idx].childObjs[idx2].gameObj);
                        }
                    }
                }
            }

            return objs.Count > 0 ? objs[0] : null;
        }

        //找到所有ID为StrId的弹窗，并将之销毁
        public bool RemovePopup(string strId)
        {
            if (connectFailedDialogId == strId)
            {
                Lucky.LuckyUtils.Log("Try Remove connectFailedDialog,Failed!");
                return false;
            }

            //找到所有ID为StrId的弹窗，并将之销毁
            List<GameObject> objs = new List<GameObject>();
            //在uiLayerPopUps队列以及uiLayerPopUps里面GameObj的子弹窗里面找
            for (int idx = 0; idx < uiLayerPopUps.Count; ++idx)
            {
                if (uiLayerPopUps[idx].strId == strId)
                {
                    objs.Add(uiLayerPopUps[idx].gameObj);

                    for (int idx2 = 0; idx2 < uiLayerPopUps[idx].childObjs.Count; ++idx2)
                    {
                        if (uiLayerPopUps[idx].childObjs[idx2].strId == strId)
                        {
                            objs.Add(uiLayerPopUps[idx].childObjs[idx2].gameObj);
                        }
                    }
                }
            }

            //在popLayerPopUps队列以及popLayerPopUps里面GameObj的子弹窗里面找
            for (int idx = 0; idx < popLayerPopUps.Count; ++idx)
            {
                if (popLayerPopUps[idx].strId == strId)
                {
                    objs.Add(popLayerPopUps[idx].gameObj);

                    for (int idx2 = 0; idx2 < popLayerPopUps[idx].childObjs.Count; ++idx2)
                    {
                        if (popLayerPopUps[idx].childObjs[idx2].strId == strId)
                        {
                            objs.Add(popLayerPopUps[idx].childObjs[idx2].gameObj);
                        }
                    }
                }
            }

            if (objs.Count == 0)
            {
                return false;
            }

            //销毁掉所有的这些弹窗
            for (int idx = 0; idx < objs.Count; ++idx)
            {
                BaseSceneEaseInOut baseScene = objs[idx].GetComponent<BaseSceneEaseInOut>();
                if (baseScene != null)
                {
                    baseScene.Dispose();
                }
            }

            return true;
        }

        #endregion


        #region public func
        public void Update()
        {
            if (needCheckToShowPopup)
            {
                //从两个队列里取优先级最高的一个弹窗，并将之弹出来
                CheckToShowPopups();
                needCheckToShowPopup = false;
            }
        }

        public bool isPopWindowExist(string id)
        {
            foreach (var item in uiLayerPopUps)
            {
                if (item.strId == id)
                    return true;
            }

            foreach (var item in popLayerPopUps)
            {
                if (item.strId == id)
                    return true;
            }
            return false;
        }

        //三个参数是否 点击空白消失，是否点击back键消失，是否阻塞住back键事件
        public bool SetPopupPanelAutoClose(GameObject gameObj, bool blankAutoClose = true, bool backKeyAutoClose = true, bool blockBackKeyEvent = true)
        {
            if (gameObj == null)
            {
                return false;
            }

            BaseSceneEaseInOut baseEIO = gameObj.GetComponent<BaseSceneEaseInOut>();

            if (baseEIO != null)
            {
                baseEIO.clickBlankAutoClose = blankAutoClose;
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region private func


        //增加一个新的弹窗：prefabName：预设路径，canvasLayer：指定是在UI层还是Pop层
        //insertInfo：如果是ui层，则是uiLayerPopUps，如果是Pop层，则是popLayerPopUps
        //showPriority：显示优先级
        private GameObject AddPopupNew(string prefabName, PopCanvasLayer canvasLayer, ref List<PopupInfo> insertInfo, int showPriority, string id = "")
        {
            //jony add
            GameObject prefab = PrefabManager.Instance.GetPrefabs(prefabName);
            if (prefab == null)
            {
                Lucky.LuckyUtils.Log("No preload, use resouce.load:" + prefabName);
                prefab = (GameObject)Resources.Load(prefabName);
                if (prefab == null)
                {
                    Lucky.LuckyUtils.Log("AddPopup, Cannot find prefab!");
                    return null;
                }

            }
            //add end


            Canvas canvas = null;
            GameObject popUp = null;

            //创建PopUp
            popUp = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;

            // added by jackmo at 2017-2-6，扫描bug，popUp判空
            if (popUp == null)
            {
                Lucky.LuckyUtils.Log("PopupManager, AddPopupNew, Cannot Instantiate the prefab!");
                return null;
            }
            // added end

            //GetCanvasByLayer：获取对应的canvas：UICanvas or PopCanvas：如果有则直接取值，如果没有则会执行创建过程
            canvas = GetCanvasByLayer(canvasLayer);

            //设置popUp位置：居中显示在屏幕上
            popUp.transform.SetParent(canvas.transform, false);
            popUp.transform.localPosition = Vector3.zero;
            popUp.transform.localScale = Vector3.one;
            (popUp.transform as RectTransform).SetAsLastSibling();

            //给所有的物件都挂上DestroyEventUI：该脚本的OnDestory函数里面会通知到PopUpMgr.OnDestroy
            //所以实现了PopUpMgr监听弹窗的销毁事件
            DestroyEventUI destroyUi = popUp.GetComponent<DestroyEventUI>();
            if (destroyUi == null)
            {
                destroyUi = popUp.AddComponent<DestroyEventUI>();
            }
            destroyUi.onDestroy += OnDestroy;

            //创建popInfo，准备插入到显示队列里面去
            PopupInfo popInfo = new PopupInfo();
            popInfo.gameObj = popUp;
            popInfo.showPriority = showPriority;
            if (id != null && !id.Equals(""))
            {
                popInfo.strId = id;
            }
            else
            {
                popInfo.strId = prefabName;
            }


            //插入过程：根据showPriority从队列尾部遍历插入
            bool insertSuc = false;
            for (int idx = insertInfo.Count - 1; idx >= 0; --idx)
            {
                if (insertInfo[idx].showPriority <= showPriority)
                {
                    insertInfo.Insert(idx + 1, popInfo);
                    insertSuc = true;
                    break;
                }
            }
            //没找到合适的位置插入，说明该弹窗的showPriority是最小的，所以往顶部插
            if (!insertSuc)
            {
                insertInfo.Insert(0, popInfo);
            }

            //刚插入进去，等下一帧update里面会执行checkshow逻辑，如果该弹窗优先级是最高的，则她会在下一帧弹出来
            popUp.GetComponent<BaseScene>().SetVisable(false);
            needCheckToShowPopup = true;

            return popUp;
        }

        //往一个弹窗上面挂载子弹窗
        //prefabName:预设名称，parent：所需要挂载的母弹窗的gameObj，showPriority：显示优先级，id：id
        private GameObject AddPopUpInParent(string prefabName, GameObject parent, int showPriority, string id = "")
        {
            //判定预设是否存在，否则返回null
            GameObject prefab = (GameObject)Resources.Load(prefabName);

            if (prefab == null)
            {
                Lucky.LuckyUtils.Log("AddPopup, Cannot find prefab!");
                return null;
            }

            Canvas canvas = null;
            GameObject popUp = null;

            //创建PopupInfo对象，准备插入到母弹窗的childObjs里面
            PopupInfo popInfo = new PopupInfo();
            popInfo.showPriority = showPriority;
            popInfo.strId = id;

            //母弹窗的相关信息：需要从uiLayerPopUps和popLayerPopUps两个队列里面去找
            PopupInfo parentPopUpInfo = null;

            //从uiLayerPopUps队列里面去找母弹窗信息
            for (int idx = 0; idx < uiLayerPopUps.Count; ++idx)
            {
                if (uiLayerPopUps[idx].gameObj == parent)
                {
                    parentPopUpInfo = uiLayerPopUps[idx];
                }
            }

            //如果uiLayerPopUps里面没找到，则再去popLayerPopUps队列里面去找母弹窗信息
            if (parentPopUpInfo == null)
            {
                for (int idx = 0; idx < popLayerPopUps.Count; ++idx)
                {
                    if (popLayerPopUps[idx].gameObj == parent)
                    {
                        parentPopUpInfo = popLayerPopUps[idx];
                    }
                }
            }

            //uiLayerPopUps和popLayerPopUps队列里面都没有找到，则输出错误日志并返回
            if (parentPopUpInfo == null)
            {
                Lucky.LuckyUtils.Log("Show Pop in paretn, can not find parent in popups!");
                return null;
            }

            //获取母弹窗父节点的canvas信息，用来设定子弹窗的一些位置信息
            canvas = parentPopUpInfo.gameObj.transform.parent.gameObject.GetComponent<Canvas>();
            if (canvas == null)
            {
                //母弹窗不是挂在canvas上的？输出错误日志并返回
                Lucky.LuckyUtils.Log("AddPopupInParent, parent father is not Canvas!");
                return null;
            }

            //母弹窗存在，且母弹窗的父节点canvas也存在，则创建子窗口组件
            popUp = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;

            //j将子窗口插入到母窗口的childObjs队列里面
            popInfo.gameObj = popUp;
            parentPopUpInfo.childObjs.Add(popInfo);

            //设置子窗口组件的位置信息
            popUp.transform.SetParent(canvas.transform, false);
            popUp.transform.localPosition = Vector3.zero;
            popUp.transform.localScale = Vector3.one;
            (popUp.transform as RectTransform).SetAsLastSibling();

            //给所有的物件都挂上DestroyEventUI：该脚本的OnDestory函数里面会通知到PopUpMgr.OnDestroy
            //所以实现了PopUpMgr监听弹窗的销毁事件
            DestroyEventUI destroyUi = popUp.GetComponent<DestroyEventUI>();
            if (destroyUi == null)
            {
                destroyUi = popUp.AddComponent<DestroyEventUI>();
            }
            destroyUi.onDestroy += OnDestroy;

            //刚插入进去，等下一帧update里面会执行checkshow逻辑，如果该弹窗优先级是最高的，则她会在下一帧弹出来
            popUp.GetComponent<BaseScene>().SetVisable(false);
            needCheckToShowPopup = true;
            return popUp;
        }

        //获取对应的canvas：UICanvas or PopCanvas：如果有则直接取值，如果没有则会执行创建过程
        private Canvas GetCanvasByLayer(PopCanvasLayer canvasLayer)
        {
            GameObject tStandardCanvs = GameObject.Find("Canvas");//用来赋值

            //依据不同的层来确定Canvas的名称
            string strCanvasName = canvasLayer == PopCanvasLayer.E_UILayer ? "UICanvas" : "PopCanvas";

            //依据名称去查找Canvas是否已经被创建了，如果没有被创建，则创建之
            GameObject tPopCanvs = GameObject.Find(strCanvasName);
            if (tPopCanvs == null)
            {
                //创建Canvas并设置摄像机信息，Layer信息，位置信息，RenderMode信息，planeDistance信息，scaler信息
                tPopCanvs = new GameObject();
                Canvas tCanvas = tPopCanvs.AddComponent<Canvas>();
                tPopCanvs.name = strCanvasName;

                tCanvas.sortingLayerName = canvasLayer == PopCanvasLayer.E_UILayer ? "UIPOP" : "pop";
                tCanvas.worldCamera = GameObject.Find("UICamera").GetComponent<Camera>();
                tCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                // added by jackmo at 2016-12-22，美术要求
                tCanvas.planeDistance = 400;
                // added end

                if (tStandardCanvs != null)
                {
                    CanvasScaler tCanvasScaler = tPopCanvs.AddComponent<CanvasScaler>();
                    tCanvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    tCanvasScaler.referenceResolution = tStandardCanvs.GetComponent<CanvasScaler>().referenceResolution;
                    tCanvasScaler.matchWidthOrHeight = tStandardCanvs.GetComponent<CanvasScaler>().matchWidthOrHeight;
                }

                tPopCanvs.AddComponent<GraphicRaycaster>();
            }

            return tPopCanvs.GetComponent<Canvas>();
        }

        //从popLayerPopUps和uiLayerPopUps里面取出一个优先级最高的弹窗，并展示出来
        private void CheckToShowPopups()
        {
            //先遍历popLayerPopUps的，因为之前插入已经是依据优先级牌序插入的，所以从尾部遍历就ok了
            if (popLayerPopUps.Count > 0)
            {
                if (currentShowPopupInfo != null)
                {
                    if (currentShowPopupInfo == popLayerPopUps[popLayerPopUps.Count - 1])
                    {
                        //尽管当前弹窗就是优先级最高的弹窗，但可能该弹窗被动态插入了子窗口，所以还是要setvisible true一下，用来把子窗口显示出来
                        currentShowPopupInfo.SetPopupInfoVisible(true);

                        return;
                    }
                }

                //当前窗口不是优先级最高的弹窗，则把当前窗口隐藏，并显示优先级最高的弹窗
                if (currentShowPopupInfo != null)
                {
                    currentShowPopupInfo.SetPopupInfoVisible(false);
                }

                currentShowPopupInfo = popLayerPopUps[popLayerPopUps.Count - 1];
                currentShowPopupInfo.SetPopupInfoVisible(true);

                return;
            }

            //遍历uiLayerPopUps的：逻辑同上面的popLayerPopUps遍历方法
            if (uiLayerPopUps.Count > 0)
            {
                if (currentShowPopupInfo != null)
                {
                    if (currentShowPopupInfo == uiLayerPopUps[uiLayerPopUps.Count - 1])
                    {
                        currentShowPopupInfo.SetPopupInfoVisible(true);
                        return;
                    }
                }

                if (currentShowPopupInfo != null)
                {
                    currentShowPopupInfo.SetPopupInfoVisible(false);
                }
                currentShowPopupInfo = uiLayerPopUps[uiLayerPopUps.Count - 1];
                currentShowPopupInfo.SetPopupInfoVisible(true);

                return;
            }

            //popLayerPopUps和uiLayerPopUps里面都没有东西，则什么都不展示，并置currentShowPopupInfo为null
            currentShowPopupInfo = null;
        }

        private bool CanClearDialog(string dialogId)
        {
            bool ret = true;
            if (dialogId == connectFailedDialogId || dialogId == noRemoveGameNoticeId
                || dialogId == FriendRoomAddChipPanel || dialogId == FRMessageCenterPanel)
            {
                ret = false;
            }
            return ret;
        }

        private void OnDestroy(GameObject gameObj)
        {
            do
            {
                //找到该弹窗，并把该弹窗的信息从相应的队列里面移除，并置标记位，让下一帧去重新检查显示新窗口
                bool hasFind = false;
                //先从uiLayerPopUps里面找
                for (int idx = 0; idx < uiLayerPopUps.Count; ++idx)
                {
                    if (uiLayerPopUps[idx].gameObj == gameObj)
                    {
                        uiLayerPopUps.RemoveAt(idx);
                        hasFind = true;
                        break;
                    }
                }

                //如果已经在uiLayerPopUps里面找到，则跳出循环
                if (hasFind)
                {
                    break;
                }

                //从popLayerPopUps里面找
                for (int idx = 0; idx < popLayerPopUps.Count; ++idx)
                {
                    if (popLayerPopUps[idx].gameObj == gameObj)
                    {
                        popLayerPopUps.RemoveAt(idx);
                        hasFind = true;
                        break;
                    }
                }

                //如果已经在popLayerPopUps里面找到，则跳出循环
                if (hasFind)
                {
                    break;
                }

                //不在popLayerPopUps里面也不在uiLayerPopUps，说明可能是一个子窗口
                //从uiLayerPopUps所有的窗口里面的子窗口里面去找
                for (int idx = 0; idx < uiLayerPopUps.Count && !hasFind; ++idx)
                {
                    for (int idx2 = 0; idx2 < uiLayerPopUps[idx].childObjs.Count; ++idx2)
                    {
                        if (uiLayerPopUps[idx].childObjs[idx2].gameObj == gameObj)
                        {
                            uiLayerPopUps[idx].childObjs.RemoveAt(idx2);
                            hasFind = true;
                            break;
                        }
                    }
                }

                //
                if (hasFind)
                {
                    break;
                }

                //不在popLayerPopUps里面也不在uiLayerPopUps，说明可能是一个子窗口
                //从popLayerPopUps所有的窗口里面的子窗口里面去找
                for (int idx = 0; idx < popLayerPopUps.Count && !hasFind; ++idx)
                {
                    for (int idx2 = 0; idx2 < popLayerPopUps[idx].childObjs.Count; ++idx2)
                    {
                        if (popLayerPopUps[idx].childObjs[idx2].gameObj == gameObj)
                        {
                            popLayerPopUps[idx].childObjs.RemoveAt(idx2);
                            hasFind = true;
                            break;
                        }
                    }
                }

                if (hasFind)
                {
                    break;
                }

                Lucky.LuckyUtils.Log("Popup On Dialog destroy, Can not find Dialog!");
            } while (false);

            if (currentShowPopupInfo != null)
            {
                if (currentShowPopupInfo.gameObj == gameObj)
                {
                    currentShowPopupInfo = null;
                }
            }

            needCheckToShowPopup = true;
        }
        #endregion

    }

    
}
