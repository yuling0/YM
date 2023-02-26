using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static UnityEngine.UI.CanvasScaler;

public class YMSceneManager : SingletonAutoMono<YMSceneManager>
{
    UnitManager unitManager;
    ConditionManager conditionManager;
    ResourceMgr resourceMgr;
    StorySystem storySystem;
    EventMgr eventMgr;
    SceneDataContainer SceneDataContainer;
    SceneLoadDataContainer sceneLoadDataContainer;
    MonsterLoadDataContainer monsterLoadDataContainer;
    NPCLoadDataContainer npcLoadDataContainer;
    StorySceneDataContainer storySceneDataContainer;
    SceneData curSceneData;
    StorySceneData storySceneData;
    int unitLoadCount = 0;
    int completeCount = 0;
    int tryLoadSceneId;
    string mapResourcePath;
    string sceneSwitchUnitPath;
    Map curMap;
    Vector3 playerPos;
    bool playerIsFacingRight;
    StoryEventArgs storyEventArgs;
    //TODO:设置初始场景
    private YMSceneManager()
    {
        SceneDataContainer = BinaryDataManager.Instance.GetContainer<SceneDataContainer>();
        sceneLoadDataContainer = BinaryDataManager.Instance.GetContainer<SceneLoadDataContainer>();
        monsterLoadDataContainer = BinaryDataManager.Instance.GetContainer<MonsterLoadDataContainer>();
        npcLoadDataContainer = BinaryDataManager.Instance.GetContainer<NPCLoadDataContainer>();
        storySceneDataContainer = BinaryDataManager.Instance.GetContainer<StorySceneDataContainer>();
        conditionManager = ConditionManager.Instance;
        unitManager = UnitManager.Instance;
        storySystem = StorySystem.Instance;
        resourceMgr = ResourceMgr.Instance;
        eventMgr = EventMgr.Instance;
        mapResourcePath = "Prefabs/Map/";
        sceneSwitchUnitPath = "Prefabs/Unit/Other/SceneSwitchUnit";
        eventMgr.AddEventListener(Consts.E_OnHideSceneComplete, ContinueLoad);
        eventMgr.AddEventListener(Consts.E_OnShowSceneComplete, OnShowSceneComplete);
        eventMgr.AddEventListener<StoryEventArgs>(Consts.E_LoadStoryScene, LoadStoryScene);
    }
    public void TrySwitchScene(string sceneSwicthData)
    {
        //场景切换数据格式：场景ID - 场景切换对象ID
        string[] sceneIDAndUnitIdMap = sceneSwicthData.Split('-');
        curSceneData = SceneDataContainer.GetSceneData(int.Parse(sceneIDAndUnitIdMap[0]));
        TryLoadScene(curSceneData.sceneName, curSceneData.sceneSwitchUnitAndPositionsMap[int.Parse(sceneIDAndUnitIdMap[1])]);
    }
    public void TryLoadScene(string sceneName, Vector2 playerPos)
    {
        if (curSceneData == null || curSceneData.sceneName != sceneName)
        {
            curSceneData = SceneDataContainer.GetSceneData(sceneName);
        }
        if (!storySystem.CheckEventTriggerOnSceneLoadBefore(curSceneData.sceneId))
        {
            //tryLoadSceneId = curSceneData.sceneId;
            UIManager.Instance.HidePanel<GamePanel>();
            UIManager.Instance.Push<CutScenePanel>();
            //LoadSceneAsync(sceneName);
            this.playerPos = playerPos;
        }
    }
    public void LoadStoryScene(StoryEventArgs args)
    {
        storyEventArgs = args;
        storySceneData = storySceneDataContainer.GetStorySceneData(int.Parse(args.stringArgs));
        curSceneData = SceneDataContainer.GetSceneData(storySceneData.sceneName);
        playerPos = storySceneData.playerPosition;
        playerIsFacingRight = storySceneData.playerIsFacingRight;
        if (!storySystem.CheckEventTriggerOnSceneLoadBefore(curSceneData.sceneId))
        {
            UIManager.Instance.HidePanel<GamePanel>();
            UIManager.Instance.Push<CutScenePanel>();
        }
    }
    private void LoadScene()
    {
        //加载场景地图
        resourceMgr.LoadAssetAsync<Map>(mapResourcePath + curSceneData.sceneName, (map) =>
        {
            curMap = map;
            curMap.transform.parent = this.transform;
            completeCount = 0;
            unitLoadCount = 0;
            //加载场景切换单位
            foreach (KeyValuePair<int, Vector3> unit in curSceneData.sceneSwitchUnitAndPositionsMap)
            {
                unitLoadCount++;
                unitManager.ShowUnit(unit.Key, sceneSwitchUnitPath, unit.Value, curSceneData.sceneSwicthUnitAndSceneMap[unit.Key], (unit) => { OnUnitLoadCompleted(); });
            }

            //加载玩家
            unitLoadCount++;
            unitManager.ShowUnit(Consts.U_Arche, playerPos, ShowPlayerInfoArgs.Create(playerIsFacingRight), (unit) =>
            {
                GameManager.PlayerCore = unit.UnitLogic as Core;
                CameraController.Instance.OnInit(InitCameraData.Create(unit.transform, curMap));
                OnUnitLoadCompleted();
                GameManager.DisablePlayerControl();
            });
            if (storyEventArgs == null)
            {
                int sceneLoadDataId = storySystem.GetSceneLoadDataID(curSceneData.sceneId);

                if (sceneLoadDataId == -1)
                {
                    switch (storySystem.TimeBucket)
                    {
                        case E_TimeBucket.AM:
                            sceneLoadDataId = curSceneData.amSceneLoadDataID;
                            break;
                        case E_TimeBucket.PM:
                            sceneLoadDataId = curSceneData.pmSceneLoadDataID;
                            break;
                        case E_TimeBucket.Night:
                            sceneLoadDataId = curSceneData.nightSceneLoadDataID;
                            break;
                    }
                }
                LoadSceneData(sceneLoadDataId);
            }
            else
            {
                unitLoadCount += storySceneData.npcLoadIdList.Count;
                //加载剧情需要的NPC
                foreach (var npcId in storySceneData.npcLoadIdList)
                {
                    NPCLoadData npcLoadData = npcLoadDataContainer.GetNPCLoadData(npcId);
                    ShowNPCInfoArgs args = ShowNPCInfoArgs.Create(npcLoadData.isFacingRight);
                    unitManager.ShowUnit(npcLoadData.unitId, npcLoadData.npcPosition, args, (unit) =>
                    {
                        OnUnitLoadCompleted();
                        ReferencePool.Instance.Release(args);
                    });
                }
                unitLoadCount += storySceneData.monsterLoadIdList.Count;
                //加载剧情需要的Monster
                foreach (var monsterId in storySceneData.monsterLoadIdList)
                {
                    MonsterLoadData monsterLoadData = monsterLoadDataContainer.GetMonsterLoadData(monsterId);
                    CreateMonsterInfo createMonsterInfo = CreateMonsterInfo.Create(monsterLoadData.patrolPoints, monsterLoadData.birthPoint);
                    unitManager.ShowUnit(monsterLoadData.unitId, monsterLoadData.birthPoint, createMonsterInfo, (unit) =>
                    {
                        OnUnitLoadCompleted();
                        ReferencePool.Instance.Release(createMonsterInfo);
                    });
                }
            }

        });
    }

    private void LoadSceneData(int sceneLoadDataID)
    {
        SceneLoadData sceneLoadData = sceneLoadDataContainer.GetSceneLoadData(sceneLoadDataID);
        //List<string> conditionList = sceneLoadData.conditionList;
        //for (int i = 0; i < conditionList.Count; i++)
        //{
        //    //if (conditionManager.CheckConditionIsTrue(conditionList[i]))
        //    //{
        //    //    unitLoadCount++;
        //    //    unitManager.ShowUnit(sceneLoadData.conditionUnitList[i], sceneLoadData.conditionUnitPositionList[i], null, (unit) => { OnUnitLoadCompleted(); });
        //    //}
        //    string [] conditionStr = conditionList[i].Split('&');
        //    try
        //    {
        //        if (eventMgr.OnEventTrigger<string, bool>(conditionStr[0], conditionStr[0]))
        //        {
        //            unitLoadCount++;
        //            unitManager.ShowUnit(sceneLoadData.conditionUnitList[i], sceneLoadData.conditionUnitPositionList[i], null, (unit) => { OnUnitLoadCompleted(); });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.Log(e.Message);
        //    }
        //}
        unitLoadCount += sceneLoadData.monsterLoadIdList.Count;
        for (int i = 0; i < sceneLoadData.monsterLoadIdList.Count; i++)
        {
            MonsterLoadData monsterLoadData = monsterLoadDataContainer.GetMonsterLoadData(sceneLoadData.monsterLoadIdList[i]);
            CreateMonsterInfo createMonsterInfo = CreateMonsterInfo.Create(monsterLoadData.patrolPoints, monsterLoadData.birthPoint);
            unitManager.ShowUnit(monsterLoadData.unitId, monsterLoadData.birthPoint, createMonsterInfo, (unit) => 
            {
                OnUnitLoadCompleted();
                ReferencePool.Instance.Release(createMonsterInfo);
            });
        }
    }

    private void ContinueLoad()
    {
        //TODO:可以用对象池
        if (curMap != null)
        {
            Destroy(curMap.gameObject);
        }
        LoadScene();
    }

    private void OnUnitLoadCompleted()
    {
        completeCount++;
        if (storyEventArgs != null && storySceneData.isNeedSetCameraPosition)
        {
            CameraController.Instance.isFollow = false;
            CameraController.Instance.transform.position = storySceneData.cameraPosition;
        }
        if (completeCount == unitLoadCount)
        {
            eventMgr.OnEventTrigger(Consts.E_OnSceneLoaded);   //场景加载完成
            Debug.Log($"{nameof(YMSceneManager)}场景加载完成");
        }
    }

    private void OnShowSceneComplete()
    {
        if (storyEventArgs != null)
        {
            ReferencePool.Instance.Release(storyEventArgs);
            storyEventArgs = null;
        }
        Debug.Log($"检查场景:{curSceneData.sceneName}切换后是否有事件触发");
        if (!storySystem.CheckEventTriggerOnSceneLoadComplete(curSceneData.sceneId))
        {
            UIManager.Instance.Push<GamePanel>();
            GameManager.EnablePlayerControl();
        }
    }
}
