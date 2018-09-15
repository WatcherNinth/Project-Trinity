using UnityEngine;
using System.Collections;

namespace Lucky
{ 
    public class BaseScene : BaseUI {

        protected float posY;
        protected bool m_Visable = true;
        protected int quadScreenHeight = 1024 * 4;

        protected override void Start()
        {
            base.Start();
        }

        protected override void InitUI()
        {
            base.InitUI();
        }

        public virtual void SetVisable(bool visible)
        {
            if (!visible && m_Visable)
            {
                Vector3 setPos = new Vector3(gameObject.transform.localPosition.x, quadScreenHeight, gameObject.transform.localPosition.z);
                posY = gameObject.transform.localPosition.y;
                gameObject.transform.localPosition = setPos;
                m_Visable = !m_Visable;

                BlankClickDestroy blk = gameObject.GetComponent<BlankClickDestroy>();
                if (blk != null)
                {
                    blk.SetButtonClickable(visible);
                }
            }
            if (visible && !m_Visable)
            {
                Vector3 setPos = new Vector3(gameObject.transform.localPosition.x, posY, gameObject.transform.localPosition.z);
                gameObject.transform.localPosition = setPos;
                m_Visable = !m_Visable;

                BlankClickDestroy blk = gameObject.GetComponent<BlankClickDestroy>();
                if (blk != null)
                {
                    blk.SetButtonClickable(visible);
                }
            }
        }

        public bool GetIsVisible()
        {
            return m_Visable;
        }

        public void Dispose()
        {

        }

        protected override void UpdateView()
        {
            Lucky.LuckyUtils.Log("scene update view");
            base.UpdateView();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
        }
    }
}
