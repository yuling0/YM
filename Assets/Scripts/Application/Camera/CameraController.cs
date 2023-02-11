using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private static CameraController instance;
    private Camera _curCamera;
    public Vector3 _targetPos;
    public Transform _FollowObj;
    private Vector2 _limitRange;
    public Vector3 offset;
    public float _lerpMultplier;
    public Vector2 deashArea;       //死域：摄像机在一定范围内不会跟随目标
    public float test;
    public Vector4 limitRange;      //限制摄像机范围 x为左边界 、 y 为右边界 w 为上边界 z为下边界
    public Vector4 edge;      //限制摄像机范围 x为左边界 、 y 为右边界 w 为上边界 z为下边界

    public Vector4 deathRange;
    public Vector4 curRange;

    public static CameraController Instance => instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
        _curCamera = GetComponent<Camera>();
    }
    public  void OnInit(object userData)
    {
        InitCameraData data = userData as InitCameraData;
        if (data == null)
        {
            throw new System.Exception($"{data} is involid");
        }
        _FollowObj = data.PlayerTF;
        limitRange = data.Map.GetLimitRange();
        
        edge.x = limitRange.x + _curCamera.aspect * _curCamera.orthographicSize;  //摄像机坐标能到达的地图左边界（往下以此类推）
        edge.y = limitRange.y - _curCamera.aspect * _curCamera.orthographicSize;
        edge.w = limitRange.w - _curCamera.orthographicSize;
        edge.z = limitRange.z + _curCamera.orthographicSize;
        _targetPos = _FollowObj.position + offset;
        if (_targetPos.x < edge.x)
        {
            _targetPos.x = edge.x;
        }
        if (_targetPos.x > edge.y)
        {
            _targetPos.x = edge.y;
        }
        if (_targetPos.y > edge.w)
        {
            _targetPos.y = edge.w;
        }
        if (_targetPos.y < edge.z)
        {
            _targetPos.y = edge.z;
        }
        transform.position = _targetPos;
    }
#if UNITY_EDITOR
    private void Start()
    {
        //limitRange = Map.Instance.GetLimitRange(); //这里限制的是摄像机坐标（注意摄像机的坐标在屏幕中心）
        edge.x = limitRange.x + _curCamera.aspect * _curCamera.orthographicSize;  //摄像机坐标能到达的地图左边界（往下以此类推）
        edge.y = limitRange.y - _curCamera.aspect * _curCamera.orthographicSize;
        edge.w = limitRange.w - _curCamera.orthographicSize;
        edge.z = limitRange.z + _curCamera.orthographicSize;
    }

#endif
    // Update is called once per frame
    void LateUpdate()
    {
        if (_FollowObj == null) return;
        //TODO:分辨率改变，可能会改变摄像机宽高比，需要重新计算限制范围
        edge.x = limitRange.x + _curCamera.aspect * _curCamera.orthographicSize;  //摄像机坐标能到达的地图左边界（往下以此类推）
        edge.y = limitRange.y - _curCamera.aspect * _curCamera.orthographicSize;
        edge.w = limitRange.w - _curCamera.orthographicSize;
        edge.z = limitRange.z + _curCamera.orthographicSize;
        //Debug.Log($"摄像机的宽高比{_curCamera.aspect}");
        deathRange.x = transform.position.x - deashArea.x;
        deathRange.y = transform.position.x + deashArea.x;
        deathRange.w = transform.position.y + deashArea.y;
        //deathRange.z = transform.position.y - deashArea.y;
        deathRange.z = transform.position.y;        //这里选择y轴下边界没有死域

        _targetPos = _FollowObj.position + offset;

        //curRange.x = _targetPos.x - _curCamera.aspect * _curCamera.orthographicSize;
        //curRange.y = _targetPos.x + _curCamera.aspect * _curCamera.orthographicSize;
        //curRange.w = _targetPos.y +  _curCamera.orthographicSize;
        //curRange.z = _targetPos.y -  _curCamera.orthographicSize;


        if(_targetPos.x < edge.x)
        {
            _targetPos.x = edge.x;
        }
        if (_targetPos.x > edge.y)
        {
            _targetPos.x = edge.y;
        }
        if (_targetPos.y > edge.w)
        {
            _targetPos.y = edge.w;
        }
        if (_targetPos.y < edge.z)
        {
            _targetPos.y = edge.z;
        }
        DrawUtility.DrawRectangle(_targetPos, new Vector2(0.1f, 0.1f), Color.yellow);
        Vector3 tempPoint = transform.position;

        test = Mathf.Abs(Mathf.Abs(transform.position.y) - Mathf.Abs(_targetPos.y));

        if (_targetPos.x > deathRange.y || _targetPos.x < deathRange.x)
        {
            tempPoint.x = Mathf.Lerp(tempPoint.x, _targetPos.x, Time.deltaTime * _lerpMultplier);
        }

        if (_targetPos.y > deathRange.w || _targetPos.y < deathRange.z)
        {
            tempPoint.y = Mathf.Lerp(tempPoint.y, _targetPos.y, Time.deltaTime * _lerpMultplier);
        }
        tempPoint.z = Mathf.Lerp(tempPoint.z, _targetPos.z, Time.deltaTime * _lerpMultplier);

        transform.position = tempPoint;
        DrawUtility.DrawRectangle(this.transform, deashArea, Color.red);
        //transform.position = Vector3.Lerp(transform.position, _targetPos, Time.deltaTime * _lerpMultplier);
    }
}
