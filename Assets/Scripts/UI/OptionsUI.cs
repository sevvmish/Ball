using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Button optionsB;
    [SerializeField] private Button continueB;
    [SerializeField] private Button soundButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Sprite soundOnSprite;
    [SerializeField] private Sprite soundOffSprite;
    [SerializeField] private Sprite musicOnSprite;
    [SerializeField] private Sprite musicOffSprite;

    // Start is called before the first frame update
    void Start()
    {
        continueB.onClick.AddListener(() => 
        {
            UISound.Instance.PlaySound(SoundsUI.click);
            optionsB.gameObject.SetActive(true);
            optionsPanel.SetActive(false);
        });

        soundButton.onClick.AddListener(() =>
        {
            if (Globals.IsSoundOn)
            {
                Globals.IsSoundOn = false;
                soundButton.transform.GetChild(0).GetComponent<Image>().sprite = soundOffSprite;
                AudioListener.volume = 0;
            }
            else
            {
                UISound.Instance.PlaySound(SoundsUI.click);
                Globals.IsSoundOn = true;
                soundButton.transform.GetChild(0).GetComponent<Image>().sprite = soundOnSprite;
                AudioListener.volume = 1f;
            }

            SaveLoadManager.Save();
        });

        musicButton.onClick.AddListener(() =>
        {
            if (Globals.IsMusicOn)
            {
                Globals.IsMusicOn = false;
                musicButton.transform.GetChild(0).GetComponent<Image>().sprite = musicOffSprite;
                AmbientSound.Instance.StopAll();
            }
            else
            {
                UISound.Instance.PlaySound(SoundsUI.click);
                Globals.IsMusicOn = true;
                musicButton.transform.GetChild(0).GetComponent<Image>().sprite = musicOnSprite;
                AmbientSound.Instance.ContinuePlaying();
            }

            SaveLoadManager.Save();
        });
    }

    public void OpenPanel()
    {
        optionsB.gameObject.SetActive(false);
        optionsPanel.SetActive(true);

        if (Globals.IsSoundOn)
        {
            soundButton.transform.GetChild(0).GetComponent<Image>().sprite = soundOnSprite;
        }
        else
        {
            soundButton.transform.GetChild(0).GetComponent<Image>().sprite = soundOffSprite;
        }

        if (Globals.IsMusicOn)
        {
            musicButton.transform.GetChild(0).GetComponent<Image>().sprite = musicOnSprite;
        }
        else
        {
            musicButton.transform.GetChild(0).GetComponent<Image>().sprite = musicOffSprite;
        }
    }
}
