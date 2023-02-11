using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using YMFramework.ObjectPool;
using static UnityEngine.UI.CanvasScaler;

public class UnitManager : SingletonBase<UnitManager>
{
    private Dictionary<string, LinkedList<Unit>> nameUnitMap;
    private Dictionary<int, Unit> idUnitMap;
    private PoolMgr poolMgr;
    private UnitDataContainer unitDataContainer;
    private IObjectPool<UnitObject> objectPool;
    private GameObject unitManagerObj;
    private HashSet<Unit> addUnits;
    private HashSet<Unit> reclyceUnits;
    private int serialId;
    private UnitManager ()
    {
        nameUnitMap = new Dictionary<string, LinkedList<Unit>>();
        idUnitMap = new Dictionary<int, Unit>();
        addUnits = new HashSet<Unit>();
        reclyceUnits = new HashSet<Unit>();
        poolMgr = PoolMgr.Instance;
        unitDataContainer = BinaryDataManager.Instance.GetContainer<UnitDataContainer>();
        objectPool = ObjectPoolManager.Instance.CreateObjectPool<UnitObject>("UnitObject");
        unitManagerObj = new GameObject(nameof(UnitManager));
        Object.DontDestroyOnLoad(unitManagerObj);
        MonoMgr.Instance.AddUpateAction(OnUpdate);
        MonoMgr.Instance.AddFixedUpateAction(OnFixedUpdate);
        EventMgr.Instance.AddEventListener(Consts.E_OnHideSceneComplete, Release);
        serialId = 0;
    }
    public void ShowUnit(string unitName, object userData, UnityAction<Unit> onShowComplete = null)
    {
        ShowUnit(unitName, Vector3.zero, userData, onShowComplete);
    }
    public void ShowUnit(string unitName ,Vector3 position, object userData, UnityAction<Unit> onShowComplete = null)
    {
        if (string.IsNullOrEmpty(unitName))
        {
            Debug.LogError("Unit name is involid.");
        }
        UnitData unitData = unitDataContainer.GetUnitData(unitName);
        string path = unitData.resourcePath;
        UnitObject unitObject = objectPool.Spawn(path);
        int id = unitData.isUnique ? unitData.id : serialId++;
        if (unitObject == null)
        {
            ResourceMgr.Instance.LoadAssetAsync<GameObject>(path,(obj) => 
            {
                unitObject = UnitObject.Create(path, obj, unitManagerObj);
                objectPool.Register(unitObject, true);
                obj.transform.position = position;
                Unit unit = obj.GetOrAddComponent<Unit>();
                unit.OnInit(id, unitName, obj , userData);
                addUnits.Add(unit);
                unit.OnShow(userData);
                onShowComplete?.Invoke(unit);
            });
        }
        else
        {
            GameObject obj = unitObject.Target as GameObject;
            Unit unit = obj.GetOrAddComponent<Unit>();
            unit.OnInit(id, unitName, obj , userData);
            obj.transform.position = position;
            addUnits.Add(unit);
            unit.OnShow(userData);
            onShowComplete?.Invoke(unit);
        }
    }
    public void ShowUnit(int id ,string unitPath,Vector3 position,object userData,UnityAction<Unit> onShowComplete = null)
    {
        UnitObject unitObject = objectPool.Spawn(unitPath);
        if (unitObject == null)
        {
            ResourceMgr.Instance.LoadAssetAsync<GameObject>(unitPath, (obj) =>
            {
                unitObject = UnitObject.Create(unitPath, obj, unitManagerObj);
                objectPool.Register(unitObject, true);
                obj.transform.position = position;
                Unit unit = obj.GetOrAddComponent<Unit>();
                unit.OnInit(id, obj.name, obj, userData);
                addUnits.Add(unit);
                unit.OnShow(userData);
                onShowComplete?.Invoke(unit);
            });
        }
        else
        {
            GameObject obj = unitObject.Target as GameObject;
            Unit unit = obj.GetOrAddComponent<Unit>();
            unit.OnInit(id, obj.name, obj, userData);
            obj.transform.position = position;
            addUnits.Add(unit);
            unit.OnShow(userData);
            onShowComplete?.Invoke(unit);
        }
    }

    public void ShowUnit(int id , Vector3 position , object userData ,UnityAction<Unit> onShowComplete = null)
    {
        UnitData unitData = unitDataContainer.GetUnitData(id);
        if (unitData == null)
        {
            Debug.Log($"{id} is involid");
        }
        ShowUnit(unitData.unitName, position, userData,onShowComplete);
    }
    public void HideUnit(int id , object userData)
    {
        if (idUnitMap.ContainsKey(id))
        {
            idUnitMap[id].OnHide();
            reclyceUnits.Add(idUnitMap[id]);
        }
    }

    public void HideUnit(Unit unit , object userData)
    {
        HideUnit(unit.ID,userData);
    }

    public Unit GetUnit(int id)
    {
        if (idUnitMap.ContainsKey(id))
        {
            return idUnitMap[id];
        }
        return null;
    }

    public Unit GetUnit(string unitName)
    {
        if (nameUnitMap.ContainsKey(unitName) && nameUnitMap[unitName].Count > 0)
        {
            return nameUnitMap[unitName].First.Value;
        }
        return null;
    }

    public Unit[] GetUnits(string unitName)
    {
        if (nameUnitMap.ContainsKey(unitName) && nameUnitMap[unitName].Count > 0)
        {
            return nameUnitMap[unitName].ToArray();
        }
        return null;
    }

    public void Release()
    {
        foreach ( Unit unit in idUnitMap.Values)
        {
            unit.OnRecycle();
            objectPool.UnSpawn(unit.UnitInstance);
        }
        nameUnitMap.Clear();
        idUnitMap.Clear();
    }

    public void OnUpdate()
    {
        foreach (Unit unit in idUnitMap.Values)
        {
            if (reclyceUnits.Contains(unit))
            {
                continue;
            }
            unit.OnUpdate();
        }

        foreach (Unit unit in addUnits)
        {
            if (!nameUnitMap.ContainsKey(unit.UnitName))
            {
                nameUnitMap.Add(unit.UnitName, new LinkedList<Unit>());
            }
            nameUnitMap[unit.UnitName].AddLast(unit);

            if (idUnitMap.ContainsKey(unit.ID))
            {
                Debug.LogError($"Unit is exist where id equals {unit.ID}");
            }
            idUnitMap.Add(unit.ID, unit);
        }

        foreach (Unit unit in reclyceUnits)
        {
            if (nameUnitMap.ContainsKey(unit.UnitName))
            {
                nameUnitMap[unit.UnitName].Remove(unit);
            }

            if (idUnitMap.ContainsKey(unit.ID))
            {
                idUnitMap.Remove(unit.ID);
            }
            objectPool.UnSpawn(unit.UnitInstance);
        }

        addUnits.Clear();
        reclyceUnits.Clear();
    }

    public void OnFixedUpdate()
    {
        foreach (Unit unit in idUnitMap.Values)
        {
            if (reclyceUnits.Contains(unit))
            {
                continue;
            }
            unit.OnFixedUpdate();
        }

        foreach (Unit unit in addUnits)
        {
            if (!nameUnitMap.ContainsKey(unit.UnitName))
            {
                nameUnitMap.Add(unit.UnitName, new LinkedList<Unit>());
            }
            nameUnitMap[unit.UnitName].AddLast(unit);

            if (idUnitMap.ContainsKey(unit.ID))
            {
                Debug.LogError($"Unit is exist where id equals {unit.ID}");
            }
            idUnitMap.Add(unit.ID, unit);
        }

        foreach (Unit unit in reclyceUnits)
        {
            if (nameUnitMap.ContainsKey(unit.UnitName))
            {
                nameUnitMap[unit.UnitName].Remove(unit);
            }

            if (idUnitMap.ContainsKey(unit.ID))
            {
                idUnitMap.Remove(unit.ID);
            }
            objectPool.UnSpawn(unit.UnitInstance);
        }

        addUnits.Clear();
        reclyceUnits.Clear();
    }
}
