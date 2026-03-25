# TASK DONE

> Chuẩn hóa file lúc 13:56 25/03/2026 do `TaskDone.md` cũ bị lỗi ký tự.
> Các task cũ không có timestamp gốc cho từng entry, nên tạm dùng cùng mốc chuẩn hóa này để giữ lịch sử.

## Entry 01
- Done: 13:56 25/03/2026
- Task: Player Movement
- Files:
  - Assets/Scripts/NPC/PlayerController.cs
  - Assets/Scripts/NPC/PlayerInput.cs

## Entry 02
- Done: 13:56 25/03/2026
- Task: Grass Encounter And UI Panel
- Files:
  - Assets/Scripts/Core/SingletonBehaviour.cs
  - Assets/Scripts/World/Encounter/EncounterConfig.cs
  - Assets/Scripts/World/Encounter/EncounterManager.cs
  - Assets/Scripts/World/Grass/GrassSpawner.cs
  - Assets/Scripts/World/Grass/GrassTrigger.cs
  - Assets/Scripts/Pet/PetData.cs
  - Assets/Scripts/Pet/PetInstance.cs
  - Assets/Scripts/Skill/SkillData.cs
  - Assets/Scripts/Skill/SkillInstance.cs
  - Assets/Scripts/Battle/BattleCalculator.cs
  - Assets/Scripts/Battle/BattleUnit.cs
  - Assets/Scripts/Battle/BattleManager.cs
  - Assets/Scripts/UI/Core/UIPanel.cs
  - Assets/Scripts/UI/Core/UIManager.cs
  - Assets/Scripts/UI/Panels/PanelPetUI.cs
  - Assets/Scripts/UI/Buttons/BtnPetUI.cs
  - Assets/Scripts/UI/Buttons/CloseButtonUI.cs
  - Assets/Scripts/UI/Buttons/BtnShopUI.cs
  - Assets/Scripts/UI/Buttons/BtnBagUI.cs
  - Assets/Scripts/UI/Buttons/BtnMapUI.cs
  - Assets/Scripts/UI/Buttons/BtnSettingUI.cs

## Entry 03
- Done: 13:56 25/03/2026
- Task: UPDATE Grass Encounter And UI Panel - bổ sung Debug.Log để test flow tương tác, encounter và battle
- Files:
  - Assets/Scripts/World/Grass/GrassTrigger.cs
  - Assets/Scripts/World/Grass/GrassSpawner.cs
  - Assets/Scripts/World/Encounter/EncounterManager.cs
  - Assets/Scripts/Battle/BattleUnit.cs
  - Assets/Scripts/Battle/BattleManager.cs
  - Assets/Scripts/UI/Core/UIPanel.cs
  - Assets/Scripts/UI/Core/UIManager.cs
  - Assets/Scripts/UI/Buttons/BtnPetUI.cs
  - Assets/Scripts/UI/Buttons/CloseButtonUI.cs

## Entry 04
- Done: 13:56 25/03/2026
- Task: Y-Axis Sprite Sorting
- Files:
  - Assets/Scripts/World/YSortRenderer.cs

## Entry 05
- Done: 13:56 25/03/2026
- Task: UPDATE Y-Axis Sprite Sorting - tối giản `YSortRenderer`, ưu tiên `SortingGroup` nếu có
- Files:
  - Assets/Scripts/World/YSortRenderer.cs

## Entry 06
- Done: 13:56 25/03/2026
- Task: UPDATE Y-Axis Sprite Sorting - bổ sung `orderOffset` để tách lớp render chân, cỏ, thân trên
- Files:
  - Assets/Scripts/World/YSortRenderer.cs

## Entry 07
- Done: 13:56 25/03/2026
- Task: UPDATE Y-Axis Sprite Sorting - gỡ bỏ toàn bộ Y sort vì không hiệu quả
- Files:
  - Assets/Scripts/World/YSortRenderer.cs
  - Assets/Scripts/World/YSortRenderer.cs.meta

## Entry 08
- Done: 13:56 25/03/2026
- Task: UPDATE Player Movement - test chỉnh collider player trong Map Forest về feet collider
- Files:
  - Assets/Scenes/Map Forest.unity

## Entry 09
- Done: 13:56 25/03/2026
- Task: UPDATE Player Movement - rollback phần chỉnh collider player trong Map Forest
- Files:
  - Assets/Scenes/Map Forest.unity

## Entry 10
- Done: 13:56 25/03/2026
- Task: UPDATE Y-Axis Sprite Sorting - rollback phần ảnh hưởng còn sót trong settings và tilemap renderer
- Files:
  - ProjectSettings/GraphicsSettings.asset
  - ProjectSettings/TagManager.asset
  - Assets/Scenes/Map Forest.unity

## Entry 11
- Done: 14:16 25/03/2026
- Task: Base Pet Type System
- Files:
  - Assets/Scripts/Pet/PetType.cs
  - Assets/Scripts/Pet/PetData.cs
  - Assets/Scripts/Skill/SkillData.cs

## Entry 12
- Done: 14:22 25/03/2026
- Task: Base Pet Level And Exp Progression
- Files:
  - Assets/Scripts/Pet/PetProgression.cs
  - Assets/Scripts/Pet/PetData.cs
  - Assets/Scripts/Pet/PetInstance.cs


## Entry 13
- Done: 14:27 25/03/2026
- Task: Fix IDE Compile Include For Pet Base Types
- Files:
  - Assembly-CSharp.csproj


## Entry 14
- Done: 15:02 25/03/2026
- Task: Runtime Pet Storage, Autosave, Battle Checkpoint And Firefox Sample Logic
- Files:
  - Assets/Scripts/Core/SaveGameService.cs
  - Assets/Scripts/Core/SaveGameService.cs.meta
  - Assets/Scripts/Pet/PlayerProgressState.cs
  - Assets/Scripts/Pet/PlayerProgressState.cs.meta
  - Assets/Scripts/Pet/SamplePetContent.cs
  - Assets/Scripts/Pet/SamplePetContent.cs.meta
  - Assets/Scripts/Pet/PetInstance.cs
  - Assets/Scripts/Battle/BattleCalculator.cs
  - Assets/Scripts/Battle/BattleManager.cs
  - Assets/Scripts/World/Encounter/EncounterManager.cs

## Entry 15
- Done: 20:59 25/03/2026
- Task: Fix Legacy Spirit Battle Compile Errors And Add Missing MoveBase
- Files:
  - Assets/Scripts/Battle/Move.cs
  - Assets/Scripts/Battle/MoveBase.cs
  - Assets/Scripts/Battle/MoveBase.cs.meta
  - Assets/Scripts/Spirit/SpiritBase.cs
  - Assets/Scripts/Spirit/Spirit.cs
  - Assembly-CSharp.csproj

## Entry 16
- Done: 21:06 25/03/2026
- Task: UPDATE TASK: Runtime Pet Storage, Autosave, Battle Checkpoint And Firefox Sample Logic - gộp logic battle mới vào bộ file UI battle cũ và bỏ file battle trùng
- Files:
  - Assets/Scripts/Battle/BattleSystem.cs
  - Assets/Scripts/Battle/BattleHud.cs
  - Assets/Scripts/Battle/BattleDialogBox.cs
  - Assets/Scripts/Battle/BattleUnit1.cs
  - Assets/Scripts/Battle/BattleManager.cs
  - Assets/Scripts/Battle/BattleManager.cs.meta
  - Assets/Scripts/Battle/BattleUnit.cs
  - Assets/Scripts/Battle/BattleUnit.cs.meta

## Entry 17
- Done: 21:10 25/03/2026
- Task: UPDATE TASK: Runtime Pet Storage, Autosave, Battle Checkpoint And Firefox Sample Logic - gom logic Spirit/Move vào Pet/Skill và xóa file code legacy thừa
- Files:
  - Assets/Scripts/Pet/PetData.cs
  - Assets/Scripts/Pet/PetInstance.cs
  - Assets/Scripts/Skill/SkillData.cs
  - Assets/Scripts/Skill/SkillInstance.cs
  - Assets/Scripts/Battle/Move.cs
  - Assets/Scripts/Battle/Move.cs.meta
  - Assets/Scripts/Battle/MoveBase.cs
  - Assets/Scripts/Battle/MoveBase.cs.meta
  - Assets/Scripts/Spirit/Spirit.cs
  - Assets/Scripts/Spirit/Spirit.cs.meta
  - Assets/Scripts/Spirit/SpiritBase.cs
  - Assets/Scripts/Spirit/SpiritBase.cs.meta
  - Assets/Scripts/Spirit.meta
  - Assembly-CSharp.csproj

## Entry 18
- Done: 21:18 25/03/2026
- Task: Grass Roaming Pet Encounter Spawn And Touch Trigger Battle
- Files:
  - Assets/Scripts/World/Grass/GrassSpawner.cs
  - Assets/Scripts/World/Grass/GrassTrigger.cs
  - Assets/Scripts/World/Grass/GrassEncounterPet.cs
  - Assets/Scripts/World/Grass/GrassEncounterPet.cs.meta
  - Assembly-CSharp.csproj

## Entry 19
- Done: 22:05 25/03/2026
- Task: NPC Quest Battle Trigger With PetData And Enemy Level Scaling
- Files:
  - Assets/Scripts/NPC/NPCBattleTrigger.cs
  - Assets/Scripts/NPC/NPCBattleTrigger.cs.meta
  - Assets/Scripts/NPC/NPCQuestBattleData.cs
  - Assets/Scripts/NPC/NPCQuestBattleData.cs.meta
  - Assets/Scripts/World/Encounter/EncounterManager.cs
  - Assets/Scripts/Pet/PetInstance.cs
  - Assets/Scripts/Battle/BattleSystem.cs
  - Assembly-CSharp.csproj

## Entry 20
- Done: 22:08 25/03/2026
- Task: Separate Player Scripts Into Dedicated Folder
- Files:
  - Assets/Scripts/Player/PlayerController.cs
  - Assets/Scripts/Player/PlayerController.cs.meta
  - Assets/Scripts/Player/PlayerInput.cs
  - Assets/Scripts/Player/PlayerInput.cs.meta
  - Assets/Scripts/NPC/PlayerController.cs
  - Assets/Scripts/NPC/PlayerController.cs.meta
  - Assets/Scripts/NPC/PlayerInput.cs
  - Assets/Scripts/NPC/PlayerInput.cs.meta
  - Assembly-CSharp.csproj

## Entry 21
- Done: 22:15 25/03/2026
- Task: UPDATE TASK: NPC Quest Battle Trigger With PetData And Enemy Level Scaling - add debug logs for trigger detection and battle flow validation
- Files:
  - Assets/Scripts/NPC/NPCBattleTrigger.cs

## Entry 22
- Done: 22:16 25/03/2026
- Task: Add Debug Logs For Main Buttons, Settings And Grass Encounter Touch Points
- Files:
  - Assets/Scripts/UI/Buttons/BtnBagUI.cs
  - Assets/Scripts/UI/Buttons/BtnMapUI.cs
  - Assets/Scripts/UI/Buttons/BtnSettingUI.cs
  - Assets/Scripts/UI/Buttons/BtnShopUI.cs
  - Assets/Scripts/MainMenu/AudioSetting.cs
  - Assets/Scripts/MainMenu/BackButtonUI.cs
  - Assets/Scripts/MainMenu/ExitButtonUI.cs
  - Assets/Scripts/MainMenu/PlayButtonUI.cs
  - Assets/Scripts/MainMenu/ResolutionSetting.cs
  - Assets/Scripts/MainMenu/SettingsButtonUI.cs
  - Assets/Scripts/World/Grass/GrassEncounterPet.cs

## Entry 23
- Done: 22:19 25/03/2026
- Task: UPDATE TASK: Add Debug Logs For Main Buttons, Settings And Grass Encounter Touch Points - reduce repeated logs and stop Animator warning spam
- Files:
  - Assets/Scripts/NPC/NPCBattleTrigger.cs
  - Assets/Scripts/World/Grass/GrassTrigger.cs
  - Assets/Scripts/Player/PlayerController.cs
  - Assets/Scripts/NPC/CharacterController.cs

## Entry 24
- Done: 22:21 25/03/2026
- Task: UPDATE TASK: Add Debug Logs For Main Buttons, Settings And Grass Encounter Touch Points - fix duplicate AudioListener spam in Map Forest
- Files:
  - Assets/Scenes/Map Forest.unity

## Entry 25
- Done: 22:36 25/03/2026
- Task: Fix AudioListener Spam Across Scene Loads And Auto Bootstrap Empty Battle Scene
- Files:
  - Assets/Scripts/Core/AudioListenerGuard.cs
  - Assets/Scripts/Core/AudioListenerGuard.cs.meta
  - Assets/Scripts/Battle/BattleSceneAutoSetup.cs
  - Assets/Scripts/Battle/BattleSceneAutoSetup.cs.meta
  - Assembly-CSharp.csproj

## Entry 26
- Done: 22:40 25/03/2026
- Task: Prevent Auto Trigger On Load By Adding 5 Second Gameplay Trigger Lock
- Files:
  - Assets/Scripts/Core/GameplayTriggerGuard.cs
  - Assets/Scripts/Core/GameplayTriggerGuard.cs.meta
  - Assets/Scripts/NPC/NPCBattleTrigger.cs
  - Assets/Scripts/World/Grass/GrassTrigger.cs
  - Assets/Scripts/World/Grass/GrassEncounterPet.cs
  - Assembly-CSharp.csproj

## Entry 27
- Done: 22:42 25/03/2026
- Task: UPDATE TASK: Fix AudioListener Spam Across Scene Loads And Auto Bootstrap Empty Battle Scene - fix runtime font, reference binding and duplicate listener guard logs
- Files:
  - Assets/Scripts/Core/AudioListenerGuard.cs
  - Assets/Scripts/Battle/BattleSceneAutoSetup.cs

## Entry 28
- Done: 22:55 25/03/2026
- Task: Bag And Inventory MVP Integration From AN Source With Existing UI Structure
- Files:
  - Assets/Scripts/Inventory/ItemData.cs
  - Assets/Scripts/Inventory/InventoryItemStack.cs
  - Assets/Scripts/Inventory/InventoryManager.cs
  - Assets/Scripts/Inventory/BagSceneBinder.cs
  - Assets/Scripts/UI/Buttons/BtnBagUI.cs
  - Assets/Scripts/UI/Buttons/ItemSlotUI.cs
  - Assets/Scripts/UI/Panels/PanelBagUI.cs
  - Assembly-CSharp.csproj

## Entry 29
- Done: 22:58 25/03/2026
- Task: Remove Unused TextMesh Pro Documentation And Example Assets
- Files:
  - Assets/TextMesh Pro/Documentation
  - Assets/TextMesh Pro/Examples & Extras
  - Assembly-CSharp.csproj

## Entry 30
- Done: 23:04 25/03/2026
- Task: UPDATE TASK: Runtime Pet Storage, Autosave, Battle Checkpoint And Firefox Sample Logic - avoid editor direct play restoring player to autosave position
- Files:
  - Assets/Scripts/Core/SaveGameService.cs
  - Assets/Scripts/MainMenu/PlayButtonUI.cs

## Entry 31
- Done: 23:07 25/03/2026
- Task: UPDATE TASK: Runtime Pet Storage, Autosave, Battle Checkpoint And Firefox Sample Logic - rollback editor-only save position bypass to keep always-on restore flow
- Files:
  - Assets/Scripts/Core/SaveGameService.cs
  - Assets/Scripts/MainMenu/PlayButtonUI.cs

## Entry 32
- Done: 23:23 25/03/2026
- Task: Tilemap Runtime Refresh To Clear Stale Ghost Tiles On Scene Load
- Files:
  - Assets/Scripts/World/TilemapSceneRefresh.cs
  - Assembly-CSharp.csproj

## Entry 33
- Done: 23:46 25/03/2026
- Task: UPDATE TASK: Runtime Pet Storage, Autosave, Battle Checkpoint And Firefox Sample Logic - detach persistent managers from scene System root to keep only map camera in gameplay scene
- Files:
  - Assets/Scripts/Core/SingletonBehaviour.cs
  - Assets/Scripts/World/Encounter/EncounterManager.cs
  - Assets/Scripts/UI/Core/UIManager.cs
