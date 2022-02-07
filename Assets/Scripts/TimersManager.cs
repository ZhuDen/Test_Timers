using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimersManager : MonoBehaviour
{
    public static TimersManager Instance;

    [Header("Задайте сколько нужно таймеров")]
    public int CountButtonTimers;

    public float DelaySpawnButtons;
    public GameObject PrefabButtonTimer, PrefabWindowTimer;
    public Transform ParentButtonTimers, ParentWindowTimer;

    public string ExtensionTextButton = "Таймер"; // по хорошему конечно лучше из локализации подгружать :)

    private GameObject[] WindowsTimers;
    private List<GameObject> ButtonsTimer = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        WindowsTimers = new GameObject[CountButtonTimers];
    }

    private void Start()
    {
        StartCoroutine(SpawnButtons());
    }

    private IEnumerator SpawnButtons ()
    {
        int countTimers = 0;
        while(countTimers < CountButtonTimers)
        {
            GameObject butTime = Instantiate(PrefabButtonTimer, ParentButtonTimers);
            ButtonsTimer.Add(butTime);
            butTime.GetComponent<ButTimer>().SetTextInButton(ExtensionTextButton + " " + (countTimers + 1));
            int index = countTimers; // Так нужно делать в Unity, иначе не будет работать
            butTime.GetComponent<ButTimer>().ButtonTimer.onClick.AddListener(delegate { ShowWindowTimer(index); });
            butTime.GetComponent<ButtonAnimationsMove>().ShowButton();
            countTimers ++;
            yield return new WaitForSeconds(DelaySpawnButtons);
        }

        ParentButtonTimers.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void HideAllButtonsTimer ()
    {
        ParentButtonTimers.GetComponent<CanvasGroup>().blocksRaycasts = false;
        StopCoroutine(DelayShowAllButtonsTimer());
        StartCoroutine(DelayHideAllButtonsTimer());
    }

    private IEnumerator DelayHideAllButtonsTimer()
    {
        for (int i = 0; i < ButtonsTimer.Count; i++)
        {
            ButtonsTimer[i].GetComponent<ButtonAnimationsMove>().HideButton();
            yield return new WaitForSeconds(DelaySpawnButtons);
        }
    }

    public void ShowAllButtonsTimer()
    {
        StopCoroutine(DelayHideAllButtonsTimer());
        StartCoroutine(DelayShowAllButtonsTimer());
    }

    private IEnumerator DelayShowAllButtonsTimer()
    {
        for (int i = 0; i < ButtonsTimer.Count; i++)
        {
            ButtonsTimer[i].GetComponent<ButtonAnimationsMove>().ShowButton();
            yield return new WaitForSeconds(DelaySpawnButtons);
        }
        ParentButtonTimers.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    private void ShowWindowTimer (int number)
    {
        HideAllButtonsTimer();
        if (WindowsTimers[number] == null)
        {
            WindowsTimers[number] = Instantiate(PrefabWindowTimer, ParentWindowTimer);
            WindowsTimers[number].GetComponent<CountdownTimer>().LoadTimer(number);
            WindowsTimers[number].GetComponent<CountdownTimer>().ShowWindow();
        }
        else
        {
            WindowsTimers[number].GetComponent<CountdownTimer>().ShowWindow();
        }
    }

    public void OnCompeletedTimer (int number)
    {
        ButtonsTimer[number].GetComponent<ButTimer>().SetColorButton(Color.green);
    }

    public void OnResetTimer(int number)
    {
        ButtonsTimer[number].GetComponent<ButTimer>().SetColorButton(Color.white);
    }
}
