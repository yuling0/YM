using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class JsonDataMgr : SingletonBase<JsonDataMgr>
{

    private JsonDataMgr ()
    {
        if(!Directory.Exists(Consts.Streaming_Json_Data_Path))
        {
            Directory.CreateDirectory(Consts.Streaming_Json_Data_Path);
        }

        if(!Directory.Exists(Consts.Persistent_Json_Data_Path))
        {
            Directory.CreateDirectory(Consts.Persistent_Json_Data_Path);
        }

    }
    public T LoadData<T>(string path)
    {
        string dataPath = Consts.Persistent_Json_Data_Path + path + ".json";

        if(!File.Exists(dataPath))
        {
            dataPath = Consts.Streaming_Json_Data_Path + path + ".json";

            if (!File.Exists(dataPath))
            {

                Debug.Log($"未找到该路径：{path} 的Json文件");

                return default(T);

            }
        }

        string json = File.ReadAllText(dataPath);

        return JsonMapper.ToObject<T>(json);

    }

    public void SaveData<T>(string path ,T data, bool isSavePersistentData = true)
    {
        string dataPath = (isSavePersistentData ? Consts.Persistent_Json_Data_Path : Consts.Streaming_Json_Data_Path) + path + ".json";

        string jsonStr = JsonMapper.ToJson(data);

        string directory = dataPath.Substring(0, dataPath.LastIndexOf('/'));

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }


        File.WriteAllText(dataPath, jsonStr);
    }
}
