using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public LevelManager GetLevelManager => levelManager;
    public UIManager GetUI => UI;
    public bool IsPlaying { get; private set; }
    public float PointerClickedCount;

    [SerializeField] private LevelManager levelManager;
    [SerializeField] private UIManager UI;
    [SerializeField] private InputManager inputManager;

    private bool isWin, isLose;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (Globals.IsInitiated)
        {
            playWhenInitialized();
            localize();
        }
    }

    
    public void CheckWinCondition()
    {
        if (levelManager.IsAllBlocksHit() && !isWin)
        {
            isWin = true;
            IsPlaying = false;
            Globals.MainPlayerData.Lvl++;
            SaveLoadManager.Save();
            StartCoroutine(playWin());
        }
        else if (!isWin)
        {
            if (!levelManager.IsAnyActiveBall() && !isLose)
            {
                isLose = true;
                IsPlaying = false;
                StartCoroutine(playLose());
            }
        }        
    }

    private IEnumerator playLose()
    {
        yield return new WaitForSeconds(0.3f);
        UISound.Instance.PlaySound(SoundsUI.lose);
        levelManager.Restart();
        print("lose");

        yield return new WaitForSeconds(0.3f);
        isLose = false;
        IsPlaying = true;
        inputManager.Restart();
    }

    private IEnumerator playWin()
    {
        yield return new WaitForSeconds(1f);
        levelManager.HideAll();
        UI.ShowWinPanel();
        UISound.Instance.PlaySound(SoundsUI.win);
        GoToNextLevel();
        print("win");
    }

    public void GoToNextLevel()
    {
        StartCoroutine(playNextLevel());
    }
    private IEnumerator playNextLevel()
    {
        yield return new WaitForSeconds(2f);
        ScreenSaver.Instance.Close();

        yield return new WaitForSeconds(Globals.SCREEN_SAVER_AWAIT / 2);
        UI.HideWinPanel();
        yield return new WaitForSeconds(Globals.SCREEN_SAVER_AWAIT / 2);
        SceneManager.LoadScene("Gameplay");
    }

    private void Update()
    {
        if (PointerClickedCount > 0) PointerClickedCount -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
        {
            Globals.MainPlayerData = new PlayerData();
            SaveLoadManager.Save();
            SceneManager.LoadScene("Gameplay");
        }

        if (YandexGame.SDKEnabled && !Globals.IsInitiated)
        {
            Globals.IsInitiated = true;

            SaveLoadManager.Load();

            print("SDK enabled: " + YandexGame.SDKEnabled);
            Globals.CurrentLanguage = YandexGame.EnvironmentData.language;
            print("language set to: " + Globals.CurrentLanguage);

            Globals.IsMobile = YandexGame.EnvironmentData.isMobile;
            print("platform mobile: " + Globals.IsMobile);

            if (Globals.MainPlayerData.S == 1)
            {
                Globals.IsSoundOn = true;
                AudioListener.volume = 1;
            }
            else
            {
                Globals.IsSoundOn = false;
                AudioListener.volume = 0;
            }

            if (Globals.MainPlayerData.Mus == 1)
            {
                Globals.IsMusicOn = true;
            }
            else
            {
                Globals.IsMusicOn = false;
            }

            print("sound is: " + Globals.IsSoundOn);

            if (Globals.TimeWhenStartedPlaying == DateTime.MinValue)
            {
                Globals.TimeWhenStartedPlaying = DateTime.Now;
                Globals.TimeWhenLastInterstitialWas = DateTime.Now;
                Globals.TimeWhenLastRewardedWas = DateTime.Now.Subtract(new TimeSpan(1, 0, 0));
            }

            localize();
            playWhenInitialized();
        }
    }

    private void playWhenInitialized()
    {
        if (Globals.IsMobile)
        {
            QualitySettings.antiAliasing = 2;
        }
        else
        {
            QualitySettings.antiAliasing = 4;
        }

        YandexGame.StickyAdActivity(!Globals.MainPlayerData.AdvOff);
        levelManager.SetData(Globals.MainPlayerData.Lvl);        
        UI.StartLevel();
    }

    public void GameReady()
    {
        IsPlaying = true;
    }

    private void localize()
    {
        Globals.Language = Localization.GetInstanse(Globals.CurrentLanguage).GetCurrentTranslation();
        
    }
}
