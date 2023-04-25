using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ChessType
{
    ���,����,����,����,��Ƥ,��ͷ
}

public enum Indentity
{
    My,Enemy,Without
}

public enum ChessBlockStatus
{
    ����,��,��,����,�Լ���
}

/// <summary>
/// ���ӻ���
/// </summary>
public class Chess : MonoBehaviour
{

    [Header("������Ϣ")]
    public Indentity indentity;     //���ӵĹ���
    public ChessType chessType;     //���ӵı�ʶ
    public int Hp = 1;              //Ѫ��
    public int nowPosition=-1;   //��ǰ��λ��
    public int NowPosition
    {
        get
        {
            return nowPosition;
        }
    }


    //��ϵ���ӵ����ԣ�Ҳ���Ƕ��߼�������
    public int extraNumber;

    public Move _move;//�ƶ��ű�


    /*
     ���õ�ǰλ��
     */
    public void SetNowPosition(int position)
    {
        this.nowPosition = position;
    }

    /*
     λ��+1
     */
    public void PositionIndexIncrease()
    {
        nowPosition = (nowPosition + 1) % BattleManager.Instance.allowchessBlocks;
    }

    /*
     ����˺�
     */
    public void Harm()
    {
        Hp--;
        if (Hp <= 0)
        {
            Die();
        }
    }

    /*
     * ����
     */
    private void Die()
    {
        //��Ӧ�ļ���������
        BattleManager.Instance.ChessCountDecrease(indentity);
        //�Ƚ�����������Ϣ��ɾ��
        BattleManager.Instance.RemoveChess(nowPosition,this);
        //����������
        Destroy(gameObject);
    }

    /*
     ���õ����ƶ��Ľű�
     */
    public virtual void PrepareMove()
    {


        //Debug.Log("�������");
        //�ƶ�֮ǰ���жϵ�ǰλ�������޹�ϵ������оʹ�������Ч��

        //�����ű��������������move�ű��ǻ��Զ��رսű�

        //����Ŀ��λ�ã��ж����޵��ˣ�����ǵ��˵��������ֱ�Ӹɵ���

    }

    /*
    �ƶ���������ƺ���
     */
    public virtual void LastMove()
    {
        //Debug.Log("����߿�");
    }

    /// <summary>
    /// �жϵ�ǰ�����Ƿ�Ϊ�Է�������
    /// </summary>
    /// <returns></returns>
    public bool IsArriveLifeGate() {

        int aimIndex = indentity == Indentity.My ? BattleManager.Instance.EnemylifeGate : BattleManager.Instance.MylifeGate;
        if (nowPosition == aimIndex) return true;
        return false;
    }

    /// <summary>
    /// �������ڵз���������Ͷ����1
    /// </summary>
    public  void EntryVictoryPoint() {

        //�ƶ���Ŀ��㣬˲��
        _move._Move2();
        //֪ͨbattlemanager������Ϸ
        BattleManager.Instance.EntryLifeGateCallback();


    }


}


