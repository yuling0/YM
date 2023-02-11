using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PterosaurAttribute : CharacterAttibute
{
    PterosaurAI aiController;
    PterosaurInfo info;
    PterosaurMovementController movementController;
    protected override void InitAbility()
    {
        base.InitAbility();
        aiController = GetComponentInCore<PterosaurAI>();
        movementController = GetComponentInCore<PterosaurMovementController>();
        info = _info as PterosaurInfo;
        EventMgr.Instance.AddMultiParameterEventListener<MonsterBeHitEventArgs>(Hurt);
    }

    private void Hurt(MonsterBeHitEventArgs args)
    {
        //这里偷懒直接写死 因为只有翼龙才有损伤部位，本来要写一个组件判断这个部位是否存在这个对象中。     
        if (args.MonsterObj.CompareTag("Weakness"))
        {
            //损伤部位是弱点
            //设置击退值
            movementController.SetVelocityX(args.RepelVelocity, false);
            float targetHp = CurHP > args.DamageValue ? CurHP - args.DamageValue : 0;
            aiController.SetHert();
            //通知面板更新血量
        }
        else if(args.MonsterObj.CompareTag("DamageDetectObject"))
        {
            //损伤部位不是弱点
        }
        //Debug.Log(args.MonsterObj.name);
    }
}
