using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Lucky
{
    
    public class EffectManager
    {
        private static EffectManager m_tEffectManager;
        private GameObject _tEffet = null;
        private GameObject _tPopEffet = null;

        public GameObject m_tEffet
        {
            set
            {
                _tEffet = value;
            }
            get
            {
                return _tEffet;
            }
        }
        public GameObject m_tPopEffet
        {
            set
            {
                _tPopEffet = value;
            }
            get
            {
                return _tPopEffet;
            }
        }

        static public EffectManager instance
        {
            get
            {
                if (m_tEffectManager == null)
                {
                    m_tEffectManager = new EffectManager();
                }
                return m_tEffectManager;
            }
        }
    }


    public enum E_SHOW_PROIORTY
    {
        TOP = 0,
        MIDDLE = 1,
        LOW = 2,
    };

    /*
    /// <summary>
    /// panel的基础类，主要提供进入和退出的一些默认效果
    /// </summary>
    public class BaseSceneEaseInOut : MonoBehaviour
    {
        public TKCallback.FunVoid OnReturnClick;
        public Button returnBtn;
        private CanvasGroup cg;
        protected Canvas parentCanvas;
        protected GameObject m_tMaskBlur;

        public E_SHOW_PROIORTY priority = E_SHOW_PROIORTY.TOP;


        //参数设置
        private float EffectDisposeTime = 0.1f;
        private float EffectEnterTime = 0.25f;
        private float EffectMin = 0.65f;
        private float EffectDisposeMin = 0.9f;
        private float EffectMax = 1.0f;

        //开关设置
        public bool destroyOnClose = true;//选择false，则会缓存该GameObject
        public bool stay = false;
        public bool useBlur = true;

        public bool hideMainCanvas = true;

        public bool clickBlankAutoClose = false;   //是否点击空白消失，
        public bool backKeyAutoClose = false;   //是否点击back键消失
        public bool blockBackKeyEvent = true;   //是否阻塞住back键事件
        public bool changeScaleWhenDispose = true;//dispose时，是否播放scale淡出动画

        private bool mNeedPlayPopupSound = true;
        private bool mNeedPlayDisposeSound = true;


        //状态
        protected bool isDestroyed = false;
        protected bool hasInitUi = false;
        protected bool hasDispose = false;
        private bool hasAddBlurRefenceSuc = false;

        //过于耗时不再使用
        //比如全屏的背景不做放缩动画，只对内容做放缩动画，这时候把背景名称给到这里就ok  由于效率问题，尽量不要用
        //private string notDoAniObjectName = null;
        //private List<Transform> childrenTransform = null;
        //private List<Vector3> childrenOriginScale = null;

        private string parentCanvasName = "";

        private bool mHasCheckToCreateBlurImage = false;
        private int mLateUpdateFrameCount = 0;
        RawImage ri;

        protected virtual void Awake()
        {
            cg = GetComponent<CanvasGroup>();
        }
        protected virtual void Start()
        {

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

        [System.Obsolete("会造成性能严重下降，尽量不要用此函数")]
        public void SetNotDoAniObjectName(string strName)
        {
            //notDoAniObjectName = strName; 过于耗时不再使用
        }

        public void SetScale(float scaleValue)
        {
            
            Vector3 scaleVector = new Vector3(scaleValue, scaleValue, 1.0f);
            transform.localScale = scaleVector;

            // 下面的写法过于耗时，不再使用

            ////要记住每个child的scale
            ////notDoAniObjectName设置了的情况下，对组件的所有除了notDoAniObjectName之外的子节点做放缩动画
            ////否则就做整体放缩动画
            //if (notDoAniObjectName == null)
            //{
            //    transform.localScale = scaleVector;
            //}
            //else
            //{
            //    if (childrenTransform == null)
            //    {
            //        childrenTransform = new List<Transform>();
            //        childrenOriginScale = new List<Vector3>();
            //        foreach (Transform child in gameObject.transform)
            //        {
            //            if (child.gameObject.name == notDoAniObjectName)
            //            {
            //                continue;
            //            }

            //            childrenTransform.Add(child);
            //            childrenOriginScale.Add(new Vector3(child.localScale.x, child.localScale.y, 1.0f));
            //        }
            //    }

            //    for (int idx = 0; idx < childrenTransform.Count; ++idx)
            //    {
            //        childrenTransform[idx].localScale = new Vector3(childrenOriginScale[idx].x * scaleValue, childrenOriginScale[idx].y * scaleValue, 1.0f);
            //    }
            //}
        }

        public void setUseBlur(bool useBlur)
        {
            if (!hasInitUi)
            {
                this.useBlur = useBlur;
            }
            else
            {
                Debug.Log("Can not change useBlur Value when init over!");
            }
        }

        protected virtual void InitUI()
        {
            hasInitUi = true;

            if (GetComponent<CanvasGroup>() == null)
                cg = gameObject.AddComponent<CanvasGroup>();  //添加这个属性控制透明度

            InitButtonEvent();

            parentCanvas = GetComponentInParent<Canvas>();
            parentCanvasName = parentCanvas.name;

            //就算是不点击空白处消失，也要把空白处补上。省的点击事件透传到底层面板去
            BlankClickDestroy blankAutoDestroy = gameObject.GetComponent<BlankClickDestroy>();
            if (blankAutoDestroy == null)
            {
                blankAutoDestroy = gameObject.AddComponent<BlankClickDestroy>();
            }

            if (clickBlankAutoClose)
            {
                blankAutoDestroy.onBlankClick.AddListener(this.Dispose);
            }

            if (useBlur)
            {
                GameObject tEffect = GetBlurEffectInCanvas(parentCanvas);

                if (tEffect != null)
                {//如果计数放到uptatelate里面，2个界面交替出现的时候可能因为这个计数导致界面闪烁
                    tEffect.GetComponent<BlurEffect>().Retaincount++;
                    hasAddBlurRefenceSuc = true;
                }
            }
        }

        protected virtual void InitButtonEvent()
        {
            if (returnBtn != null)
            {
                returnBtn.onClick.AddListener(delegate()
                {
                    Debug.Log("BaseSceneEaseInOut return click");
                    Dispose();
                });
            }
        }


        
        protected virtual void LateUpdate()
        {
            //create blur

            if (mHasCheckToCreateBlurImage || parentCanvas == null || mLateUpdateFrameCount < 1)
            {
                ++mLateUpdateFrameCount;
                //m_tCanvas为null，说明外面继承了该类，确没有调用到InitUI（start方法被重载。这是一种错误的用法！）
                return;
            }

            mHasCheckToCreateBlurImage = true;

            if (!useBlur)
            {
                return;
            }

            //TKLog.Debug(gameObject.name);
            GameObject tEffect = GetBlurEffectInCanvas(parentCanvas);

            if (tEffect == null)
            { //不要重复创建
                tEffect = CreateBlur();

#if UNITY_IOS || UNITY_EDITOR
                if (hideMainCanvas)
                {
                    tEffect.AddComponent<HideMainCanvas>();
                }
#endif
                //保存起来，加快性能
                SetBlurEffectbyCanvas(parentCanvas, tEffect);
                tEffect.GetComponent<BlurEffect>().Retaincount++;
            }
            else
            {
                if (!hasAddBlurRefenceSuc)
                {
                    hasAddBlurRefenceSuc = true;
                    tEffect.GetComponent<BlurEffect>().Retaincount++;
                }
            }

            m_tMaskBlur = tEffect;
            //m_tMaskBlur.GetComponent<CanvasGroup>().alpha = 1;

            m_tMaskBlur.GetComponent<RectTransform>().SetAsFirstSibling();

            ri = m_tMaskBlur.GetComponent<RawImage>();
#if UNITY_IOS || UNITY_EDITOR
            ri.color = ColorConfig.getColor(ColorConfig.COLOR_GRAY_DEEP);
#else
            ri.color = new Color(0f, 0f, 0f, 0.75f);
#endif
            ri.raycastTarget = false;

            //TKLog.Error("InitUI BlurImage Retaincount  " + m_tCanvas.name + "= " + tEffect.GetComponent<BlurEffect>().Retaincount);
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
                //防重入
                return;
            }
            hasDispose = true;
            PlayDisposeSound();

            bool bMaskAlpha = false;
            HideMainCanvas mainCanvas = null;
            if (m_tMaskBlur)
            {
                mainCanvas = m_tMaskBlur.GetComponent<HideMainCanvas>();

                BlurEffect eff = m_tMaskBlur.GetComponent<BlurEffect>();
                if (eff.Retaincount < 1)
                {
                    eff.Disappear(EffectDisposeTime);
                    bMaskAlpha = true;
                }
            }
            //bool bMaskAlpha = (m_tMaskBlur != null && m_tMaskBlur.GetComponent<BlurEffect>().Retaincount <= 1);
            //RectTransform rt = transform as RectTransform;
            
            iTween.ValueTo(gameObject, iTween.Hash(
                "ignoretimescale", true,// added by jackmo at 2017-1-7
                "from", EffectMax,
                "to", EffectDisposeMin,
                "time", EffectDisposeTime,
                "onupdate", TKCallback.CreateAction(delegate(object x)
                {
                    try
                    {
                        if (x == null)
                        {
                            return;
                        }
                        if (changeScaleWhenDispose)
                        {
                            SetScale((float)x);
                        }
                        //rt.localScale = new Vector3((float)x, (float)x, 1);
                        float iAlpha = (float)((float)x - EffectDisposeMin) / (EffectMax - EffectDisposeMin);

                        if (cg == null)
                        {
                            return;
                        }

                        cg.alpha = iAlpha / 0.75f;

                        if (bMaskAlpha)
                        {
                            if (mainCanvas != null)
                            {
                                mainCanvas.DisableComponent();
                            }
                        }
                    }
                    catch (System.Exception e)
                    {
                        Debug.Log("BaseSceneEaseInOut Dispose:" + e.ToString());
                    }
                    //rt.offsetMin = new Vector2(screenWidth * (float)x, 0);
                    //rt.offsetMax = new Vector2(screenWidth * (float)x, 0);
                }),
                "oncomplete", "QuitComplete", "easetype", iTween.EaseType.easeInCubic
            ));
        }

        public void QuitComplete()
        {
            BackHandle();
        }

        public virtual void Enter()
        {
            PlayPopupSound();

            if (cg != null) //先初始化为不可见，否则效果会唐突
                cg.alpha = (float)0;

            bool bMaskAlpha = (m_tMaskBlur != null && m_tMaskBlur.GetComponent<BlurEffect>().Retaincount <= 1);

            //if (bMaskAlpha)
            //{
            //    //m_tMaskBlur.GetComponent<CanvasGroup>().alpha = 0;
            //}

            RectTransform rt = transform as RectTransform;

            if (rt.parent == null)
                return;

            float screenWidth = (rt.parent as RectTransform).sizeDelta.x;
            iTween.ValueTo(gameObject, iTween.Hash(
                // added by jackmo at 2017-1-7
                "ignoretimescale", true,
                // added end
                "from", EffectMin,
                "to", EffectMax,
                "time", EffectEnterTime,
                "onupdate", TKCallback.CreateAction(delegate(object x)
                {
                    SetScale((float)x);
                    float iAlpha = (float)((float)x - EffectMin) / (EffectMax - EffectMin);
                    cg.alpha = iAlpha;

                }),
                "oncomplete", "EnterComplete", "easetype", iTween.EaseType.easeOutBack
            ));
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

            //要等比例缩放：
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

        private GameObject GetBlurEffectInCanvas(Canvas canvas)
        {
            // added by jackmo at 2016-11-22
            if (canvas == null)
                return null;
            // added end

            if (canvas.name.Contains("Pop"))
            {
                return EffectManager.instance.m_tPopEffet;
            }
            else
            {
                return EffectManager.instance.m_tEffet;
            }
        }

        private void SetBlurEffectbyCanvas(Canvas canvas, GameObject tEffect)
        {
            if (parentCanvasName.Contains("Pop"))
            {
                EffectManager.instance.m_tPopEffet = tEffect;
            }
            else
            {
                EffectManager.instance.m_tEffet = tEffect;
            }
        }

        private GameObject CreateBlur()
        {

#if UNITY_IOS || UNITY_EDITOR
            float alpha = 0;
            if (cg != null) //先初始化为不可见，让截屏截不到自己
            {
                alpha = cg.alpha;
                cg.alpha = (float)0;
            }

            Canvas canvas = gameObject.GetComponentInParent<Canvas>();

            //生成blurimage组件，添加到canvas根节点中去
            m_tMaskBlur = new GameObject();
            m_tMaskBlur.layer = gameObject.layer;
            m_tMaskBlur.name = "BlurImage" + canvas.name;

            if (m_tMaskBlur.GetComponent<CanvasGroup>() == null)
                m_tMaskBlur.AddComponent<CanvasGroup>();  //添加这个属性控制透明度


            //【1】准备好截屏数据并通过处理这张图片
            //【2】添加blur组件承载这张模糊图片
            //处理rtIn，输出rtOut
            BlurEffect be = m_tMaskBlur.AddComponent<BlurEffect>();
            RenderTexture rtOut = ScreenShotOnUICamera();  //截屏数据
            be.init();
            if(rtOut != null)
            {
                be.Blur(ref rtOut);

                RectTransform rt = m_tMaskBlur.AddComponent<RectTransform>();
                rt.sizeDelta = new Vector2(rtOut.width, rtOut.height);
                rt.SetParent(canvas.transform, false);

                rt.localPosition = Vector3.zero;
            }
            
            RectTransform blankRT = m_tMaskBlur.GetComponent<RectTransform>();
            Canvas parentCanvas = gameObject.GetComponentInParent<Canvas>();
            blankRT.transform.SetParent(parentCanvas.transform);
            blankRT.anchorMin = Vector2.zero;
            blankRT.anchorMax = Vector2.one;
            blankRT.localPosition = Vector3.zero;
            blankRT.localScale = Vector3.one;
            blankRT.offsetMin = new Vector2(0, 0);
            blankRT.offsetMax = new Vector2(0, 0);

            //【3】把图片通过RawImage挂上去
            //把图片加到blurimage组件中
            RawImage BlurImg = m_tMaskBlur.AddComponent<RawImage>();
            BlurImg.material = new Material(Shader.Find("Sprites/Default"));

            BlurImg.texture = rtOut;

            //【4】blurimage一直放popwin的前面
            //排一下顺序
            m_tMaskBlur.GetComponent<RectTransform>().SetSiblingIndex(gameObject.GetComponent<RectTransform>().GetSiblingIndex());

            //【4】恢复透明度
            if (cg != null) //先初始化为不可见，让截屏截不到自己
            {
                cg.alpha = alpha;
            }

#else
            Canvas canvas = gameObject.GetComponentInParent<Canvas>();
            m_tMaskBlur = new GameObject();
            m_tMaskBlur.layer = gameObject.layer;
            m_tMaskBlur.name = "BlurImage" + canvas.name;

            if (m_tMaskBlur.GetComponent<CanvasGroup>() == null)
            {
                CanvasGroup cgtemp = m_tMaskBlur.AddComponent<CanvasGroup>();  //添加这个属性控制透明度
                cgtemp.alpha = 0f;
            }

            BlurEffect be = m_tMaskBlur.AddComponent<BlurEffect>();


            RectTransform rt = m_tMaskBlur.AddComponent<RectTransform>();
            //rt.sizeDelta = new Vector2(rtOut.width, rtOut.height);
            rt.SetParent(canvas.transform, false);

            rt.localPosition = Vector3.zero;

            RectTransform blankRT = m_tMaskBlur.GetComponent<RectTransform>();
            Canvas parentCanvas = gameObject.GetComponentInParent<Canvas>();
            blankRT.transform.SetParent(parentCanvas.transform);
            blankRT.anchorMin = Vector2.zero;
            blankRT.anchorMax = Vector2.one;
            blankRT.localPosition = Vector3.zero;
            blankRT.localScale = Vector3.one;
            blankRT.offsetMin = new Vector2(0, 0);
            blankRT.offsetMax = new Vector2(0, 0);

            RawImage BlurImg = m_tMaskBlur.AddComponent<RawImage>();
            BlurImg.texture = null;
            BlurImg.color = new Color(0, 0, 0, 0.75f);

            m_tMaskBlur.GetComponent<RectTransform>().SetSiblingIndex(gameObject.GetComponent<RectTransform>().GetSiblingIndex());

#endif
            return m_tMaskBlur;
        }

        //********** 安卓物理返回键处理 **********
        public override bool OnKeyBackRelease()
        {
            if (backKeyAutoClose)
            {
                Dispose();
                return true;
            }

            if (!blockBackKeyEvent)
            {
                return false;
            }

            return true;
        }

        protected override void OnDestroy()
        {
            returnBtn = null;
            // added by jackmo at 2016-11-1，一定要调用父类方法，否则面板销毁时没有移除监听的事件
            base.OnDestroy();
            // added end
            isDestroyed = true;

            if (!useBlur)
            {
                return;
            }

            GameObject BlurImage = GetBlurEffectInCanvas(parentCanvas);
            if (BlurImage != null)
            {//只能删除没人引用的blur，只要有人引用blur就常驻
                BlurImage.GetComponent<BlurEffect>().Retaincount -= 1;

                Debug.Log("OnDestroy BlurImage Retaincount  "+ parentCanvas.name+ "= "+ BlurImage.GetComponent<BlurEffect>().Retaincount);

                if (BlurImage.GetComponent<BlurEffect>().Retaincount == 0)
                {
                    BlurImage.SetActive(false);
                    Destroy(BlurImage);
                    SetBlurEffectbyCanvas(parentCanvas, null);
                }
                else
                {
                    BlurImage.GetComponent<CanvasGroup>().alpha = 1;
                }
            }
        }
    }
    
    */
}