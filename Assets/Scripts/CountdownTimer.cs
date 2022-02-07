using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class CountdownTimer : MonoBehaviour
{
    private int Hours, Minutes, Seconds;
    public Button ButStartTimer;
    public EventTrigger ButMinusTime, ButPlusTime;
    public Text TextTime;
    private int MyNumberTimer;

    public Animation WindowAnimation;
    public AnimationClip AnimationShow, AnimationHide;

    private bool IsStartedTimer, ClampingPlus, ClampingMinus;
    private float tmPlus, tmMinus, speed, rate = 1f;

    private void Awake()
    {
        SetTriggerButMinus();
        SetTriggerButPlus();
        ButStartTimer.onClick.AddListener(StartTimer);
    }

    public void SetTriggerButMinus()
    {
        EventTrigger trigger = ButMinusTime;

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { OnPointerDownMinus((PointerEventData)data); });
        trigger.triggers.Add(entry);

        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerUp;
        entry2.callback.AddListener((data) => { OnPointerUpMinus((PointerEventData)data); });
        trigger.triggers.Add(entry2);
    }

    public void SetTriggerButPlus()
    {
        EventTrigger trigger = ButPlusTime;

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) => { OnPointerDownPlus((PointerEventData)data); });
        trigger.triggers.Add(entry);

        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerUp;
        entry2.callback.AddListener((data) => { OnPointerUpPlus((PointerEventData)data); });
        trigger.triggers.Add(entry2);
    }

    private void OnPointerDownMinus(PointerEventData data)
    {
        MinusTime();
        tmMinus = 0;
        rate = 1f;
        ClampingMinus = true;
    }

    private void OnPointerUpMinus(PointerEventData data)
    {
        ClampingMinus = false;
    }

    private void OnPointerDownPlus(PointerEventData data)
    {
        PlusTime();
        tmPlus = 0;
        rate = 1f;
        ClampingPlus = true;
    }

    private void OnPointerUpPlus(PointerEventData data)
    {
        ClampingPlus = false;
    }

    public void ShowWindow ()
    {
        WindowAnimation.clip = AnimationShow;
        WindowAnimation.Play();
    }

    public void HideWindow()
    {
        WindowAnimation.clip = AnimationHide;
        WindowAnimation.Play();
    }

    public void LoadTimer (int _myNumberTimer)
    {
        MyNumberTimer = _myNumberTimer;
        Hours = PlayerPrefs.GetInt(Constants.Hours_Timer + MyNumberTimer);
        Minutes = PlayerPrefs.GetInt(Constants.Minutes_Timer + MyNumberTimer);
        Seconds = PlayerPrefs.GetInt(Constants.Seconds_Timer + MyNumberTimer);
    }

    private void SaveTimer()
    {
        PlayerPrefs.SetInt(Constants.Hours_Timer + MyNumberTimer, Hours);
        PlayerPrefs.SetInt(Constants.Minutes_Timer + MyNumberTimer, Minutes);
        PlayerPrefs.SetInt(Constants.Seconds_Timer + MyNumberTimer, Seconds);
    }

    private void StartTimer()
    {
        if (!IsStartedTimer)
        {
            if (Hours > 0 || Minutes > 0 || Seconds > 0)
            {
                TimersManager.Instance.OnResetTimer(MyNumberTimer);
                StartCoroutine(StartCountdown());
            }
        }
        TimersManager.Instance.ShowAllButtonsTimer();
        HideWindow();
    }

    private void PlusTime ()
    {
        Seconds += 1;
        UpdateTimerPlus();
    }

    private void MinusTime()
    {
        if (Hours <= 0 && Minutes <= 0 && Seconds <= 0)
            return;
        Seconds -= 1;
        UpdateTimerMinus();
    }

    private void UpdateTimerMinus ()
    {
        if (Seconds < 0)
        {
            if (Minutes > 0)
            {
                Minutes--;
                Seconds = 59;
            }
            else
            {
                if (Hours > 0)
                {
                    Hours--;
                    Minutes = 59;
                    Seconds = 59;
                }
            }
        }
    }

    private void UpdateTimerPlus()
    {
        if (Seconds > 59)
        {
            Minutes++;
            if(Minutes > 59)
            {
                Hours++;
                Minutes = 0;
            }
            Seconds = 0;
        }
    }

    private IEnumerator StartCountdown()
    {
        IsStartedTimer = true;
        while (Hours > 0 || Minutes > 0 || Seconds > 0)
        {
            yield return new WaitForSeconds(1.0f);
            Seconds--;
            UpdateTimerMinus();
        }
        TimersManager.Instance.OnCompeletedTimer(MyNumberTimer);
        IsStartedTimer = false;
    }

    private void ShowTime ()
    {
        TextTime.text = string.Format("{0:00}:{1:00}:{2:00}", Hours, Minutes, Seconds);
    }

    private void Update()
    {
        ShowTime();
        if(ClampingPlus)
        {
            tmPlus += Time.deltaTime;
            if (tmPlus >= ÑalculateRate())
            {
                PlusTime();
                tmPlus = 0;
            }
        }
        if (ClampingMinus)
        {
            tmMinus += Time.deltaTime;
            if (tmMinus >= ÑalculateRate())
            {
                MinusTime();
                tmMinus = 0;
            }
        }
    }

    private float ÑalculateRate ()
    {
        if (rate > 0.05f)
            rate = rate - Time.deltaTime / 3f;

        return rate;
    }

    private void OnDisable()
    {
        SaveTimer();
    }
}
