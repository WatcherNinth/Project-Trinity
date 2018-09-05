using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

namespace Lucky
{
    public enum E_SHOW_PROIORTY
    {
        TOP = 0,
        MIDDLE = 1,
        LOW = 2,
    };

    
    /// <summary>
    /// panel�Ļ����࣬��Ҫ�ṩ������˳���һЩĬ��Ч��
    /// </summary>
    public class BaseSceneEaseInOut : BaseScene
    {
        public Action OnReturnClick;
        public Button returnBtn;
        private CanvasGroup cg;
        protected Canvas parentCanvas;

        public E_SHOW_PROIORTY priority = E_SHOW_PROIORTY.TOP;


        //��������
        private float EffectDisposeTime = 0.1f;
        private float EffectEnterTime = 0.25f;
        private float EffectMin = 0.65f;
        private float EffectDisposeMin = 0.9f;
        private float EffectMax = 1.0f;

        //��������
        public bool destroyOnClose = true;//ѡ��false����Ỻ���GameObject
        public bool stay = false;


        public bool clickBlankAutoClose = false;   //�Ƿ����հ���ʧ��
        public bool changeScaleWhenDispose = true;//disposeʱ���Ƿ񲥷�scale��������

        private bool mNeedPlayPopupSound = true;
        private bool mNeedPlayDisposeSound = true;


        //״̬
        protected bool isDestroyed = false;
        protected bool hasInitUi = false;
        protected bool hasDispose = false;
        private bool hasAddBlurRefenceSuc = false;

        private string parentCanvasName = "";

        private bool mHasCheckToCreateBlurImage = false;
        private int mLateUpdateFrameCount = 0;
        RawImage ri;

        float num;

        protected virtual void Awake()
        {
            cg = GetComponent<CanvasGroup>();
        }

        protected override void Start()
        {
            base.Start();
        }

        public void setHasCheckToCreateBlurImage(bool v)
        {
            this.mHasCheckToCreateBlurImage = v;
        }

        public void SetNeedPlaySound(bool needPlayPopupSound, bool needPlayDisposeSound)
        {
            mNeedPlayPopupSound = needPlayPopupSound;
            mNeedPlayDisposeSound = needPlayDisposeSound;
        }

        [System.Obsolete("��������������½���������Ҫ�ô˺���")]
        public void SetNotDoAniObjectName(string strName)
        {
            //notDoAniObjectName = strName; ���ں�ʱ����ʹ��
        }

        public void SetScale(float scaleValue)
        {
            
            Vector3 scaleVector = new Vector3(scaleValue, scaleValue, 1.0f);
            transform.localScale = scaleVector;
        }

        protected override void InitUI()
        {
            base.InitUI();
            hasInitUi = true;

            if (GetComponent<CanvasGroup>() == null)
                cg = gameObject.AddComponent<CanvasGroup>();  //���������Կ���͸����

            InitButtonEvent();

            parentCanvas = GetComponentInParent<Canvas>();
            parentCanvasName = parentCanvas.name;

            //�����ǲ�����հ״���ʧ��ҲҪ�ѿհ״����ϡ�ʡ�ĵ���¼�͸�����ײ����ȥ
            BlankClickDestroy blankAutoDestroy = gameObject.GetComponent<BlankClickDestroy>();
            if (blankAutoDestroy == null)
            {
                blankAutoDestroy = gameObject.AddComponent<BlankClickDestroy>();
            }

            if (clickBlankAutoClose)
            {
                blankAutoDestroy.onBlankClick.AddListener(this.Dispose);
            }

        }

        protected virtual void InitButtonEvent()
        {
            if (returnBtn != null)
            {
                returnBtn.onClick.AddListener(delegate()
                {
                    Dispose();
                });
            }
        }

        protected override void UpdateView()
        {
            base.UpdateView();
        }


        protected virtual void LateUpdate()
        {
            //create blur
            if (mHasCheckToCreateBlurImage || parentCanvas == null || mLateUpdateFrameCount < 1)
            {
                ++mLateUpdateFrameCount;
                //m_tCanvasΪnull��˵������̳��˸��࣬ȷû�е��õ�InitUI��start���������ء�����һ�ִ�����÷�����
                return;
            }

            mHasCheckToCreateBlurImage = true;
            base.LateUpdate();
        }

        // modified by jackmo at 2016-11-14 virtual
        protected virtual void BackHandle()
        // modifiedend
        {
            if (OnReturnClick != null)
            {
                OnReturnClick();
            }

            gameObject.SetActive(false);
            if (destroyOnClose)
            {
                Destroy(gameObject);
            }
        }

        public virtual void Dispose()
        {
            if (hasDispose)
            {
                //������
                return;
            }
            hasDispose = true;

            float num = EffectMax;
           Tween tween = DOTween.To(
               () => num ,
               x => num = x ,
               EffectDisposeMin,
               EffectDisposeTime
           );

            tween.OnUpdate
            (
                 () =>  onDisposeUpdate(num)
            );

            tween.OnComplete
            (
                () => QuitComplete()
            );
        }

        public void onDisposeUpdate(float x)
        {
            try
            {
                if (x == null)
                {
                    return;
                }
                if (changeScaleWhenDispose)
                {
                    SetScale(x);
                }
                //rt.localScale = new Vector3((float)x, (float)x, 1);
                float iAlpha = (x - EffectDisposeMin) / (EffectMax - EffectDisposeMin);

                if (cg == null)
                {
                    return;
                }
            }
            catch (Exception e)
            {
                Debug.Log("BaseSceneEaseInOut Dispose:" + e.ToString());
            }
        }

        public void QuitComplete()
        {
            BackHandle();
        }

        public virtual void Enter()
        {

            if (cg != null) //�ȳ�ʼ��Ϊ���ɼ�������Ч������ͻ
                cg.alpha = (float)0;

            //if (bMaskAlpha)
            //{
            //    //m_tMaskBlur.GetComponent<CanvasGroup>().alpha = 0;
            //}

            RectTransform rt = transform as RectTransform;

            if (rt.parent == null)
                return;

            float screenWidth = (rt.parent as RectTransform).sizeDelta.x;

            float num = EffectMin;
            Tween tween = DOTween.To(
                () => num,
                x => num = x,
                EffectMax,
                EffectEnterTime
            );

            tween.OnUpdate
            (
                 () => onEnterUpdate(num)
            );

            tween.OnComplete
            (
                () => EnterComplete()
            );
            
        }

        public void onEnterUpdate(float x)
        {
            SetScale((float)x);
            float iAlpha = (float)((float)x - EffectMin) / (EffectMax - EffectMin);
            cg.alpha = iAlpha;
        }

        public virtual void EnterComplete()
        {

        }

        public RenderTexture ScreenShotOnUICamera()
        {
            GameObject cameraGO = GameObject.Find("UICamera");
            if (cameraGO == null)
            {
                Debug.Log("curr scene has no UICamera");
                return null;
            }
            Camera camera = cameraGO.GetComponent<Camera>();
            Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            Canvas parentCanvas = gameObject.GetComponentInParent<Canvas>();

            Vector2 size = new Vector2();
            if (parentCanvas != null)
            {
                size = new Vector2(parentCanvas.GetComponent<RectTransform>().sizeDelta.x, parentCanvas.GetComponent<RectTransform>().sizeDelta.y);
            }


            //Vector3 oldScale = canvas.transform.localScale;

            //Ҫ�ȱ������ţ�
            //float scaleValue = blurImageWidth / canvas.GetComponent<RectTransform>().sizeDelta.x;
            //float blurImageHeight = canvas.GetComponent<RectTransform>().sizeDelta.y * scaleValue;
            RenderTexture tRender = RenderTexture.GetTemporary((int)((canvas.GetComponent<RectTransform>().sizeDelta.x + 0.5f) / 2.0f)
                                                                , (int)((canvas.GetComponent<RectTransform>().sizeDelta.y + 0.5f) / 2.0f), 24);//new RenderTexture(iWidth, iHeight, 0);
            RenderTexture.active = tRender;
            camera.targetTexture = tRender;
            camera.Render();
            camera.targetTexture = null;
            RenderTexture.active = null;
            //canvas.transform.localScale = oldScale;

            if (parentCanvas != null)
            {
                parentCanvas.GetComponent<RectTransform>().sizeDelta = size;
            }

            return tRender;
        }

        protected override void OnDestroy()
        {
            returnBtn = null;
            // added by jackmo at 2016-11-1��һ��Ҫ���ø��෽���������������ʱû���Ƴ��������¼�
            base.OnDestroy();
            // added end
            isDestroyed = true;

            
        }
    }
    
    
}