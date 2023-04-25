using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Move : MonoBehaviour
{
    private BoardManager boardManager;//������Ϣ
    private Chess chess;              //������Ϣ
    //public int index = 0;//��¼��ǰPlayerλ�ڵڼ��������

    Vector3 deviation = new Vector3(0, 0.6f, 0);//���������Ӳ���ģ�룬ֱ���ƶ�������񴦻���Ϊy��ֵ��û�����̣���Ϊ��ʱ��û�и���ģ�

    Vector3 targetPosition;

    public float speed = 1;

    bool isMove = false;
    bool isMove2 = false;//�ƶ�һ����

    //��紫��ģ���Ҫ�ߵĲ���
    private int stepNum = 0;

    void Start()
    {
        // ��ȡBoardManager�ű�
        boardManager = GameObject.FindObjectOfType<BoardManager>();
        chess = GetComponent<Chess>();
    }

    void Update() 
    {

        if (isMove) 
        { 
            isMove = false;
            StartCoroutine(MoveTowords()); 
        } else if (isMove2)
        {
            isMove2 = false;
            StartCoroutine(MoveTowords2());
        }
    }


    //�����ṩһ���ƶ�stepNum���Ľӿ�
    public void _Move(int stepNum)
    {
        this.stepNum = stepNum;
        isMove = true;
    }

    //�����ṩһ���ƶ�stepNum���Ľӿ�
    public void _Move2()
    {
        isMove2 = true;
    }


    /*
     Э�̿�ʼ�ƶ���Ӧ������
     */
    public IEnumerator MoveTowords()
    {
        Debug.Log("��������" + stepNum); 
        int targetIndex = (chess.NowPosition + stepNum) % BattleManager.Instance.allowchessBlocks;; //��ȡĿ�������±�


        Destroy(gameObject.GetComponent<Rigidbody>());//ɾ�������ӵĸ��壬��ֹ������������ײ

        //ѭ��stepNum��
        while (stepNum-- > 0) 
        {
            //����λ��+1
            chess.PositionIndexIncrease();

            // ��ȡĿ����block��λ�ã���һ���Ƚ������λ�ã�������block�����ķ�Χ֮�ڵ�һ��position��
            targetPosition = boardManager.SetPosition(boardManager.boardObjects[chess.NowPosition].transform);
            //targetPosition = boardManager.boardPositions[chess.NowPosition];

            //ÿһ֡�����ƶ�������ֱ������Ŀ����blockλ��
            while (transform.position != targetPosition + deviation) 
            { 
                transform.position = Vector3.MoveTowards
                    (transform.position, targetPosition + deviation, speed * Time.deltaTime); 
                yield return null; 
            } 
        }

        //֪ͨBattleManager״̬�Ѿ��ı䣬��ô֪ͨ������ͨ������Stepnum������
        BattleManager.Instance.StepNum = -1;//������������ѡ���κ����ӽ����ƶ��ˡ���Ҫ���½�stepnum��Ϊ>0�����ֲſ�������ѡ��

        //����chess���ƶ���ű����������ƺ���
        gameObject.GetComponent<Chess>().LastMove();

        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<Move>().enabled = false; //�����ƶ��ű�
        

    }

    /// <summary>
    /// �ƶ�һ��
    /// </summary>
    /// <returns></returns>
    public IEnumerator MoveTowords2()
    {

        int index = GetComponent<Chess>().indentity == Indentity.My ? 9 : 8;

        targetPosition = BattleManager.Instance.boardManager.boardObjects[index].position;
        /*gameObject.transform.position = targetPosition;
        yield return null;*/
        Destroy(gameObject.GetComponent<Rigidbody>());//ɾ�������ӵĸ��壬��ֹ������������ײ

        //ÿһ֡�����ƶ�������ֱ������Ŀ����blockλ��
        while (transform.position != targetPosition + deviation)
        {
            transform.position = Vector3.MoveTowards
                (transform.position, targetPosition + deviation, speed * Time.deltaTime);
            yield return null;
        }
        BattleManager.Instance.StepNum = -1;//������������ѡ���κ����ӽ����ƶ��ˡ���Ҫ���½�stepnum��Ϊ>0�����ֲſ�������ѡ��
        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<Move>().enabled = false; //�����ƶ��ű�

    }


}



