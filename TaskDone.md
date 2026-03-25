# TASK DONE

## Player Movement
- Mô tả:
  - Di chuyển 4 hướng (up, down, left, right)
  - Không đi chéo (ưu tiên trục X)
  - Có lastMove để giữ hướng idle

- Kiến trúc:
  - Dùng PlayerController (shared)
  - Input tách riêng qua PlayerInput (IMovementInput)

- Animator:
  - Sử dụng param: moveX, moveY, lastX, lastY
  - Thiết kế phù hợp AnyState (walk + idle 4 hướng)

- File liên quan:
  - PlayerController.cs
  - PlayerInput.cs

- Trạng thái: DONE

## Grass Encounter And UI Panel
- Mô tả:
  - Grass encounter trigger vào BattleScene với random PetData
  - Pet, Skill, Battle runtime classes và damage formula
  - Btn_Pet toggle Panel_Pet và Btn_Close hide panel

- File liên quan:
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

- Trạng thái: DONE

UPDATE TASK: Grass Encounter And UI Panel
- Lý do:
  - Bổ sung Debug.Log để test flow tương tác, button click, encounter và battle

- Phạm vi ảnh hưởng:
  - Assets/Scripts/World/Grass/GrassTrigger.cs
  - Assets/Scripts/World/Grass/GrassSpawner.cs
  - Assets/Scripts/World/Encounter/EncounterManager.cs
  - Assets/Scripts/Battle/BattleUnit.cs
  - Assets/Scripts/Battle/BattleManager.cs
  - Assets/Scripts/UI/Core/UIPanel.cs
  - Assets/Scripts/UI/Core/UIManager.cs
  - Assets/Scripts/UI/Buttons/BtnPetUI.cs
  - Assets/Scripts/UI/Buttons/CloseButtonUI.cs
