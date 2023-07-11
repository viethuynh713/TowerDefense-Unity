using System;
using UnityEngine;
using DG.Tweening;

public class InitLobby : MonoBehaviour
{
    private void OnEnable()
    {
        transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBounce);
    }
}
