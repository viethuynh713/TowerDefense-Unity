using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ButtonAnimation : MonoBehaviour
{

    private void OnEnable()
    {
        OnExit();
    }

    public void OnEnter()
    {
        DOTween.KillAll(true);
         transform.DOScale(Vector3.one*1.2f, 0.4f).SetEase(Ease.OutQuart);
    }
    public void OnExit()
    {
        DOTween.KillAll(true);

        transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.InQuart);
    }
}
