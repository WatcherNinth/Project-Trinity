using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/Tweening/Interaction/Button Scale")]
    public class TweenBtnScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        public Transform target;
        public TweenMain.Method method = TweenMain.Method.Linear;
        public TweenMain.Style style = TweenMain.Style.Once;
        public Vector3 hover = new Vector3(1.0f, 1.0f, 1.0f);
        public Vector3 pressed = new Vector3(0.9f, 0.9f, 0.9f);
        public float duration = 0.1f;

        private Vector3 _Scale;
        private bool _Started = false;

        private bool hovered = false;

        void Start()
        {
            if (!_Started)
            {
                _Started = true;
                if (target == null) target = transform;
                _Scale = target.localScale;
            }
        }

        void OnDisable()
        {
            if (_Started && target != null)
            {
                TweenScale tc = target.GetComponent<TweenScale>();

                if (tc != null)
                {
                    tc.value = _Scale;
                    tc.enabled = false;
                }
            }
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            if (enabled)
            {
                if (!_Started) Start();
                Button btn = GetComponent<Button>();
                if (btn != null && !btn.interactable) return;
                TweenScale.Tween(target.gameObject, duration, target.localScale, pressed, style, method);
            }
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (enabled)
            {
                if (!_Started) Start();
                Button btn = GetComponent<Button>();
                if (btn != null && !btn.interactable) return;
                if (hovered)
                    TweenScale.Tween(target.gameObject, duration, target.localScale, hover, style, method);
                else
                    TweenScale.Tween(target.gameObject, duration, target.localScale, _Scale, TweenMain.Style.Once, method);
            }
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            hovered = true;
            if (enabled)
            {
                if (!_Started) Start();
                Button btn = GetComponent<Button>();
                if (btn != null && !btn.interactable) return;
                TweenScale.Tween(target.gameObject, duration, target.localScale, hover, style, method);
            }
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            hovered = false;
            if (enabled)
            {
                if (!_Started) Start();
                Button btn = GetComponent<Button>();
                if (btn != null && !btn.interactable) return;
                TweenScale.Tween(target.gameObject, duration, target.localScale, _Scale, TweenMain.Style.Once, method);
            }
        }
    }
}
