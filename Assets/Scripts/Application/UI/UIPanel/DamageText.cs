using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float duration;
    public Vector2 velocity;
    public float g;
    private TMP_Text text;
    private float timer;
    private Vector2 tempVel;
    private RectTransform rectTransform;
    public bool IsNeedUnSpwan => timer >= duration;
    private void Awake()
    {
        text= GetComponent<TMP_Text>();
        rectTransform = transform as RectTransform;
    }
    public void OnInit(int damage)
    {
        timer = 0f;
        text.alpha = 1f;
        TweenManager.To(() => text.alpha, (val) => text.alpha = val, 1f, 0f, duration);
        tempVel = velocity;
        if(rectTransform.anchoredPosition.x < 0)
        {
            tempVel.x = -tempVel.x;
        }
        text.text = damage.ToString();
    }
    public void OnUpdate()
    {
        timer += Time.deltaTime;
        rectTransform.anchoredPosition += tempVel * Time.deltaTime;
        tempVel.y += g * Time.deltaTime;
    }
}
