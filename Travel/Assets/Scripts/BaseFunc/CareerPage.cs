using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
namespace Texas
{
    /// <summary>
    /// 生涯功能分页（常规场、锦标赛、好友赛、牌局回顾）
    /// </summary>
    class CareerPage : MonoBehaviour
    {
        //视图列表（可以是公共模版）
        [SerializeField]
        private string[] mChildViewPrefabs;

        //上方的Tab按钮根节点
        [SerializeField]
        private Transform mTabRoot;

        //上方的Tab按钮根节点
        [SerializeField]
        private Transform mViewRoot;

        private Dictionary<int, CareerView> mChildViewDict = new Dictionary<int, CareerView>();

        //上方的Tab按钮列表
        //[SerializeField]
        private List<Toggle> mTabBtnList = new List<Toggle>();

        //[SerializeField]
        private CareerPageID mPageType;

        //pageID用于区分是哪一个页签 常规场-0 锦标赛-1 好友赛事-2 牌局记录-3
        public int pageID = -1;

        public CareerPanel Owner
        {
            get;
            set;
        }

        void Awake()
        {
            //搜索table按钮列表
            for (int i = 0; i < mTabRoot.childCount; i++)
            {
                var child = mTabRoot.GetChild(i);
                if (child && child.gameObject.activeSelf)
                {
                    var tab = child.GetComponentInChildren<Toggle>();
                    if (tab)
                    {
                        mTabBtnList.Add(tab);
                    }
                }
            }

            //float beg = Time.realtimeSinceStartup;

            //for (int i = 0; i < mViewList.Count; i++)
            //{
            //    //检测模版VIEW
            //    if (mViewList[i].transform.parent != mViewRoot)
            //    {
            //        var tempObj = mViewList[i].gameObject;
            //        //如果没有挂在自己的节点下，那么就是外部引用的模版，直接复制一份，挂在自己节点下面
            //        CareerView view = TKUtils.CreateGameObj<CareerView>(tempObj, transform, true);
            //        if (view != null)
            //        {
            //            RectTransform temp_trans = tempObj.transform as RectTransform;
            //            RectTransform trans = view.transform as RectTransform;

            //            trans.anchorMax = temp_trans.anchorMax;
            //            trans.anchorMin = temp_trans.anchorMin;
            //            trans.offsetMax = temp_trans.offsetMax;
            //            trans.offsetMin = temp_trans.offsetMin;

            //            view.transform.position = tempObj.transform.position;
            //            view.gameObject.SetActive(false);
            //            mViewList[i] = view;
            //        }
            //    }
            //}

            for (int i = 0; i < mTabBtnList.Count; i++)
            {
                var toggle = mTabBtnList[i];
                if (toggle)
                {
                    int index = i;
                    toggle.onValueChanged.AddListener((isOn) =>
                    {
                        OnTabChanged(index, isOn);
                    });
                }
            }

            //TKLog.Error("build page: " + (Time.realtimeSinceStartup - beg));
        }

        void OnTabChanged(int index, bool isOn)
        {
            if (index >= mChildViewPrefabs.Length)
            {
                TKLog.Error("视图下标越界");
                return;
            }

            if (mChildViewPrefabs[index] == null)
            {
                TKLog.Error("没有对应的视图");
                return;
            }

            if (isOn)
                ShowView(index);
            else
                HideView(index);
        }

        void Start()
        {
            //打开第一个分页
            for (int i = 0; i < mTabBtnList.Count; i++)
            {
                if (i == 0)
                {
                    mTabBtnList[i].isOn = true;
                    ShowView(i);
                }
                else
                {
                    mTabBtnList[i].isOn = false;
                }
            }
        }

        private void ShowView(int index)
        {
            var view = GetView(index);
            if (view)
            {
                view.Show();
            }
        }

        private void HideView(int index)
        {
            var view = GetView(index);
            if (view)
            {
                view.Hide();
            }
        }

        private CareerView GetView(int index, bool autoCreate = true)
        {
            CareerView view = null;
            if (mChildViewDict.TryGetValue(index, out view) == false && autoCreate)
            {
                view = TKUtils.CreatePanelFromResource<CareerView>("Texas/Career/" + mChildViewPrefabs[index], mViewRoot);
                if (view)
                {
                    view.OwnerPageID = pageID;
                    view.Owner = this;
                    TKUtils.MakeFullStretch(view.transform);
                    mChildViewDict.Add(index, view);
                }
            }
            return view;
        }

        public void Show()
        {
            gameObject.SetActive(true);

            if (pageID == (int)CareerPageID.FriendPage)
            {
                //好友赛事
                LogReportModel.instance.PushClickData(LogReportModel.ScenesType.e_Career, LogReportModel.ActionType.e_Career_FriendPage);
            }
            else if(pageID == (int)CareerPageID.MatechRecordPage)
            {
                //牌局记录
                LogReportModel.instance.PushClickData(LogReportModel.ScenesType.e_Career, LogReportModel.ActionType.e_Career_MatchRecordPage);
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        //获取当前页签ID
        public int getPageID()
        {
            return pageID;
        }
    }
}
*/
