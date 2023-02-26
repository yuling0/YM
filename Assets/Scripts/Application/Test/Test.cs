using DG.Tweening;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class Person : ScriptableObject
{
    public string name;
    public int nums;
    public string sex;
    public bool b;
    public Person(string name, int nums, string sex, bool b) => (this.name, this.nums, this.sex, this.b) = (name, nums, sex, b);
    internal void Deconstruct(out string name, out int num)
    {
        name = this.name;
        num = this.nums;
    }
}
//[ExecuteInEditMode]
public class Test : SerializedMonoBehaviour
{


    public List<TestClass> testClasses = new List<TestClass>();
    public UnityAction action;
    public Transform A;
    public Transform P;
    public Vector3 normal;
    public Transform uiTf;
    public bool flag;
    Animator anim;
    public (int, string) valueTupleSample;
    public MovementController mc;
    public DialogDataContainer container;

    public DialogNodeBase cur;
    public bool canContinue = true;

    public int testInt = 0;

    Dictionary<Action, object> dic = new Dictionary<Action, object>();
    public float playSoundInterval = 0.3f;
    public float timer = 0f;
    [Multiline(5)]
    public string content;
    [ReadOnly]
    public List<string> realContent;
    Sequence s;
    public SpriteRenderer sr;
    public CutScenePanel cutScenePanel;
    public TimelineAsset TestTimeline;
    public ScriptableObjectTest objectTest;
    //[/*My,*/OnCollectionChanged("OnChanged")]
    //public float[] fs = new float[] { 1.1f, 1.2f, 1.3f };

    //void OnChanged(InspectorProperty property,CollectionChangeInfo info,object value)
    //{
    //    switch (info.ChangeType)
    //    {
    //        case CollectionChangeType.Unspecified:
    //            print("Unspecified");
    //            break;
    //        case CollectionChangeType.Add:
    //            print("Add");
    //            break;
    //        case CollectionChangeType.Insert:
    //            print("Insert");
    //            break;
    //        case CollectionChangeType.RemoveValue:
    //            print("RemoveValue");
    //            break;
    //        case CollectionChangeType.RemoveIndex:
    //            print("RemoveIndex");
    //            break;
    //        case CollectionChangeType.Clear:
    //            print("Clear");
    //            break;
    //        case CollectionChangeType.RemoveKey:
    //            print("RemoveKey");
    //            break;
    //        case CollectionChangeType.SetKey:
    //            print("SetKey");
    //            break;
    //        default:
    //            break;
    //    }
    //    print(property.Name);
    //    print("当前索引"+ info.Index);
    //    print("当前选项的索引" + info.SelectionIndex);
    //}
    void Start()
    {
        //s = DOTween.Sequence();
        //s.Append(DOTween.To(() => testInt, (val) => { testInt = val; }, 100, 2f))
        //    .Join(DOTween.To(() => content, (val) => { content = val; }, "哦呦呦，好几把难过\n呜呜呜~~~", 5f))
        //    .Append(DOTween.To(() => testInt, (val) => { testInt = val; }, -100, 2f))
        //    .AppendCallback(() => { Debug.Log("oyy"); });
        valueTupleSample = new ValueTuple<int, string>(1, "1");
        valueTupleSample.Item1 = 2;
        anim = GetComponent<Animator>();
        DontDestroyOnLoad(this.gameObject);
        //cur = container.nodes[0] as DialogNode;

        string s = default;
        print(s);
        SceneManager.sceneLoaded += (scene, mode) =>
        {
            Debug.Log("OnSceneLoaded: " + scene.name);
            Debug.Log(mode);
        };

        //TweenManager.To(() => testInt, (val) => { testInt = val; },-100,100,3f).SetTarget(this);

        //Type type = typeof(float[]);

        ////反射判断类型是否为数组
        //if(type.HasElementType)
        //{
        //    print(type.GetElementType().FullName);
        //}
        ////反射判断类型是否为泛型
        //if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
        //{
        //    foreach (var item in type.GetGenericArguments())
        //    {
        //        print(item.Name);
        //    }
        //}
        //var methods = this.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance);

        InventoryMgr.Instance.AddItem(1);
        InventoryMgr.Instance.AddItem(1);
        InventoryMgr.Instance.AddItem(1);
        InventoryMgr.Instance.AddItem(2);
        InventoryMgr.Instance.AddItem(2);
        InventoryMgr.Instance.AddItem(2);
        InventoryMgr.Instance.AddItem(2);
        InventoryMgr.Instance.AddItem(20);
        InventoryMgr.Instance.AddItem(20);
        InventoryMgr.Instance.AddItem(20);
        InventoryMgr.Instance.AddItem(20);
        InventoryMgr.Instance.AddItem(20);
        InventoryMgr.Instance.AddItem(21);
        InventoryMgr.Instance.AddItem(21);
        InventoryMgr.Instance.AddItem(21);
        InventoryMgr.Instance.AddItem(21);
        InventoryMgr.Instance.AddItem(22);
        InventoryMgr.Instance.AddItem(22);
        InventoryMgr.Instance.AddItem(22);
        InventoryMgr.Instance.AddItem(22);
        InventoryMgr.Instance.AddItem(1);
        InventoryMgr.Instance.AddItem(1);
        InventoryMgr.Instance.AddItem(22);
        InventoryMgr.Instance.AddItem(22);
        EventMgr.Instance.AddEventListener("DialogEnd",(bool val)=> { canContinue = val; });
        EventMgr.Instance.AddEventListener("PrintOyy", ()=> { print("哦呦呦"); EventMgr.Instance.OnEventTrigger(Consts.E_DialogContinue); });
        EventMgr.Instance.AddEventListener("ShowPanel", (string val)=> { UIManager.Instance.Push<InventoryPanel>(); });
        EventMgr.Instance.AddEventListener("GetGold", (int val)=> { print($"获得金币:{val}枚"); EventMgr.Instance.OnEventTrigger(Consts.E_DialogContinue); });
        EventMgr.Instance.AddEventListener("SelectionPanelOnConfirmSelect", (int val)=> 
        {
            if (cur is SelectionNode)
            {
                cur = cur.GetOutputPort($"options {val}").GetConnection(0).node as DialogNodeBase;
                if (cur is EventNode)
                {
                    EventNode en = cur as EventNode;
                    switch (en.parameterType)
                    {
                        case E_EventArgsType.E_Null:
                            EventMgr.Instance.OnEventTrigger(en.eventName);
                            break;
                        case E_EventArgsType.E_String:
                            EventMgr.Instance.OnEventTrigger(en.eventName, en.strValue);
                            break;
                        case E_EventArgsType.E_Int:
                            EventMgr.Instance.OnEventTrigger(en.eventName, en.intValue);
                            break;
                        case E_EventArgsType.E_Float:
                            EventMgr.Instance.OnEventTrigger(en.eventName, en.floatValue);
                            break;
                        case E_EventArgsType.E_Bool:
                            EventMgr.Instance.OnEventTrigger(en.eventName, en.boolValue);
                            break;
                        case E_EventArgsType.E_Custom:
                            EventMgr.Instance.OnEventTrigger(en.eventName, en.customValue);
                            break;
                    }
                }
                cur = cur?.GetOutputPort("OutputPort")?.GetConnection(0).node as DialogNodeBase;
                canContinue = true;
            }
        } );
    }
    bool Function()
    {
        Debug.Log("哦呦呦");
        return true;
    }
    public void SplitContent()
    {
        List<string> strList = new List<string>();
        string[] strs = content.Split('\n');
        StringBuilder sb = new StringBuilder();
        foreach (var s in strs)
        {
            sb.Clear();
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '.' || s[i] == '。' || s[i] == '？' || s[i] == '！')
                {
                    while (i < s.Length && (s[i] == '.' || s[i] == '。' || s[i] == '？' || s[i] == '！'))
                    {
                        sb.Append(s[i]);
                        i++;
                    }
                    if (i != s.Length)
                    {
                        strList.Add(sb.ToString());
                        sb.Clear();
                    }
                    i -= 1;
                }
                else
                {
                    sb.Append(s[i]);
                }

                if (i == s.Length - 1)
                {
                    sb.Append('\n');
                    strList.Add(sb.ToString());
                    sb.Clear();
                }
            }
        }
        realContent = strList;
    }
    void Update()
    {
        SplitContent();
        //Vector3 AP = P.position - A.position;
        //Debug.DrawLine(A.position, P.position,Color.blue);// AP
        //Debug.DrawLine(A.position,  A.position + normal,Color.green);// 法向量

        //Vector3 DP = P.position - (Vector3.Dot(AP, normal) / normal.magnitude) * normal.normalized;

        //Debug.DrawLine(P.position, DP, Color.green);// 法向量
        
        timer += Time.deltaTime;
        //ProcessDialog();
        if (!DialogueManager.Instance.IsInDialog && Input.GetKeyDown(KeyCode.V))
        {
            //DialogManager.Instance.StartDialog(container);
            //GameManager.Instance.testBool = true;
            //UIManager.Instance.Push<CutScenePanel>();
            //CutsceneManager.Instance.Resume();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            //YMSceneManager.Instance.LoadSceneAsync("TestScene", () => { print("场景加载完成"); });
            //UIManager.Instance.Pop();
            //ResourceMgr.Instance.LoadAsset<GameObject>("Prefabs/Unit/NPC/Arche’s Father");
            //BinaryDataManager.Instance.SaveData(objectTest, "AI/Test.Ling",false);

            //TimelineAsset timelineAsset = ResourceMgr.Instance.LoadAsset<TimelineAsset>("TimeLine/TestTimeline");
            //CutsceneManager.Instance.PlayCutscene(timelineAsset);
            //mc.RunTowards(10, null);
            //mc.Flip(null);
            //UIManager.Instance.Push<StoryPanel>();
            //UnitManager.Instance.ShowUnit(200005, new Vector3(-0.5f, -2.5f, 0), null);
            //UnitManager.Instance.ShowUnit(100001, new Vector3(-1.8f, -2.5f, 0), null);
            //UnitManager.Instance.ShowUnit(200004, new Vector3(-3.0f, -2.5f, 0), null);
            //CutsceneManager.Instance.PlayCutscene(TestTimeline);
            YMSceneManager.Instance.LoadStoryScene(StoryEventArgs.Create("1", null));
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            //YMSceneManager.Instance.LoadSceneAsync("TestScene", () => { print("场景加载完成"); });
            //UIManager.Instance.Pop();
            //ResourceMgr.Instance.LoadAsset<GameObject>("Prefabs/Unit/NPC/Arche’s Father");
            //BinaryDataManager.Instance.SaveData(objectTest, "AI/Test.Ling",false);

            //TimelineAsset timelineAsset = ResourceMgr.Instance.LoadAsset<TimelineAsset>("TimeLine/TestTimeline");
            //CutsceneManager.Instance.PlayCutscene(timelineAsset);
            //mc.RunTowards(10, null);
            //mc.Flip(null);
            UIManager.Instance.Pop();
            //UnitManager.Instance.ShowUnit(200005, new Vector3(-0.5f, -2.5f, 0), null);
            //UnitManager.Instance.ShowUnit(100001, new Vector3(-1.8f, -2.5f, 0), null);
            //UnitManager.Instance.ShowUnit(200004, new Vector3(-3.0f, -2.5f, 0), null);
            //CutsceneManager.Instance.PlayCutscene(TestTimeline);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            // UnitManager.Instance.ShowUnit("XiFeng", new Vector3(-4, -1.7f, 0f));
            // UnitManager.Instance.ShowUnit("Arche", new Vector3(-2.67f, -2.41f, 0f));
            UIManager.Instance.Push<GamePanel>();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            // UnitManager.Instance.ShowUnit("XiFeng", new Vector3(-4, -1.7f, 0f));
            // UnitManager.Instance.ShowUnit("Arche", new Vector3(-2.67f, -2.41f, 0f));
            UIManager.Instance.HidePanel<GamePanel>();
        }
        if (Input.GetMouseButtonDown(0))
        {
            EventMgr.Instance.OnMultiParameterEventTrigger<DamageTextEventArgs>(DamageTextEventArgs.Create(UnityEngine.Random.Range(1, 101), Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            //UnitManager.Instance.ShowUnit("Sana",new Vector3(-0.6f, -2.5f, 0f));
            CutsceneManager.Instance.PlayCutscene(TestTimeline);
            //UIManager.Instance.Push<GamePanel>();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //UIManager.Instance.Push<GamePanel>();
            //UnitManager.Instance.Release();
            YMSceneManager.Instance.TrySwitchScene("1009-1");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //UIManager.Instance.Pop();
            YMSceneManager.Instance.TrySwitchScene("1002-1");
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            SoundManager.Instance.PlayBGM("Combat01");
            action.Invoke();
        }
        if (timer >= playSoundInterval && Input.GetKey(KeyCode.I))
        {
            timer = 0f;
            PlaySoundParams playSoundParams = PlaySoundParams.Create();
            playSoundParams.Pitch = 1.15f;
            SoundManager.Instance.PlaySound(E_SoundType.Other, "DialogSound", playSoundParams);
        }
    }

    private void ProcessDialog()
    {
        if (canContinue && Input.GetKeyDown(KeyCode.V))
        {
            Vector2 pos = Camera.main.WorldToScreenPoint(GameManager.PlayerTF.position + Vector3.up * 1.5f);
            GameObject obj = new GameObject();
            obj.transform.position = pos;
            if (cur is DialogNode)
            {
                UIManager.Instance.ShowDialog(pos, cur as DialogNode);
            }
            else if (cur is SelectionNode)
            {
                UIManager.Instance.Push<SelectionPanel>((panel) =>
                {
                    panel.SetOptions(cur as SelectionNode);
                });
            }
            if (cur is EndNode)
            {
                canContinue = false;
                return;
            }

            if (!(cur is SelectionNode) && cur.GetOutputPort("OutputPort").ConnectionCount > 0)
            {
                cur = cur?.GetOutputPort("OutputPort")?.GetConnection(0).node as DialogNodeBase;
            }

            canContinue = false;

        }
    }
}


[System.Serializable]
public class TestClass
{
    [LabelText("描述")]
    public string description;
    
    public int val;
}
