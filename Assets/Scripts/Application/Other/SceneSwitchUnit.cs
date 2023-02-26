using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitchUnit : UnitLogic
{
    public Vector2 triggerRange;
    private InputMgr inputMgr;
    private bool isUpPress;
    private bool inRanage;
    [SerializeField]
    private string sceneSwitchData;
    public override void OnInit(Unit unit, object userData)
    {
        base.OnInit(unit, userData);
        inputMgr = InputMgr.Instance;
    }
    public override void OnShow(object userData)
    {
        base.OnShow(userData);
        sceneSwitchData = userData as string;
    }
    public override void OnUpdate()
    {
        if (inRanage && inputMgr.GetKeyDown(Consts.K_Up))
        {
            print("´¥·¢ÇÐ»»³¡¾°");
            YMSceneManager.Instance.TrySwitchScene(sceneSwitchData);
        }
    }
//#if UNITY_EDITOR
//    private void Awake()
//    {
//        inputMgr = InputMgr.Instance;
//    }

//    private void Update()
//    {
//        if (inRanage && inputMgr.GetKeyDown(Consts.K_Up))
//        {
//            print("´¥·¢ÇÐ»»³¡¾°");
//        }
//    }
//#endif

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRanage = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inRanage = false;
        }
    }
}
