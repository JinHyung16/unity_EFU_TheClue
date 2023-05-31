using HughEnumData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : InteractiveObject
{
    [SerializeField] private Transform noteTrans;
    [Header("DoorKey UI sprite")]
    [SerializeField] private Sprite noteImage;

    public Sprite GetNoteUISprite { get { return this.noteImage; } }

    private Vector3 offset;

    //NoteCanvas에서 Note Panel을 열 때 index로 사용, Blue = 0, Pink = 1, Yellow = 2;
    public int noteIndex;

    private void Start()
    {
        offset = new Vector3(0, 0.3f, 0);
    }

    #region InteractiveObject Override
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ThemeSecondPresenter.GetInstance != null && ThemeSecondPresenter.GetInstance.InteractiveTypeNum != 1)
            {
                InteractiveManager.GetInstance.IsInteractive = true;
                this.Interacitve();
            }
        }
    }

    protected override void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractiveManager.GetInstance.IsInteractive = true;
            this.Interacitve();
        }
    }
    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InteractiveManager.GetInstance.IsInteractive = false;
            this.NotInteractvie();
        }
    }

    protected override void Interacitve()
    {
        GameManager.GetInstance.VisibleInteractiveCanvas(noteTrans, offset);
        InteractiveManager.GetInstance.SetInteractiving(this);
    }

    protected override void NotInteractvie()
    {
        GameManager.GetInstance.InvisibleInteractiveCanvas();
        InteractiveManager.GetInstance.SetInteractvieObjToInventory(null);
    }

    public override InteractiveType GetInteractiveType()
    {
        return InteractiveType.ThemeSeecond_Note;
    }

    #endregion
}
