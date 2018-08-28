using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lucky
{
    /// <summary>
    /// 生涯视图基类（数据、手牌、位置、战绩等）
    /// </summary>
    /*
    class CareerView : BaseScene
    {
        //拥有父页卡的ID
        public int OwnerPageID
        {
            get;
            set;
        }

        public CareerPage Owner
        {
            get;
            set;
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 获取PageID 常规场-锦标赛-好友赛事 （0-1-2）
        /// </summary>
        public int getCareerViewBeyondPageID()
        {
            int currentPageID = -1;
            
            CareerPage page = gameObject.transform.parent.parent.GetComponent<CareerPage>();
            if (page != null)
            {
                currentPageID = page.getPageID();
                if (currentPageID == -1)
                {
                    TKLog.Error("CareerView checkDataIsCorrect getPageID error!");
                }
            }
            return currentPageID;
        }
    }
    */
}
