using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManagement : MonoBehaviour
{
    public GameObject currentShownCharacter;
    public string currentShownCharacterName;

    public GameObject[] Character;
    int x = 0;
    Vector3 leftPos = new Vector3(-4.5f, -2.74f, 0f);
    Vector3 rightPos = new Vector3(4.5f, -2.74f, 0f);
    Vector3 middlePos = new Vector3(0f, -2.74f, 0f);

    bool swipeNext;
    bool swipePrev;

    public GameObject selectButn;
    public GameObject TapToSelect;  
    public string TapToSelectMsg = "Tap to select";
    public GameObject SelectParticle;

    public Color CamClr;

    public AudioClip character_select_sound;
    public AudioClip character_swap_sound;

    void Start()
    {
        for(int i = 0; i < Character.Length; i++)
        {
            if(Character[i].name == PlayerPrefs.GetString("selected character"))
            {
                x = i;
                break;
            }
        }

        for (int i = 0; i < x; i++)
        {
            Character[i].transform.position = leftPos;
            Character[i].GetComponent<Animator>().SetBool("shrink", true);
        }
        for (int i = x + 1; i < Character.Length; i++)
        {
            Character[i].transform.position = rightPos;
            Character[i].GetComponent<Animator>().SetBool("shrink", true);
        }
        Character[x].transform.position = middlePos;
        Character[x].GetComponent<Animator>().SetBool("shrink", false);

        TapToSelectMsg = "Tap to select";
    }

    void Update()
    {
        TapToSelect.GetComponent<Text>().text = TapToSelectMsg;

        currentShownCharacter = Character[x];
        currentShownCharacterName = Character[x].name;

        if(currentShownCharacter && currentShownCharacter.GetComponent<CharScript>().isSelectable())
        {
            selectButn.GetComponent<Button>().interactable = true;
            
            TapToSelect.GetComponent<Text>().color = Color.white;
        }
        else
        {
            selectButn.GetComponent<Button>().interactable = false;
            TapToSelectMsg = "Can't select";
            TapToSelect.GetComponent<Text>().color = Color.red;
        }

        if (swipeNext)
        {
            Character[x].GetComponent<Animator>().SetBool("shrink", true);
            Character[x+1].GetComponent<Animator>().SetBool("shrink", false);

            Character[x].transform.position = Vector3.MoveTowards(Character[x].transform.position, leftPos, 0.25f);
            Character[x + 1].transform.position = Vector3.MoveTowards(Character[x + 1].transform.position, middlePos, 0.25f);
            if (Character[x].transform.position.x <= leftPos.x && Character[x + 1].transform.position.x <= middlePos.x)
            {
                x++;
                swipeNext = false;
            }
            TapToSelectMsg = "Tap to select";
        }
        if (swipePrev)
        {
            Character[x - 1].GetComponent<Animator>().SetBool("shrink", false);
            Character[x].GetComponent<Animator>().SetBool("shrink", true);

            Character[x].transform.position = Vector3.MoveTowards(Character[x].transform.position, rightPos, 0.25f);
            Character[x - 1].transform.position = Vector3.MoveTowards(Character[x - 1].transform.position, middlePos, 0.25f);
            if (Character[x].transform.position.x >= rightPos.x && Character[x - 1].transform.position.x >= middlePos.x)
            {
                x--;
                swipePrev = false;
            }
            TapToSelectMsg = "Tap to select";
        }
    }

    public void SelectCharacter()
    {
        AudioSource.PlayClipAtPoint(character_select_sound, Camera.main.transform.position);

        StartCoroutine("CamColorChange");

        PlayerPrefs.SetString("selected character", currentShownCharacterName);

        TapToSelectMsg = PlayerPrefs.GetString("selected character") + " selected.";

        Instantiate(SelectParticle, new Vector3(0, 2.6f, 0), Quaternion.identity);

        print(PlayerPrefs.GetString("selected character") + " selected.");
    }

    IEnumerator CamColorChange()
    {
        Camera.main.GetComponent<Camera>().backgroundColor = Color.black;

        yield return new WaitForSeconds(SelectParticle.GetComponent<ParticleSystem>().duration);

        Camera.main.GetComponent<Camera>().backgroundColor = CamClr;
    }

    public void SwipeNextCharacter()
    {
        AudioSource.PlayClipAtPoint(character_swap_sound, Camera.main.transform.position);
        if (Character.Length - 1 > x)
        {
            swipeNext = true;
        }
    }
    public void SwipePrevCharacter()
    {
        AudioSource.PlayClipAtPoint(character_swap_sound, Camera.main.transform.position);
        if (x != 0)
        {
            swipePrev = true;
        }
    }
}