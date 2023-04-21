using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameStatus
{
    Start,MyLayTime,EnemyLayTime,MyAction, MyDraw, EnemyAction,EnemyDraw
}


/// <summary>
/// 单例的游戏
/// </summary>
public class BattleManager : Singelton<BattleManager>
{


    public BoardManager boardManager;

    //维护本局游戏的状态
    public GameStatus status = GameStatus.Start;

    //记录当前的每一个卦上的棋子数量
    //key为卦的编号，valus是里面棋子的实例链表
    //这里规定：坤卦 乾卦 兑卦 坎卦 震卦 巽卦 离卦 艮卦 阳门 阴门
    //           0    1    2    3    4    5    6    7    8    9    
    public Dictionary<int, List<Chess>> BolckInfos = new Dictionary<int, List<Chess>>();
    public const int circleBlockNum = 8;//记录最大的格子（循环时的）

    private List<int> layNumberOrder = new List<int>();//每次棋子放置的最大次数
    private const int layMaxRound = 4;//最大的放置回合数
    private int layRound = 0;//当前的放置回合
    private int layNumber;//当前棋子的放置次数


    public Selectable selectable;//管理鼠标点击接口

    public DiceRolling diceRolling;//投塞子接口
    private int stepNum = 0;//投塞子获得的步数，同时也作为是否已经投了塞子的标志位
    public int StepNum
    {
        get
        {
            return stepNum;
        }
        set
        {
            stepNum = value;
        }
    }

    //状态切换事件
    public UnityEvent StatusChangeEvent = new UnityEvent();

    //切换到my
    public UnityEvent myActionEvent = new UnityEvent();
    //切换到enemy
    public UnityEvent enemyActionEvent = new UnityEvent();



    private bool isLay = true;//是否为放置棋子的回合

    public int allowchessBlocks = 8;


    private void Start()
    {
        Play();
    }

    /*
     * 游戏开始
     */
    public void Play()
    {
        layNumberOrder.Add(2);
        layNumberOrder.Add(3);
        layNumberOrder.Add(3);
        layNumberOrder.Add(3);
        layNumberOrder.Add(1);
        isLay = true;

        layRound = 0;
        layNumber = layNumberOrder[layRound];

        //状态需要切换
        status = GameStatus.MyLayTime;
        
        StatusChangeEvent?.Invoke();//显示ui的调用

    }

    /*
     * 回合状态切换
     */
    public void ChangeActionStatus()
    {
        if (status == GameStatus.MyAction)
        {
            status = GameStatus.EnemyDraw;
        }else if(status == GameStatus.EnemyAction)
        {
            status = GameStatus.MyDraw;
        }
        StatusChangeEvent?.Invoke();
    }

    /*
     状态从draw->action
     */
    public void StateDraw_Action()
    {
        if (status == GameStatus.EnemyDraw)
        {
            status = GameStatus.EnemyAction;
            enemyActionEvent?.Invoke();
        }
        else if (status == GameStatus.MyDraw)
        {
            status = GameStatus.MyAction;
            myActionEvent?.Invoke();
        }
        StatusChangeEvent?.Invoke();
    }


    /*
     放置切换
     */
    public void ChangeLayStatus()
    {
        if(status == GameStatus.EnemyLayTime)
        {
            status = GameStatus.MyLayTime;
        }
        else if(status == GameStatus.MyLayTime)
        {
            //进入行动阶段
            if(layMaxRound == layRound)
            {
                status = GameStatus.EnemyDraw;//棋子放置完毕，开始抽卡并且行动
                StatusChangeEvent?.Invoke();
                isLay = false;
                return;
            }
            status = GameStatus.EnemyLayTime;
        }

        //本轮的放置次数重置
        layRound++;
        layNumber = layNumberOrder[layRound];

        StatusChangeEvent?.Invoke();
    }


    /*
     * 触发回合结束按钮
     */
    public void OnEndTurn()
    {
        //这里说明是切换行动
        if(!isLay)
        {
            ChangeActionStatus();
            StepNum = 0;//重置标志
        }
        else
        {//切换放置
            ChangeLayStatus();
        }
    }

    /*
     * 添加棋子实例到字典中保存
     */
    public void AddChess(int i, Chess chess)
    {
        if (BolckInfos.ContainsKey(i))
        {
            BolckInfos[i].Add(chess);
        }
        else
        {
            List<Chess> chesss = new List<Chess>();
            chesss.Add(chess);
            BolckInfos[i] = chesss;
        }
    }

    /*
     移除当前的棋格中棋子实例
     */
    public void RemoveChess(int i,Chess chess)
    {
        if (i < 0 || i >= circleBlockNum)
        {
            Debug.Log("BattleManager.RemoveChess越界");
            return;//安全校验
        }

        List<Chess> chs = BolckInfos[i];
        bool deleteflag = chs.Remove(chess);

        if (!deleteflag)
        {
            Debug.Log("BattleManager.RemoveChess失败");
        }

    }


    /*
     *根据类型来查询当前棋格有无目标类型的实例；
     */
    public bool IsExistTargetChessTypeByID(int position,ChessType chessType)
    {

        if (position < 0 || position >= circleBlockNum) return false;

        foreach (Chess chess in BolckInfos[position])
        {
            ChessType type=chess.chessType;
            if (type== chessType)
            {
                return true;
            }
        }

        return false;
    }

    /*
    *根据类型来查询当前棋格有无目标类型的实例；
    *主要用来找半夏
    */
    public Chess IsExistTargetChessTypeByID2(int position, ChessType chessType,Indentity indentity)
    {

        if (position < 0 || position >= circleBlockNum) return null;

        foreach (Chess chess in BolckInfos[position])
        {
            ChessType type = chess.chessType;
            Indentity indent = chess.indentity;
            if (type == chessType && indent == indentity)
            {
                return chess;
            }
        }

        return null;
    }

    /*
     随机获取棋格中一个敌方棋子，并且随机返回一个棋子脚本
     */
    public Chess RamGetAimChessInBlocks(int position,Indentity indentity)
    {
        //因为当前发出这个请求的棋子也在这里，所以需要遍历一下获取敌方棋子
        List<Chess> chessList = new List<Chess>();
        foreach (Chess ch in BolckInfos[position])
        {
            if (ch.indentity == indentity)
            {
                chessList.Add(ch);
            }
        }

        int aimChessCount = chessList.Count;

        if(aimChessCount == 0){
            return null;
        }

        //随机获取
        System.Random rand = new System.Random();
        int aimIndex = rand.Next(aimChessCount);

        return chessList[aimIndex];


    }


    /*
     判断是当前的卦是什么状态
     */
    public ChessBlockStatus GetChessBlockStatus(int blockIndex)
    {
        //空，没信息和数量为0
        if (BolckInfos.ContainsKey(blockIndex) == false || BolckInfos[blockIndex].Count == 0)
        {
            return ChessBlockStatus.空;
        }


        //敌方的身份
        Indentity indentity = status == GameStatus.MyAction ? Indentity.Enemy : Indentity.My;


        int counter = 0;

        foreach (Chess chess in BolckInfos[blockIndex])
        {
            
            if (chess.indentity == indentity)
            {
                counter++;
            }

        }

        if (counter == 0)
        {//自己人
            return ChessBlockStatus.自己人;
        }
        else if (counter == 1)
        {//弱棋
            return ChessBlockStatus.弱棋;
        }
        else if (counter > 1)
        {//垒
            return ChessBlockStatus.垒;
        }
        else
        {//错误
            return ChessBlockStatus.错误;
        }

    }


    /*
     获取我方垒中的一个敌方棋子（乌头/半夏）,传过来的是敌方身份
     */
    public Chess getOneOppositeChess(int pos,Indentity ind)
    {
        foreach (Chess ch in BolckInfos[pos])
        {
            if (ch.indentity != ind)
            {
                return ch;
            }
        }

        return null;
    }


    /*
     放置阶段  判断当前的棋格有没有对方的棋子在上面
     */
    public bool IsExistOtherChessInBolck(int id)
    {

        if (id <0 )
        {
            return false;
        }

        //对方的身份
        Indentity indentity = status == GameStatus.MyLayTime ? Indentity.Enemy : Indentity.My;


        if (!BolckInfos.ContainsKey(id))
        {
            return false;
        }

        foreach (Chess chess in BolckInfos[id])
        {
            if(chess.indentity== indentity)
            {
                return true;
            }
        }

        return false;
    }


    /*
     投筛子
     */
    public void StartRolling()
    {
        if(status!=GameStatus.EnemyDraw && status != GameStatus.MyDraw)
        {
            return;
        }

        if (StepNum != 0)//本回合已经投了
        {
            return;
        }
        //协程
        diceRolling.StartRolling();

    }

    /*
    本轮放置次数--
    */
    public void LayNumberDecrease()
    {
        layNumber--;
    }
    /*
     获取本轮放置次数
     */
    public int GetLayNumber()
    {
        return layNumber;
    }



    /*
     移动棋子
     */
    public void MoveChess()
    {
        Chess chess = selectable.selectedObject.GetComponent<Chess>();
        chess.PrepareMove();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && selectable.selectedObject !=null)
        {
            //StepNum = 3;//测试
            MoveChess();
            selectable.selectedObject = null;
        }
    }





    /*
    开启某个棋格的毒区，设置block脚本里的目标身份即可
     */
    public void OpenPoisonousCircle(int pos, Indentity ind)
    {
        boardManager.OpenPoisonousCircle(pos, ind);
    }

    /*
     关闭某个棋格的毒圈
     */
    public void ClosePoisonousCircle(int pos)
    {
        boardManager.ClosePoisonousCircle(pos);
    }

    /*
     棋格的毒圈是否开启
     */
    public GameObject IsOpenPoisonousCircle(int pos)
    {
        return boardManager.IsOpenPoisonousCircle(pos);
    }


}
