using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel
{

    public Image img_PlayerHP;
    public Image img_PlayerMP;
    public Image img_PlayerStateBar;
    public Image img_PlayerIcon;
    public Text txt_PlayerName;
    public Text txt_PlayerLevel;
    public Image img_MonsterStateBar;
    public Image img_MonsterBG;
    public Image img_MonsterIcon;
    public Image img_MonsterHP;

    private SpriteRenderer _curMonsterSpriteRenderer;       //当前怪物的精灵渲染组件
    private MonsterAttribute _curMonsterAbility;              //当前怪物能力组件
    private DamageTextController damageTextController;
    public override void OnInit()
    {
        base.OnInit();
		img_PlayerHP = GetControl<Image>("img_PlayerHP");
		img_PlayerMP = GetControl<Image>("img_PlayerMP");
		img_PlayerStateBar = GetControl<Image>("img_PlayerStateBar");
		img_PlayerIcon = GetControl<Image>("img_PlayerIcon");
		txt_PlayerName = GetControl<Text>("txt_PlayerName");
		txt_PlayerLevel = GetControl<Text>("txt_PlayerLevel");
		img_MonsterStateBar = GetControl<Image>("img_MonsterStateBar");
		img_MonsterBG = GetControl<Image>("img_MonsterBG");
        img_MonsterIcon = GetControl<Image>("img_MonsterIcon");
		img_MonsterHP = GetControl<Image>("img_MonsterHP");
        damageTextController = GetControl<DamageTextController>("DamageTextController");
        _hidePanelSequence.onComplete = null;
        EventMgr.Instance.AddMultiParameterEventListener<UpdateMonsterUIInfoArgs>(UpdateMonsterUIInfo);
        EventMgr.Instance.AddMultiParameterEventListener<DamageTextEventArgs>(GenerateDamageText);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (InputMgr.Instance.GetKeyDown(Consts.K_Menu))
        {
            UIManager.Instance.Push<MainMenuPanel>();
            
        }
        img_MonsterIcon.sprite = _curMonsterSpriteRenderer?.sprite;
        //Debug.Log((transform as RectTransform).anchoredPosition);
    }

    public override void OnCover()
    {
        base.OnCover();
        _hidePanelSequence.Restart();
        GameManager.Instance.GamePauseOrContinue();
    }

    public override void OnReveal()
    {
        base.OnReveal();
        Debug.Log((_uIComponents[0].transform as RectTransform).anchoredPosition);
        Debug.Log((_uIComponents[1].transform as RectTransform).anchoredPosition);
        _showPanelSequence.Restart();
        GameManager.Instance.GamePauseOrContinue();
    }
    private void UpdateMonsterUIInfo(UpdateMonsterUIInfoArgs args)
    {
        _curMonsterAbility = args._monsterAbility;
        _curMonsterSpriteRenderer = _curMonsterAbility.SpriteRenderer;
        img_MonsterHP.fillAmount = (float)_curMonsterAbility.CurHP / _curMonsterAbility.MaxHP;
        img_MonsterHP.DOFillAmount((float)args._targetValue / _curMonsterAbility.MaxHP, 1f);
    }

    private void GenerateDamageText(DamageTextEventArgs args)
    {
        damageTextController.GenerateDamageText(args.Damage,args.ScreenPosition);
    }
}