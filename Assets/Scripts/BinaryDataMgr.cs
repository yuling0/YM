using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

public class BinaryDataMgr
{
    private static BinaryDataMgr instance = new BinaryDataMgr();



    private BinaryDataMgr()
    {
        Init();
    }
    public static BinaryDataMgr Instance => instance;

    public Dictionary<string, BaseContainer> dic = new Dictionary<string, BaseContainer>();
    private void Init()
    {
    }
    /// <summary>
    /// 加载二进制数据表
    /// </summary>
    /// <typeparam name="T">数据表的容器</typeparam>
    /// <typeparam name="K">数据类</typeparam>
    private void LoadTable<T,K>() where T:BaseContainer where K : class
    {
        object container = Activator.CreateInstance(typeof(T));//生成数据容器
        FieldInfo dicField = typeof(T).GetField("dic");//获取容器里的字典字段
        object dicObj = dicField.GetValue(container);//获取字典对象
        MethodInfo add = dicObj.GetType().GetMethod("Add");//获取字典的Add方法

        string path = Consts.BINARY_DATA_PATH + typeof(K).Name + ".ling";//数据表的路径（根据数据表的类型名来获取路径）

        byte[] bs = File.ReadAllBytes(path);//读取文件中的所有字节

        int ind = 0;//字节数组读取的当前索引位置
        int row = BitConverter.ToInt32(bs,ind);//读取行数
        ind += 4;
        int len = BitConverter.ToInt32(bs, ind);//读取长度
        ind += 4;
        string name = Encoding.UTF8.GetString(bs, ind, len);//读取主键的变量名（用于通过反射获取值）
        ind += len;
        Type type = typeof(K);//获取数据类的类型
        FieldInfo[] fieldInfo = type.GetFields();//获取所有字段

        for (int i = 0; i < row; i++)//遍历行
        {
            object obj = Activator.CreateInstance(type);

            for (int j = 0; j < fieldInfo.Length; j++)//遍历字段
            {
                if(fieldInfo[j].FieldType == typeof(int))
                {
                    fieldInfo[j].SetValue(obj, BitConverter.ToInt32(bs, ind));
                    ind += 4;
                }
                else if (fieldInfo[j].FieldType == typeof(float))
                {
                    fieldInfo[j].SetValue(obj, BitConverter.ToSingle(bs, ind));
                    ind += 4;
                }
                else if (fieldInfo[j].FieldType == typeof(bool))
                {
                    fieldInfo[j].SetValue(obj, BitConverter.ToBoolean(bs, ind));
                    ind += 1;
                }
                else if (fieldInfo[j].FieldType == typeof(string))
                {
                    len = BitConverter.ToInt32(bs, ind);
                    ind += 4;
                    string str = Encoding.UTF8.GetString(bs, ind, len);
                    ind += len;
                    fieldInfo[j].SetValue(obj, str);
                }
            }
            add.Invoke(dicObj, new object[] { type.GetField(name).GetValue(obj), obj });//加入到数据容器的字典中

        }
        dic.Add(typeof(T).Name, container as T);//加入到管理器的字典中



    }
    public void SaveData<T>(string directory, string path, T obj) where T : class, new()
    {
        directory = Application.persistentDataPath + "/" + directory + "/";
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        using (FileStream fs = new FileStream(directory + path, FileMode.OpenOrCreate, FileAccess.Write))
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(fs, obj);
        }

    }

    public T LoadData<T>(string directory, string path) where T : class, new()
    {
        string dataPath = Application.persistentDataPath + "/" + directory + "/" + path;
        if (!File.Exists(dataPath))
        {
            return Activator.CreateInstance(typeof(T)) as T;
        }
        T obj = null;
        using (FileStream fs = new FileStream(dataPath, FileMode.Open, FileAccess.Read))
        {
            BinaryFormatter bf = new BinaryFormatter();
            obj = bf.Deserialize(fs) as T;
        }

        return obj;
    }
}
