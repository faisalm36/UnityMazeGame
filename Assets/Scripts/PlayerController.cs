using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Transform cam;               // assign Main Camera
    public float walkSpeed = 4f;
    public float runSpeed = 7f;
    public float gravity = -20f;
    public float jumpHeight = 1.5f;
    public float mouseSensitivity = 200f;

    CharacterController cc;
    Vector3 velocity;
    float xRot = 0f;
    Vector3 spawnPoint;
    float obstacleCooldown = 0.25f;
    float lastObstacleTime = -10f;

    void Start()
    {
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        spawnPoint = transform.position;
    }

    void Update()
    {
        // Mouse look (FPS): yaw on body, pitch on camera
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * mouseX);
        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -80f, 80f);
        cam.localRotation = Quaternion.Euler(xRot, 0f, 0f);

        // Movement
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 move = transform.right * h + transform.forward * v;
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        cc.Move(move.normalized * speed * Time.deltaTime);

        // Jump + gravity
        if (cc.isGrounded && velocity.y < 0) velocity.y = -2f;
        if (cc.isGrounded && Input.GetKeyDown(KeyCode.Space))
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
            Debug.Log("Space pressed");


        // Escape unlocks cursor (optional)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

    }

    public void Respawn()
    {
        // Simple respawn: place back at spawn point + zero vertical vel
        cc.enabled = false;
        transform.position = spawnPoint;
        velocity = Vector3.zero;
        cc.enabled = true;
    }

    // Detect physical contact with red obstacles (non-trigger colliders)
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (Time.time - lastObstacleTime < obstacleCooldown) return;
        if (hit.collider.CompareTag("Obstacle"))
        {
            lastObstacleTime = Time.time;
            GameManager.Instance.HitObstacle();
        }
    }
}
