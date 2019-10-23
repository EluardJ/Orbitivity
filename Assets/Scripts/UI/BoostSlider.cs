#region Author
/////////////////////////////////////////
//   Judicaël Eluard
/////////////////////////////////////////
#endregion

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostSlider : MonoBehaviour
{
    #region Variables
    private Image fillImage;
    private Slider boostSlider;
    #endregion

    #region Unity's Functions
    void Start()
    {
        boostSlider = gameObject.GetComponent<Slider>();
        fillImage = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        fillImage.color = Color.yellow;

        GameEvents.current.onSliderChange += AdjustSliderValue;
        GameEvents.current.onSliderImageColorChange += AdjustFillImageColor;
    }
    #endregion

    #region Functions
    private void AdjustSliderValue(float number)
    {
        boostSlider.value = number;
    }

    private void AdjustFillImageColor(Color color)
    {
        fillImage.color = color;
    }
    #endregion
}
