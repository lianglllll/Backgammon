using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 乌头 : Chess
{
    // Start is called before the first frame update
    void Start()
    {
        chessType = ChessType.乌头;
        extraNumber = 0;
        _move = GetComponent<Move>();//todo这里会有可能有错误
    }



    /*
     相反，存在敌方半夏的时候，随机毒死本格中一个敌方棋子
     */
    public bool 相反()
    {

        //1.敌方的身份
        Indentity otherIdentity = Indentity.My == indentity ? Indentity.Enemy : Indentity.My;

        //2.先判断当前棋格中有无敌方半夏，如果有随机
        Chess chess = BattleManager.Instance.IsExistTargetChessTypeByID2(nowPosition, ChessType.半夏, otherIdentity);
        if (chess == null) return false;

        //3.如果这里有敌方的生姜就不会触发相反效果
        chess = BattleManager.Instance.IsExistTargetChessTypeByID2(nowPosition, ChessType.生姜, otherIdentity);
        if (chess != null) return false;


        //触发相反效果，随机对本棋格中一个敌方单位造成伤害
        Chess ch = BattleManager.Instance.RamGetAimChessInBlocks(nowPosition, otherIdentity);
        ch.Harm();

        return true;

    }







    /*
    调用调用移动的脚本
    */
    public override void PrepareMove()
    {        
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
            Chess aimchess = BattleManager.Instance.BolckInfos[nowPosition][0];
            aimchess.Harm();

        }
        else if (blockStatus == ChessBlockStatus.垒)
        {

            //看看能不能触发相反
            bool trigger  = 相反();

            if (trigger)
            {//相反触发
             //需要标记这个位置，变成毒区是，每回合杀死一个指定身份的棋子
             //通知battlemanager，由battlemanager进行转发
                BattleManager.Instance.OpenPoisonousCircle(nowPosition, indentity == Indentity.My ? Indentity.Enemy : Indentity.My);
                //这个毒如果没有解除将持续到游戏结束，，，
            }
            else
            {//没触发，就是自杀
                Destroy(gameObject);
                return;//后面不用执行了
            }
        }
        else if (blockStatus == ChessBlockStatus.错误)
        {
            Debug.LogError("chess.LastMove fail");
            return;
        }

        //在新的棋格添加自己的实例
        BattleManager.Instance.AddChess(nowPosition, this);

    }



}
