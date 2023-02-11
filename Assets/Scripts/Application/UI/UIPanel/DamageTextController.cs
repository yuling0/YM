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
}
public class DamageTextController : UIComponent
{
    IObjectPool<DamageTextObject> damageTextPool;
    List<DamageText> updateList;
    static string damageTextPrefabPath = Consts.P_UI + "DamageText";
    public override void OnInit()
    {
        base.OnInit();
        damageTextPool = ObjectPoolManager.Instance.CreateObjectPool<DamageTextObject>(nameof(damageTextPool));
        updateList = new List<DamageText>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

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
    public void GenerateDamageText(int damageValue,Vector2 screenPosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_rt, screenPosition, null, out Vector2 localPoint);

        DamageTextObject damageTextObject = damageTextPool.Spawn();
        if (damageTextObject == null)
        {
            ResourceMgr.Instance.LoadAssetAsync<DamageText>(damageTextPrefabPath, (text) => 
            {
                DamageTextObject textObject = DamageTextObject.Create(text);
                damageTextPool.Register(textObject, true);
                updateList.Add(text);
                text.transform.localPosition = localPoint;
                text.OnInit(damageValue);
                text.transform.SetParent(this.transform, false);
            });
        }
        else
        {

            updateList.Add(damageTextObject.DamageText);
            damageTextObject.DamageText.transform.localPosition = localPoint;
            damageTextObject.DamageText.OnInit(damageValue);
        }
    }


}
