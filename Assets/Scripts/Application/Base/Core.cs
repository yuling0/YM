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

# if UNITY_EDITOR
    private void Awake()
    {
        OnInit(null, null);
    }
    private void OnEnable()
    {
        foreach (var c in _components)
        {
            c.OnEnableComponent();
        }
    }
    private void OnDisable()
    {
        foreach (var c in _components)
        {
            c.OnDisableComponent();
        }
    }
    public void Update()
    {
        foreach (var c in _components)
        {
            c.OnUpdateComponent();
        }
    }

    public void FixedUpdate()
    {
        foreach (var c in _components)
        {
            c.OnFixedUpdateComponent();
        }
    }
#endif
    public override void OnInit(Unit unit, object userData)
    {
        base.OnInit(unit, userData);
        InitComponent();
    }

    public override void OnShow(object userData)
    {
        foreach (var c in _components)
        {
            c.OnEnableComponent();
        }
    }

    public override void OnHide(object userData)
    {
        foreach (var c in _components)
        {
            c.OnDisableComponent();
        }
    }

    public override void OnUpdate()
    {
        foreach (var c in _components)
        {
            c.OnUpdateComponent();
        }
    }

    public override void OnFixedUpdate()
    {
        foreach (var c in _components)
        {
            c.OnFixedUpdateComponent();
        }
    }

    public virtual void InitComponent() 
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
            _components[i].Init(this);
        }
    }
    public int ID => unit.ID;
    public string UnitName => info.unitName;
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
