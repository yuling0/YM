using Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
public static class ExcelTools 
{
    /// <summary>
    /// 根据Excel文件生成对应的数据结构类
    /// </summary>
    [MenuItem("YMTools/GenerateData")]
    public static void GenerateData()
    {
        if(!Directory.Exists(Consts.EXCEL_PATH))
        {
            Directory.CreateDirectory(Consts.EXCEL_PATH);
        }

        if (!Directory.Exists(Consts.DATA_CLASS_PATH))
        {
            Directory.CreateDirectory(Consts.DATA_CLASS_PATH);
        }

        if (!Directory.Exists(Consts.DATA_Container_PATH))
        {
            Directory.CreateDirectory(Consts.DATA_Container_PATH);
        }

        if(!Directory.Exists(Consts.BINARY_DATA_PATH))
        {
            Directory.CreateDirectory(Consts.BINARY_DATA_PATH);
        }

        string[] filePath = Directory.GetFiles(Consts.EXCEL_PATH);              //获取文件夹下所有Excel文件完整路径

        for (int i = 0; i < filePath.Length; i++)
        {
            FileInfo info = new FileInfo(filePath[i]);
            if (info.Extension != ".xlsx" && info.Extension != ".xls")
                continue;

            DataSet set = null;

            using (FileStream fs = File.Open(filePath[i],FileMode.Open,FileAccess.Read))
            {
                IExcelDataReader excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fs);//读出Excel
                set = excelDataReader.AsDataSet();
                fs.Close();
            }

            for (int j = 0; j < set.Tables.Count; j++)
            {
                GenerateDataClass(set.Tables[j]);
                GenerateDataContainer(set.Tables[j]);
                GenerateBinaryData(set.Tables[j]);
            }
        }
        AssetDatabase.Refresh();

    }

    private static void GenerateDataClass(DataTable table)
    {
        DataRow nameRow = GetDataNameRow(table);
        DataRow typeRow = GetDataTypeRow(table);
        DataRow descriptionRow= GetDataDescriptionRow(table);
        StringBuilder sb = new StringBuilder();
        string path = Consts.DATA_CLASS_PATH + table.TableName + ".cs";
        using (FileStream fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
        {
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using UnityEngine;");
            sb.AppendLine("public class " + table.TableName);
            sb.AppendLine("{");
            for(int i = 0 ; i < table.Columns.Count ; i++)
            {
                /// <summary>
                /// 
                /// </summary>
                string[] descriptions = descriptionRow[i].ToString().Split('\n');
                sb.AppendLine("\t/// <summary>");
                sb.Append("\t///");
                for (int j = 0; j < descriptions.Length; j++)
                {
                    sb.Append(descriptions[j]);
                }
                sb.AppendLine();
                sb.AppendLine("\t/// </summary>");
                sb.AppendLine("\tpublic " + typeRow[i].ToString() + " " + nameRow[i].ToString() + ";");
            }
            sb.AppendLine("}");
            fs.Close();
        }
        File.WriteAllText(path, sb.ToString());
    }



    private static void GenerateDataContainer(DataTable table)
    {
        string[] keyType = GetKeyTypes(table);
        StringBuilder sb = new StringBuilder();
        string path = Consts.DATA_Container_PATH + table.TableName + "Container.cs";

        using (FileStream fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
        {
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("public class " + table.TableName + "Container : BaseContainer");
            sb.AppendLine("{");
            for (int i = 0; i < keyType.Length; i++)
            {
                sb.AppendLine($"\tpublic Dictionary<{ keyType[i]},{table.TableName}> dic{i + 1} = new Dictionary<{keyType[i]},{table.TableName}>();");
            }

            for (int i = 0; i < keyType.Length; i++)
            {
                sb.AppendLine($"\tpublic {table.TableName} Get{table.TableName} ({keyType[i]} key)");
                sb.AppendLine("\t{");
                sb.AppendLine($"\t\tif(dic{i+1}.ContainsKey(key))");
                sb.AppendLine("\t\t{");
                sb.AppendLine($"\t\t\treturn dic{i+1}[key];");
                sb.AppendLine("\t\t}");
                sb.AppendLine("\t\treturn null;");
                sb.AppendLine("\t}");
            }
            
            sb.AppendLine("}");
            fs.Close();
        }

        File.WriteAllText(path, sb.ToString());
    }

    private static void GenerateBinaryData(DataTable table)
    {
        using (FileStream fs = File.Open(Consts.BINARY_DATA_PATH + table.TableName + ".ling",FileMode.OpenOrCreate,FileAccess.Write))   //文件后缀可以随意
        {
            int cnt = table.Rows.Count - Consts.START_INDEX;        //有几行数据
            fs.Write(BitConverter.GetBytes(cnt), 0, 4);
            DataRow typeRow = GetDataTypeRow(table);

            string[] keyName = GetKeyTypeNames(table);

            fs.Write(BitConverter.GetBytes(keyName.Length), 0, 4);  //写入key的个数
            //依次写入键名
            for (int i = 0; i < keyName.Length; i++)
            {
                byte[] keyArr = Encoding.UTF8.GetBytes(keyName[i]);
                fs.Write(BitConverter.GetBytes(keyArr.Length), 0, 4);   //写入keyName的长度
                fs.Write(keyArr, 0, keyArr.Length);     //写入keyName
            }
            
            for(int i = Consts.START_INDEX ; i < table.Rows.Count ; i++)
            {
                DataRow curRow = table.Rows[i];
                for (int  j = 0;  j < table.Columns.Count;  j++)
                {
                    string typeString = typeRow[j].ToString();

                    if (typeString == "int" )    //是int类型或者枚举
                    {
                        fs.Write(BitConverter.GetBytes(int.Parse(curRow[j].ToString())), 0, 4);
                    }
                    else if (typeString == "float")
                    {
                        fs.Write(BitConverter.GetBytes(float.Parse(curRow[j].ToString())), 0, 4);
                    }
                    else if (typeString == "string" || typeString.StartsWith("List<") || typeString.StartsWith("Dictionary<") || typeString == "Vector3" || typeString.StartsWith("E_"))
                    {
                        var bs = Encoding.UTF8.GetBytes(curRow[j].ToString());
                        fs.Write(BitConverter.GetBytes(bs.Length), 0, 4);
                        fs.Write(bs, 0, bs.Length);
                    }
                    else if (typeString == "bool")
                    {
                        fs.Write(BitConverter.GetBytes(bool.Parse(curRow[j].ToString())), 0, 1);
                    }
                }

            }
            fs.Close();
        }
    }
    private static DataRow GetDataNameRow(DataTable table)
    {
        return table.Rows[0];
    }
    private static DataRow GetDataTypeRow(DataTable table)
    {
        return table.Rows[1];
    }
    private static DataRow GetDataDescriptionRow(DataTable table)
    {
        return table.Rows[3];
    }

    private static string[] GetKeyTypes(DataTable table)
    {
        DataRow type = GetDataTypeRow(table);
        DataRow row = table.Rows[2];                //Excel中的填写key的行
        List<string> typeList = new List<string>();
        for (int i = 0; i < table.Columns.Count; i++)
        {
            if(row[i].ToString() == "key")
            {
                typeList.Add(type[i].ToString());
            }
        }
        if (typeList.Count == 0)
        {
            typeList.Add(type[0].ToString());       //默认以第一个字段为字典的Key
        }
        return typeList.ToArray();
    }

    private static string[] GetKeyTypeNames(DataTable table)
    {
        DataRow nameRow = GetDataNameRow(table);
        DataRow row = table.Rows[2];
        List<string> nameList = new List<string>();
        for (int i = 0; i < table.Columns.Count; i++)
        {
            if (row[i].ToString() == "key")
            {
                nameList.Add(nameRow[i].ToString());
            }
        }
        if (nameList.Count == 0)
        {
            nameList.Add(nameRow[0].ToString()); //默认以第一个字段为字典的Key
        }
        return nameList.ToArray();    
    }
}
