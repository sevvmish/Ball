using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("level panel")]
    [SerializeField] private GameObject levelPanel;
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("win panel")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TextMeshProUGUI winText;

    [Header("options")]
    [SerializeField] private Button optionsButton;
    [SerializeField] private OptionsUI optionsUI;

    [Header("restart")]
    [SerializeField] private Button restartButton;

    [Header("skip")]
    [SerializeField] private Button skipButton;

    
    [Header("ADV rewarded")]
    [SerializeField] private Rewarded rewarded;
    [SerializeField] private GameObject rewardedPanel;
    [SerializeField] private GameObject rewardedErrorPanel;
    [SerializeField] private TextMeshProUGUI rewardedText;
    [SerializeField] private TextMeshProUGUI rewardedErrorText;
    [SerializeField] private Button rewardedOK;
    [SerializeField] private Button rewardedNO;

    [Header("tutorial")]
    [SerializeField] private GameObject tutorial1;
    [SerializeField] private RectTransform hand;
    [SerializeField] private RectTransform point1;
    [SerializeField] private RectTransform point2;
    [SerializeField] private GameObject sign1;
    [SerializeField] private TextMeshProUGUI sign1Text;
    [SerializeField] private GameObject sign2;
    [SerializeField] private TextMeshProUGUI sign2Text;
    [SerializeField] private GameObject arrow;
    [SerializeField] private Image up;
    [SerializeField] private Image down;

    [Header("final")]
    [SerializeField] private GameObject finalPanel;
    [SerializeField] private TextMeshProUGUI finalText;
    [SerializeField] private Button resetButton;

    private UISound sounds;
    private GameManager gm;
    private bool isReady;

    private void Start()
    {
        tutorial1.SetActive(false);
        hand.gameObject.SetActive(false);
        sign1.SetActive(false);
        sign2.SetActive(false);
        arrow.SetActive(false);
        finalPanel.SetActive(false);

        gm = GameManager.Instance;
        sounds = UISound.Instance;
        winPanel.SetActive(false);
        optionsButton.gameObject.SetActive(true);
        skipButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);

        rewardedPanel.SetActive(false);
        rewardedErrorPanel.SetActive(false);

        ScreenSaver.Instance.Open();

        optionsButton.onClick.AddListener(() => 
        {
            sounds.PlaySound(SoundsUI.click);
            optionsUI.OpenPanel();
        });

        resetButton.onClick.AddListener(() =>
        {
            sounds.PlaySound(SoundsUI.click);
            Globals.MainPlayerData = new PlayerData();
            SaveLoadManager.Save();
            SceneManager.LoadScene("Gameplay");
        });

        restartButton.onClick.AddListener(() =>
        {
            sounds.PlaySound(SoundsUI.click);            
            SceneManager.LoadScene("Gameplay");
        });

        skipButton.onClick.AddListener(() =>
        {
            sounds.PlaySound(SoundsUI.click);
            rewardedPanel.SetActive(true);
            rewardedText.text = Globals.Language.RewardedText;
        });
        rewardedOK.onClick.AddListener(() =>
        {
            sounds.PlaySound(SoundsUI.click);
            makeSkip();
        });
        rewardedNO.onClick.AddListener(() =>
        {
            sounds.PlaySound(SoundsUI.click);
            rewardedPanel.SetActive(false);
        });      

        
    }

    public void Restart()
    {
        isReady = false;
    }

    public void Final()
    {
        finalText.text = Globals.Language.FinalText;
        finalPanel.SetActive(true);
        levelPanel.SetActive(false);
        optionsButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);

    }

    private void Update()
    {
        
        if (Globals.IsInitiated && !isReady)
        {
            isReady = true;

            if (Globals.MainPlayerData.Lvl >= 5 && Globals.Loses >= 5)
            {
                skipButton.GetComponent<TrambleUI>().enabled = true;
                skipButton.GetComponent<RectTransform>().sizeDelta = new Vector2(150, 150);
            }
            else
            {
                skipButton.GetComponent<TrambleUI>().enabled = false;
                skipButton.GetComponent<RectTransform>().sizeDelta = new Vector2(130, 130);
            }

            if (Globals.MainPlayerData.Lvl >= 5 && Globals.MainPlayerData.Lvl < Globals.LAST_LVL && !Globals.MainPlayerData.AdvOff && (DateTime.Now - Globals.TimeWhenLastRewardedWas).TotalSeconds >= Globals.REWARDED_COOLDOWN)
            {
                skipButton.gameObject.SetActive(true);
            }
            else
            {
                skipButton.gameObject.SetActive(false);
            }
        }
    }

    public void StartLevel()
    {
        sounds.PlaySound(SoundsUI.start);
        levelPanel.SetActive(false);
        StartCoroutine(playStartLevel());

        if (Globals.MainPlayerData.Lvl == 0 && !Globals.MainPlayerData.Tut1)
        {
            StartCoroutine(playTutorial1());
        }
    }
    private IEnumerator playStartLevel()
    {
        yield return new WaitForSeconds(Globals.SCREEN_SAVER_AWAIT/2f);
        int level = Globals.MainPlayerData.Lvl;
        if (level > 0)
        {
            levelPanel.SetActive(true);
            levelText.text = Globals.Language.Level + " " + level;

            RectTransform rect = levelPanel.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0, -600);
            rect.DOAnchorPos(new Vector2(0, -40), 0.5f).SetEase(Ease.InOutQuad);

        }
        else
        {
            levelPanel.SetActive(false);
        }

        yield return new WaitForSeconds(0.5f);
        GameManager.Instance.GameReady();
    }

    private void makeSkip()
    {        
        rewarded.OnRewardedEndedOK = NextLevelAfterRewarded;
        rewarded.OnError = rewardedError;
        rewarded.ShowRewardedVideo();
    }

    private void rewardedError()
    {
        rewardedErrorText.text = Globals.Language.RewardedError;
        rewardedErrorPanel.SetActive(true);
        sounds.PlaySound(SoundsUI.error);
        StartCoroutine(playError());
    }
    private IEnumerator playError()
    {
        yield return new WaitForSeconds(3f);
        rewardedErrorPanel.SetActive(false);
    }

    private void NextLevelAfterRewarded()
    {
        gm.NextLevelForRewarded();
    }

    private IEnumerator playTutorial1()
    {
        up.color = new Color(0, 0, 0, 0);
        down.color = new Color(0, 0, 0, 0);
        up.DOColor(new Color(0, 0, 0, 1), 2f).SetEase(Ease.Linear);
        down.DOColor(new Color(0, 0, 0, 1), 2f).SetEase(Ease.Linear);

        winPanel.SetActive(false);
        optionsButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        gm.PointerClickedCount = 1000;

        yield return new WaitForSeconds(Globals.SCREEN_SAVER_AWAIT);
        Globals.MainPlayerData.Tut1 = true;
        SaveLoadManager.Save();

        tutorial1.SetActive(true);
        hand.gameObject.SetActive(true);
        Globals.IsTouched = false;

        hand.anchoredPosition = point1.anchoredPosition;
        sign1.SetActive(true);
        sign1.transform.localScale = Vector3.zero;
        sign1.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutSine);
        sign1Text.text = Globals.Language.Tutorial1_sign1;
        sounds.PlaySound(SoundsUI.pop);
        yield return new WaitForSeconds(2f);

        hand.DOAnchorPos(point2.anchoredPosition, 2f).SetEase(Ease.Linear);
        sign2.SetActive(true);
        sign2.transform.localScale = Vector3.zero;
        sign2.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutSine);
        sign2Text.text = Globals.Language.Tutorial1_sign2;
        sounds.PlaySound(SoundsUI.pop);

        for (float i = 0; i < 2; i+=0.1f)
        {
            if (Globals.IsTouched) break;
            yield return new WaitForSeconds(0.1f);
        }

        gm.PointerClickedCount = 0;

        arrow.SetActive(true);
        sign1.SetActive(false);

        for (float i = 0; i < 5; i += 0.1f)
        {
            if (Globals.IsTouched) break;
            yield return new WaitForSeconds(0.1f);
        }
        sign2.SetActive(false);
        hand.gameObject.SetActive(false);
        tutorial1.SetActive(false);
        arrow.SetActive(false);

        optionsButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }

    public void ShowWinPanel()
    {
        optionsButton.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        levelPanel.SetActive(false);

        winText.text = Globals.Language.WinText;
        winPanel.SetActive(true);
        winPanel.transform.localScale = Vector3.zero;
        winPanel.transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutQuad).OnComplete(() => { winPanel.transform.DOPunchPosition(new Vector3(20, 20, 1), 0.3f, 30).SetEase(Ease.InOutQuad); });
    }

    public void HideWinPanel()
    {
        winPanel.transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.InOutQuad);
    }
}
