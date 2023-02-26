using UnityEngine;

public class FlyingMonster : MonoBehaviour
{
    public float speed = 10f;  // �����ٶ�
    public float amplitude = 2f;  // ���
    public float frequency = 1f;  // Ƶ��

    private float horizontalPosition;  // ˮƽλ��

    void Update()
    {
        // ��ȡ��֮֡���ʱ���
        float deltaTime = Time.deltaTime;

        // ����ˮƽλ��
        horizontalPosition += speed * deltaTime;

        // ���㴹ֱλ��
        float verticalPosition = Mathf.Sin(horizontalPosition * frequency) * amplitude;

        // ����λ��
        transform.position = new Vector2(horizontalPosition, verticalPosition);
    }
}
