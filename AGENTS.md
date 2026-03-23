## RULE FINAL CHO AGENT

### 1. Giao tiếp

* Đi thẳng vào vấn đề.
* Trả lời ngắn, đúng trọng tâm.
* Không giải thích dài dòng khi chưa được hỏi.
* Khi có nhiều phương án, nêu phương án khuyến nghị trước.
* Chỉ hỏi lại khi thiếu thông tin bắt buộc để làm tiếp.

### 2. Workflow bắt buộc

* Trước khi làm bất kỳ task nào, phải đọc:

  * `AGENT_RULES.md`
  * `TaskDone.md`
* Mọi task mới phải bám theo rule hiện tại của project.
* Không tự ý bỏ qua rule vì lý do “tối ưu hơn” hay “kiến trúc đẹp hơn”.

### 3. Phạm vi task

* Chỉ xử lý đúng phần được giao.
* Không tự sửa các module không liên quan.
* Không mở rộng yêu cầu ngoài task hiện tại.
* Không tự tiện tối ưu, beautify, đổi tên, reorganize nếu không được yêu cầu.

### 4. An toàn code

* Ưu tiên giữ nguyên code đang chạy được.
* Hạn chế xóa code cũ.
* Ưu tiên theo thứ tự:

  1. so sánh,
  2. bổ sung,
  3. chỉnh tối thiểu,
  4. refactor khi thật sự cần.
* Mỗi thay đổi phải có lý do rõ ràng.
* Không được phá flow cũ chỉ để áp dụng kiến trúc mới.

### 5. Cách sửa code

* Sửa tối thiểu để đạt mục tiêu.
* Không rewrite toàn file nếu chỉ cần sửa 1 đoạn.
* Không đổi API, tên biến, tên hàm, prefab, animator param, cấu trúc folder nếu không bắt buộc.
* Nếu cần refactor, phải nêu rõ:

  * vì sao cần,
  * ảnh hưởng phần nào,
  * lợi ích cụ thể.

### 6. Khi phát hiện vấn đề

* Chỉ ra đúng lỗi chính trước.
* Không lan man sang lỗi phụ nếu chưa cần.
* Ưu tiên fix lỗi đang chặn tiến độ.
* Nếu có nhiều lỗi, sắp theo thứ tự:

  1. blocker,
  2. lỗi logic,
  3. lỗi cấu trúc,
  4. tối ưu sau.

### 7. Với project đang có sẵn

* Tôn trọng cấu trúc hiện tại của project.
* Không áp kiến trúc mới nếu project chưa cần.
* Nếu kiến trúc cũ vẫn dùng được, ưu tiên bám theo nó.
* Chỉ đề xuất thay đổi lớn khi cách cũ gây lỗi rõ ràng hoặc thật sự khó mở rộng.

### 8. Khi trả code

* Trả code dùng được ngay.
* Không trả pseudo-code nếu user đang cần code thật.
* Chỉ đưa phần cần sửa hoặc phần hoàn chỉnh theo đúng phạm vi user hỏi.
* Nếu thay đổi file hiện có, phải nói rõ:

  * thêm gì,
  * sửa gì,
  * không đụng gì.

### 9. Với refactor

* Refactor là lựa chọn cuối, không phải mặc định.
* Chỉ refactor khi:

  * code đang lỗi lặp lại,
  * conflict kiến trúc,
  * khó mở rộng trực tiếp,
  * sửa nhỏ không đủ giải quyết.
* Refactor phải giữ nguyên behavior cũ trừ khi có yêu cầu đổi.

### 10. Với delete / replace

* Không xóa trước rồi viết lại trừ khi bắt buộc.
* Ưu tiên giữ code cũ để đối chiếu.
* Nếu cần thay thế, phải mô tả rõ phần bị thay và lý do.
* Không xóa các phần có thể còn dùng khi chưa xác nhận.

### 11. Done công việc

* Task đã ghi trong `TaskDone.md` = khóa logic.
* Không được sửa code liên quan tới task DONE nếu không có yêu cầu rõ ràng.
* Không refactor, không đổi tên, không tối ưu lại task DONE.
* Nếu cần thay đổi task DONE, phải ghi:

  * `UPDATE TASK: <tên task>`
  * lý do
  * phạm vi ảnh hưởng

### 12. Quản lý task

* Mỗi task hoàn thành phải ghi vào `TaskDone.md`.
* Mỗi task trong `TaskDone.md` cần có:

  * tên task,
  * mô tả ngắn,
  * file/module liên quan,
  * trạng thái: DONE.
* Không được chỉnh nội dung task DONE trừ khi có yêu cầu rõ ràng.

### 13. Nguyên tắc làm việc tối ưu

* Hiểu đúng yêu cầu trước khi sửa.
* Chốt hướng ngắn gọn rồi mới đi vào code.
* Làm từng bước nhỏ, kiểm soát được.
* Sau mỗi bước, ưu tiên trạng thái chạy ổn định.
* Không tạo thêm rủi ro cho các phần đang hoạt động tốt.

---

## Gợi ý file nên dùng trong project

* `AGENT_RULES.md` → chứa bộ rule final này
* `TaskDone.md` → ghi các task đã xong, khóa logic tương ứng

Tôi có thể viết tiếp ngay cho bạn bản **AGENT_RULES.md hoàn chỉnh** và **TaskDone.md khởi tạo với task Player Movement đã done**.
