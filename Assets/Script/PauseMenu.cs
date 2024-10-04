using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuCanvas;  // 引用暂停菜单的 Canvas
    public Image pauseMenuBackground;   // 引用菜单背景图片
    public Sprite[] backgroundImages;   // 6 张背景图片

    private Animator ani;

    private bool isPaused = false;
    private CountdownTimer countdownTimer; // 引用倒计时脚本

    public AudioSource ClickaudioSource;
    public AudioClip ClickSound;

    void Start()
    {
        ani = pauseMenuCanvas.GetComponent<Animator>();
        ani.updateMode = AnimatorUpdateMode.UnscaledTime;

        // 获取倒计时脚本对象
        countdownTimer = FindObjectOfType<CountdownTimer>();
    }

    void Update()
    {
        // 按下 ESC 键时切换暂停菜单
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }

        // 如果菜单是暂停状态，动态更新背景
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
        isPaused = !isPaused;  // 切换暂停状态
        ani.SetBool("Appear", isPaused);

        // 暂停或恢复游戏
        if (isPaused)
        {
            StartCoroutine(WaitForPause());  // 等待动画播放完毕后暂停
        }
        else
        {
            Time.timeScale = 1f;  // 恢复游戏
        }
    }

    IEnumerator WaitForPause()
    {
        yield return new WaitForSecondsRealtime(0.25f);
        Time.timeScale = 0f;  // 暂停游戏
    }

    void UpdateBackgroundImage()
    {
        // 根据倒计时选择背景
        float remainingTime = countdownTimer.timeRemaining / 60;  // 获取剩余时间，以分钟为单位

        // 设置不同时间段的背景图片
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

    // 重新开始游戏
    public void RestartGame()
    {
        if (ClickaudioSource != null && ClickSound != null)
        {
            ClickaudioSource.PlayOneShot(ClickSound);
        }
        Time.timeScale = 1f;  // 确保时间恢复正常速度
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // 重新加载当前场景
    }

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
        SceneManager.LoadScene("VolumeSettings", LoadSceneMode.Additive);
    }

    public void ReturnToMainMenu()
    {
        if (ClickaudioSource != null && ClickSound != null)
        {
            ClickaudioSource.PlayOneShot(ClickSound);
        }
        StartCoroutine(LoadSceneAfterSound());
        Time.timeScale = 1f;  // 恢复时间速度
        SceneManager.LoadScene("MainMenu");  // 加载主菜单场景
    }

    IEnumerator LoadSceneAfterSound()
    {
        // 等待音效播放完毕
        yield return new WaitForSeconds(0.5f);
    }
}
