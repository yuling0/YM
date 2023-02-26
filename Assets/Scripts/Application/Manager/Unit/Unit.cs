using UnityEngine;

public class Unit : MonoBehaviour ,IUnit
{
    [SerializeField]
    private int id;
    [SerializeField]
    private string unitName;
    private object unitInstacne;
    private UnitLogic unitLogic;
    private bool inited;
    private bool isVisible;
    private bool IsVisible
    {
        set
        {
            if (isVisible != value)
            {
                gameObject.SetActive(value);
                isVisible = value;
            }
        }
        get
        {
            return isVisible;
        }
    }
    public int ID => id;

    public string UnitName => unitName;

    public object UnitInstance => unitInstacne;

    public UnitLogic UnitLogic => unitLogic;
    public void OnHide(object userData = null)
    {
        IsVisible = false;
        unitLogic?.OnHide(userData);
    }

    public void OnInit(int id, string unitName, object unitInstance, object userData = null)
    {
        this.id = id;
        if (inited) return;
        inited = true;
        this.unitName = unitName;
        this.unitInstacne = unitInstance;
        IsVisible= true;
        unitLogic = ((GameObject)unitInstacne).GetComponent<UnitLogic>();
        unitLogic?.OnInit(this,userData);
    }

    public void OnRecycle(object userData = null)
    {
        IsVisible = false;
        unitLogic?.OnRecycle(userData);
    }

    public void OnShow(object userData = null)
    {
        IsVisible = true;
        unitLogic?.OnShow(userData);
    }

    public void OnUpdate()
    {
        unitLogic?.OnUpdate();
    }

    public void OnFixedUpdate()
    {
        unitLogic?.OnFixedUpdate();
    }
}
