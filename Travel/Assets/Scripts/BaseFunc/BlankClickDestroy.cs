using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

namespace Lucky
{
    public class BlankClickDestroy : MonoBehaviour
    {
        [Serializable]
        public class BlankClickedEvent : UnityEvent { }

        // Event delegates triggered on click.
        [SerializeField]
        private BlankClickedEvent m_OnBlankClick = new BlankClickedEvent();
        private float distance = 600;
        private Button mBlankButton = null;

        public BlankClickedEvent onBlankClick
        {
            get { return m_OnBlankClick; }
            set { m_OnBlankClick = value; }
        }

        public void OnPointerClick()
        {
            m_OnBlankClick.Invoke();//点击后的动作，通常是面板隐藏动画
        }

        public void Start()
        {
            GameObject blankGO = new GameObject();
            blankGO.name = "blankClickDestroy";
            RectTransform blankRT = blankGO.AddComponent<RectTransform>();
            //blankGO.AddComponent<InvisibleImage>();

            Button btn = blankGO.AddComponent<Button>();
            btn.transition = Selectable.Transition.None;
            btn.onClick.AddListener(OnPointerClick);

            Canvas canvas = gameObject.GetComponentInParent<Canvas>();

            blankGO.transform.SetParent(canvas.transform);
            //blankGO.transform.SetParent(gameObject.transform.parent);
            blankRT.anchorMin = Vector2.zero;
            blankRT.anchorMax = Vector2.one;
            blankRT.localPosition = Vector3.zero;
            blankRT.localScale = Vector3.one;
            // maskRT.sizeDelta = Vector2.zero;
            blankRT.offsetMin = new Vector2(-distance, -distance);
            blankRT.offsetMax = new Vector2(distance, distance);
            //Texture2D texture2d = new Texture2D(AppConfig.APP_WIDTH, AppConfig.APP_HEIGHT);
            //blankImage.sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), Vector2.zero);
            //texture2d.SetPixel(0, 0, Color.white);
            //texture2d.Apply();
            //blankImage.color = new Color(0, 0, 0, 0.05f);
            //blankImage.material = new Material(Shader.Find("Sprites/Default"));
            blankRT.transform.SetParent(gameObject.transform);
            blankRT.SetAsFirstSibling();

            mBlankButton = btn;

            //去base里面取数据
            BaseScene baseScene = gameObject.GetComponent<BaseScene>();
            if (baseScene != null)
            {
                SetButtonClickable(baseScene.GetIsVisible());
            }
            else
            {
                SetButtonClickable(gameObject.activeSelf);
            }
        }

        public void SetButtonClickable(bool canClick)
        {
            if (mBlankButton == null)
            {
                return;
            }

            mBlankButton.gameObject.SetActive(canClick);
            
            if (canClick)
            {
                //ResetPos
                RectTransform blankRT = mBlankButton.GetComponent<RectTransform>();
                Canvas canvas = gameObject.GetComponentInParent<Canvas>();

                if (canvas == null)
                {
                    //父亲已经被销毁？
                    return;
                }

                blankRT.transform.SetParent(canvas.transform);
                blankRT.anchorMin = Vector2.zero;
                blankRT.anchorMax = Vector2.one;
                blankRT.localPosition = Vector3.zero;
                blankRT.localScale = Vector3.one;
                blankRT.offsetMin = new Vector2(-distance, -distance);
                blankRT.offsetMax = new Vector2(distance, distance);
                blankRT.transform.SetParent(gameObject.transform);
                blankRT.SetAsFirstSibling();
            }
        }

        public void DestroyGameObject()
        {
            Destroy(gameObject);
        }

        public void OnDestroy()
        {
            // modified by jackmo at 2016-12-22，判空
            if (mBlankButton != null)
                Destroy(mBlankButton.gameObject);
            // modified end
        }
    }
}

