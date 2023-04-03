using HughEnumData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : InteractiveObject
{
    [SerializeField] private Transform noteTransform;

    [Header("DoorKey UI sprite")]
    [SerializeField] private Sprite noteImage;

    private Vector3 offset;
    public Sprite GetNoteUISprite { get { return this.noteImage; } }

    private void Start()
    {
        offset = new Vector3(0, 0.8f, 0);
    }

    private void OnDisable()
    {
        GameManager.GetInstance.InvisibleInteractiveCanvas();
        NotInteractvie();
        this.gameObject.transform.position = noteTransform.position;
    }
    #region InteractiveObject Override
    protected override void OnTriggerEnter(Collider other)
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
        GameManager.GetInstance.VisibleInteractiveCanvas(noteTransform, offset);
        InteractiveManager.GetInstance.SetInteractiving(this);
        InteractiveManager.GetInstance.SetInteractvieObjToInventory(this.gameObject);
    }

    protected override void NotInteractvie()
    {
        GameManager.GetInstance.InvisibleInteractiveCanvas();
        InteractiveManager.GetInstance.SetInteractvieObjToInventory(null);
    }

    public override InteractiveType GetInteractiveType()
    {
        return InteractiveType.ThemeSecond_Note;
    }

    #endregion
}
