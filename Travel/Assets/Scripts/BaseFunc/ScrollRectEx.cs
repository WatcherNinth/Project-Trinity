using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Lucky
{
    public class ScrollRectEx : ScrollRect
    {
        //下拉刷新：
        public interface IScrollViewEndDragEvent
        {
            void OnEndDragEvent(PointerEventData eventData);
        }

        private IScrollViewEndDragEvent scrollViewEndDragEventListener;

        public void SetDragOverEventListener(IScrollViewEndDragEvent listener)
        {
            scrollViewEndDragEventListener = listener;
        }

        public void RemoveDragOverEventListener()
        {
            scrollViewEndDragEventListener = null;
        }

        //反编译后发现原来代码是
        /*if (eventData.button != PointerEventData.InputButton.Left)
        {
            return;
        }
        this.m_Dragging = false;*/

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);

            if (eventData.button != PointerEventData.InputButton.Left)
            {
                //drag状态并没有发生改变，不做任何判断：
                return;
            }

            if (scrollViewEndDragEventListener != null)
            {
                scrollViewEndDragEventListener.OnEndDragEvent(eventData);
            }
        }
    }
}