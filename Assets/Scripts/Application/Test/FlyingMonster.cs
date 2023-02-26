using UnityEngine;

public class FlyingMonster : MonoBehaviour
{
    public float speed = 10f;  // 飞行速度
    public float amplitude = 2f;  // 振幅
    public float frequency = 1f;  // 频率

    private float horizontalPosition;  // 水平位置

    void Update()
    {
        // 获取两帧之间的时间差
        float deltaTime = Time.deltaTime;

        // 计算水平位置
        horizontalPosition += speed * deltaTime;

        // 计算垂直位置
        float verticalPosition = Mathf.Sin(horizontalPosition * frequency) * amplitude;

        // 更新位置
        transform.position = new Vector2(horizontalPosition, verticalPosition);
    }
}
