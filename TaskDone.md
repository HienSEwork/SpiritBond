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