using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    public void SetHP(float hpNormalized)
    {
        slider.value = hpNormalized;
    }
}