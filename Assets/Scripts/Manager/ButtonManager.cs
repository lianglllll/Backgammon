using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject sevenLove;
    public GameObject chessBook;
    public GameObject handbookButton;
    public GameObject handbookUI;
    public GameObject menuUI;
    public GameObject gameUI;
    public GameObject memberList;
    public GameObject settingUI;
    public GameObject settingImage;
    public GameObject target;
    
    public StoryCardController storyCardController;

    private void Awake()
    {
        sevenLove.SetActive(false);
        chessBook.SetActive(false);
        handbookButton.SetActive(false);
        gameUI.SetActive(false);
        handbookUI.SetActive(false);
        memberList.SetActive(false);
        settingUI.SetActive(false);

        menuUI.SetActive(true);

    }

    //�����¼�
    //������ͼ
    public void OpenSevenLove()
    {
        sevenLove.SetActive(true);
        chessBook.SetActive(false);
    }
    //�������ϵͼ
    public void OpenChessBook()
    {
        chessBook.SetActive(true);
        sevenLove.SetActive(false);
    }
    //���ֲ���
    public void OpenHandBook()
    {
        chessBook.SetActive(true);
        handbookButton.SetActive(true);

    }//�ر��ֲ���
    public void CloseHandBook()
    {
        chessBook.SetActive(false);
        sevenLove.SetActive(false);
        handbookButton.SetActive(false);
    }
    //��ʼ��Ϸ
    public void OpenGameUI()
    {
        menuUI.SetActive(false);
        gameUI.SetActive(true);
        handbookUI.SetActive(true);
        settingUI.SetActive(true);
        storyCardController.OpenCard();
    }
    //����Ա����
    public void OpenList()
    {
        memberList.SetActive(true);
    }
    //�ر���Ա����
    public void CloseList()
    {
        memberList.SetActive(false);
    }

    public void OpenSetting()
    {
        settingImage.SetActive(true);

    }
    public void CloseSetting()
    {
        settingImage.SetActive(false);
    }
    //���¼��س���
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
        
    }
    public void BackMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
