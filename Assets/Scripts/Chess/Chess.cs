using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ChessType
{
    大黄,黄苓,生姜,半夏,陈皮,乌头
}

public enum Indentity
{
    My,Enemy,Without
}

public enum ChessBlockStatus
{
    弱棋,垒,空,错误,自己人
}

/// <summary>
/// 棋子基类
/// </summary>
public class Chess : MonoBehaviour
{

    [Header("棋子信息")]
    public Indentity indentity;     //棋子的归属
    public ChessType chessType;     //棋子的标识
    public int Hp = 1;              //血量
    public int nowPosition=-1;   //当前的位置
    public int NowPosition
    {
        get
        {
            return nowPosition;
        }
    }


    //关系附加的属性，也就是多走几步而已
    public int extraNumber;

    public Move _move;//移动脚本


    /*
     设置当前位置
     */
    public void SetNowPosition(int position)
    {
        this.nowPosition = position;
    }

    /*
     位置+1
     */
    public void PositionIndexIncrease()
    {
        nowPosition = (nowPosition + 1) % BattleManager.Instance.allowchessBlocks;
    }



    /*
     造成伤害
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
     * 死亡
     */
    private void Die()
    {
        //相应的计数器减少
        BattleManager.Instance.ChessCountDecrease(indentity);
        //先将物体从棋格信息中删除
        BattleManager.Instance.RemoveChess(nowPosition,this);
        //将物体销毁
        Destroy(gameObject);
    }

    /*
     调用调用移动的脚本
     */
    public virtual void PrepareMove()
    {


        //Debug.Log("你爹来罗");
        //移动之前先判断当前位置上有无关系，如果有就触发特殊效果

        //启动脚本，等他完成任务，move脚本是会自动关闭脚本

        //到达目标位置，判断有无敌人（如果是敌人的弱棋可以直接干掉）

    }

    /*
    移动结束后的善后工作
     */
    public virtual void LastMove()
    {
        //Debug.Log("你爹走咯");
    }


}


