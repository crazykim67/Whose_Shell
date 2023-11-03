using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    #region Instance

    private static SoundManager instance;
    
    public static SoundManager Instance
    {
        get
        {
            if(instance = null)
            {
                instance = new SoundManager();
                return instance;
            }

            return instance;
        }
    }

    #endregion

    public AudioSource bgSound;

    [Header("BackGround Sound List")]
    public AudioClip[] clipList;

    [Header("Audio Mixer")]
    public AudioMixer mixer;

    public float currentBgLength;

    public float curretClipTime;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);

        bgSound = this.GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (bgSound == null)
            return;

        if (bgSound.isPlaying)
            curretClipTime = bgSound.time;
    }

    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject go = new GameObject(sfxName + "Sound");
        AudioSource audioSource = go.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = mixer.FindMatchingGroups("SFX")[0];
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(go, clip.length);
    }

    public void BackGroundPlay(AudioClip clip)
    {
        if (bgSound == null)
            return;

        bgSound.outputAudioMixerGroup = mixer.FindMatchingGroups("BackGround")[0];
        bgSound.clip = clip;
        bgSound.loop = true;
        if(bgSound != null)
            bgSound.Play();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        for (int i = 0; i < clipList.Length; i++)
        {
            if (scene.name == clipList[i].name)
            {
                BackGroundPlay(clipList[i]);
                currentBgLength = clipList[i].length;
            }
        }
    }

    public void OnSceneAsyncLoaded(string _scene)
    {
        // 수정 예정
        if (_scene.Equals("LoginMenu"))
        {
            bgSound.Stop();
        }
    }

    public void MasterVolume(float volume)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
    }

    public void BGVolume(float volume)
    {
        mixer.SetFloat("BgVolume", Mathf.Log10(volume) * 20);
    }

    public void SfxVolume(float volume)
    {
        mixer.SetFloat("SfxVolume", Mathf.Log10(volume) * 20);
    }

}
