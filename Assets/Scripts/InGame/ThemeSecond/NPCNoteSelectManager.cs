using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCNoteSelectManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> noteList = new List<GameObject>();
    [SerializeField] private List<Transform> noteTransformList = new List<Transform>();

    #region Static
    public static NPCNoteSelectManager GetInstance;
    private void Awake()
    {
        GetInstance = this;
    }
    #endregion

    private void Start()
    {
        for (int i = 0; i < noteList.Count; i++)
        {
            noteList[i].SetActive(false);
        }
    }

    public void NoteVisibleToSelect()
    {
        for (int i = 0; i < noteList.Count; i++)
        {
            noteList[i].transform.position = noteTransformList[i].transform.position;
            noteList[i].transform.rotation = noteTransformList[i].transform.rotation;
            noteList[i].SetActive(true);
        }
    }

    public void NoteInvisible()
    {
        for (int i = 0; i < noteList.Count; i++)
        {
            noteList[i].transform.position = new Vector3(0, 0, 0);
            noteList[i].SetActive(false);
        }
    }
}
