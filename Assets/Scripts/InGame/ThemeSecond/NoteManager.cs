using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    #region Static
    public static NoteManager GetInstance;
    private void Awake()
    {
        GetInstance = this;
    }
    #endregion

    [Header("NoteCanvas의 Panel")]
    [SerializeField] private List<GameObject> notePanelList = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < notePanelList.Count; i++)
        {
            notePanelList[i].SetActive(false);
        }
    }
    public void NotePanelOpen(int index)
    {
        notePanelList[index].SetActive(true);
    }

    public void NotePanelClose()
    {
        for (int i = 0; i < notePanelList.Count; i++)
        {
            notePanelList[i].SetActive(false);
        }
        GameManager.GetInstance.IsUIOpen = false;
    }
}
