using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public AudioSource ClickaudioSource;
    public AudioClip ClickSound;

    // ��ʼ��Ϸ
    public void StartGame()
    {
        if (ClickaudioSource != null && ClickSound != null)
        {
            ClickaudioSource.PlayOneShot(ClickSound);
        }
        StartCoroutine(LoadSceneAfterSound());
        SceneManager.LoadScene("Game");
    }

    // ������
    public void OpenSettings()
    {
        if (ClickaudioSource != null && ClickSound != null)
        {
            ClickaudioSource.PlayOneShot(ClickSound);
        }
        StartCoroutine(LoadSceneAfterSound());
        // ���浱ǰ����������
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);

        // �л����������ó���
        SceneManager.LoadScene("VolumeSettings");
    }

    // �˳���Ϸ
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
        // �ȴ���Ч�������
        yield return new WaitForSeconds(0.5f);
    }
}
