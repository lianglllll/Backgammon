using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryCardController : MonoBehaviour
{
    public Camera CardCamera; //卡牌相机
    public List<GameObject> Card = new List<GameObject>();

    Vector3 Y180 = new Vector3(0, 180, 0);


    private void Start()
    {
        for(int i = 0; i < Card.Count; i++)
        {
            Card[i].SetActive(false);
        }
    }
    
    public void OpenCard()
    {
        for (int i = 0; i < Card.Count; i++)
        {
            Card[i].SetActive(true);
        }
    }
    // 旋转物体到目标角度的函数
    public void RotateObject(int i)
    {
        StartCoroutine(RotateObjectCoroutine(i));
    }

    // 逐帧旋转物体的协程
    private IEnumerator RotateObjectCoroutine(int i)
    {
        float startAngle = Card[i].transform.eulerAngles.y; // 记录起始角度
        float endAngle = startAngle + 180f; // 目标角度

        while (Card[i].transform.eulerAngles.y < endAngle) // 当当前角度小于目标角度时，持续旋转
        {
            Card[i].transform.Rotate(Y180, 200f * Time.deltaTime); // 以Y轴旋转180度，每秒旋转100度
            yield return null; // 等待一帧
        }

        Card[i].transform.eulerAngles = new Vector3(0, endAngle, 0); // 将旋转角度设置为目标角度
    }

}
