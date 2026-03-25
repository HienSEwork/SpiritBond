using SpiritBond.Core;
using UnityEngine;

namespace SpiritBond.UI.Core
{
    public class UIManager : SingletonBehaviour<UIManager>
    {
        public void Toggle(UIPanel panel)
        {
            if (panel == null)
            {
                Debug.LogWarning("[UIManager] Toggle failed because panel is null.");
                return;
            }

            Debug.Log($"[UIManager] Toggle panel: {panel.gameObject.name}");
            panel.Toggle();
        }

        public void Show(UIPanel panel)
        {
            if (panel == null)
            {
                Debug.LogWarning("[UIManager] Show failed because panel is null.");
                return;
            }

            Debug.Log($"[UIManager] Show panel: {panel.gameObject.name}");
            panel.Show();
        }

        public void Hide(UIPanel panel)
        {
            if (panel == null)
            {
                Debug.LogWarning("[UIManager] Hide failed because panel is null.");
                return;
            }

            Debug.Log($"[UIManager] Hide panel: {panel.gameObject.name}");
            panel.Hide();
        }
    }
}
