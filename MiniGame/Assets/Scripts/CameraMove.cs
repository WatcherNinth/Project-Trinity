using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

    public void MoveForward()
    {
        iTween.MoveBy(gameObject, iTween.Hash
        (
            "x", 1920,
            "time", 1f,
            "loopType", "none",
            "easeType", iTween.EaseType.linear
        )
        );
        
    }
}
