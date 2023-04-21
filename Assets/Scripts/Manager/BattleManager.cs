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
/// ��������Ϸ
/// </summary>
public class BattleManager : Singelton<BattleManager>
{


    public BoardManager boardManager;

    //ά��������Ϸ��״̬
    public GameStatus status = GameStatus.Start;

    //��¼��ǰ��ÿһ�����ϵ���������
    //keyΪ�Եı�ţ�valus���������ӵ�ʵ������
    //����涨������ Ǭ�� ���� ���� ���� ���� ���� ���� ���� ����
    //           0    1    2    3    4    5    6    7    8    9    
    public Dictionary<int, List<Chess>> BolckInfos = new Dictionary<int, List<Chess>>();
    public const int circleBlockNum = 8;//��¼���ĸ��ӣ�ѭ��ʱ�ģ�

    private List<int> layNumberOrder = new List<int>();//ÿ�����ӷ��õ�������
    private const int layMaxRound = 4;//���ķ��ûغ���
    private int layRound = 0;//��ǰ�ķ��ûغ�
    private int layNumber;//��ǰ���ӵķ��ô���


    public Selectable selectable;//����������ӿ�

    public DiceRolling diceRolling;//Ͷ���ӽӿ�
    private int stepNum = 0;//Ͷ���ӻ�õĲ�����ͬʱҲ��Ϊ�Ƿ��Ѿ�Ͷ�����ӵı�־λ
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

    //״̬�л��¼�
    public UnityEvent StatusChangeEvent = new UnityEvent();

    //�л���my
    public UnityEvent myActionEvent = new UnityEvent();
    //�л���enemy
    public UnityEvent enemyActionEvent = new UnityEvent();



    private bool isLay = true;//�Ƿ�Ϊ�������ӵĻغ�

    public int allowchessBlocks = 8;


    private void Start()
    {
        Play();
    }

    /*
     * ��Ϸ��ʼ
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

        //״̬��Ҫ�л�
        status = GameStatus.MyLayTime;
        
        StatusChangeEvent?.Invoke();//��ʾui�ĵ���

    }

    /*
     * �غ�״̬�л�
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
     ״̬��draw->action
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
     �����л�
     */
    public void ChangeLayStatus()
    {
        if(status == GameStatus.EnemyLayTime)
        {
            status = GameStatus.MyLayTime;
        }
        else if(status == GameStatus.MyLayTime)
        {
            //�����ж��׶�
            if(layMaxRound == layRound)
            {
                status = GameStatus.EnemyDraw;//���ӷ�����ϣ���ʼ�鿨�����ж�
                StatusChangeEvent?.Invoke();
                isLay = false;
                return;
            }
            status = GameStatus.EnemyLayTime;
        }

        //���ֵķ��ô�������
        layRound++;
        layNumber = layNumberOrder[layRound];

        StatusChangeEvent?.Invoke();
    }


    /*
     * �����غϽ�����ť
     */
    public void OnEndTurn()
    {
        //����˵�����л��ж�
        if(!isLay)
        {
            ChangeActionStatus();
            StepNum = 0;//���ñ�־
        }
        else
        {//�л�����
            ChangeLayStatus();
        }
    }

    /*
     * �������ʵ�����ֵ��б���
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
     �Ƴ���ǰ�����������ʵ��
     */
    public void RemoveChess(int i,Chess chess)
    {
        if (i < 0 || i >= circleBlockNum)
        {
            Debug.Log("BattleManager.RemoveChessԽ��");
            return;//��ȫУ��
        }

        List<Chess> chs = BolckInfos[i];
        bool deleteflag = chs.Remove(chess);

        if (!deleteflag)
        {
            Debug.Log("BattleManager.RemoveChessʧ��");
        }

    }


    /*
     *������������ѯ��ǰ�������Ŀ�����͵�ʵ����
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
    *������������ѯ��ǰ�������Ŀ�����͵�ʵ����
    *��Ҫ�����Ұ���
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
     �����ȡ�����һ���з����ӣ������������һ�����ӽű�
     */
    public Chess RamGetAimChessInBlocks(int position,Indentity indentity)
    {
        //��Ϊ��ǰ����������������Ҳ�����������Ҫ����һ�»�ȡ�з�����
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

        //�����ȡ
        System.Random rand = new System.Random();
        int aimIndex = rand.Next(aimChessCount);

        return chessList[aimIndex];


    }


    /*
     �ж��ǵ�ǰ������ʲô״̬
     */
    public ChessBlockStatus GetChessBlockStatus(int blockIndex)
    {
        //�գ�û��Ϣ������Ϊ0
        if (BolckInfos.ContainsKey(blockIndex) == false || BolckInfos[blockIndex].Count == 0)
        {
            return ChessBlockStatus.��;
        }


        //�з������
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
        {//�Լ���
            return ChessBlockStatus.�Լ���;
        }
        else if (counter == 1)
        {//����
            return ChessBlockStatus.����;
        }
        else if (counter > 1)
        {//��
            return ChessBlockStatus.��;
        }
        else
        {//����
            return ChessBlockStatus.����;
        }

    }


    /*
     ��ȡ�ҷ����е�һ���з����ӣ���ͷ/���ģ�,���������ǵз����
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
     ���ý׶�  �жϵ�ǰ�������û�жԷ�������������
     */
    public bool IsExistOtherChessInBolck(int id)
    {

        if (id <0 )
        {
            return false;
        }

        //�Է������
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
     Ͷɸ��
     */
    public void StartRolling()
    {
        if(status!=GameStatus.EnemyDraw && status != GameStatus.MyDraw)
        {
            return;
        }

        if (StepNum != 0)//���غ��Ѿ�Ͷ��
        {
            return;
        }
        //Э��
        diceRolling.StartRolling();

    }

    /*
    ���ַ��ô���--
    */
    public void LayNumberDecrease()
    {
        layNumber--;
    }
    /*
     ��ȡ���ַ��ô���
     */
    public int GetLayNumber()
    {
        return layNumber;
    }



    /*
     �ƶ�����
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
            //StepNum = 3;//����
            MoveChess();
            selectable.selectedObject = null;
        }
    }





    /*
    ����ĳ�����Ķ���������block�ű����Ŀ����ݼ���
     */
    public void OpenPoisonousCircle(int pos, Indentity ind)
    {
        boardManager.OpenPoisonousCircle(pos, ind);
    }

    /*
     �ر�ĳ�����Ķ�Ȧ
     */
    public void ClosePoisonousCircle(int pos)
    {
        boardManager.ClosePoisonousCircle(pos);
    }

    /*
     ���Ķ�Ȧ�Ƿ���
     */
    public GameObject IsOpenPoisonousCircle(int pos)
    {
        return boardManager.IsOpenPoisonousCircle(pos);
    }


}
