using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int id;

    //todo��¼������Ϲ��ص����ӣ�battlemanager����block

    public Indentity harmIndentity = Indentity.Without;

    /*
     �Ա����е�ĳ��harmIndentity��ݵ���������˺�
     */
    private void RamdomHarm()
    {
        if (harmIndentity == Indentity.Without) return;
        Chess chess =  BattleManager.Instance.RamGetAimChessInBlocks(id, harmIndentity);
        //ɱ����
        if (chess == null) return;
        chess.Harm();
    }

    /*
     ������Ȧ
     */
    public void OpenPoisonousCircle()
    {

        if (harmIndentity == Indentity.Without) return;
        Indentity temp = harmIndentity == Indentity.Enemy ? Indentity.My : Indentity.Enemy;//˭������

        //ͨ����������ж��Ĳ�ͬ���¼�
        if (Indentity.My == temp)
        {
            BattleManager.Instance.myActionEvent.AddListener(RamdomHarm);

        }else if(Indentity.Enemy == temp)
        {
            BattleManager.Instance.enemyActionEvent.AddListener(RamdomHarm);
               
        }


    }

    /*
     �رն�Ȧ
     */
    public void ClosePoisonousCircle()
    {
        Indentity temp = harmIndentity == Indentity.Enemy ? Indentity.My : Indentity.Enemy;//˭������

        //ͨ�������ȡ������
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
