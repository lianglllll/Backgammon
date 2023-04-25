using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Selectable : MonoBehaviour
{
    public GameObject selectedObject;//��ǰѡ��obj
    private Camera mainCamera;
    public BoardManager boardManager;

    public bool EnableLefMouseClick = true;
    public Indentity indentity ;//ֻ��ѡ�Լ�����


    bool onCardGroup = false;
    bool canPut = false;

    private void Awake()
    {
        BattleManager.Instance.StatusChangeEvent.AddListener(indentityChange);
    }

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (EnableLefMouseClick)
        {
            LeftMouseClick();
        }
        if (!EnableLefMouseClick)
        {
            RightMouseClick();
        }
    }

    /*
     ���β�������ݸı䣬���ڿ������ѡ���Լ�������
     */
    public void indentityChange()
    {
        //����battlemanager
        if (BattleManager.Instance.status == GameStatus.MyLayTime || BattleManager.Instance.status == GameStatus.MyAction)
        {
            indentity = Indentity.My;
        }
        else
        {
            indentity = Indentity.Enemy;
        }
    }




    //����¼����������Ƴ�������
    void LeftMouseClick()
    {


        if (Input.GetMouseButtonDown(0))
        {
            //�ر��������
            if (BattleManager.Instance.status != GameStatus.MyLayTime && BattleManager.Instance.status != GameStatus.EnemyLayTime)
            {
                EnableLefMouseClick = false;
            }


            //�жϱ����Ƿ��Ѿ����������
            if (BattleManager.Instance.GetLayNumber() == 0)
            {
                return;
            }

            LayerMask chessMask = LayerMask.GetMask("Chess");
            LayerMask chessBlockMask = LayerMask.GetMask("ChessBlock");
            LayerMask cardGroupMask = LayerMask.GetMask("CardGroup");

            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            bool onCardGroup = Physics.Raycast(ray, out hit, Mathf.Infinity, cardGroupMask);

            if (onCardGroup && selectedObject == null
                    && Physics.Raycast(ray, out hit, Mathf.Infinity, chessMask)
                        && hit.transform.gameObject.CompareTag("chessman")//ѡ��׶Σ����߼�⵽��������Ҫ�������ǩ������ſ���
                            && hit.transform.gameObject.GetComponent<Chess>().indentity == indentity)//���һ�����Ҫ����ĳ����Ӫ�Ĳ���
            {
                selectedObject = hit.transform.gameObject;
                Debug.Log("ѡ������" + selectedObject);
                //canPut = true;
            }
            else if (selectedObject != null && Physics.Raycast(ray, out hit, Mathf.Infinity, chessBlockMask)//���ý׶ηŵ�chessm�㼶��
                        && !BattleManager.Instance.IsExistOtherChessInBolck(boardManager.GetChessBlockIndex(hit.transform)))//���Ҹ�����û�ез�������
            {
                selectedObject.transform.position = hit.point;

                //��Ҫ�����Ӧ�Ĺ��ϼ������gameobject
                //��ô��ȡ�أ�
                int i = boardManager.GetChessBlockIndex(hit.transform);//��ȡ�����±�

                BattleManager.Instance.AddChess(i, selectedObject.GetComponent<Chess>());//��ӵ�battlemanager��������

                Debug.Log("Ŀ������λ��" + hit.transform.gameObject+":"+i);

                //�����ӵ�λ����Ϣ��ֵ��������������ǰ����һ������У�
                selectedObject.GetComponent<Chess>().SetNowPosition(i);
                //�ر��ƶ��ű�
                selectedObject.GetComponent<Move>().enabled = false;

                //������ʱ����
                selectedObject = null;

                //����Ҫ����battleManager��ֻ��Ҫ�����޷��������Ӽ��ɣ���ʱ�������Ȼ��ȥ���غϽ����İ�ť
                BattleManager.Instance.LayNumberDecrease();//���óɹ������ַ��ô�����һ
            }
        }
    }


    //�Ҽ��¼���ѡ�����ӣ��ſ�ѡ�����ӵ��ƶ��ű�
    void RightMouseClick()
    {


        if (Input.GetMouseButtonDown(1))
        {
            //�ж��Ƿ�Ϊ�ж��׶β����Ѿ�����ɸ��
            if ((BattleManager.Instance.status != GameStatus.MyAction && BattleManager.Instance.status != GameStatus.EnemyAction)|| BattleManager.Instance.StepNum <= 0)
            {
                return;//��ʱ���������ѡ��
            }

            LayerMask chessMask = LayerMask.GetMask("Chess");

            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, chessMask) 
                    && hit.transform.gameObject.CompareTag("chessman")
                        && hit.transform.gameObject.GetComponent<Chess>().indentity == indentity)//���ϱ��غϲ�������Ӫ
            {
                if (selectedObject != null)
                {
                    selectedObject.GetComponent<Move>().enabled = false;//���ܻ�����ѡ�У��ر���һ���ű�
                    selectedObject = null;
                }

                selectedObject = hit.transform.gameObject;
                Debug.Log("ѡ������" + selectedObject);

                selectedObject.GetComponent<Move>().enabled = true;//�ſ����Ľű����ȴ��ƶ��¼�����
            }
        }
    }


}