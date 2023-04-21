using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayStatus : MonoBehaviour
{
    public Text statusText;


    // Start is called before the first frame update

    private void Awake()
    {
        BattleManager.Instance.StatusChangeEvent.AddListener(UpdataStatus);

    }


    //¸üÐÂtextUI
    public void UpdataStatus()
    {
        statusText.text = BattleManager.Instance.status.ToString();
    }


}
