using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public GameObject[] stars;

    public GameObject levelNameText;

    Button buttonCompo;

    public int starRank;

    public string levelName;

    public AudioClip ButtonTapSound;

    void Start()
    {
        buttonCompo = GetComponent<Button>();

        levelName = gameObject.name;

        starRank = PlayerPrefs.GetInt(levelName);

        levelNameText.GetComponent<Text>().text = levelName;

        for(int i=0; i<starRank; i++)
        {
            stars[i].active = true;
        }

        //print(PlayerPrefs.GetInt("level unlocked", 4) + " ==== " + SceneManager.GetSceneByName(levelName).buildIndex);

        //if (PlayerPrefs.GetInt("level unlocked", 4) < SceneManager.GetSceneByName(levelName).)
        //{
        //    buttonCompo.interactable = false;
        //}
    }

    public void LoadSelectedLevel()
    {
        StartCoroutine("playButtonTap");
        Application.LoadLevel(levelName);
    }

    IEnumerator playButtonTap()
    {
        AudioSource.PlayClipAtPoint(ButtonTapSound, Camera.main.transform.position);
        yield return new WaitForSeconds(ButtonTapSound.length);
    }
}
