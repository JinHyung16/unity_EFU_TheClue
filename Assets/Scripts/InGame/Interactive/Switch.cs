using HughEnumData;
using UnityEngine;

public class Switch : InteractiveObject
{
    [SerializeField] private Transform switchTransform;
    [SerializeField] private Transform switchBtnTransform;

    private Vector3 offset;

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
        GameManager.GetInstance.VisibleInteractiveCanvas(switchTransform, offset);
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
        return InteractiveType.ThemeFirst_Switch;
    }

    #endregion

    private void Start()
    {
        switchBtnTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        offset = new Vector3(0, 1.0f, -0.3f);
    }

    public void SwitchButtonRotate()
    {
        if (ThemeFirstPresenter.GetInstance.IsSwitchOn)
        {
            switchBtnTransform.Rotate(-20.0f, 0, 0);
        }
        else
        {
            switchBtnTransform.Rotate(20.0f, 0, 0);
        }
    }
}
