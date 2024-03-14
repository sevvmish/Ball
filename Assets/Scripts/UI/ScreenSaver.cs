using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSaver : MonoBehaviour
{
    public static ScreenSaver Instance { get; private set; }

    [SerializeField] private RectTransform left;
    [SerializeField] private RectTransform right;

    private float speed = 1;

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

        left.gameObject.SetActive(true);
        right.gameObject.SetActive(true);

        speed = Globals.SCREEN_SAVER_AWAIT;
    }


    public void Open()
    {
        left.gameObject.SetActive(true);
        right.gameObject.SetActive(true);

        left.anchoredPosition = new Vector2(-1400,0);
        right.anchoredPosition = new Vector2(1400,0);

        left.DOAnchorPos(new Vector2(-3400, 0), speed).SetEase(Ease.InSine);
        right.DOAnchorPos(new Vector2(3400, 0), speed).SetEase(Ease.InSine);
    }

    public void Close()
    {
        left.gameObject.SetActive(true);
        right.gameObject.SetActive(true);

        left.anchoredPosition = new Vector2(-3400, 0);
        right.anchoredPosition = new Vector2(3400, 0);

        left.DOAnchorPos(new Vector2(-1400, 0), speed).SetEase(Ease.InSine);
        right.DOAnchorPos(new Vector2(1400, 0), speed).SetEase(Ease.InSine);
    }
}
