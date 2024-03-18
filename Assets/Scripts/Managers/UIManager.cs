using DG.Tweening;
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

    [Header("reset data TO DEL")]
    [SerializeField] private Button resetButton;

    private UISound sounds;
    private GameManager gm;

    private void Start()
    {
        gm = GameManager.Instance;
        sounds = UISound.Instance;
        winPanel.SetActive(false);
        optionsButton.gameObject.SetActive(true);
        skipButton.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);

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
            SceneManager.LoadScene("Gameplay");
        });
    }

    public void StartLevel()
    {
        sounds.PlaySound(SoundsUI.start);
        levelPanel.SetActive(false);
        StartCoroutine(playStartLevel());
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
