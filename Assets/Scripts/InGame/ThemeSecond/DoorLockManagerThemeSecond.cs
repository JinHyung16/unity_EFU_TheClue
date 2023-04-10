using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DoorLockManagerThemeSecond : MonoBehaviour
{
    [Header("DoorLock Key Pad 관련")]
    [SerializeField] private List<Button> buttonList = new List<Button>();
    [SerializeField] private TMP_Text displayText;

    private string input;

    private void Start()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].onClick.AddListener(InputButton);
        }

        displayText.text = null;
        input = null;
    }
    /// <summary>
    /// button 클릭시 호출되는 함수
    /// </summary>
    private void InputButton()
    {
        input = EventSystem.current.currentSelectedGameObject.name;
        UpdateDoorLockDisplay(input);
    }

    private void UpdateDoorLockDisplay(string idx)
    {
        if (4 < displayText.text.Length)
        {
            return;
        }
        displayText.text += idx;
    }

    public void ClearDisplay()
    {
        displayText.text = null;
        input = null;
    }

    public void InputDone()
    {
        if (!string.IsNullOrEmpty(displayText.text))
        {
            ThemeSecondPresenter.GetInstance.DoneDoorLock(displayText.text);
        }
    }
}
