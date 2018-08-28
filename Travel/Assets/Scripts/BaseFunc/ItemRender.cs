using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Lucky
{
    public class ItemRender : BaseUI
    {

        public delegate void ItemClick(ItemRender item);

        public event ItemClick onItemClick;
        protected object m_Data;

        protected int _index = -1;
        public int index
        {
            get
            {
                return _index;
            }
            set
            {
                //if (_index == value)
                //    return;

                _index = value;
                // added by jackmo 
                //InvalidView();  index和data设置成对出现，这里调用会连续两帧刷新两次。
                // added end
            }
        }
        //数组长度
        public int count { get; set; } //这个有何用途？？？

        public object data
        {
            get
            {
                return m_Data;
            }
            set
            {
                //if (m_Data == value)
                //    return;

                m_Data = value;
                UpdateView();
            }
        }

        private object[] _source;
        public object[] source
        {
            get
            {
                return _source;
            }
            set
            {
                _source = value;
            }
        }
        

        protected object m_param;
        public object param
        {
            get
            {
                return m_param;
            }
            set
            {
                m_param = value;
            }
        }

        protected override void OnDestroy()
        {
            m_Data = null;
            m_param = null;
            _source = null;
            base.OnDestroy();
            
        }
    }
}

