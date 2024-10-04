using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public AudioSource ClickaudioSource;
    public AudioClip ClickSound;

    // 开始游戏
    public void StartGame()
    {
        if (ClickaudioSource != null && ClickSound != null)
        {
            ClickaudioSource.PlayOneShot(ClickSound);
        }
        StartCoroutine(LoadSceneAfterSound());
        SceneManager.LoadScene("Game");
    }

    // 打开设置
    public void OpenSettings()
    {
        if (ClickaudioSource != null && ClickSound != null)
        {
            ClickaudioSource.PlayOneShot(ClickSound);
        }
        StartCoroutine(LoadSceneAfterSound());
        // 保存当前场景的名称
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);

        // 切换到音量设置场景
        SceneManager.LoadScene("VolumeSettings");
    }

    // 退出游戏
    public void QuitGame()
    {
        if (ClickaudioSource != null && ClickSound != null)
        {
            ClickaudioSource.PlayOneShot(ClickSound);
        }
        StartCoroutine(LoadSceneAfterSound());
        Debug.Log("Game is exiting!");
        Application.Quit();
    }

    IEnumerator LoadSceneAfterSound()
    {
        // 等待音效播放完毕
        yield return new WaitForSeconds(0.5f);
    }
}
