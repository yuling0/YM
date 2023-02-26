using UnityEngine;

//静态变量类（主要是为了偷懒）
public static class Consts
{

    #region 数据路径
    public static readonly string EXCEL_PATH = Application.dataPath + "/ArtRes/Excel/";//Excel的存储路径
    public static readonly string DATA_CLASS_PATH = Application.dataPath + "/Scripts/DataInfoClss/";//生成的数据结构类的存储路径
    public static readonly string DATA_Container_PATH = Application.dataPath + "/Scripts/Containers/";//生成的数据容器类的存储路径
    public static readonly string BINARY_DATA_PATH = Application.streamingAssetsPath + "/Binary/";//生成的二进制数据的存储路径

    public static readonly int START_INDEX = 4;//写入二进制数据时，开始的行索引

    public static readonly string Streaming_Json_Data_Path = Application.streamingAssetsPath + "/Json/"; //json类型的初始数据路径

    public static readonly string Persistent_Json_Data_Path = Application.persistentDataPath + "/Json/"; //持久性的json数据路径

    public static readonly string Streaming_Binary_Data_Path = Application.streamingAssetsPath + "/Binary/"; //二进制类型的初始数据路径

    public static readonly string Persistent_Binary_Data_Path = Application.persistentDataPath + "/Binary/"; //二进制类型的持久数据路径


    #endregion

    #region 动画名称常量

    //尔茄动画
    public const string A_Idle = "Idle";

    public const string A_Walk = "Walk";

    public const string A_Run = "Run";

    public const string A_Defend = "Defend";

    public const string A_Hurt = "Hurt";

    public const string A_Crouch = "Crouch";

    public const string A_CrouchEnterAndExit = "CrouchEnterAndExit";

    public const string A_ForwardRoll = "ForwardRoll";

    public const string A_BackwardRoll = "BackwardRoll";

    public const string A_JumpEnter = "JumpEnter";

    public const string A_JumpStay = "JumpStay";

    public const string A_FallEnter = "FallEnter";

    public const string A_FallExit = "FallExit";

    public const string A_FallStay = "FallStay";

    public const string A_JumpKickEnter = "JumpKickEnter";

    public const string A_JumpKickStay = "JumpKickStay";

    public const string A_JumpKickExitNoHit = "JumpKickNoHit";

    public const string A_JumpKickExitHit = "JumpKickHit";

    public const string A_TurnLeft = "TurnLeft";

    public const string A_TurnRight = "TurnRight";

    public const string A_DrawSword = "DrawSword";

    public const string A_RetractSword = "RetractSword";

    public const string A_Buffer = "Buffer";

    public const string A_BattleBuffer = "BattleBuffer";

    public const string A_NormalAttack = "NormalAttack";

    public const string A_NormalJumpAttack = "NormalJumpAttack";

    public const string A_WideSlash = "WideSlash";

    public const string A_BattleIdle = "BattleIdle";

    public const string A_BattleWalk = "BattleWalk";

    public const string A_BattleRun = "BattleRun";

    public const string A_BattleJumpEnter = "BattleJumpEnter";

    public const string A_BattleJumpStay = "BattleJumpStay";

    public const string A_BattleFallEnter = "BattleFallEnter";

    public const string A_BattleFallStay = "BattleFallStay";

    public const string A_BattleFallExit = "BattleFallExit";

    public const string A_BattleForwardRoll = "BattleForwardRoll";

    public const string A_BattleBackwardRoll = "BattleBackwardRoll";

    public const string A_BattleCourch = "BattleCourch";

    public const string A_BattleCourchEnter = "BattleCourchEnter";

    public const string A_BattleDefend = "BattleDefend";

    public const string A_BattleHurt = "BattleHurt";

    public const string A_KnightThrust = "KnightThrust";

    public const string A_LeapingSlash = "LeapingSlash";

    public const string A_Thrust = "Thrust";

    public const string A_SnapThrust = "SnapThrust";

    public const string A_UpSlash = "UpSlash";

    public const string A_ComboAttackForward = "ComboAttackForward";

    public const string A_ComboAttackUp = "ComboAttackUp";

    public const string A_BackwardLeapingSlash = "BackwardLeapingSlash";

    public const string A_TheFullMoonSlash = "TheFullMoonSlash";

    public const string A_DownStabEnter = "DownStabEnter";

    public const string A_DownStabStay = "DownStabStay";

    public const string A_DownStabExit = "DownStabExit";

    public const string A_WheelSlash = "WheelSlash";

    //蝙蝠动画
    public const string A_SprintEnter = "SprintEnter";
    public const string A_SprintStay = "SprintStay";
    public const string A_Move = "Move";

    //蛇动画
    public const string A_Bite = "Bite";
    public const string A_WideBite = "WideBite";
    public const string A_Fall = "Fall";
    public const string A_WideBiteHit = "WideBiteHit";
    public const string A_WideBiteEnter = "WideBiteEnter";

    #endregion

    #region 玩家状态
    public const string S_Idle = "IdleState";
    public const string S_Walk = "WalkState";
    public const string S_Run = "RunState";
    public const string S_Jump = "JumpState";
    public const string S_FallEnter = "FallEnter";
    public const string S_FallStay = "FallStay";
    public const string S_FallExit = "FallExit";
    public const string S_ForwardRoll = "ForwardRollState";
    public const string S_BackwardRoll = "BackwardRollState";
    public const string S_Courch = "CourchState";
    public const string S_Defend = "DefendState";
    public const string S_JumpKickEnter = "JumpKickEnterState";
    public const string S_JumpKickStay = "JumpKickStayState";
    public const string S_JumpKickHit = "JumpKickHitState";
    public const string S_JumpKickNoHit = "JumpKickNoHitState";
    public const string S_FlipState = "FlipState";
    public const string S_DrawSword = "DrawSword";
    public const string S_NormalAttack = "NormalAttackState";
    public const string S_NormalJumpAttack = "NormalJumpAttackState";
    public const string S_KnightThrust = "KnightThrustState";                   //骑士突刺
    public const string S_LeapingSlash = "LeapingSlashState";                   //跳跃回旋斩
    public const string S_WideSlash = "WideSlashState";                         //有小段位移的斩击
    public const string S_SnapThrust = "SnapThrustState";                       //向后突刺
    public const string S_Thrust = "ThrustState";                               //向前突刺
    public const string S_Buffer = "BufferState";                               //缓冲状态
    public const string S_UpSlash = "UpSlashState";                             //上斩

    public const string S_ComboAttackUp = "ComboAttackUpState";                 //连击上斩
    public const string S_ComboAttackForward = "ComboAttackForwardState";       //连击前斩
    public const string S_BackwardLeapingSlash = "BackwardLeapingSlashState";   //反向跳跃回旋斩
    public const string S_TheFullMoonSlash = "TheFullMoonSlashState";           //圆月斩
    public const string S_DownStabEnterState = "DownStabEnterState";            //下刺
    public const string S_DownStabStayState = "DownStabStayState";              //下刺
    public const string S_DownStabExitState = "DownStabExitState";              //下刺
    public const string S_WheelSlashState = "WheelSlashState";                  //车轮斩



    #endregion

    #region 怪物状态
    public const string S_MonsterAttackState = "MonsterAttackState";
    public const string S_HurtState = "HurtState";
    //蝙蝠状态
    public const string S_ChaseState = "ChaseState";
    public const string S_SprintEnterState = "SprintEnterState";
    public const string S_SprintStayState = "SprintStayState";
    public const string S_PatrolState = "PatrolState";
    //蛇状态
    public const string S_BiteState = "BitState";
    public const string S_WideBiteState = "WideBiteState";
    public const string S_JumpEnterState = "JumpEnterState";
    public const string S_JumpStayState = "JumpStayState";
    public const string S_WideBiteHitState = "WideBiteHitState";
    public const string S_WideBiteEnterState = "WideBiteEnterState";
    #endregion

    #region 按键名称
    public const string K_Left = "LeftArrow";
    public const string K_Right = "RightArrow";
    public const string K_Down = "DownArrow";
    public const string K_Up = "UpArrow";
    public const string K_Jump = "Jump";
    public const string K_Attack = "Attack";
    public const string K_DrawSword = "DrawSword";
    public const string K_Menu = "Menu";


    #endregion

    #region 技能名称
    public const string J_ForwardRoll = "ForwardRoll";
    public const string J_BackwardRoll = "BackwardRoll";
    public const string J_JumpKickRight = "JumpKickRight";
    public const string J_JumpKickLeft = "JumpKickLeft";
    public const string J_LeapingSlashRight = "LeapingSlashRight";
    public const string J_LeapingSlashLeft = "LeapingSlashLeft";
    public const string J_WideSlashLeft = "WideSlashLeft";
    public const string J_WideSlashRight = "WideSlashRight";
    public const string J_Thrust = "Thrust";
    public const string J_NormalAttack = "NormalAttack";
    public const string J_UpSlash = "UpSlash";
    public const string J_BackwardLeapingSlashLeft = "BackwardLeapingSlashLeft";
    public const string J_BackwardLeapingSlashRight = "BackwardLeapingSlashRight";
    public const string J_WheelSlash = "WheelSlash";

    public const string J_Up = "Up";
    public const string J_Down = "Down";
    public const string J_Left = "Left";
    public const string J_Right = "Right";




    #endregion

    #region Resources资源路径
    public const string P_Prefabs = "Prefabs/";
    public const string P_SoundAgent = P_Prefabs + "Sound/SoundAgent";
    public const string P_SoundGroup = P_Prefabs + "Sound/SoundGroup";
    public const string P_Ghosting = P_Prefabs + "Player/Ghosting";
    public const string P_Hit = P_Prefabs + "Effects/HitEffect";
    public const string P_UI = P_Prefabs +"UI/";

    public const string P_ScriptableObjectsPath = "Assets/Resources/ScriptableObjects";
    public const string P_AI = "Assets/Resources/ScriptableObjects/AI";
    public static readonly string P_AudioClipPath = "Audio";
    public static readonly string P_DialogDataPath = "ScriptableObjects/DialogData";
    public static readonly string P_CutscenePath = "Cutscene";
    #endregion

    #region 音效切片名称
    //BGM

    //UI音效
    public const string C_Select = "Select";    //选中时或弹出菜单时
    public const string C_Back = "Back";        //回退菜单时
    public const string C_MoveSelector = "MoveSelector";

    //战斗音效



    #endregion

    #region 事件名称
    public const string E_DialogContinue = "DialogContinue";
    public const string E_OnSelectionPanelConfirm = "OnSelectionPanelConfirm";
    public const string E_OnSceneLoaded = "OnSceneLoaded";
    public const string E_OnHideSceneComplete = "OnHideSceneComplete";
    public const string E_OnShowSceneComplete = "OnShowSceneComplete";
    public const string E_TrySwitchScene = "TrySwitchScene";
    public const string E_StartDialog = "StartDialog";
    public const string E_SwitchScene = "SwitchScene";
    public const string E_LoadStoryScene = "LoadStoryScene";
    public const string E_PlayCutscene = "PlayCutscene";
    #endregion

    #region 事件触发条件类型字符串
    /// <summary>
    /// 场景加载后触发
    /// </summary>
    public const string ET_OnSceneLoadComplete = "OnSceneLoadComplete";

    /// <summary>
    /// 场景加载之前触发
    /// </summary>
    public const string ET_OnSceneLoadBefore = "OnSceneLoadBefore";

    public const string ET_OnCutscenePlayed = "OnCutscenePlayed";
    #endregion

    #region 单位名称
    public const string U_Ghosting = "Ghosting";
    public const string U_Arche = "Arche";
    public const string U_Camera = "Camera";
    #endregion
}
