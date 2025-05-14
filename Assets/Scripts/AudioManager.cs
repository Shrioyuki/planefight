using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance=FindAnyObjectByType<AudioManager>();
            }
            return _instance;
        }
    }
    private AudioSource audioSource;
    public AudioClip buttonClip;
    // Start is called before the first frame update
    void Start()
    {
        audioSource=gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayButtonClip()
    {
        audioSource.PlayOneShot(buttonClip, 0.2f);
    }
}
