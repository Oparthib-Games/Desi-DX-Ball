using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyManager : MonoBehaviour
{
    public GameObject text;
    public string myName;

    void Start()
    {
        myName = this.gameObject.name;

        text.GetComponent<Text>().text = PlayerPrefs.GetInt(myName).ToString();
    }
}
