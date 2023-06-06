using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class OpenPanel : MonoBehaviour
{
    public RectTransform RectTransformOpen;
    public RectTransform RectTransformParent;
    public RectTransform RectTransformClose;

    private Vector2 _currentPositionOpen;

    private void Awake()
    {
        _currentPositionOpen = RectTransformOpen.anchoredPosition;
    }

    private void OnEnable()
    {
        OpenClick();
    }

    private void OpenClick()
    {
        RectTransformOpen.anchoredPosition = new Vector2(_currentPositionOpen.x + RectTransformOpen.rect.width,_currentPositionOpen.y);

        RectTransformOpen.DOAnchorPosX(_currentPositionOpen.x, 0.3f).SetEase(Ease.OutCirc);
    }

    public void CloseClick()
    {
        RectTransformOpen.DOAnchorPosX(_currentPositionOpen.x + RectTransformOpen.rect.width, 0.3f).SetEase(Ease.InCirc).OnComplete(
            () =>
            {
                RectTransformParent.gameObject.SetActive(false);
                RectTransformClose.gameObject.SetActive(true);
                RectTransformOpen.anchoredPosition = _currentPositionOpen;
            });
    }
}
