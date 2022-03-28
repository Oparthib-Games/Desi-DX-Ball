using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharScript : MonoBehaviour
{
    public int like;
    public int love;
    public int haha;
    public int wow;
    public int sad;
    public int angry;

    public int rec_like;
    public int rec_love;
    public int rec_haha;
    public int rec_wow;
    public int rec_sad;
    public int rec_angry;

    void Start()
    {
        like = PlayerPrefs.GetInt("like");
        love = PlayerPrefs.GetInt("love");
        haha = PlayerPrefs.GetInt("haha");
        wow = PlayerPrefs.GetInt("wow");
        sad = PlayerPrefs.GetInt("sad");
        angry = PlayerPrefs.GetInt("angry");
    }

    public bool isSelectable()
    {
        if(rec_like > like || rec_love > love || rec_haha > haha || rec_wow > wow || rec_sad > sad || rec_angry > angry)
        {
            return false;
        }
        return true;
    }
}
