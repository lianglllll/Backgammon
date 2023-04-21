using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public Transform[] boardObjects; // �洢Scene�����̸������
    public List<Vector3> boardPositions= new List<Vector3>(); // �洢����λ����Ϣ���б�

    void Awake()
    {
        //ͨ��Transform��ȡposition����ƫ��
        foreach (Transform i in boardObjects)
        {
            boardPositions.Add(i.position);
        }
    }

    public Vector3 SetPosition(Transform obj)
    {
        // ��ȡ����Transform�����λ�ú�����
        Vector3 position = obj.position;
        Vector3 scale = obj.lossyScale;

        // ���������objƽ���ϵĵ�
        float x = Random.Range(position.x - scale.x / 2, position.x + scale.x / 2);
        float z = Random.Range(position.z - scale.z / 2, position.z + scale.z / 2);
        Vector3 randomPoint = new Vector3(x, position.y, z);

        // ������ɵĵ��Ƿ���obj��״��Χ��
        Collider objCollider = obj.GetComponent<Collider>();
        if (objCollider != null && !objCollider.bounds.Contains(randomPoint))
        {
            // ������ɵĵ㲻��obj����״��Χ�ڣ�����������
            return SetPosition(obj);
        }

        return randomPoint;
    }

    /*
     * ��ȡ��������λ�õ��±�
     */
    public int GetChessBlockIndex(Transform aim)
    {
        for (int i = 0; i < boardObjects.Length; i++)
        {
            if (boardObjects[i] == aim)
            {
                return i;
            }
        }
        return -1;
    }


    /*
     ����ĳ�����Ķ���������block�ű����Ŀ����ݼ���
     */
    public void OpenPoisonousCircle(int pos,Indentity ind)
    {
        boardObjects[pos].GetComponent<Block>().harmIndentity = ind;
        boardObjects[pos].GetComponent<Block>().OpenPoisonousCircle();
    }

    /*
     �ر�ĳ�����Ķ�Ȧ
     */
    public void ClosePoisonousCircle(int pos)
    {
        boardObjects[pos].GetComponent<Block>().ClosePoisonousCircle();
    }


    /*
     �ж�����Ƿ����˶�Ȧ
     */
    public GameObject IsOpenPoisonousCircle(int pos)
    {
        GameObject aimBlock = boardObjects[pos].gameObject;

        if(aimBlock.GetComponent<Block>().harmIndentity != Indentity.Without)
        {
            return aimBlock;
        }

        return null;
    }


}