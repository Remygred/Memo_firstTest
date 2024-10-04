using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float timeRemaining;  // 20 ���ӵĵ���ʱʱ�䣬��λΪ��
    private bool timerIsRunning = false;
    public GameObject WinMenu;

    public AudioSource audioSource;  // ���ڲ�����Ч�� AudioSource
    public AudioClip winSound;     // ������Ч

    public bool OK = true;
    public CharacterAtribute Character;

    void Start()
    {
        // ��ʼ������ʱΪ����״̬
        timerIsRunning = true;
        UpdateTimerDisplay();  // ���³�ʼʱ����ʾ
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                // ÿ֡���ٵ���ʱʱ��
                timeRemaining -= Time.deltaTime;

                // ���� UI ��ʾ
                UpdateTimerDisplay();
            }
            else
            {
                // ʱ�䵽��ֹͣ����ʱ
                timeRemaining = 0;
                timerIsRunning = false;
                UpdateTimerDisplay();
                WinGame();
            }
        }
    }

    void UpdateTimerDisplay()
    {
        // ��ʱ���ʽ��Ϊ����:��
        float minutes = Mathf.FloorToInt(timeRemaining / 60);
        float seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // ��Ϸʤ���Ĵ���
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
        Time.timeScale = 0f;  // ��ͣ��Ϸ
    }
}
