using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaSelect : MonoBehaviour
{
    public int starsNeededToUnlock;
    public int currentTotalStars;
    string levelNameGenerate;

    public GameObject arenaPanel;
    public GameObject respectiveArena;

    void Start()
    {
        for(int i=0; i<18; i++)
        {
            levelNameGenerate = "Level " + i.ToString();

            currentTotalStars += PlayerPrefs.GetInt(levelNameGenerate);

            print(levelNameGenerate + " = " + currentTotalStars);   ///Prints Total Stars
            //print(levelNameGenerate + " = " + PlayerPrefs.GetInt(levelNameGenerate)); ///Prints Star of each level 
        }

        if(currentTotalStars >= starsNeededToUnlock)
        {
            GetComponent<Button>().interactable = true;
        }
        else
        {
            GetComponent<Button>().interactable = false;
        }
    }

    public void ActiveArena()
    {
        respectiveArena.active = true;
        arenaPanel.active = false;
    }
}
