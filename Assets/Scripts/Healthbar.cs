using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField]
    Slider healthbarSlider;
    [SerializeField]
    private Image fill;


    private Color lowColour = Color.red;
    private Color highColour = Color.green;

    [SerializeField]
    Vector3 offset;

    public void ChangeHealth(float percent)
    {
        float p = percent / 100;
        healthbarSlider.value = p;
        fill.color = Color.Lerp(lowColour, highColour, p);

    }

#if UNITY_EDITOR
    [SerializeField]
    private Text debugStateText;

    public void SetDebugStateText(string state)
    {
        debugStateText.text = state;
    }
#endif
}
