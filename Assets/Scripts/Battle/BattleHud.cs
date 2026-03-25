using SpiritBond.Pet;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;

    public void SetData(PetInstance petInstance)
    {
        if (petInstance == null || petInstance.petData == null)
        {
            if (nameText != null)
            {
                nameText.text = string.Empty;
            }

            if (levelText != null)
            {
                levelText.text = string.Empty;
            }

            if (hpBar != null)
            {
                hpBar.SetHP(0f);
            }

            return;
        }

        if (nameText != null)
        {
            nameText.text = petInstance.petData.petName;
        }

        if (levelText != null)
        {
            levelText.text = "Lvl " + petInstance.level;
        }

        RefreshHP(petInstance);
    }

    public void RefreshHP(PetInstance petInstance)
    {
        if (hpBar == null || petInstance == null)
        {
            return;
        }

        int maxHP = petInstance.petData != null ? petInstance.petData.maxHP : 0;
        float hpNormalized = maxHP > 0 ? (float)petInstance.currentHP / maxHP : 0f;
        hpBar.SetHP(hpNormalized);
    }
}
