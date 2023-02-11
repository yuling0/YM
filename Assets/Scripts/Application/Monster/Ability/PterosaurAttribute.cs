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
        //����͵��ֱ��д�� ��Ϊֻ�������������˲�λ������Ҫдһ������ж������λ�Ƿ������������С�     
        if (args.MonsterObj.CompareTag("Weakness"))
        {
            //���˲�λ������
            //���û���ֵ
            movementController.SetVelocityX(args.RepelVelocity, false);
            float targetHp = CurHP > args.DamageValue ? CurHP - args.DamageValue : 0;
            aiController.SetHert();
            //֪ͨ������Ѫ��
        }
        else if(args.MonsterObj.CompareTag("DamageDetectObject"))
        {
            //���˲�λ��������
        }
        //Debug.Log(args.MonsterObj.name);
    }
}
