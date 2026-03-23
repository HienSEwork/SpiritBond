using UnityEngine;

namespace SpiritBond.NPC
{
    [DisallowMultipleComponent]
    public sealed class NPCIdleInput : MonoBehaviour, IMovementInput
    {
        public Vector2 GetMoveInput()
        {
            return Vector2.zero;
        }
    }
}
