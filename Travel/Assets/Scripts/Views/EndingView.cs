using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Lucky;
using System.Threading;

public class EndingView : MonoBehaviour {

    
    public Image image;

    public Sprite[] images;
    public AudioClip[] audioClips;
    public float[] time;
    private AudioSource audiosource;

    private void Awake()
    {
        audiosource = GetComponent<AudioSource>();
    }

    void Start()
    {
        
        StartCoroutine(PrefabManager.Instance.Init("Main"));

    }

    private IEnumerator ShowImages()
    {
        image.sprite = images[0];
        while(true)
        {
            
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(time[0]);
    }

}
