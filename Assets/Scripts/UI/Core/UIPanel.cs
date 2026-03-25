using UnityEngine;

namespace SpiritBond.UI.Core
{
    public class UIPanel : MonoBehaviour
    {
        public virtual void Show()
        {
            gameObject.SetActive(true);
            Debug.Log($"[UIPanel] Show: {gameObject.name}");
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            Debug.Log($"[UIPanel] Hide: {gameObject.name}");
        }

        public virtual void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);
            Debug.Log($"[UIPanel] Toggle: {gameObject.name} => {gameObject.activeSelf}");
        }
    }
}
