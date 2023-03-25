using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    private float startTime = 0.0f;

    private int minTime = 0;
    private float secTime = 0.0f;

    public bool IsTimerStart { get; set; } = false;
    public bool IsTimerDone { get; private set; } = false; //10분이 되었을 때 true로 변환

    private void Start()
    {
        startTime = Time.time;
        IsTimerStart = true;
    }

    private void Update()
    {
        if (IsTimerStart)
        {
            TimerStart();
        }
    }

    public void TimerStart()
    {
        curTime += (startTime - Time.deltaTime);
        secTime = curTime / 60.0f;
        if (60.0f <= secTime)
        {
            curTime = 0.0f;
            minTime += 1;
        }

        if (10 <= minTime)
        {
            IsTimerDone = true;
        }
        timerText.text = minTime.ToString("D0") + ":" + secTime.ToString("F0");
    }
}
