using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Lucky
{

    /// <summary>
    /// 
    ///  所有UI组件类的基类，提供一个延时间刷新的功能
    ///  @author bughuang
    /// 
    /// </summary>
    public class BaseUI : MonoBehaviour
    {
        //*************  属性定义  **************


        protected bool isWaitToUpdate = true;


        //************ 公共方法 *********


        /// <summary>
        /// 一些影响界面的属性变更时可调用该方法，使界面无效，等下一帧就会调用UpdateView刷新，提升性能
        /// </summary>
        public void InvalidView()
        {
            isWaitToUpdate = true;
        }

        public void Refresh()
        {
            isWaitToUpdate = false;
        }

        //******** 子类可重写方法 ********
        /// <summary>
        /// aragorn,Awake先于 start调用，特殊条件下需要使用
        /// </summary>
        protected virtual void Awake()
        {

        }

        /// <summary>
        /// 子类切勿重新定义此方法，可以选择override它，然后Base.Start();
        /// </summary>
        protected virtual void Start()
        {
            InitUI();
        }

        /// <summary>
        /// 子类切勿重新定义此方法，可以选择override它，然后Base.Update();
        /// </summary>
        protected virtual void Update()
        {

        }
        protected virtual void LateUpdate()
        {
            if (isWaitToUpdate)
            {
                isWaitToUpdate = false;
                UpdateView();
                isWaitToUpdate = false;
            }
        }


        /// <summary>
        /// 子类切勿重新定义此方法，可以选择override它，然后Base.OnDestroy();
        /// </summary>
        protected virtual void OnDestroy()
        {

        }

        /// <summary>
        /// UI初始化
        /// </summary>
        protected virtual void InitUI()
        {
        }


        /// <summary>
        /// 界面更新,是延时更新，提升性能
        /// </summary>
        protected virtual void UpdateView()
        {
            Debug.Log("ui update view");
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        




        //*************   私有方法 *************

        //******** UI事件处理 ********

    }

}