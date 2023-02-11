using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class PlayerController2D : CharacterController2D
{

    private Rigidbody2D rig;
    [InlineEditor]
    public ArcheInfo info;                  //���ѵ���Ϣ

    [Title("״̬����")]
    public float h;                         //ˮƽ������
    public Vector2 vel;                     //������ٶ�
    public bool isGrounded;                 //�Ƿ��ڵ���
    public bool isFacingRight;              //�Ƿ������ұ�
    public LayerMask groundMask;            //����Ĳ㼶
    public Transform groundCheck;           //�����������
    public float radius = 0.02f;                    //������ʹ�õ�С��뾶

    public float nextCheckGroundInterval = 0.05f;   //��һ�μ������ʱ��������ֹ������
    public float checkGroundTimer;          //������ļ�����

    public bool isJumpPress;
    public bool isJumpStay;
    public bool isRunning;                  //�Ƿ�Ϊ�ܲ�״̬
    public bool doubleKey;                  //˫���ĳ��� �Ƿ���

    public float preArrowKeyTime;           //��һ�η�������µ�ʱ��
    public float allowMaxInterval = 0.2f;   //�ܲ��İ�������������ʱ����

    public float fallForce;                 //����ʩ�ӵ���
    public float shortJumpFallForce;        //����ʱ��ʩ�ӵ�������
    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        Init();
    }

    public override void Init()
    {

    }

    private void Update()
    {
        isRunning = false;

        mc.CheckGround();

        h = (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) - (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);

        isJumpStay = Input.GetKey(KeyCode.C);

        if (Input.GetKeyDown(KeyCode.C))
        {
            isJumpPress = true;

        }


        if(Input.GetKeyDown(KeyCode.RightArrow)|| Input.GetKeyDown(KeyCode.LeftArrow))          //����Ƿ�˫��
        {
            if(Time.time - preArrowKeyTime <= allowMaxInterval)
            {
                doubleKey = true;
            }

            preArrowKeyTime = Time.time;
        }

        if (doubleKey &&( Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))) //���˫����Ч���Ƿ��ڳ���
        {
            isRunning = true;
        }
        else//���ٳ���
        {
            doubleKey = false;
        }


        print(doubleKey);
    }
    void FixedUpdate()
    {
        //mc.Move(h, isRunning, isJumpPress, isJumpStay);

        //isJumpPress = false;
    }






}
