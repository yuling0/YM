using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

public class BinaryDataManager : SingletonBase<BinaryDataManager>
{
    private Dictionary<string, BaseContainer> dic = new Dictionary<string, BaseContainer>();

    private BinaryDataManager()
    {
        Init();
    }
    public void Init()
    {
        LoadContainer<CharacterSoundData, CharacterSoundDataContainer>();
        LoadContainer<UnitData, UnitDataContainer>();
        LoadContainer<MissionData, MissionDataContainer>();
        LoadContainer<ChapterData, ChapterDataContainer>();
        LoadContainer<StoryConditionData, StoryConditionDataContainer>();
        LoadContainer<SceneData, SceneDataContainer>();
        LoadContainer<SceneSwitchUnitData, SceneSwitchUnitDataContainer>();
        LoadContainer<SceneLoadData, SceneLoadDataContainer>();
        LoadContainer<StoryEventData, StoryEventDataContainer>();
    }
    
    /// <summary>
    /// 获取从excel中序列化的数据容器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetContainer<T>() where T : BaseContainer
    {
        string key = typeof(T).Name;
        if(!dic.TryGetValue(key,out BaseContainer container))
        {
            Debug.LogError("未能找到" + key + "容器");
        }
        return container as T;
    }

    /// <summary>
    /// 加载从excel序列化的数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    private void LoadContainer<T,K>() where K: BaseContainer
    {
        string fileName = typeof(T).Name;
        BaseContainer container = Activator.CreateInstance<K>();    //创建容器对象
        //FieldInfo dicField = typeof(K).GetField("dic");             //获取字典字段
        //object dicObj = dicField.GetValue(container);               //获取字典对象
        //MethodInfo add = dicObj.GetType().GetMethod("Add");         //获取Add方法

        byte[] bs = File.ReadAllBytes(Consts.BINARY_DATA_PATH + fileName + ".ling");   //读取文件中所有字节
        int index = 0;                          //当前所读的字节索引

        int dataCount = BitConverter.ToInt32(bs, index);        //读取数据长度
        index += 4;

        int keyCount = BitConverter.ToInt32(bs, index);         //读出key的个数
        index += 4;

        object[] dicObj = new object[keyCount];                 //字典的容器对象
        string[] keyNames = new string[keyCount];               //键名的容器
        MethodInfo[] addMethod = new MethodInfo[keyCount];
        for (int i = 0; i < keyCount; i++)
        {
            dicObj[i] = typeof(K).GetField($"dic{i + 1}").GetValue(container);
            int len = BitConverter.ToInt32(bs, index);
            index += 4;
            string keyName = Encoding.UTF8.GetString(bs, index, len);
            index += len;
            keyNames[i] = keyName;
            addMethod[i] = dicObj[i].GetType().GetMethod("Add");
        }
        //int keyLength = BitConverter.ToInt32(bs, index);
        //index += 4;

        //string keyName = Encoding.UTF8.GetString(bs, index , keyLength);    //读取出键名（用于反射获取值）
        //index += keyLength;
        FieldInfo[] fields = typeof(T).GetFields();

        for (int i = 0; i < dataCount; i++)
        {
            T obj = Activator.CreateInstance<T>();
            for (int j = 0; j < fields.Length; j++)
            {
                if(fields[j].FieldType == typeof(int) || typeof(Enum).IsAssignableFrom(fields[j].FieldType))
                {
                    int val = BitConverter.ToInt32(bs, index);
                    index += 4;
                    fields[j].SetValue(obj, val);

                }
                else if (fields[j].FieldType == typeof(float))
                {
                    float val = BitConverter.ToSingle(bs, index);
                    index += 4;
                    fields[j].SetValue(obj, val);

                }
                else if (fields[j].FieldType == typeof(string))
                {
                    int len = BitConverter.ToInt32(bs, index);
                    index += 4;
                    string str = Encoding.UTF8.GetString(bs, index, len);
                    fields[j].SetValue(obj, str);
                    index += len;

                }
                else if (fields[j].FieldType == typeof(bool))
                {
                    bool val = BitConverter.ToBoolean(bs, index);
                    index += 1;
                    fields[j].SetValue(obj, val);
                }
                //else if(fields[j].FieldType == typeof(List<Vector3>))
                //{
                //    List<Vector3> list = new List<Vector3>();
                //    int len = BitConverter.ToInt32(bs, index);
                //    index += 4;
                //    string s = Encoding.UTF8.GetString(bs, index, len);
                //    index+= len;
                //    string[] strs = s.Split('\n');
                //    foreach (var item in strs)
                //    {
                //        string[] vec = item.Split(',');
                //        list.Add(new Vector3(float.Parse(vec[0]), float.Parse(vec[1]), float.Parse(vec[2])));
                //    }
                //    fields[j].SetValue(obj,list);
                //}
                else if(fields[j].FieldType.IsGenericType && fields[j].FieldType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    object list = Activator.CreateInstance(fields[j].FieldType);
                    MethodInfo add = fields[j].FieldType.GetMethod("Add");
                    Type[] genericParameters = fields[j].FieldType.GetGenericArguments();
                    int len = BitConverter.ToInt32(bs, index);
                    index += 4;
                    string s = Encoding.UTF8.GetString(bs, index, len);
                    index += len;
                    string[] strs = s.Split('\n');
                    foreach (var item in strs)
                    {
                        if (genericParameters[0] == typeof(Vector3))
                        {
                            string[] vec = item.Split(',');
                            add.Invoke(list, new object[] { new Vector3(float.Parse(vec[0]), float.Parse(vec[1]), float.Parse(vec[2])) });
                        }
                        else
                        {
                            add.Invoke(list, new object[] { Convert.ChangeType(item, genericParameters[0]) });
                        }
                    }
                    fields[j].SetValue(obj, list);

                }
                else if (fields[j].FieldType.IsGenericType && fields[j].FieldType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    object dic = Activator.CreateInstance(fields[j].FieldType);
                    MethodInfo add = fields[j].FieldType.GetMethod("Add");
                    int len = BitConverter.ToInt32(bs, index);
                    index += 4;
                    string s = Encoding.UTF8.GetString(bs, index, len);
                    index += len;
                    if (s != "null" && s!= string.Empty)
                    {
                        Type[] genericParameters = fields[j].FieldType.GetGenericArguments();
                        string[] keyValuePairArr = s.Split('\n');
                        foreach (var kv in keyValuePairArr)
                        {
                            string[] kvPair = kv.Split('&');
                            object key = null;
                            object value = null;
                            if (genericParameters[0] == typeof(Vector3))
                            {
                                string[] vec = kvPair[0].Split(',');
                                key = new Vector3(float.Parse(vec[0]), float.Parse(vec[1]), float.Parse(vec[2]));
                            }
                            else
                            {
                                key = Convert.ChangeType(kvPair[0], genericParameters[0]);
                            }

                            if (genericParameters[1] == typeof(Vector3))
                            {
                                string[] vec = kvPair[1].Split(',');
                                value = new Vector3(float.Parse(vec[0]), float.Parse(vec[1]), float.Parse(vec[2]));
                            }
                            else
                            {
                                value = Convert.ChangeType(kvPair[1], genericParameters[1]);
                            }

                            add.Invoke(dic, new object[] { key, value });
                        }
                    }
                    fields[j].SetValue(obj, dic);
                }
            }
            for (int j = 0; j < keyCount; j++)
            {
                addMethod[j].Invoke(dicObj[j], new object[] { typeof(T).GetField(keyNames[j]).GetValue(obj), obj });
            }
            //object o = typeof(T).GetField(keyName).GetValue(obj);
            //add.Invoke(dicObj, new object[] { typeof(T).GetField(keyName).GetValue(obj),obj });
        }

        dic.Add(typeof(K).Name, container);
    }

    /// <summary>
    /// 保存数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data">数据</param>
    /// <param name="path">相对路径（相对于StreamingAssets或persistentDataPath）需要加文件后缀名</param>
    /// <param name="isSavePersistentData">是否存入持久化数据文件夹（可读可写）</param>
    public void SaveData<T>(T data , string path , bool isSavePersistentData = true)
    {
        path = (isSavePersistentData ? Consts.Persistent_Binary_Data_Path : Consts.Streaming_Binary_Data_Path)  + path ;
        string directory = path.Substring(0, path.LastIndexOf('/'));
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var bs = SerializationUtility.SerializeValue<T>(data, DataFormat.Binary);

        Debug.Log(path);
        File.WriteAllBytes(path, bs);

    }

    public T LoadData<T>(string path)
    {
        string dataPath = Consts.Streaming_Binary_Data_Path + path;
        if (!File.Exists(dataPath))
        {
            dataPath = Consts.Persistent_Binary_Data_Path + path;
            if (!File.Exists(dataPath))
            {
                Debug.LogError($"不存在该路径下的文件：{path}");
                return default;
            }
        }

        var bs = File.ReadAllBytes(dataPath);

        T data = SerializationUtility.DeserializeValue<T>(bs,DataFormat.Binary);

        return data;
    }
}
