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
        transform.position = middle;

        Vector2 diff = stop - start;
        rt.sizeDelta = new Vector2(Mathf.Abs(diff.x), rt.sizeDelta.y);

        float tan = diff.y / diff.x;
        float angle = Mathf.Tan(tan) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);

        float dictance = Vector3.Distance(start,stop);
        n = (int)dictance / 225;

        Show();
    }
	
    private void onAirLineUpdate(float num)
    {
        airline.uvRect = new Rect(num, 1, n, 1);
    }

    private void Show()
    {
        airline.uvRect = new Rect(0, 1, n, 1);

        float num = Min;
        tween = DOTween.To(
            () => num,
            x => num = x,
            Max,
            Time
        ).SetEase(Ease.Linear).SetLoops(-1);
        tween.OnUpdate
            (
                 () => onAirLineUpdate(num)
            );

        
    }
}
