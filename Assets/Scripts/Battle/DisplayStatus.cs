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
        statusTextDict.Add(GameStatus.EnemyAction, "红方行动");
        statusTextDict.Add(GameStatus.EnemyDraw, "红方命运");
        statusTextDict.Add(GameStatus.EnemyLayTime, "红方放置");
        statusTextDict.Add(GameStatus.EnemyWin, "红方获胜");

        statusTextDict.Add(GameStatus.MyAction, "蓝方行动");
        statusTextDict.Add(GameStatus.MyDraw, "蓝方命运");
        statusTextDict.Add(GameStatus.MyLayTime, "蓝方放置");
        statusTextDict.Add(GameStatus.MyWin, "蓝方获胜");

    }
    private void Start()
    {
        UpdataStatus();
    }

    //更新textUI
    public void UpdataStatus()
    {
        statusText.text = statusTextDict[BattleManager.Instance.status];
    }


}
