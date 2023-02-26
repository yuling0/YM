using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static Transform _playerTF;
    private static Core _playerCore;
    private DamageTextController damageTextController;
    public static  Transform PlayerTF
    {
        get
        {
            if (_playerTF == null)
            {
                if (_playerCore == null)
                {
                    Unit arche = UnitManager.Instance.GetUnit("Arche");
                    if (arche == null)
                    {
                        Debug.LogError("当前场景中不存在玩家对象");
                    }
                    _playerCore = arche.UnitLogic as Core;
                }
                _playerTF = _playerCore.transform;
            }
            return _playerTF;
        }
    }
    public static Core PlayerCore
    {
        set { _playerCore = value; }
        get => _playerCore;
    }
    private void Awake()
    {
        InitGameModule();
    }

    private void InitGameModule()
    {
        BinaryDataManager.Instance.Init();
        damageTextController = new DamageTextController();
        CutsceneManager.Instance.Init();
        DialogueManager.Instance.Init();
        SoundManager.Instance.Init();
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        damageTextController.OnUpdate();
    }
    public static void GamePauseOrContinue()
    {
        Time.timeScale = Time.timeScale > 0f ? 0f : 1f;
        _playerCore.enabled = Time.timeScale > 0f ? true : false;
    }

    //TODO:这里需要改成UnitManager获取
    public static void EnablePlayerControl()
    {
        if (_playerCore == null || CutsceneManager.Instance.IsPlaying || DialogueManager.Instance.IsInDialog) return;
        _playerCore.Enable = true;
    }
    public static void DisablePlayerControl()
    {
        if (_playerCore == null) return;
        _playerCore.Enable = false;
    }


}
