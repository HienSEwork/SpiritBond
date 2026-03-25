using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] Text dialogText;

    public void SetDialog(string dialog)
    {
        if (dialogText != null)
        {
            dialogText.text = dialog;
        }
    }
}
