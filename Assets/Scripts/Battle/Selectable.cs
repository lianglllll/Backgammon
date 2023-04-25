using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Selectable : MonoBehaviour
{
    public GameObject selectedObject;//当前选中obj
    private Camera mainCamera;
    public BoardManager boardManager;

    public bool EnableLefMouseClick = true;
    public Indentity indentity ;//只能选自己的牌


    bool onCardGroup = false;
    bool canPut = false;

    private void Awake()
    {
        BattleManager.Instance.StatusChangeEvent.AddListener(indentityChange);
    }

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (EnableLefMouseClick)
        {
            LeftMouseClick();
        }
        if (!EnableLefMouseClick)
        {
            RightMouseClick();
        }
    }

    /*
     本次操作的身份改变，用于控制玩家选中自己的棋子
     */
    public void indentityChange()
    {
        //根据battlemanager
        if (BattleManager.Instance.status == GameStatus.MyLayTime || BattleManager.Instance.status == GameStatus.MyAction)
        {
            indentity = Indentity.My;
        }
        else
        {
            indentity = Indentity.Enemy;
        }
    }




    //左键事件：将棋子移出卡组区
    void LeftMouseClick()
    {


        if (Input.GetMouseButtonDown(0))
        {
            //关闭左键功能
            if (BattleManager.Instance.status != GameStatus.MyLayTime && BattleManager.Instance.status != GameStatus.EnemyLayTime)
            {
                EnableLefMouseClick = false;
            }


            //判断本轮是否已经放置完毕了
            if (BattleManager.Instance.GetLayNumber() == 0)
            {
                return;
            }

            LayerMask chessMask = LayerMask.GetMask("Chess");
            LayerMask chessBlockMask = LayerMask.GetMask("ChessBlock");
            LayerMask cardGroupMask = LayerMask.GetMask("CardGroup");

            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            bool onCardGroup = Physics.Raycast(ray, out hit, Mathf.Infinity, cardGroupMask);

            if (onCardGroup && selectedObject == null
                    && Physics.Raycast(ray, out hit, Mathf.Infinity, chessMask)
                        && hit.transform.gameObject.CompareTag("chessman")//选择阶段，射线检测到的物体需要是这个标签的物体才可以
                            && hit.transform.gameObject.GetComponent<Chess>().indentity == indentity)//并且还是需要符合某个阵营的才行
            {
                selectedObject = hit.transform.gameObject;
                Debug.Log("选中棋子" + selectedObject);
                //canPut = true;
            }
            else if (selectedObject != null && Physics.Raycast(ray, out hit, Mathf.Infinity, chessBlockMask)//放置阶段放到chessm层级上
                        && !BattleManager.Instance.IsExistOtherChessInBolck(boardManager.GetChessBlockIndex(hit.transform)))//并且格子上没有敌方的棋子
            {
                selectedObject.transform.position = hit.point;

                //需要在相对应的挂上加上这个gameobject
                //怎么获取呢？
                int i = boardManager.GetChessBlockIndex(hit.transform);//获取棋盘下标

                BattleManager.Instance.AddChess(i, selectedObject.GetComponent<Chess>());//添加到battlemanager管理本棋子

                Debug.Log("目标物体位置" + hit.transform.gameObject+":"+i);

                //给棋子的位置信息赋值（告诉棋子它当前在哪一个棋格中）
                selectedObject.GetComponent<Chess>().SetNowPosition(i);
                //关闭移动脚本
                selectedObject.GetComponent<Move>().enabled = false;

                //重置临时变量
                selectedObject = null;

                //不需要告诉battleManager，只需要让其无法放置棋子即可，到时候玩家自然会去按回合结束的按钮
                BattleManager.Instance.LayNumberDecrease();//放置成功，本轮放置次数减一
            }
        }
    }


    //右键事件：选中棋子，放开选中棋子的移动脚本
    void RightMouseClick()
    {


        if (Input.GetMouseButtonDown(1))
        {
            //判断是否为行动阶段并且已经抽了筛子
            if ((BattleManager.Instance.status != GameStatus.MyAction && BattleManager.Instance.status != GameStatus.EnemyAction)|| BattleManager.Instance.StepNum <= 0)
            {
                return;//此时不允许进行选中
            }

            LayerMask chessMask = LayerMask.GetMask("Chess");

            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, chessMask) 
                    && hit.transform.gameObject.CompareTag("chessman")
                        && hit.transform.gameObject.GetComponent<Chess>().indentity == indentity)//符合本回合操作的阵营
            {
                if (selectedObject != null)
                {
                    selectedObject.GetComponent<Move>().enabled = false;//可能会重新选中，关闭上一个脚本
                    selectedObject = null;
                }

                selectedObject = hit.transform.gameObject;
                Debug.Log("选中棋子" + selectedObject);

                selectedObject.GetComponent<Move>().enabled = true;//放开它的脚本，等待移动事件触发
            }
        }
    }


}