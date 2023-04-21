using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int id;

    //todo记录本棋格上挂载的棋子，battlemanager管理block

    public Indentity harmIndentity = Indentity.Without;

    /*
     对本格中的某个harmIndentity身份的棋子造成伤害
     */
    private void RamdomHarm()
    {
        if (harmIndentity == Indentity.Without) return;
        Chess chess =  BattleManager.Instance.RamGetAimChessInBlocks(id, harmIndentity);
        //杀光了
        if (chess == null) return;
        chess.Harm();
    }

    /*
     开启毒圈
     */
    public void OpenPoisonousCircle()
    {

        if (harmIndentity == Indentity.Without) return;
        Indentity temp = harmIndentity == Indentity.Enemy ? Indentity.My : Indentity.Enemy;//谁触发的

        //通过身份来进行订阅不同的事件
        if (Indentity.My == temp)
        {
            BattleManager.Instance.myActionEvent.AddListener(RamdomHarm);

        }else if(Indentity.Enemy == temp)
        {
            BattleManager.Instance.enemyActionEvent.AddListener(RamdomHarm);
               
        }


    }

    /*
     关闭毒圈
     */
    public void ClosePoisonousCircle()
    {
        Indentity temp = harmIndentity == Indentity.Enemy ? Indentity.My : Indentity.Enemy;//谁触发的

        //通过身份来取消订阅
        if (Indentity.My == temp)
        {
            BattleManager.Instance.myActionEvent.RemoveListener(RamdomHarm);

        }
        else if (Indentity.Enemy == temp)
        {
            BattleManager.Instance.enemyActionEvent.RemoveListener(RamdomHarm);

        }

        harmIndentity = Indentity.Without;

    }



}
