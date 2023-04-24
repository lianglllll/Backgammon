using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryCardController : MonoBehaviour
{
    public Camera CardCamera; //�������
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
    // ��ת���嵽Ŀ��Ƕȵĺ���
    public void RotateObject(int i)
    {
        StartCoroutine(RotateObjectCoroutine(i));
    }

    // ��֡��ת�����Э��
    private IEnumerator RotateObjectCoroutine(int i)
    {
        float startAngle = Card[i].transform.eulerAngles.y; // ��¼��ʼ�Ƕ�
        float endAngle = startAngle + 180f; // Ŀ��Ƕ�

        while (Card[i].transform.eulerAngles.y < endAngle) // ����ǰ�Ƕ�С��Ŀ��Ƕ�ʱ��������ת
        {
            Card[i].transform.Rotate(Y180, 200f * Time.deltaTime); // ��Y����ת180�ȣ�ÿ����ת100��
            yield return null; // �ȴ�һ֡
        }

        Card[i].transform.eulerAngles = new Vector3(0, endAngle, 0); // ����ת�Ƕ�����ΪĿ��Ƕ�
    }

}
