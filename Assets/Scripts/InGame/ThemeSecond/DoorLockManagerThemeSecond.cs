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

    private string password;
    private void Start()
    {
        for (int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].onClick.AddListener(InputButton);
        }

        displayText.text = null;
        password = null;
    }

    private void OnEnable()
    {
        password = null;
        displayText.text = null;
    }
    /// <summary>
    /// button 클릭시 호출되는 함수
    /// </summary>
    private void InputButton()
    {
        UpdateDoorLockDisplay(EventSystem.current.currentSelectedGameObject.name);
    }

    private void UpdateDoorLockDisplay(string idx)
    {
        password += idx;
        if (password.Length <= 4)
        {
            displayText.text = password;
        }
    }

    public void ClearDisplay()
    {
        displayText.text = null;
        password = null;
    }

    public void InputDone()
    {
        if (!string.IsNullOrEmpty(displayText.text))
        {
            ThemeSecondPresenter.GetInstance.DoneDoorLock(displayText.text);
            displayText.text = null;
            password = null;
        }
    }
}
