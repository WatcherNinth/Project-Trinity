using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;


namespace Lucky
{

    public class BaseGrid : BaseUI, ScrollRectEx.IScrollViewEndDragEvent
    {
        public delegate void RefreshDataHandler();
        public delegate void RefreshAllDataHandler();      //拉到最顶部后的那个  下拉刷新：
        public delegate void ListViewUpdate(List<ItemRender> items);

        public event RefreshDataHandler onRefreshData;

        public event ListViewUpdate OnViewUpdate;

        public event RefreshAllDataHandler onRefreshAllData;   //拉到最顶部后的那个  下拉刷新：

        public ScrollRect scrollRect;  
        [SerializeField]
        protected int m_ItemWidth = 100;
        public int ItemWidth { get { return m_ItemWidth; } set { m_ItemWidth = value; } }
        [SerializeField]
        protected int m_ItemHeight = 100;
        public int ItemHeight { get { return m_ItemHeight; } set { m_ItemHeight = value; } }


        [SerializeField]
        protected int m_RowGap = 1;
        public int RowGap { get { return m_RowGap; } set { m_RowGap = value; } }
        [SerializeField]
        protected int m_ColGap = 1;
        public int ColGap { get { return m_ColGap; } set { m_ColGap = value; } }
        [SerializeField]
        protected int m_ViewWidth = 750;
        public int ViewWidth { get { return m_ViewWidth; } set { m_ViewWidth = value; } }
        [SerializeField]
        protected int m_ViewHeight = 1334;

        [SerializeField]
        protected RectTransform m_ViewPort;

        public int ViewHeihgt { get { return m_ViewHeight; } set { m_ViewHeight = value; } }

        //初始位置偏移量
        public int OffSetTop = 0;
        //底部扩展偏移量
        public int OffSetBottom = 0;
        public bool showRefreshGO = false;
        protected object[] _source;

        /// <summary>
        /// 上下各缓存多少页
        /// </summary>
        public int mPageCounts = 0;

        protected PoolObjectManager _poolManager;
        protected RectTransform _rectTransform;
        protected LayoutElement _layoutElement;
        protected List<ItemRender> _itemChildren;

        protected int actualOffsetTop = 0;
        protected int actualOffsetBottom = 0;

        protected bool _createChildComplete = false;

        protected int _moveIndex = -1;

        protected bool _busyGO = false;
        
        protected override void Awake()
        {
            base.Awake();
            _rectTransform = GetComponent<RectTransform>();
            _layoutElement = GetComponent<LayoutElement>();
            _poolManager = GetComponent<PoolObjectManager>();

        }

        protected override void Start()
        {
            base.Start();

            if (m_ViewPort != null)
            {
                //设置滚动视口区域高宽
                Vector2 size = m_ViewPort.rect.size;
                ViewWidth = (int)size.x;
                ViewHeihgt = (int)size.y;
            }

            actualOffsetTop = OffSetTop;
            actualOffsetBottom = OffSetBottom;


            if (scrollRect != null)
            {
                ScrollRectEx scrollRectEx = scrollRect.gameObject.GetComponent<ScrollRectEx>();
                if (scrollRectEx != null)
                {
                    scrollRectEx.SetDragOverEventListener(this);
                }
            }
        }

        public void OnEndDragEvent(UnityEngine.EventSystems.PointerEventData eventData)
        {
            //Debug.LogError("OnEndDragEvent！");
            //目前只处理垂直的scrollview，水平的暂时没有类似需求，不考虑：
            if (scrollRect != null && scrollRect.vertical)
            {
               // Debug.LogError("OnEndDragEvent！SizeDeltay:" + _rectTransform.sizeDelta.y);
                if (_rectTransform.localPosition.y < -70.0f)
                {
                    //Need RefreshData!!!
                    //Debug.LogError("触发下拉刷新！");
                    if (onRefreshAllData != null)
                    {
                        onRefreshAllData();
                    }
                }
            }
        }

        public void setPoolObjectManager(PoolObjectManager tPoolObjectManager)
        {
            _poolManager = tPoolObjectManager;
        }

        protected bool needUpdateListView = false;


        public object[] source
        {
            get
            {
                return _source;
            }
            set
            {
                _source = value;
                _createChildComplete = false;
            //    UpdateListView();
            //     needUpdateListView = true;
                InvalidView();
            }
        }

        // added by jackmo at 2016-12-15
        /// <summary>
        /// _source中的某一项值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public ItemRender getItem(object value)
        {
            ItemRender item = null;
            if (_itemChildren != null)
            {
                for (var i = 0; i < _itemChildren.Count; ++i)
                {
                    if (_itemChildren[i] != null && _itemChildren[i].data == value)
                    {
                        item = _itemChildren[i];
                    }
                }
            }
            return item;
        }
        // added end


        protected override void LateUpdate()
        {
            base.LateUpdate();
            UpdatePos();
            ReFreshData();
        }

        protected override void UpdateView()
        {
            base.UpdateView();


            if (_source == null || _source.Length == 0)
            {
                if (_itemChildren != null)
                {
                    for (int i = 0; i < _itemChildren.Count; i++)
                    {
                        if (_itemChildren[i] != null)
                        {
                            Destroy(_itemChildren[i].gameObject);
                        }
                    }
                }
                _itemChildren = null;
                _poolManager.ClearPool();
                return;
            }

            if (_busyGO) 
            {
                _busyGO = false;
            }
//             if (busyGO != null)
//             {
//                 Destroy(busyGO);
//                 busyGO = null; 
//             }

            if (_itemChildren != null)
            {
                for (int i = 0; i < _itemChildren.Count; ++i)
                {
                    if (_itemChildren[i] != null)
                    {
                        _itemChildren[i].gameObject.SetActive(false);
                        _itemChildren[i] = null;
                    }
                }
            }

            if (_source != null)
            {
                if (_itemChildren == null)
                {
                    _itemChildren = new List<ItemRender>();
                }
                if (scrollRect == null)
                {
                    _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, ItemHeight);
                    _rectTransform.offsetMin = new Vector2(0, _rectTransform.offsetMin.y);
                    _rectTransform.offsetMax = new Vector2(0, _rectTransform.offsetMax.y);
                    SetLayoutElement();
                }
                else
                {
                    if (scrollRect.horizontal && !scrollRect.vertical)//水平方向滑动的物体
                    {
                        _rectTransform.sizeDelta = new Vector2(ItemWidth, _rectTransform.sizeDelta.y);
                        _rectTransform.offsetMin = new Vector2(_rectTransform.offsetMin.x, 0);
                        _rectTransform.offsetMax = new Vector2(_rectTransform.offsetMax.x, 0);
                        SetLayoutElement();
                    }
                    else if (scrollRect.vertical && !scrollRect.horizontal)
                    {
                        _rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, ItemHeight);
                        _rectTransform.offsetMin = new Vector2(0, _rectTransform.offsetMin.y);
                        _rectTransform.offsetMax = new Vector2(0, _rectTransform.offsetMax.y);
                        SetLayoutElement();
                    }
                }
                InitListView();

            }
            
        }

        protected virtual void InitListView()
        {
            float sizeDeltaY = 0;
            float sizeDeltaX = 0;
            for (int i = 0; i < _source.Length; i++)
            {
                if (scrollRect == null || scrollRect.vertical)
                {
                    if (sizeDeltaY > ViewHeihgt * (mPageCounts + 1))
                    {
                        if (_itemChildren.Count <= i)
                        {
                            _itemChildren.Add(null);
                        }
                        else
                        {
                            _itemChildren[i] = null;
                        }
                    }
                    else
                    {
                        if (_itemChildren.Count <= i)
                        {
                            _itemChildren.Add(CreateItemRender(_source[i], i));
                        }
                        else
                        {
                            if (_itemChildren[i] == null)
                            {
                                ItemRender tmpRender = CreateItemRender(_source[i], i);
                                _itemChildren[i] = tmpRender;
                            }
                            else
                            {
                                ResetItemRender(_itemChildren[i], i, _source[i]);
                            }


                        }
                    }
                    sizeDeltaY = (i + 1) * (ItemHeight + m_RowGap) + actualOffsetTop + actualOffsetBottom;
                }
                else if (scrollRect.horizontal)
                {
                    if (sizeDeltaX > ViewWidth * (mPageCounts + 1))
                    {
                        if (_itemChildren.Count <= i)
                        {
                            _itemChildren.Add(null);
                        }
                        else
                        {
                            _itemChildren[i] = null;
                        }
                    }
                    else
                    {
                        if (_itemChildren.Count <= i)
                        {
                            _itemChildren.Add(CreateItemRender(_source[i], i));
                        }
                        else
                        {
                            if (_itemChildren[i] == null)
                            {
                                ItemRender tmpRender = CreateItemRender(_source[i], i);
                                _itemChildren[i] = tmpRender;
                            }
                            else
                            {
                                ResetItemRender(_itemChildren[i], i, _source[i]);
                            }

                        }
                    }
                    sizeDeltaX = (i + 1) * (ItemWidth + m_ColGap) + actualOffsetTop + actualOffsetBottom;
                }
            }
            while (_itemChildren.Count > _source.Length)
            {
                ItemRender restItem = _itemChildren[_source.Length];
                _itemChildren.RemoveAt(_source.Length);
                if (restItem != null)
                {
                    _poolManager.DestroyItem(restItem.gameObject);

                }
            }
            preY = _rectTransform.localPosition.y;
            preX = _rectTransform.localPosition.x;
            _createChildComplete = true;
           
            if (scrollRect == null || scrollRect.vertical)
            {
                _rectTransform.sizeDelta = new Vector2(ViewWidth, sizeDeltaY);
                _rectTransform.offsetMin = new Vector2(0, _rectTransform.offsetMin.y);
                _rectTransform.offsetMax = new Vector2(0, _rectTransform.offsetMax.y);
                SetLayoutElement();
                MoveToIndex();
                AdjustmentItemActiveVertical();
                
            }
            else if (scrollRect.horizontal)
            {
                _rectTransform.sizeDelta = new Vector2(sizeDeltaX, ViewWidth);
                _rectTransform.offsetMin = new Vector2(_rectTransform.offsetMin.x, 0);
                _rectTransform.offsetMax = new Vector2(_rectTransform.offsetMax.x, 0);
                SetLayoutElement();
                MoveToIndex();
                AdjusetmentItemActiveHorizontal();
            }
        }

        protected virtual ItemRender CreateItemRender(object itemData, int index)
        {
            GameObject itemGO = _poolManager.GetGameObject(gameObject.GetComponent<RectTransform>());
            ItemRender itemRender = null;
            if (itemGO != null)
            {
                itemRender = itemGO.GetComponent<ItemRender>();
                ResetItemRender(itemRender, index, itemData);
            }
            //itemGO.SetActive(true);
            //rt.SetSiblingIndex(index);
            return itemRender;
        }

        protected virtual void ResetItemRender(ItemRender itemRender, int index, object itemData)
        {
            RectTransform rt = itemRender.gameObject.GetComponent<RectTransform>();
            if (scrollRect == null || scrollRect.vertical)
            {
                rt.localPosition = new Vector3(0, -index * (ItemHeight + m_RowGap) - actualOffsetTop, 0);

                rt.offsetMin = new Vector2(0, rt.offsetMin.y);
                rt.offsetMax = new Vector2(0, rt.offsetMax.y);
            }
            else if (scrollRect.horizontal)
            {
                rt.localPosition = new Vector3(index * (ItemWidth + m_ColGap) + actualOffsetTop, 0, 0);
                rt.offsetMin = new Vector2(rt.offsetMin.x, 0);
                rt.offsetMax = new Vector2(rt.offsetMax.x, 0);
            }
            itemRender.index = index;
            itemRender.count = _source.Length;
           
            //itemRender.source = source;
            itemRender.data = itemData;
           
        }
        
        //上一次 scrollContainer的Y轴位置
        protected float preY = 0;
        protected float preX = 0;
        //位置发生变化，拖动的时候交换元素
        protected virtual void UpdatePos()
        {
            if (_source == null || _source.Length == 0)
            {
                return;
            }
            float currX = _rectTransform.localPosition.x;
            float currY = _rectTransform.localPosition.y;
            if (Mathf.Abs(preY - currY) > ViewHeihgt * mPageCounts + 1)
            {
                AdjustmentItemActiveVertical();
                preY = currY;
                
            }

            if (Mathf.Abs(preX - currX) > ViewWidth * mPageCounts +1)
            {
                AdjusetmentItemActiveHorizontal();
                preX = currX;
                
            }
            if (OnViewUpdate != null) OnViewUpdate(_itemChildren);
        }

        protected virtual void AdjustmentItemActiveVertical()
        {
            float currY = _rectTransform.localPosition.y;
            currY -= actualOffsetTop;
            for (int i = 0; i < _itemChildren.Count; i++)
            {
                ItemRender itemRender = _itemChildren[i];
                float itemY = -i * (ItemHeight + m_RowGap);
                if (itemRender == null)
                {

                    if ((currY - ViewHeihgt * (mPageCounts) < Mathf.Abs(itemY) + ItemHeight) && currY + ViewHeihgt * (mPageCounts + 1) > Mathf.Abs(itemY))
                    {
                        itemRender = CreateItemRender(_source[i], i);
                        _itemChildren[i] = itemRender;
                    }
           /*         if ((currY + itemY< 3*ItemHeight) && (currY+itemY) > -ViewHeihgt - 3*ItemHeight)
                    {
                        itemRender = CreateItemRender(_source[i], i);
                        _itemChildren[i] = itemRender;
                    }*/
                    
                }
                else
                {
               /*     if ((currY + itemY) > 3 * ItemHeight || (currY + itemY) < -ViewHeihgt - 3 * ItemHeight)
                    {
                        _itemChildren[i] = null;
                        itemRender.gameObject.SetActive(false);
                    }*/

                    if (currY - ViewHeihgt * mPageCounts > Mathf.Abs(itemY) + ItemHeight)
                    {
                        _itemChildren[i] = null;
                        itemRender.gameObject.SetActive(false);
                    }

                    if (Mathf.Abs(itemY) > ViewHeihgt * (mPageCounts + 1) + currY)
                    {
                        _itemChildren[i] = null;
                        itemRender.gameObject.SetActive(false);
                    }
                }
            }
        }

        protected virtual void AdjusetmentItemActiveHorizontal()
        {
            float currX = _rectTransform.localPosition.x;
            for (int i = 0; i < _itemChildren.Count; i++)
            {
                ItemRender itemRender = _itemChildren[i];
                float itemX = i * (ItemWidth + m_ColGap);
                if (itemRender == null)
                {
                    if ((itemX + currX) > -ItemWidth && (itemX + currX) < ViewWidth + ItemWidth)
                    {
                        itemRender = CreateItemRender(_source[i], i);
                        _itemChildren[i] = itemRender;
                    }
                /*    if (itemX > -(ItemWidth + m_ColGap * 3) && itemX < ViewWidth + (ItemWidth + m_ColGap) * 3)
                    {
                        itemRender = CreateItemRender(_source[i], i);
                        _itemChildren[i] = itemRender;
                    }*/  
                }
                else
                {
                    if ((itemX + currX) <= -ItemWidth || (itemX + currX) >= ViewWidth+ ItemWidth)
                    {
                        _itemChildren[i] = null;
                        itemRender.gameObject.SetActive(false);
                    }
                  /*  if (itemX <= -(ItemWidth + m_ColGap * 3))
                    {
                        _itemChildren[i] = null;
                        itemRender.gameObject.SetActive(false);
                    }
                    else if (itemX >= ViewWidth + (ItemWidth + m_ColGap) * 3)
                    {
                        _itemChildren[i] = null;
                        itemRender.gameObject.SetActive(false);
                    }*/
                    

                }
            }
        }

        //private GameObject busyGO;
        /// <summary>
        /// 是否请求刷数据，指用户滑动到最高点时，出现转圈动画，只会出现一次 已出现就不出来了
        /// </summary>
        protected virtual void ReFreshData()
        {
            if (_source == null || _source.Length == 0)
            {
                return;
            }

            if (_busyGO) return;
            //if (busyGO != null) return;
            if (!showRefreshGO) return;
            float currX = _rectTransform.localPosition.x;
            float currY = _rectTransform.localPosition.y;
            if ( scrollRect == null || scrollRect.vertical )
            {
                if (_rectTransform.sizeDelta.y < ViewHeihgt) return;
                if ((_rectTransform.sizeDelta.y - currY) < ViewHeihgt)
                {
                    _busyGO = true;
                    //_rectTransform.sizeDelta = new Vector2(_rectTransform.sizeDelta.x, _rectTransform.sizeDelta.y + 100);
                    if (onRefreshData != null)
                    {
                        onRefreshData();
                    }
                }
            }
            else if (scrollRect != null && scrollRect.horizontal)
            {
                if (_rectTransform.sizeDelta.x < ViewWidth) return;
                if ((_rectTransform.sizeDelta.x + currX) < ViewWidth)
                {
                    _busyGO = true;
                    
                    if (onRefreshData != null)
                    {
                        onRefreshData();
                    }
                }
            }
     //       if(scrollRect.scrollSensitivity)

        }

        /// <summary>
        /// 移动到指定位置
        /// </summary>
        /// <param name="itemData"></param>
        public void MoveToItem(object itemData)
        {
            if (_source != null)
            {
                for (int i = 0; i < _source.Length; i++)
                {
                    if (_source[i] == itemData)
                    {
                        _moveIndex = i;
                        if (_createChildComplete)
                        {
                            MoveToIndex();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 恢复原始位置
        /// </summary>
        public void ResetPosition()
        {
            if (_rectTransform != null)
            {
                _rectTransform.localPosition = Vector3.zero;
            }
        }

        protected virtual void MoveToIndex()
        {
            if (scrollRect != null && _moveIndex != -1)
            {
                if (scrollRect.vertical)
                {
                    float itemY = -_moveIndex * (ItemHeight + m_RowGap)-OffSetTop;
                    _rectTransform.localPosition = new Vector3(_rectTransform.localPosition.x, Mathf.Abs(itemY), _rectTransform.localPosition.z);
                }else if (scrollRect.horizontal)
                {
                    float itemX = _moveIndex * (ItemWidth + m_ColGap);
                    _rectTransform.localPosition = new Vector3(-itemX, _rectTransform.localPosition.y, _rectTransform.localPosition.z);
                }
                
                _moveIndex = -1;
            }
        }

        

        protected override void OnDestroy()
        {
            _rectTransform = null;
            if (_itemChildren != null)
            {
                _itemChildren.Clear();
                _itemChildren = null;
            }
            _source = null;
            _poolManager = null;
            base.OnDestroy();
        }

        protected void SetLayoutElement()
        {
            if(_layoutElement!=null)
            {
                _layoutElement.preferredHeight = _rectTransform.sizeDelta.y;
                _layoutElement.preferredWidth = _rectTransform.sizeDelta.x;
            }
        }

    }

}

