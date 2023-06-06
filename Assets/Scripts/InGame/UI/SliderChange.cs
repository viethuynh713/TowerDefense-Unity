using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderChange : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void OnSliderChange()
    {
        GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(slider.value).ToString();
    }
}
