using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
    public Slider barFill;
    public Image barImage;

    private bool _isFlashing;

    // Set the bar's maximum sliding value
    public void SetMaxValue(int maxValue)
    {
        barFill.maxValue = maxValue;
        barFill.value = maxValue;
    }

    // Give bar a new value
    public void SetValue(int value)
    {
        barFill.value = value;

        if (!_isFlashing)
            StartCoroutine(DamageFlash());
    }

    private IEnumerator DamageFlash()
    {
        _isFlashing = true;
        barImage.color = new Color(barImage.color.r, barImage.color.g, barImage.color.b, 0.5f);

        yield return new WaitForSeconds(0.10f);

        _isFlashing = false;
        barImage.color = new Color(barImage.color.r, barImage.color.g, barImage.color.b, 1f);
    }
}
