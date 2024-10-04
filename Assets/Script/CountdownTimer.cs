using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float timeRemaining;  // 20 分钟的倒计时时间，单位为秒
    private bool timerIsRunning = false;
    public GameObject WinMenu;

    public AudioSource audioSource;  // 用于播放音效的 AudioSource
    public AudioClip winSound;     // 死亡音效

    public bool OK = true;
    public CharacterAtribute Character;

    void Start()
    {
        // 初始化倒计时为运行状态
        timerIsRunning = true;
        UpdateTimerDisplay();  // 更新初始时间显示
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                // 每帧减少倒计时时间
                timeRemaining -= Time.deltaTime;

                // 更新 UI 显示
                UpdateTimerDisplay();
            }
            else
            {
                // 时间到，停止倒计时
                timeRemaining = 0;
                timerIsRunning = false;
                UpdateTimerDisplay();
                WinGame();
            }
        }
    }

    void UpdateTimerDisplay()
    {
        // 将时间格式化为分钟:秒
        float minutes = Mathf.FloorToInt(timeRemaining / 60);
        float seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // 游戏胜利的处理
    void WinGame()
    {
        Character.OK = false;

        if(OK) StartCoroutine(WaitForPause());
    }

    IEnumerator WaitForPause()
    {
        
        Animator ani = WinMenu.GetComponent<Animator>();
        ani.SetBool("Win", true);
        GameObject bgmManager = GameObject.FindWithTag("BGMManager");
        if (bgmManager != null)
        {
            bgmManager.GetComponent<AudioSource>().volume = 0f;
        }
        if (audioSource != null && winSound != null)
        {
            audioSource.PlayOneShot(winSound);
        }
        yield return new WaitForSecondsRealtime(0.5f);
        bgmManager.GetComponent<AudioSource>().volume = 1f;
        Time.timeScale = 0f;  // 暂停游戏
    }
}
