using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Inst
    {
        get
        {
            return _inst;
        }
    }
    private static AudioManager _inst;

    public CSound[] sounds;

    void Awake()
    {
        if (_inst != null && _inst != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _inst = this;

        DontDestroyOnLoad(this.gameObject); //no destruye el objeto entre escenas

        foreach (CSound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        CSound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            return;
        }


        s.source.Play();
    }

    public void Stop(string name)
    {
        CSound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            return;
        }


        s.source.Stop();
    }


    // Start is called before the first frame update
    void Start()
    {
        Play("musica");
        Play("mar");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
