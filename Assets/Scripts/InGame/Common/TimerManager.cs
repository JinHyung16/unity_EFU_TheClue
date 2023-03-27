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

    private float themeFirstClearTime; //minTime과 비교

    public bool IsTimeDone { get; private set; } = false;

    public float ThemeTime 
    {
        get
        {
            return this.minTime;
        }
        set
        { 
            this.curTime = value; 
        } 
    }
    public string CurTimeString { get; private set; }
    public bool IsTimerStart { get; set; } = false;

    private void Start()
    {
        themeFirstClearTime = 10.0f;

        Debug.Log("TimerManager - startTime: ");
        minTime = 0.0f;
        secTime = 0.0f;
        curTime = 0.0f;
        IsTimeDone = false;
        IsTimerStart = true;
    }

    private void Update()
    {
        if (IsTimerStart && !IsTimeDone)
        {
            if (curTime > 0.0f)
            {
                curTime -= Time.deltaTime;
                UpdateTimer(curTime);
            }
            else
            {
                if (curTime != 0.0f)
                {
                    curTime = 0.0f;
                    UpdateTimer(curTime);
                }
            }
        }
    }

    public void UpdateTimer(float time)
    {
        minTime = Mathf.FloorToInt(time / 60);
        secTime = Mathf.FloorToInt(time % 60);

        timerText.text = minTime.ToString("F0") + ":" + secTime.ToString("F0");
        CurTimeString = minTime.ToString("F0") + ":" + secTime.ToString("F0");

        ThemeClearInTime();
    }

    private void ThemeClearInTime()
    {
        if (themeFirstClearTime <= minTime && ThemeFirstPresenter.GetInstance != null)
        {
            IsTimeDone = true;
            ThemeFirstPresenter.GetInstance.GameClear(false);
        }
    }
}
