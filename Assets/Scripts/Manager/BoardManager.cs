using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public Transform[] boardObjects; // 存储Scene中棋盘格的数组
    public List<Vector3> boardPositions= new List<Vector3>(); // 存储格子位置信息的列表

    void Awake()
    {
        //通过Transform获取position存在偏差
        foreach (Transform i in boardObjects)
        {
            boardPositions.Add(i.position);
        }
    }

    public Vector3 SetPosition(Transform obj)
    {
        // 获取给定Transform对象的位置和缩放
        Vector3 position = obj.position;
        Vector3 scale = obj.lossyScale;

        // 随机生成在obj平面上的点
        float x = Random.Range(position.x - scale.x / 2, position.x + scale.x / 2);
        float z = Random.Range(position.z - scale.z / 2, position.z + scale.z / 2);
        Vector3 randomPoint = new Vector3(x, position.y, z);

        // 检查生成的点是否在obj形状范围内
        Collider objCollider = obj.GetComponent<Collider>();
        if (objCollider != null && !objCollider.bounds.Contains(randomPoint))
        {
            // 如果生成的点不在obj的形状范围内，则重新生成
            return SetPosition(obj);
        }

        return randomPoint;
    }

    /*
     * 获取卦象所在位置的下标
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
     开启某个棋格的毒区，设置block脚本里的目标身份即可
     */
    public void OpenPoisonousCircle(int pos,Indentity ind)
    {
        boardObjects[pos].GetComponent<Block>().harmIndentity = ind;
        boardObjects[pos].GetComponent<Block>().OpenPoisonousCircle();
    }

    /*
     关闭某个棋格的毒圈
     */
    public void ClosePoisonousCircle(int pos)
    {
        boardObjects[pos].GetComponent<Block>().ClosePoisonousCircle();
    }


    /*
     判断棋格是否开启了毒圈
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