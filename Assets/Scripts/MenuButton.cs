using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuButton : MonoBehaviour
{
    public TextMeshProUGUI buttonText;

    public void OnHover()
    {
        buttonText.fontStyle = FontStyles.Underline;
    }

    public void OnExit()
    {
        buttonText.fontStyle = FontStyles.Bold;
    }
}
