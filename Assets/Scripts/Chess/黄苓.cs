using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
public class 黄苓 : Chess
{

    void Start()
    {
        chessType = ChessType.黄苓;
        单行();
        extraNumber = 0;
        _move = GetComponent<Move>();//todo这里会有可能有错误
    }

    /*
     单行
     */
    public void 单行()
    {
        Hp = 2;
    }


    /*
     相使，与另外的1个或者以上的棋子(大黄)，向前走两步
     */
    public void 相使()
    {
        extraNumber += 2;
    }

    /*
     相恶，向后走一步
     */
    public void 相恶()
    {
        extraNumber -= 1;
    }


    /*
    调用调用移动的脚本
    */
    public override void PrepareMove()
    {
        //移动之前先判断当前位置上有无关系，如果有就触发特殊效果
        if (BattleManager.Instance.IsExistTargetChessTypeByID(nowPosition, ChessType.大黄))
        {
            相使();
        }
        if (BattleManager.Instance.IsExistTargetChessTypeByID(nowPosition, ChessType.生姜))
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
        {//因为黄苓可以待在别人的垒中

            //扣血
            List<Chess> aimchessList = BattleManager.Instance.BolckInfos[nowPosition];
            Chess aimChess = BattleManager.Instance.getOneOppositeChess(nowPosition, indentity);
            if (aimChess != null)
            {
                aimChess.Harm();

            }
        }
        else if (blockStatus == ChessBlockStatus.垒)
        {
            //对自己造成伤害
            Hp--;
            if (Hp <= 0)
            {
                Destroy(gameObject);
                return;//后面不用执行了

            }
            //还没凉

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
