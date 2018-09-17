using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Lucky;
using System.Threading;

public class EndingView : MonoBehaviour {

    void Start()
    {
        AudioManager.Instance.Stop(Audios.BG);
        AudioManager.Instance.PlayMusic(Audios.Ending,true);
    }

}
