using UnityEngine;

public class CloudMove : MonoBehaviour
{
    public float speed = 0.5f; // Tốc độ bay
    public float leftBoundary = -15f; // Điểm bắt đầu (bên trái)
    public float rightBoundary = 15f; // Điểm kết thúc (bên phải)

    void Update()
    {
        // Di chuyển sang phải
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Nếu bay quá biên phải thì quay lại biên trái
        if (transform.position.x > rightBoundary)
        {
            transform.position = new Vector3(leftBoundary, transform.position.y, transform.position.z);
        }
    }
}