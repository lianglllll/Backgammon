using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;


public class DiceRolling : MonoBehaviour
{
    // ��������������� GameObject �ĸ�����
    public GameObject diceFacesContainer;

    // ��������ʱ��
    public float animationDuration = 2.0f;

    // ÿ��ͣ���������������
    private int index;
    public int Index
    {
        get
        {
            int temp = index;
            index = 0;
            return temp;
        }
        set
        {
            index = value;
        }

    }

    private bool isRolling = false;
    private GameObject[] diceFaces;

    void Start()
    {
        // ��ȡ����������� GameObject
        diceFaces = new GameObject[diceFacesContainer.transform.childCount];
        for (int i = 0; i < diceFacesContainer.transform.childCount; i++)
        {
            diceFaces[i] = diceFacesContainer.transform.GetChild(i).gameObject;
            diceFaces[i].SetActive(false); // ������������Ϊ���ɼ�
        }
    }

    public IEnumerator RollDice()
    {
        float rollTime = 2f; // ���ӹ���ʱ��
        float elapsedTime = 0f; // �Ѿ���ȥ��ʱ��

        // ���ѡ��һ�������棬����������Ϊ�ɼ�
        index  = Random.Range(0, diceFaces.Length);

        //��������ݳ�ȥ
        BattleManager.Instance.StepNum = index + 1;



        // ���ѡ�����ӵ���
        while (elapsedTime < rollTime)
        {
            int randomIndex = Random.Range(0, diceFaces.Length);
            for (int i = 0; i < diceFaces.Length; i++)
            {
                diceFaces[i].SetActive(i == randomIndex);
            }
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        // �ر�����
        for (int i = 0; i < diceFaces.Length; i++)
        {
            diceFaces[i].SetActive(false);
        }
        diceFaces[index].SetActive(true);

        BattleManager.Instance.StateDraw_Action();

        //yield return new WaitForSeconds(5f);
        //diceFaces[index].SetActive(false);
    }



    // �����ṩ��Ͷɸ�ӽӿ�
    public void StartRolling()
    {
        if (!isRolling)
        {
            StartCoroutine(RollDice());
        }
    }

}
