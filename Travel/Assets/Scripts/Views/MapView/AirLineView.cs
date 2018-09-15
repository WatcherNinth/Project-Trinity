using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class AirLineView : MonoBehaviour {

    public float Time = 0.5f;

    private RawImage airline;
    private float Min = 0.0f;
    private float Max = 1.0f;
    private RectTransform rt;

    private int n;
    private Tween tween = null;

    private void Awake()
    {
        airline = GetComponent<RawImage>();
        rt = GetComponent<RectTransform>();
        n = 5;
        gameObject.SetActive(false);
    }

    // Use this for initialization
    void Start () {
        Show();
    }

    private void OnDestroy()
    {
        tween.Pause();
    }

    public void Show(Vector3 start, Vector3 stop)
    {
        Vector3 middle = (start + stop) / 2.0f;
        rt.anchoredPosition3D = middle;
        rt.localScale = new Vector3(1, 1, 1);
        Vector2 diff = stop - start;

        rt.sizeDelta = new Vector2(diff.magnitude, rt.sizeDelta.y);

        float cos = diff.x / diff.magnitude;
        float angle = Mathf.Acos(cos) * Mathf.Rad2Deg;
        if (diff.y < 0)
            angle = -angle;
        transform.eulerAngles = new Vector3(0, 0, angle);
        

        float dictance = diff.magnitude;
        n = (int)dictance / 225;
        gameObject.SetActive(true);
        Show();
    }
	
    private void onAirLineUpdate(float num)
    {
        airline.uvRect = new Rect(num, 1, n, 1);
    }

    private void Show()
    {
        airline.uvRect = new Rect(Max, 1, n, 1);

        float num = Max;
        tween = DOTween.To(
            () => num,
            x => num = x,
            Min,
            Time
        ).SetEase(Ease.Linear).SetLoops(-1);
        tween.OnUpdate
            (
                 () => onAirLineUpdate(num)
            );

        
    }
}
