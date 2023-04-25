using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayStatus : MonoBehaviour
{
    public Text statusText;

    private Dictionary<GameStatus, string> statusTextDict = new Dictionary<GameStatus, string>();
    // Start is called before the first frame update

    private void Awake()
    {
        BattleManager.Instance.StatusChangeEvent.AddListener(UpdataStatus);
        statusTextDict.Add(GameStatus.EnemyAction, "�췽�ж�");
        statusTextDict.Add(GameStatus.EnemyDraw, "�췽����");
        statusTextDict.Add(GameStatus.EnemyLayTime, "�췽����");
        statusTextDict.Add(GameStatus.EnemyWin, "�췽��ʤ");

        statusTextDict.Add(GameStatus.MyAction, "�����ж�");
        statusTextDict.Add(GameStatus.MyDraw, "��������");
        statusTextDict.Add(GameStatus.MyLayTime, "��������");
        statusTextDict.Add(GameStatus.MyWin, "������ʤ");

    }
    private void Start()
    {
        UpdataStatus();
    }

    //����textUI
    public void UpdataStatus()
    {
        statusText.text = statusTextDict[BattleManager.Instance.status];
    }


}
