using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Gameplay")]
    public int lives = 3;
    public int totalCoins;
    private int collectedCoins = 0;

    [Header("Refs")]
    public PlayerController player;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Auto-count coins in scene (tagged "Coin")
        totalCoins = GameObject.FindGameObjectsWithTag("Coin").Length;
        Debug.Log($"Coins in level: {totalCoins}");
        Debug.Log($"Lives: {lives}");
    }

    public void CoinCollected()
    {
        collectedCoins++;
        Debug.Log($"Coins collected: {collectedCoins}/{totalCoins}");
        if (collectedCoins >= totalCoins)
        {
            Debug.Log("You Win!");
            QuitGame();
        }
    }

    private bool invulnerable = false;
    private float lastHitTime = -10f;
    public float hitCooldown = 1.0f;

    public void HitObstacle()
    {
        if (Time.time - lastHitTime < hitCooldown) return; // cooldown
        lastHitTime = Time.time;

        lives--;
        Debug.Log($"Ouch! Lives left: {lives}");
        if (lives <= 0)
        {
            Debug.Log("Game Over!");
            QuitGame();
        }
        else
        {
            // Respawn player
            if (player != null) player.Respawn();
        }
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        // stop play mode in Editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
