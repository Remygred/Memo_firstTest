using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VolumeControl : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider attackSlider;
    public Slider otherSlider;

    private float initialMasterVolume;
    private float initialMusicVolume;
    private float initialAttackVolume;
    private float initialOtherVolume;

    public AudioSource ClickaudioSource;
    public AudioClip ClickSound;

    void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat("SumVolume", 0);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0);
        attackSlider.value = PlayerPrefs.GetFloat("AttackVolume", 0);
        otherSlider.value = PlayerPrefs.GetFloat("OtherVolume", 0);

        initialMasterVolume = masterSlider.value;
        initialMusicVolume = musicSlider.value;
        initialAttackVolume = attackSlider.value;
        initialOtherVolume = otherSlider.value;

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        attackSlider.onValueChanged.AddListener(SetAttackVolume);
        otherSlider.onValueChanged.AddListener(SetOtherVolume);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("SumVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", volume);
    }

    public void SetAttackVolume(float volume)
    {
        audioMixer.SetFloat("AttackVolume", volume);
    }

    public void SetOtherVolume(float volume)
    {
        audioMixer.SetFloat("OtherVolume", volume);
    }

    public void ApplyChanges()
    {

        if (ClickaudioSource != null && ClickSound != null)
        {
            ClickaudioSource.PlayOneShot(ClickSound);
        }
        StartCoroutine(LoadSceneAfterSound());

        PlayerPrefs.SetFloat("SumVolume", masterSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("AttackVolume", attackSlider.value);
        PlayerPrefs.SetFloat("OtherVolume", otherSlider.value);

        ReturnToPreviousMenu();
    }

    public void ReturnWithoutSaving()
    {

        if (ClickaudioSource != null && ClickSound != null)
        {
            ClickaudioSource.PlayOneShot(ClickSound);
        }
        StartCoroutine(LoadSceneAfterSound());

        masterSlider.value = initialMasterVolume;
        musicSlider.value = initialMusicVolume;
        attackSlider.value = initialAttackVolume;
        otherSlider.value = initialOtherVolume;

        ReturnToPreviousMenu();
    }

    void ReturnToPreviousMenu()
    {
        // 获取保存的上一个场景名称
        string previousScene = PlayerPrefs.GetString("PreviousScene", "MainMenu");

        // 加载上一个场景
        if(previousScene == "MainMenu")
            SceneManager.LoadScene(previousScene);
        else
            SceneManager.UnloadSceneAsync("VolumeSettings");
    }

    IEnumerator LoadSceneAfterSound()
    {
        // 等待音效播放完毕
        yield return new WaitForSeconds(1f);
    }
}
