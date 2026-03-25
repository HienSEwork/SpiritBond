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
