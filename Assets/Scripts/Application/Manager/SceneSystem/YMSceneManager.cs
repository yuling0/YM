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
    SceneSwitchUnitDataContainer sceneSwitchUnitDataContainer;
    SceneLoadDataContainer sceneLoadDataContainer;
    SceneData curSceneData;
    int unitLoadCount = 0;
    int completeCount = 0;
    int tryLoadSceneId;
    string mapResourcePath;
    string sceneSwitchUnitPath;
    Map curMap;
    Vector3 playerPos;
    //TODO:设置初始场景
    private YMSceneManager()
    {
        SceneDataContainer = BinaryDataManager.Instance.GetContainer<SceneDataContainer>();
        sceneSwitchUnitDataContainer = BinaryDataManager.Instance.GetContainer<SceneSwitchUnitDataContainer>();
        sceneLoadDataContainer = BinaryDataManager.Instance.GetContainer<SceneLoadDataContainer>();
        conditionManager = ConditionManager.Instance;
        unitManager = UnitManager.Instance;
        storySystem = StorySystem.Instance;
        resourceMgr = ResourceMgr.Instance;
        eventMgr = EventMgr.Instance;
        mapResourcePath = "Prefabs/Map/";
        sceneSwitchUnitPath = "Prefabs/Unit/Other/SceneSwitchUnit";
        eventMgr.AddEventListener(Consts.E_OnHideSceneComplete, ContinueLoad);
        eventMgr.AddEventListener(Consts.E_OnShowSceneComplete, OnShowSceneComplete);
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
            tryLoadSceneId = curSceneData.sceneId;
            UIManager.Instance.Push<CutScenePanel>();
            //LoadSceneAsync(sceneName);
            this.playerPos = playerPos;
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

            unitLoadCount++;
            unitManager.ShowUnit(Consts.U_Arche, playerPos, null, (unit) =>
            {
                CameraController.Instance.OnInit(InitCameraData.Create(unit.transform, curMap));
                OnUnitLoadCompleted();
            });
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
        });
    }

    private void LoadSceneData(int sceneLoadDataID)
    {
        SceneLoadData sceneLoadData = sceneLoadDataContainer.GetSceneLoadData(sceneLoadDataID);
        List<string> conditionList = sceneLoadData.conditionList;
        for (int i = 0; i < conditionList.Count; i++)
        {
            //if (conditionManager.CheckConditionIsTrue(conditionList[i]))
            //{
            //    unitLoadCount++;
            //    unitManager.ShowUnit(sceneLoadData.conditionUnitList[i], sceneLoadData.conditionUnitPositionList[i], null, (unit) => { OnUnitLoadCompleted(); });
            //}
            string [] conditionStr = conditionList[i].Split('&');
            try
            {
                if (eventMgr.OnEventTrigger<string, bool>(conditionStr[0], conditionStr[0]))
                {
                    unitLoadCount++;
                    unitManager.ShowUnit(sceneLoadData.conditionUnitList[i], sceneLoadData.conditionUnitPositionList[i], null, (unit) => { OnUnitLoadCompleted(); });
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        unitLoadCount += sceneLoadData.unitIdList.Count;
        for (int i = 0; i < sceneLoadData.unitIdList.Count; i++)
        {
            unitManager.ShowUnit(sceneLoadData.unitIdList[i], sceneLoadData.positionList[i], null, (unit) => { OnUnitLoadCompleted(); });
        }
    }

    private void ContinueLoad()
    {
        if (curMap != null)
        {
            Destroy(curMap.gameObject);
        }
        LoadScene();
    }

    private void OnUnitLoadCompleted()
    {
        completeCount++;
        if (completeCount == unitLoadCount)
        {
            eventMgr.OnEventTrigger(Consts.E_OnSceneLoaded);   //场景加载完成
            Debug.Log($"{nameof(YMSceneManager)}场景加载完成");
        }
    }

    private void OnShowSceneComplete()
    {
        Debug.Log($"检查场景:{curSceneData.sceneName}切换后是否有事件触发");
        storySystem.CheckEventTriggerOnSceneLoadComplete(curSceneData.sceneId);
    }
}
