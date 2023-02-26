using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.ObjectPool;

public class DamageTextObject : ObjectBase
{
    private DamageText damageText;
    public DamageText DamageText => damageText;
    public static DamageTextObject Create(DamageText damageText)
    {
        DamageTextObject damageTextObject = ReferencePool.Instance.Acquire<DamageTextObject>();
        damageTextObject.Init(damageText);
        damageTextObject.damageText = damageText;
        Debug.Log(damageText.name);
        return damageTextObject;
    }
    public override void Release()
    {
        base.Release();
        damageText = null;
    }

    internal override void OnUnSpawn()
    {
        damageText.UnSpawn();
    }
}
public class DamageTextController
{
    IObjectPool<DamageTextObject> damageTextPool;
    List<DamageText> updateList;
    static string damageTextPrefabPath = Consts.P_UI + "DamageText";
    GameObject damageNumberObjectContainer;

    public DamageTextController()
    {
        damageNumberObjectContainer = new GameObject("DamageNumberObjectContainer");
        Object.DontDestroyOnLoad(damageNumberObjectContainer);
        damageTextPool = ObjectPoolManager.Instance.CreateObjectPool<DamageTextObject>(nameof(damageTextPool));
        updateList = new List<DamageText>();
        EventMgr.Instance.AddMultiParameterEventListener<DamageTextEventArgs>(GenerateDamageText);
    }

    public void OnUpdate()
    {
        for (int i = updateList.Count - 1; i >= 0 ; i --)
        {
            DamageText cur = updateList[i];
            if (cur.IsNeedUnSpwan)
            {
                damageTextPool.UnSpawn(cur);
                updateList.RemoveAt(i);
            }
            else
            {
                cur.OnUpdate();
            }
        }
    }
    private void GenerateDamageText(DamageTextEventArgs args)
    {
        GenerateDamageText(args.Damage, args.ScreenPosition);
        ReferencePool.Instance.Release(args);
    }
    private void GenerateDamageText(int damageValue,Vector2 worldPosition)
    {
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(_rt, screenPosition, null, out Vector2 localPoint);

        DamageTextObject damageTextObject = damageTextPool.Spawn();
        if (damageTextObject == null)
        {
            ResourceMgr.Instance.LoadAssetAsync<DamageText>(damageTextPrefabPath, (text) => 
            {
                DamageTextObject textObject = DamageTextObject.Create(text);
                damageTextPool.Register(textObject, true);
                updateList.Add(text);
                text.transform.position = worldPosition;
                text.OnInit(damageValue);
                text.transform.SetParent(damageNumberObjectContainer.transform, true);
            });
        }
        else
        {
            updateList.Add(damageTextObject.DamageText);
            damageTextObject.DamageText.transform.position = worldPosition;
            damageTextObject.DamageText.OnInit(damageValue);
        }
    }

    private void Release()
    {
        foreach (DamageText damageText in updateList)
        {
            damageTextPool.UnSpawn(damageText);
        }
    }
}
