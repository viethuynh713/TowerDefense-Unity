using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class TranslateUI : MonoBehaviour
{
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private Vector2 _anchorPosition;

    private void Awake()
    {
        _canvasGroup = gameObject.GetComponent<CanvasGroup>();
        _rectTransform = gameObject.GetComponent<RectTransform>();
        _anchorPosition = _rectTransform.anchoredPosition;
    }

    private void OnEnable()
    {
        OnAppear();
    }
    public void OnAppear()
    {
        DOTween.KillAll(true);
        _canvasGroup.alpha = 0;
        _rectTransform.anchoredPosition = _anchorPosition + new Vector2(100,0);
        _canvasGroup.DOFade(1, 0.3f).SetEase(Ease.InQuart);
        _rectTransform.DOAnchorPosX(_anchorPosition.x, 0.3f).SetEase(Ease.InQuart);
    }

    public void OnDisappear()
    {
        _canvasGroup.DOFade(0, 0.4f);
        _rectTransform.DOAnchorPosX(_anchorPosition.x - 100, 0.5f).SetEase(Ease.OutQuart).OnComplete(
            () => this.gameObject.SetActive(false));
    }
}
