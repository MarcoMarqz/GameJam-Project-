using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    // You Only Get One theme implementation
    public int remainingUses = 1;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void UseOnce()
    {
        remainingUses--;
        if (remainingUses <= 0)
        {
            // When the player uses their "one thing"
            Debug.Log("Used your only chance!");
            // Trigger appropriate game logic
        }
    }
    
    public void EndGame(bool isVictory)
    {
        // Load end game scene
        SceneManager.LoadScene("EndGame");
        
        // Find EndGameManager in new scene
        StartCoroutine(FindEndGameManager(isVictory));
    }
    
    private System.Collections.IEnumerator FindEndGameManager(bool isVictory)
    {
        // Wait one frame for scene to load
        yield return null;
        
        // Using the newer recommended method
        EndGameManager endGameManager = FindAnyObjectByType<EndGameManager>();
        if (endGameManager != null)
        {
            endGameManager.ShowEndGame(isVictory);
        }
    }
}