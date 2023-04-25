using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Move : MonoBehaviour
{
    private BoardManager boardManager;//棋盘信息
    private Chess chess;              //棋子信息
    //public int index = 0;//记录当前Player位于第几个棋格上

    Vector3 deviation = new Vector3(0, 0.6f, 0);//是想让棋子不穿模码，直接移动到棋各格处会因为y的值而没入棋盘（因为此时是没有刚体的）

    Vector3 targetPosition;

    public float speed = 1;

    bool isMove = false;
    bool isMove2 = false;//移动一步的

    //外界传入的，将要走的步数
    private int stepNum = 0;

    void Start()
    {
        // 获取BoardManager脚本
        boardManager = GameObject.FindObjectOfType<BoardManager>();
        chess = GetComponent<Chess>();
    }

    void Update() 
    {

        if (isMove) 
        { 
            isMove = false;
            StartCoroutine(MoveTowords()); 
        } else if (isMove2)
        {
            isMove2 = false;
            StartCoroutine(MoveTowords2());
        }
    }


    //对外提供一个移动stepNum步的接口
    public void _Move(int stepNum)
    {
        this.stepNum = stepNum;
        isMove = true;
    }

    //对外提供一个移动stepNum步的接口
    public void _Move2()
    {
        isMove2 = true;
    }


    /*
     协程开始移动对应的棋子
     */
    public IEnumerator MoveTowords()
    {
        Debug.Log("骰子数：" + stepNum); 
        int targetIndex = (chess.NowPosition + stepNum) % BattleManager.Instance.allowchessBlocks;; //获取目标棋格的下标


        Destroy(gameObject.GetComponent<Rigidbody>());//删除本棋子的刚体，防止和其他棋子碰撞

        //循环stepNum次
        while (stepNum-- > 0) 
        {
            //棋子位置+1
            chess.PositionIndexIncrease();

            // 获取目标棋block的位置，（一个比较随机的位置，但是在block给定的范围之内的一个position）
            targetPosition = boardManager.SetPosition(boardManager.boardObjects[chess.NowPosition].transform);
            //targetPosition = boardManager.boardPositions[chess.NowPosition];

            //每一帧不断移动靠近，直到到达目标棋block位置
            while (transform.position != targetPosition + deviation) 
            { 
                transform.position = Vector3.MoveTowards
                    (transform.position, targetPosition + deviation, speed * Time.deltaTime); 
                yield return null; 
            } 
        }

        //通知BattleManager状态已经改变，怎么通知，我们通过设置Stepnum来限制
        BattleManager.Instance.StepNum = -1;//次数不可以再选中任何棋子进行移动了。需要重新将stepnum置为>0的数字才可以正常选中

        //调用chess的移动后脚本，来处理善后工作
        gameObject.GetComponent<Chess>().LastMove();

        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<Move>().enabled = false; //禁用移动脚本
        

    }

    /// <summary>
    /// 移动一步
    /// </summary>
    /// <returns></returns>
    public IEnumerator MoveTowords2()
    {

        int index = GetComponent<Chess>().indentity == Indentity.My ? 9 : 8;

        targetPosition = BattleManager.Instance.boardManager.boardObjects[index].position;
        /*gameObject.transform.position = targetPosition;
        yield return null;*/
        Destroy(gameObject.GetComponent<Rigidbody>());//删除本棋子的刚体，防止和其他棋子碰撞

        //每一帧不断移动靠近，直到到达目标棋block位置
        while (transform.position != targetPosition + deviation)
        {
            transform.position = Vector3.MoveTowards
                (transform.position, targetPosition + deviation, speed * Time.deltaTime);
            yield return null;
        }
        BattleManager.Instance.StepNum = -1;//次数不可以再选中任何棋子进行移动了。需要重新将stepnum置为>0的数字才可以正常选中
        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<Move>().enabled = false; //禁用移动脚本

    }


}



