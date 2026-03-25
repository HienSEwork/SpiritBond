using UnityEngine;

namespace SpiritBond.Core
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        protected virtual bool DontDestroyOnLoadEnabled => false;

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this as T;

            if (DontDestroyOnLoadEnabled)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}
