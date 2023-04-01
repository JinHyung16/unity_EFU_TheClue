using DG.Tweening;
using HughGenerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeSecondPresenter : PresenterSingleton<ThemeSecondPresenter>
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera interactiveCam;
    [SerializeField] private Transform interactiveCamTransform;
    private Vector3 camOffset;

    [SerializeField] private ThemeSecondViewer themeSecondViewer;

    [Header("Switch SpotLight")]
    [SerializeField] private Light switchSpotLight;

    private string themeName = "ThemeSecond";
    protected override void OnAwake()
    {
        switchSpotLight.color = Color.white;
        switchSpotLight.enabled = true;
    }

    private void Start()
    {
        themeName = "ThemeSecond";
        SceneController.GetInstance.CurSceneName = themeName;

        GameManager.GetInstance.SpawnPlayer();
        GameManager.GetInstance.CameraTheme = this.mainCamera;
        GameManager.GetInstance.CameraInteractive = this.interactiveCam;
        this.mainCamera.cullingMask = 0;

        GameManager.GetInstance.IsUIOpen = false;
        GameManager.GetInstance.IsInputStop = false;

        TimerManager.GetInstance.ThemeTime = 900.0f;

        camOffset = new Vector3(-2.0f, 0, 0.0f);
    }

    /// <summary>
    /// Switch On or Off를 통해 불빛 켜고 끄기
    /// </summary>
    public void SwitchOnOff()
    {
        if (switchSpotLight.enabled)
        {
            switchSpotLight.enabled = false;
        }
        else
        {
            switchSpotLight.enabled = true;
            if (TimerManager.GetInstance.CurMinTime <= 10)
            {
                switchSpotLight.color = Color.blue;
            }
            else
            {
                switchSpotLight.color = Color.red;
            }
        }
    }

    public void DoorInteractive(bool active)
    {
        GameObject obj = InventoryManager.GetInstance.GetInvenObject();
        if (active)
        {
            if (obj != null)
            {
                obj.transform.position = interactiveCamTransform.position + camOffset;
                obj.transform.LookAt(interactiveCamTransform);
                obj.SetActive(true);
            }
            interactiveCam.depth = 1;
            GameManager.GetInstance.PlayerCameraControl(false);
            themeSecondViewer.DoorCanvasOpen();
        }
        else
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
            interactiveCam.depth = 0;
            GameManager.GetInstance.PlayerCameraControl(true);
            themeSecondViewer.CloseCanvas();
        }
    }

    #region Inventory에 들어갈 object 함수
    public void DoorKeyInventory(GameObject obj)
    {
        var doorKey = obj.GetComponent<DoorKey>();
        InventoryManager.GetInstance.PutInInventory(obj, doorKey.GetDoorKeyUISprite, UnityEngine.Color.white); ;
    }

    public void NoteInventory(GameObject obj)
    {
        var note = obj.GetComponent<Note>();
        InventoryManager.GetInstance.PutInInventory(obj, note.GetNoteUISprite, UnityEngine.Color.white); ;
    }
    #endregion
}
