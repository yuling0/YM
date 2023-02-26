using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFSM : ComponentBase, IFSM
{
    protected Dictionary<string, IState> stateDic;        //状态容器

    protected IState currentState;                        //当前处于的状态

    public bool isBattleState;

    protected SpriteRenderer spriteRenderer;

    public string currentStateStr;

    public sealed override void Init(Core core, object userData)
    {
        base.Init(core, userData);
        spriteRenderer = GetComponentInCore<AnimationController>().SpriteRenderer;
        InitFSM();
    }
    public virtual void InitFSM()
    {
        stateDic = new Dictionary<string, IState>();
    }
    //protected virtual void Start()
    //{
    //    ChangeState(Consts.S_Idle);
    //}
    public void ChangeState(string stateName)
    {

        if (stateDic.ContainsKey(stateName))
        {
            OnExit();
            currentState = stateDic[stateName];
            currentStateStr = stateName;
            OnEnter();
        }
        else
        {
            Debug.Log($"当前未找到状态名为{stateName}的状态");
        }

    }

    public IState GetCurrentState()
    {
        return currentState;
    }

    public void OnEnter()
    {
        currentState?.OnEnter();
    }

    public void OnExit()
    {
        currentState?.OnExit();
    }


    public virtual void OnUpdate()
    {
        currentState?.OnUpdate();
    }

    public void OnFixedUpdate()
    {
        currentState?.OnFixedUpdate();
    }
    public override void OnUpdateComponent()
    {
        base.OnUpdateComponent();
        
        OnConditionUpdate();

        OnUpdate();
    }

    public override void OnFixedUpdateComponent()
    {
        OnFixedUpdate();
    }

    public void OnConditionUpdate()
    {
        currentState?.OnConditionUpdate();
    }

    public void AnimationEventTrigger()
    {
        currentState?.AnimationEventTrigger();
    }

    public virtual void OnceAttackTrigger(int id)
    {

    }
    public virtual void ContinuousAttackEnter(int id)
    {

    }

    public virtual void ContinuousAttackExit(int id)
    {

    }

    public virtual void GenerateGhosting()
    {
        UnitManager.Instance.ShowUnit(Consts.U_Ghosting,_core.transform.position, spriteRenderer);
    }

}
