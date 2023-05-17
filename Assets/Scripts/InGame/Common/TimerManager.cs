using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class TimerManager : MonoBehaviour
{
    #region static
    public static TimerManager GetInstance;

    private void Awake()
    {
        GetInstance = this;
    }
    #endregion

    [SerializeField] private TMP_Text timerText;

    private float curTime = 0.0f;

    private float minTime = 0;
    private float secTime = 0.0f;

    public bool IsTimeDone { get; private set; } = false;

    public float ThemeClearTime { get; set; }
    public string CurTimeString { get; private set; }
    public int CurMinTime { get; private set; }
    public bool IsTimerStart { get; set; } = false;

    private void Start()
    {
        IsTimeDone = false;
        IsTimerStart = true;

        minTime = 0.0f;
        secTime = 0.0f;
        curTime = 0.0f;
    }
    private void Update()
    {
        if (IsTimerStart && !IsTimeDone)
        {
            if (curTime <= ThemeClearTime)
            {
                curTime += Time.deltaTime;
                UpdateTimer(curTime);
            }
        }
    }

    public void UpdateTimer(float time)
    {
        minTime = Mathf.FloorToInt(time / 60);
        secTime = Mathf.FloorToInt(time % 60);

        CurMinTime = Mathf.CeilToInt(minTime);

        timerText.text = minTime.ToString("F0") + ":" + secTime.ToString("F0");
        CurTimeString = minTime.ToString("F0") + ":" + secTime.ToString("F0");

        ThemeClearInTime();
    }

    private void ThemeClearInTime()
    {
        if (ThemeClearTime <= minTime)
        {
            if (ThemeFirstPresenter.GetInstance != null)
            {
                IsTimeDone = true;
                ThemeFirstPresenter.GetInstance.GameClear(false);
            }
            if (ThemeSecondPresenter.GetInstance != null)
            {
                IsTimeDone = true;
                ThemeSecondPresenter.GetInstance.GameClear(false);
            }
            if (ThemeThirdPresenter.GetInstance != null)
            {
                IsTimeDone = true;
                ThemeThirdPresenter.GetInstance.GameClear(false);
            }
        }

        if (10.0f <= minTime)
        {
            if (ThemeSecondPresenter.GetInstance != null)
            {
                ThemeSecondPresenter.GetInstance.IsOverTime = true;
            }
        }
    }
}
