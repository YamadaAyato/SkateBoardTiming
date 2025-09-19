using UnityEngine;
using UnityEngine.UI;

public class FillColorChange : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _image;

    private void Update()
    {
        float value = _slider.maxValue;

        if (value >= 0.9f ) _image.color = Color.yellow;
        else if (value >= 0.75f) _image.color = Color.yellowGreen;
        else if (value >= 0.5f) _image.color = Color.green;
        else _image.color = Color.gray;
    }
}
