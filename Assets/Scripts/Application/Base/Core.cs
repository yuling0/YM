using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 核心组件：充当中介者
/// </summary>
public class Core : UnitLogic
{
    private Dictionary<string, ComponentBase> _componentDir;
    private ComponentBase[] _components;
    [InlineEditor]
    public UnitInfo info;
    [SerializeField]
    private bool enable;
//# if UNITY_EDITOR
//    private void Awake()
//    {
//        OnInit(null, null);
//    }
//    private void OnEnable()
//    {
//        foreach (var c in _components)
//        {
//            c.OnEnableComponent(null);
//        }
//    }
//    private void OnDisable()
//    {
//        foreach (var c in _components)
//        {
//            c.OnDisableComponent(null);
//        }
//    }
//    public void Update()
//    {
//        foreach (var c in _components)
//        {
//            c.OnUpdateComponent();
//        }
//    }

//    public void FixedUpdate()
//    {
//        foreach (var c in _components)
//        {
//            c.OnFixedUpdateComponent();
//        }
//    }
//#endif

    public bool Enable
    {
        set
        {
            if (value)
            {
                EnableComponent();
            }
            else
            {
                DisableComponent();
            }
            enable = value;
        }
        get { return enable; }
    }
    public override void OnInit(Unit unit, object userData)
    {
        base.OnInit(unit, userData);
        InitComponent(userData);
    }

    public override void OnShow(object userData)
    {
        foreach (var c in _components)
        {
            c.OnShowUnit(userData);
        }
        Enable = true;
    }

    private void EnableComponent()
    {
        foreach (var c in _components)
        {
            c.OnEnableComponent();
        }
    }
    private void DisableComponent()
    {
        foreach (var c in _components)
        {
            c.OnDisableComponent();
        }
    }
    public override void OnHide(object userData)
    {
        foreach (var c in _components)
        {
            c.OnHideUnit(userData);
        }
    }
    public override void OnRecycle(object userData)
    {
        foreach (var c in _components)
        {
            c.OnRecycle(userData);
        }
    }
    public override void OnUpdate()
    {
        if (!enable) return;
        foreach (var c in _components)
        {
            c.OnUpdateComponent();
        }
    }

    public override void OnFixedUpdate()
    {
        if (!enable) return;
        foreach (var c in _components)
        {
            c.OnFixedUpdateComponent();
        }
    }

    public virtual void InitComponent( object userData) 
    {
        _componentDir = new Dictionary<string, ComponentBase>();

        _components = GetComponentsInChildren<ComponentBase>();


        for (int i = 0; i < _components.Length; i++)
        {
            string cn = _components[i].GetType().Name;

            if (_componentDir.ContainsKey(cn))
            {
                Debug.Log($"游戏对象{gameObject.name}有多个 {cn} 组件");
            }
            else
            {
                _componentDir.Add(cn, _components[i]);
            }

        }

        for (int i = 0; i < _components.Length; i++)
        {
            _components[i].Init(this, userData);
        }
    }
    public int ID => unit.ID;
    public string UnitName => unit.UnitName;
    public T GetComponentInCore<T>() where T : ComponentBase
    {
        string cn = typeof(T).Name;

        if (_componentDir.ContainsKey(cn))
        {
            return _componentDir[cn] as T;
        }

        foreach (var item in _componentDir.Values)
        {
            if(item is T)
            {
                return item as T;
            }
        }

        Debug.Log($"游戏对象{gameObject.name}没有这个 {cn} 组件");
        return null;
    }




}
