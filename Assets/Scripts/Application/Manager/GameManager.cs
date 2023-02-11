using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBase<GameManager>
{
    private GameManager ()
    {
    }
    private  Transform _playerTF;
    private Core _playerCore;
    public  Transform PlayerTF
    {
        get
        {
            if (_playerTF == null)
            {
                //Unit arche = UnitManager.Instance.GetUnit("Arche");
                //if (arche == null)
                //{
                //    Debug.Log("Arche不存在");
                //    return null;
                //}
                //_playerTF = arche.transform;
                _playerTF = GameObject.FindGameObjectWithTag("Player").transform;

                if (_playerTF == null)
                {
                    Debug.Log("Arche不存在");
                    return null;
                }
                _playerCore = _playerTF.GetComponent<Core>();

                if (_playerTF == null)
                {
                    Debug.Log("玩家不存在");
                }
            }
            return _playerTF;
        }
    }

    public void GamePauseOrContinue()
    {
        Time.timeScale = Time.timeScale > 0f ? 0f : 1f;
        _playerCore.enabled = Time.timeScale > 0f ? true : false;
    }

    //TODO:这里需要改成UnitManager获取
    public void EnablePlayerControl()
    {
        //_playerCore.enabled = true;
    }
    public void DisablePlayerControl()
    {
        //_playerCore.enabled = false;
    }

    public bool testBool = false;
}
