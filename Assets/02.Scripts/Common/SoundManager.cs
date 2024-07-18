using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager S_instance;
    public float soundVolum = 1.0f;     //사운드볼륨
    public bool isSoundMute = false;    //음소거
    public AudioClip clip1;
    public AudioClip clip2;

    void Awake()
    {
        if (S_instance == null)
            S_instance = this;

        else if(S_instance != this)
             Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        clip1 = Resources.Load("Sounds/hit_metal_1") as AudioClip;
        clip2 = Resources.Load("Sounds/grenade_exp2") as AudioClip;
    }

    public void PlaySound(Vector3 pos, AudioClip clip)
    {
        if (isSoundMute) { return; }

        GameObject soundObj = new GameObject("Sound!");

        //소리는 위치를 전달받음
        soundObj.transform.position = pos;

        //오디오소스 컴퍼넌트가 없다면 AddComponent<AudioSource>()로 오디오소스를 부착할 수 있다.
        AudioSource audioSource = soundObj.AddComponent<AudioSource>();

        audioSource.clip = clip;
        audioSource.minDistance = 20f;
        audioSource.maxDistance = 50f;
        audioSource.volume = soundVolum;
        audioSource.Play();
        Destroy(soundObj, clip.length);
    }
}
