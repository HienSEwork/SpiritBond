using System.Collections;
using System.Collections.Generic;
using SpiritBond.Core;
using SpiritBond.World.Encounter;
using UnityEngine;

namespace SpiritBond.NPC
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    public sealed class NPCBattleTrigger : MonoBehaviour
    {
        private enum TriggerMode
        {
            TriggerBattleImmediately = 0,
            TriggerLogThenBattle = 1
        }

        private static readonly HashSet<string> CompletedQuestIds = new HashSet<string>();

        [Header("References")]
        [SerializeField] private NPCQuestBattleData questBattleData;

        [Header("Trigger")]
        [SerializeField] private TriggerMode triggerMode = TriggerMode.TriggerLogThenBattle;
        [SerializeField] private bool requireInputKey;
        [SerializeField] private KeyCode inputKey = KeyCode.E;
        [SerializeField, Min(0f)] private float battleDelayAfterLog = 0.25f;
        [SerializeField] private bool enableDebugLogs = true;

        private Collider2D triggerCollider;
        private bool playerInside;
        private bool triggerLocked;
        private bool triggeredOnceThisScene;
        private bool inputHintShown;
        private bool startupBlockLogged;

        private void Awake()
        {
            triggerCollider = GetComponent<Collider2D>();
            if (questBattleData == null)
            {
                questBattleData = GetComponentInParent<NPCQuestBattleData>();
            }

            if (enableDebugLogs)
            {
                string npcName = GetNpcName();
                Debug.Log($"[NPCBattleTrigger] Awake on {gameObject.name} | NPC={npcName} | HasQuestData={questBattleData != null} | Collider={triggerCollider != null} | IsTrigger={triggerCollider != null && triggerCollider.isTrigger} | RequireInput={requireInputKey} | Mode={triggerMode}");
            }

            if (triggerCollider != null && !triggerCollider.isTrigger)
            {
                Debug.LogWarning($"[NPCBattleTrigger] Collider on {gameObject.name} is not set as Trigger. OnTriggerEnter2D will not fire.");
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (enableDebugLogs)
            {
                Debug.Log($"[NPCBattleTrigger] OnTriggerEnter2D on {gameObject.name} by {other.name} | Tag={other.tag}");
            }

            if (!other.CompareTag("Player"))
            {
                if (enableDebugLogs)
                {
                    Debug.Log($"[NPCBattleTrigger] Ignored trigger enter because collider tag is not Player.");
                }

                return;
            }

            playerInside = true;
            inputHintShown = false;
            startupBlockLogged = false;
            Debug.Log($"[NPCBattleTrigger] Player entered trigger for {GetNpcName()}.");

            if (GameplayTriggerGuard.IsBlocked)
            {
                if (enableDebugLogs && !startupBlockLogged)
                {
                    Debug.Log($"[NPCBattleTrigger] Trigger blocked for {GetNpcName()} during startup lock. Remaining={GameplayTriggerGuard.RemainingBlockTime:F1}s");
                    startupBlockLogged = true;
                }

                return;
            }

            if (!requireInputKey)
            {
                TryStartBattleFlow();
                return;
            }

            if (!inputHintShown)
            {
                Debug.Log($"[NPCBattleTrigger] Player entered NPC trigger for {GetNpcName()}. Press {inputKey} to interact. UI not use yet.");
                inputHintShown = true;
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!playerInside || !other.CompareTag("Player"))
            {
                return;
            }

            if (GameplayTriggerGuard.IsBlocked)
            {
                return;
            }

            if (!requireInputKey)
            {
                TryStartBattleFlow();
                return;
            }

            if (Input.GetKeyDown(inputKey))
            {
                Debug.Log($"[NPCBattleTrigger] Input key {inputKey} pressed for {GetNpcName()}.");
                TryStartBattleFlow();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            playerInside = false;
            triggerLocked = false;
            inputHintShown = false;
            startupBlockLogged = false;
            Debug.Log($"[NPCBattleTrigger] Player exited trigger for {GetNpcName()}.");
        }

        private void TryStartBattleFlow()
        {
            if (triggerLocked || !playerInside)
            {
                return;
            }

            if (questBattleData == null)
            {
                Debug.LogWarning($"[NPCBattleTrigger] Missing NPCQuestBattleData on {gameObject.name}");
                return;
            }

            if (questBattleData.EnemyPetData == null)
            {
                Debug.LogWarning($"[NPCBattleTrigger] Missing enemyPetData on {GetNpcName()}");
                return;
            }

            if (EncounterManager.Instance == null)
            {
                Debug.LogWarning("[NPCBattleTrigger] EncounterManager instance not found.");
                return;
            }

            if (IsOneTimeCompleted())
            {
                Debug.Log($"[NPCBattleTrigger] One-time battle already completed for {GetNpcName()}.");
                return;
            }

            Debug.Log($"[NPCBattleTrigger] Trigger validated for {GetNpcName()}. Starting battle flow...");
            triggerLocked = true;
            StartCoroutine(BeginBattleRoutine());
        }

        private IEnumerator BeginBattleRoutine()
        {
            if (triggerMode == TriggerMode.TriggerLogThenBattle)
            {
                string logText = string.IsNullOrWhiteSpace(questBattleData.InteractionLogText)
                    ? $"{GetNpcName()} wants to battle."
                    : questBattleData.InteractionLogText;

                Debug.Log($"[NPCBattleTrigger] {logText} UI not use yet.");

                if (battleDelayAfterLog > 0f)
                {
                    yield return new WaitForSeconds(battleDelayAfterLog);
                }
            }

            int enemyLevel = questBattleData.GetEnemyLevel();
            Debug.Log($"[NPCBattleTrigger] Starting NPC battle: {GetNpcName()} | Pet={questBattleData.EnemyPetData.petName} | Level={enemyLevel}");

            MarkCompletedIfNeeded();
            EncounterManager.Instance.StartEncounter(questBattleData.EnemyPetData, enemyLevel);
        }

        private bool IsOneTimeCompleted()
        {
            if (questBattleData == null || !questBattleData.OneTimeOnly)
            {
                return false;
            }

            if (triggeredOnceThisScene)
            {
                return true;
            }

            string questId = questBattleData.QuestId;
            return !string.IsNullOrWhiteSpace(questId) && CompletedQuestIds.Contains(questId);
        }

        private void MarkCompletedIfNeeded()
        {
            if (questBattleData == null || !questBattleData.OneTimeOnly)
            {
                return;
            }

            triggeredOnceThisScene = true;

            string questId = questBattleData.QuestId;
            if (!string.IsNullOrWhiteSpace(questId))
            {
                CompletedQuestIds.Add(questId);
            }
        }

        private string GetNpcName()
        {
            return questBattleData != null ? questBattleData.NpcDisplayName : gameObject.name;
        }

        private void OnDrawGizmosSelected()
        {
            Collider2D drawCollider = triggerCollider != null ? triggerCollider : GetComponent<Collider2D>();
            if (drawCollider == null)
            {
                return;
            }

            Gizmos.color = new Color(1f, 0.55f, 0.1f, 0.35f);
            Bounds bounds = drawCollider.bounds;
            Gizmos.DrawCube(bounds.center, bounds.size);
            Gizmos.color = new Color(1f, 0.55f, 0.1f, 1f);
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}
