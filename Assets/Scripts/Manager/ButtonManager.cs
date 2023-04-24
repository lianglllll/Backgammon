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

    //按键事件
    //打开七情图
    public void OpenSevenLove()
    {
        sevenLove.SetActive(true);
        chessBook.SetActive(false);
    }
    //打开棋类关系图
    public void OpenChessBook()
    {
        chessBook.SetActive(true);
        sevenLove.SetActive(false);
    }
    //打开手册书
    public void OpenHandBook()
    {
        chessBook.SetActive(true);
        handbookButton.SetActive(true);

    }//关闭手册书
    public void CloseHandBook()
    {
        chessBook.SetActive(false);
        sevenLove.SetActive(false);
        handbookButton.SetActive(false);
    }
    //开始游戏
    public void OpenGameUI()
    {
        menuUI.SetActive(false);
        gameUI.SetActive(true);
        handbookUI.SetActive(true);
        settingUI.SetActive(true);
        storyCardController.OpenCard();
    }
    //打开人员名单
    public void OpenList()
    {
        memberList.SetActive(true);
    }
    //关闭人员名单
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
    //重新加载场景
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        
        
    }
    public void BackMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
