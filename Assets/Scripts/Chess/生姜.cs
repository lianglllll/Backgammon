using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 生姜 : Chess
{
    void Start()
    {
        chessType = ChessType.生姜;
        extraNumber = 0;
        _move = GetComponent<Move>();//todo这里会有可能有错误
    }

    /*
    相恶
    */
    public void 相恶()
    {
        BattleManager.Instance.ShowRelationship("相恶");
        extraNumber -= 1;
    }


    /*
     相杀/相畏
     */
    public void 相杀相畏()
    {

        //解毒,当前棋格是否在毒圈的状态
        GameObject nowBlock = BattleManager.Instance.IsOpenPoisonousCircle(nowPosition);
        if (nowBlock == null) return;
        //只有毒杀本方的，才去解除
        if(nowBlock.GetComponent<Block>().harmIndentity != indentity)
        {
            return;
        }

        if (IsArriveLifeGate() && (BattleManager.Instance.StepNum + extraNumber) == 1)
        {
            EntryVictoryPoint();
            return;
        }


        BattleManager.Instance.ShowRelationship("相杀相畏");

        //触发解毒效果，就是关闭当前格子的毒杀效果
        nowBlock.GetComponent<Block>().ClosePoisonousCircle();

    }



    /*
   调用调用移动的脚本
   */
    public override void PrepareMove()
    {
        //移动之前先判断当前位置上有无关系，如果有就触发特殊效果
        if (BattleManager.Instance.IsExistTargetChessTypeByID(nowPosition, ChessType.黄苓))
        {
            相恶();
        }
        //在当前棋格信息中移除自己
        BattleManager.Instance.RemoveChess(nowPosition, this);

        //启动脚本，等他完成任务，move脚本是会自动关闭脚本
        _move._Move(BattleManager.Instance.StepNum + extraNumber);

    }

    /*
     移动结束后的善后工作，这个是给move脚本执行完之后调用的
     */
    public override void LastMove()
    {


        extraNumber = 0;//效果结束

        //到达目标位置，判断有无敌人（如果是敌人的弱棋可以直接干掉）
        //目标位置也就是自身现在的位置
        ChessBlockStatus blockStatus = BattleManager.Instance.GetChessBlockStatus(nowPosition);   //todo存在空引用

        if (blockStatus == ChessBlockStatus.弱棋)
        {
            //扣血
            Chess aimchess = BattleManager.Instance.getOneOppositeChess(nowPosition, indentity);
            if (aimchess != null)
            {
                aimchess.Harm();
            }

        }
        else if (blockStatus == ChessBlockStatus.垒)
        {
            Destroy(gameObject);
            return;//后面不用执行了

        }
        else if (blockStatus == ChessBlockStatus.错误)
        {
            Debug.LogError("chess.LastMove fail");
            return;
        }

        //触发
        相杀相畏();

        //在新的棋格添加自己的实例
        BattleManager.Instance.AddChess(nowPosition, this);

    }




}
