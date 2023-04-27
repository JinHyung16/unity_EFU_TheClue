using HughEnumData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : InteractiveObject
{
    [Header("DoorKey UI sprite")]
    [SerializeField] private Sprite noteImage;

    public Sprite GetNoteUISprite { get { return this.noteImage; } }

    //NoteCanvas에서 Note Panel을 열 때 index로 사용, Blue = 0, Pink = 1, Yellow = 2;
    public int noteIndex;

    public void SelectNote()
    {
        ThemeSecondPresenter.GetInstance.IsNPCFirstTalk = true;
        ThemeSecondPresenter.GetInstance.NPCInteractiveSelectNote(false);
    }

    #region InteractiveObject Override
    protected override void OnTriggerEnter(Collider other)
    {
    }

    protected override void OnTriggerStay(Collider other)
    {
    }
    protected override void OnTriggerExit(Collider other)
    {
    }

    protected override void Interacitve()
    {
        //InteractiveManager.GetInstance.SetInteractiving(this);
        //InteractiveManager.GetInstance.SetInteractvieObjToInventory(this.gameObject);
    }

    protected override void NotInteractvie()
    {
        //GameManager.GetInstance.InvisibleInteractiveCanvas();
        //InteractiveManager.GetInstance.SetInteractvieObjToInventory(null);
    }

    public override InteractiveType GetInteractiveType()
    {
        return InteractiveType.ThemeSecond_NPC;
    }

    #endregion
}
