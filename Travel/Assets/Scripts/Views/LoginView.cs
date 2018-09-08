using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Lucky;
using System.Threading;

public class LoginView : MonoBehaviour {


    void Start()
    {
        StartCoroutine(PrefabManager.Instance.Init("Main"));

    }

}
