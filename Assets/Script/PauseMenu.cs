using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuCanvas;  // ������ͣ�˵��� Canvas
    public Image pauseMenuBackground;   // ���ò˵�����ͼƬ
    public Sprite[] backgroundImages;   // 6 �ű���ͼƬ

    private Animator ani;

    private bool isPaused = false;
    private CountdownTimer countdownTimer; // ���õ���ʱ�ű�

    public AudioSource ClickaudioSource;
    public AudioClip ClickSound;

    void Start()
    {
        ani = pauseMenuCanvas.GetComponent<Animator>();
        ani.updateMode = AnimatorUpdateMode.UnscaledTime;

        // ��ȡ����ʱ�ű�����
        countdownTimer = FindObjectOfType<CountdownTimer>();
    }

    void Update()
    {
        // ���� ESC ��ʱ�л���ͣ�˵�
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }

        // ����˵�����ͣ״̬����̬���±���
        if (isPaused)
        {
            UpdateBackgroundImage();
        }
    }

    public void TogglePauseMenu()
    {
        if (ClickaudioSource != null && ClickSound != null)
        {
            ClickaudioSource.PlayOneShot(ClickSound);
        }
        isPaused = !isPaused;  // �л���ͣ״̬
        ani.SetBool("Appear", isPaused);

        // ��ͣ��ָ���Ϸ
        if (isPaused)
        {
            StartCoroutine(WaitForPause());  // �ȴ�����������Ϻ���ͣ
        }
        else
        {
            Time.timeScale = 1f;  // �ָ���Ϸ
        }
    }

    IEnumerator WaitForPause()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        Time.timeScale = 0f;  // ��ͣ��Ϸ
    }

    void UpdateBackgroundImage()
    {
        // ���ݵ���ʱѡ�񱳾�
        float remainingTime = countdownTimer.timeRemaining / 60;  // ��ȡʣ��ʱ�䣬�Է���Ϊ��λ

        // ���ò�ͬʱ��εı���ͼƬ
        if (remainingTime > 18)
        {
            pauseMenuBackground.sprite = backgroundImages[0];
        }
        else if (remainingTime > 15)
        {
            pauseMenuBackground.sprite = backgroundImages[1];
        }
        else if (remainingTime > 10)
        {
            pauseMenuBackground.sprite = backgroundImages[2];
        }
        else if (remainingTime > 5)
        {
            pauseMenuBackground.sprite = backgroundImages[3];
        }
        else if (remainingTime > 2)
        {
            pauseMenuBackground.sprite = backgroundImages[4];
        }
        else
        {
            pauseMenuBackground.sprite = backgroundImages[5];
        }
    }

    // ���¿�ʼ��Ϸ
    public void RestartGame()
    {
        if (ClickaudioSource != null && ClickSound != null)
        {
            ClickaudioSource.PlayOneShot(ClickSound);
        }
        Time.timeScale = 1f;  // ȷ��ʱ��ָ������ٶ�
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // ���¼��ص�ǰ����
    }

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
        SceneManager.LoadScene("VolumeSettings", LoadSceneMode.Additive);
    }

    public void ReturnToMainMenu()
    {
        if (ClickaudioSource != null && ClickSound != null)
        {
            ClickaudioSource.PlayOneShot(ClickSound);
        }
        StartCoroutine(LoadSceneAfterSound());
        Time.timeScale = 1f;  // �ָ�ʱ���ٶ�
        SceneManager.LoadScene("MainMenu");  // �������˵�����
    }

    IEnumerator LoadSceneAfterSound()
    {
        // �ȴ���Ч�������
        yield return new WaitForSeconds(0.5f);
    }
}
