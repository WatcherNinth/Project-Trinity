using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

    private const string audioPath = "Audios/";
    private List<AudioSource> audioSourceList = new List<AudioSource>();
    private Dictionary<string, AudioClip> audioclipDic = new Dictionary<string, AudioClip>();

    private static AudioManager _instance;
    private static AudioManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        _instance = this;
    }

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private AudioSource GetAudioSource()
    {
        foreach(AudioSource audioS in audioSourceList)
        {
            if (!audioS.isPlaying)
                return audioS;
        }
        AudioSource audio = gameObject.AddComponent<AudioSource>();
        audioSourceList.Add(audio);
        return audio;
    }

    private AudioClip GetAudioClip(string name)
    {
        if (audioclipDic.ContainsKey(name))
            return audioclipDic[name];
        else
        {
            AudioClip ac = Resources.Load<AudioClip>(name);
            audioclipDic.Add(name, ac);
            return ac;
        }
    }

    public void PlayMusic(string clipname,bool loop =false)
    {
        AudioClip clip = GetAudioClip(name);
        AudioSource audioSource = GetAudioSource();
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
    }

    public void Stop(string clipname)
    {
        foreach (AudioSource audioS in audioSourceList)
        {
            if (audioS.isPlaying && audioS.clip.name == clipname)
            {
                audioS.Stop();
                return;
            }      
        }
    }

    public IEnumerator Init()
    {
        string[] s =
            {
                
            };

        foreach(string clipname in s)
        {
            ResourceRequest rr = Resources.LoadAsync(audioPath+clipname);
            yield return rr;
            if (rr.asset != null)
            {
                audioclipDic.Add(clipname, (AudioClip)rr.asset);
                Debug.Log("load " + clipname);
            }
            else
                Debug.Log(clipname + "load failed");
        }
    }
}
