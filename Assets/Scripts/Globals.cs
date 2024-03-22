using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;


public class Globals : MonoBehaviour
{
    public static PlayerData MainPlayerData;
    public static bool IsSoundOn;
    public static bool IsMusicOn;
    public static bool IsInitiated;
    public static string CurrentLanguage;
    public static Translation Language;

    public static DateTime TimeWhenStartedPlaying;
    public static DateTime TimeWhenLastInterstitialWas;
    public static DateTime TimeWhenLastRewardedWas;
    public const float REWARDED_COOLDOWN = 70;
    public const float INTERSTITIAL_COOLDOWN = 70;
        
    public static bool IsMobile;
    public static bool IsDevelopmentBuild = false;
    
    public const float SCREEN_SAVER_AWAIT = 1f;

    //Gameplay
    public const float BALL_SPEED = 14;
    public static int Loses;
    public static bool IsTouched;
    public const int LAST_LVL = 100;
}
