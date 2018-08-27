using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToolBarView : MonoBehaviour {

    public Button up;
    public Button down;

    private bool test = false;

	// Use this for initialization
	void Start () {
        up.onClick.AddListener(MoveDown);
        down.onClick.AddListener(MoveUp);
	}

    public void MoveDown()
    {
        test = true;
        iTween.MoveTo(gameObject, iTween.Hash
            (
                "y", -transform.position.y,
                "time",1f,
                "loopType","none",
                "easeType",iTween.EaseType.easeInOutExpo,
                "oncomplete", "OnDownComplete"
            )
            );
    }

    public void MoveUp()
    {
        iTween.MoveTo(gameObject, iTween.Hash
        (
            "y", -transform.position.y,
            "time", 1f,
            "loopType", "none",
            "easeType", iTween.EaseType.easeInOutExpo,
            "oncomplete", "OnUpComplete"
        )
        );
    }

    public void OnDownComplete()
    {
        up.gameObject.SetActive(false);
        down.gameObject.SetActive(true);
    }

    public void OnUpComplete()
    {
        up.gameObject.SetActive(true);
        down.gameObject.SetActive(false);
    }
}
