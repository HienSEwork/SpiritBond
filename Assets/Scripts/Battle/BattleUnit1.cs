using SpiritBond.Pet;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] Image unitImage;

    public PetInstance PetInstance { get; private set; }

    private void Awake()
    {
        if (unitImage == null)
        {
            unitImage = GetComponent<Image>();
        }
    }

    public void Setup(PetInstance petInstance)
    {
        PetInstance = petInstance;

        if (unitImage == null || petInstance == null || petInstance.petData == null)
        {
            return;
        }

        unitImage.sprite = petInstance.petData.battleSprite;
        unitImage.SetNativeSize();
    }

    public void TakeDamage(int damage)
    {
        if (PetInstance == null)
        {
            return;
        }

        PetInstance.currentHP = Mathf.Max(0, PetInstance.currentHP - Mathf.Max(0, damage));
    }
}
