# Hướng Dẫn Set Up Dự Án SpiritBondMain

## 1. Mục đích file này

File này dùng để handoff cho teammate khi clone dự án, biết cần cài gì, mở scene nào, hệ thống hiện tại đang ở trạng thái nào, và cần tiếp tục code từ đâu.

## 2. Yêu cầu môi trường

- OS: Windows là môi trường đang dùng hiện tại.
- Unity Editor: `2022.3.62f3`
- Khuyến nghị dùng:
  - Unity Hub
  - Visual Studio 2022 hoặc JetBrains Rider
  - Git

## 3. Clone dự án

```bash
git clone <repo-url>
cd SpiritBondMain
```

Lưu ý:

- Không commit các thư mục sinh ra bởi Unity như `Library/`, `Temp/`, `Logs/`, `Obj/`, `Build/`, `Builds/`, `UserSettings/`.
- `.gitignore` đã được tạo sẵn ở root project để giảm size repo.
- Hiện tại không có `.gitattributes`, và chưa thấy dấu hiệu đang dùng Git LFS.

## 4. Mở project lần đầu

1. Mở Unity Hub.
2. Chọn `Add project` và trỏ đến thư mục `SpiritBondMain`.
3. Mở bằng Unity version `2022.3.62f3`.
4. Chờ Unity import package lần đầu.
5. Nếu IDE chưa sync project:
   - Visual Studio: mở project solution Unity tạo ra.
   - Rider: mở thư mục project và chờ Rider index.

## 5. Packages hiện có

Project đang dùng các package chính sau trong `Packages/manifest.json`:

- `com.unity.feature.2d`
- `com.unity.textmeshpro`
- `com.unity.ugui`
- `com.unity.timeline`
- `com.unity.visualscripting`
- `com.unity.test-framework`
- `com.unity.ide.visualstudio`
- `com.unity.ide.rider`

Khi clone về, Unity sẽ tự restore package từ `Packages/manifest.json`.

## 6. Scene cần mở để tiếp tục code

Scene gameplay đang có asset:

- `Assets/Art/Map/Scenes/Map Forest.unity`
- `Assets/Art/Map/Scenes/Map Ice.unity`
- `Assets/Scenes/SampleScene.unity`

Lưu ý quan trọng:

- `Build Settings` hiện tại vẫn đang enable `Assets/Scenes/SampleScene.unity`.
- Scene đang dùng để test gameplay hiện tại là `Assets/Art/Map/Scenes/Map Forest.unity`.
- Khi teammate vào code tiếp, nên mở `Map Forest.unity` trước để kiểm tra Player và NPC.

## 7. Cấu trúc gameplay hiện tại

### 7.1 Player

Player đang dùng script riêng:

- [PlayerController.cs](C:/pocket/SpiritBondMain/Assets/Scripts/NPC/PlayerController.cs)

Tính chất hiện tại:

- Player đọc input trực tiếp bằng `Input.GetAxisRaw("Horizontal")` và `Input.GetAxisRaw("Vertical")`.
- Không cho đi chéo, ưu tiên trục X.
- Di chuyển bằng `Rigidbody2D.velocity`.
- Animator dùng 4 tham số:
  - `moveX`
  - `moveY`
  - `lastX`
  - `lastY`
- `lastMove` được lưu để idle đúng hướng cuối.

Lưu ý:

- `PlayerController` chỉ được gắn cho Player.
- Không gắn `PlayerController` cho NPC.

### 7.2 NPC

NPC đang dùng hệ thống riêng, tách khỏi Player:

- [CharacterController.cs](C:/pocket/SpiritBondMain/Assets/Scripts/NPC/CharacterController.cs)
- [IMovementInput.cs](C:/pocket/SpiritBondMain/Assets/Scripts/NPC/IMovementInput.cs)
- [NPCIdleInput.cs](C:/pocket/SpiritBondMain/Assets/Scripts/NPC/NPCIdleInput.cs)
- [NPCRandomInput.cs](C:/pocket/SpiritBondMain/Assets/Scripts/NPC/NPCRandomInput.cs)

Kiến trúc hiện tại:

- `CharacterController` là movement/animation controller chung cho NPC.
- Nguồn input của NPC đi qua `IMovementInput`.
- `NPCIdleInput` cho NPC đứng yên.
- `NPCRandomInput` cho NPC di chuyển ngẫu nhiên theo timer.

Lưu ý:

- Mỗi NPC chỉ nên có duy nhất 1 input component implement `IMovementInput`.
- `CharacterController` dùng để chạy NPC, không dùng thay `PlayerController` cho Player trong trạng thái hiện tại.

### 7.3 Camera

Camera đang follow target bằng:

- [CameraFollow.cs](C:/pocket/SpiritBondMain/Assets/Scripts/Camera/CameraFollow.cs)

Trạng thái:

- Camera follow mềm bằng `Vector3.Lerp`.
- Cần đảm bảo field `target` trỏ vào Player trong scene đang test.

## 8. Prefab quan trọng

Character prefabs hiện có:

- `Assets/Prefabs/Characters/Player.prefab`
- `Assets/Prefabs/Characters/NPC_Random.prefab`
- `Assets/Prefabs/Characters/NPC_Idle.prefab`
- `Assets/Prefabs/Characters/CharacterBase.prefab`

Quy ước hiện tại:

- `Player.prefab` dùng `PlayerController`.
- `CharacterBase.prefab` là base cho NPC, đang được gắn `CharacterController`.
- `NPC_Random.prefab` kế thừa từ `CharacterBase` và dùng `NPCRandomInput`.
- `NPC_Idle.prefab` kế thừa từ `CharacterBase` và override input để đứng yên.

Lưu ý rất quan trọng:

- Trước đây đã có lỗi NPC nhận cùng input với Player vì `PlayerController` bị gắn vào prefab NPC.
- Lỗi này đã được sửa bằng cách tách controller:
  - Player giữ `PlayerController`
  - NPC dùng `CharacterController`
- Nếu teammate tạo prefab mới, cần kiểm tra lại Inspector để tránh gắn nhầm script controller.

## 9. Tiến độ đã làm trong game

### Đã xong

- Đã có scene map để test gameplay:
  - `Map Forest`
  - `Map Ice`
- Đã có player prefab và npc prefabs có thể đặt trong map.
- Đã có player movement 4 hướng theo phong cách top-down.
- Đã có animation parameter cho player:
  - `moveX`
  - `moveY`
  - `lastX`
  - `lastY`
- Đã có camera follow player.
- Đã có npc movement random bằng timer.
- Đã có npc đứng yên bằng input idle.
- Đã tách logic Player và NPC để không nhận chung input.
- Đã tạo `.gitignore` để giảm file rác khi làm việc nhóm.

### Đang có / cần xác nhận trong Unity

- Cần test lại trong `Map Forest.unity` sau mỗi lần pull để chắc:
  - Player chỉ nhận input bàn phím.
  - NPC random tự đi, không nhận input player.
  - Animator chuyển đúng idle/walk theo 4 hướng.
- Build Settings chưa trỏ đúng scene gameplay chính.

### Chưa làm hoặc chưa hoàn thiện

- Chưa có hệ thống dialogue hoàn chỉnh.
- Chưa có hệ thống interact NPC hoàn chỉnh.
- Chưa có battle / encounter flow hoàn chỉnh.
- Chưa có save/load.
- Chưa có UI gameplay đầy đủ.
- Chưa có test automation.
- Chưa có quy trình spawn/NPC behavior nâng cao như patrol, chase, follow.

## 10. Cách teammate tiếp tục code từ điểm hiện tại

### Nếu code player

Làm việc tại:

- [PlayerController.cs](C:/pocket/SpiritBondMain/Assets/Scripts/NPC/PlayerController.cs)
- `Assets/Prefabs/Characters/Player.prefab`

Nguyên tắc:

- Giữ player input trực tiếp.
- Nếu sửa animator player, giữ bộ tham số:
  - `moveX`
  - `moveY`
  - `lastX`
  - `lastY`
- Không đưa `IMovementInput` vào player nếu chưa thống nhất lại kiến trúc.

### Nếu code NPC

Làm việc tại:

- [CharacterController.cs](C:/pocket/SpiritBondMain/Assets/Scripts/NPC/CharacterController.cs)
- [IMovementInput.cs](C:/pocket/SpiritBondMain/Assets/Scripts/NPC/IMovementInput.cs)
- [NPCIdleInput.cs](C:/pocket/SpiritBondMain/Assets/Scripts/NPC/NPCIdleInput.cs)
- [NPCRandomInput.cs](C:/pocket/SpiritBondMain/Assets/Scripts/NPC/NPCRandomInput.cs)

Nguyên tắc:

- Muốn thêm AI mới thì tạo thêm một script mới implement `IMovementInput`.
- Không sửa trực tiếp `CharacterController` nếu chỉ cần thêm kiểu di chuyển mới.
- Mỗi NPC object chỉ có 1 input script.

Ví dụ các behavior có thể thêm sau:

- `NPCPatrolInput`
- `NPCFollowTargetInput`
- `NPCChasePlayerInput`

## 11. Checklist sau khi clone cho teammate

1. Mở project bằng Unity `2022.3.62f3`.
2. Chờ package restore xong.
3. Mở `Assets/Art/Map/Scenes/Map Forest.unity`.
4. Chạy Play Mode và kiểm tra:
   - Player đi bằng WASD / mũi tên.
   - Player idle đúng hướng cuối.
   - NPC_Random đi ngẫu nhiên.
   - Camera follow player.
5. Nếu thêm scene gameplay mới, cập nhật `Build Settings`.
6. Khi sửa prefab, kiểm tra đúng script controller:
   - Player -> `PlayerController`
   - NPC -> `CharacterController` + 1 `IMovementInput`

## 12. Các file cần commit khi code

Cần commit:

- `Assets/`
- `Packages/`
- `ProjectSettings/`
- Tất cả file `.meta`
- Scene, prefab, animation, controller, script liên quan thay đổi

Không commit:

- `Library/`
- `Temp/`
- `Logs/`
- `Obj/`
- `Build/`
- `Builds/`
- `UserSettings/`
- cache editor / IDE files

## 13. Ghi chú quan trọng để tránh vỡ project

- Không attach `PlayerController` vào NPC.
- Không attach nhiều hơn 1 script input NPC vào cùng một object.
- Khi refactor prefab base, phải test lại cả `Player.prefab` và `NPC_Random.prefab`.
- Nếu thay đổi animator parameter names, phải sửa đồng bộ trong code và Animator Controller.
- Nếu thay đổi Unity version, thông báo cả team trước khi nâng cấp.

## 14. Điểm bắt đầu để code tiếp đề xuất

Thứ tự ưu tiên để code tiếp:

1. Chuẩn hóa scene gameplay chính và Build Settings.
2. Hoàn thiện interaction với NPC.
3. Bổ sung random encounter / battle trigger.
4. Tách rõ hơn gameplay systems ra khỏi movement.
5. Thêm AI input mới cho NPC.
