using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;


public class DiceRolling : MonoBehaviour
{
    // 包含所有骰子面的 GameObject 的父对象
    public GameObject diceFacesContainer;

    // 动画持续时间
    public float animationDuration = 2.0f;

    // 每次停留的骰子面的索引
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
        // 获取所有骰子面的 GameObject
        diceFaces = new GameObject[diceFacesContainer.transform.childCount];
        for (int i = 0; i < diceFacesContainer.transform.childCount; i++)
        {
            diceFaces[i] = diceFacesContainer.transform.GetChild(i).gameObject;
            diceFaces[i].SetActive(false); // 将骰子面设置为不可见
        }
    }

    public IEnumerator RollDice()
    {
        float rollTime = 2f; // 骰子滚动时间
        float elapsedTime = 0f; // 已经过去的时间

        // 随机选择一个骰子面，并将其设置为可见
        index  = Random.Range(0, diceFaces.Length);

        //将结果传递出去
        BattleManager.Instance.StepNum = index + 1;



        // 随机选择骰子的面
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

        // 关闭骰子
        for (int i = 0; i < diceFaces.Length; i++)
        {
            diceFaces[i].SetActive(false);
        }
        diceFaces[index].SetActive(true);

        BattleManager.Instance.StateDraw_Action();

        //yield return new WaitForSeconds(5f);
        //diceFaces[index].SetActive(false);
    }



    // 对外提供的投筛子接口
    public void StartRolling()
    {
        if (!isRolling)
        {
            StartCoroutine(RollDice());
        }
    }

}
