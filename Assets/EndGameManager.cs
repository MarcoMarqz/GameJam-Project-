using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    public TextMeshProUGUI endGameMessageText;
    public string victoryMessage = "Victory!";
    public string defeatMessage = "Game Over";
    public string mainMenuSceneName = "MainMenu";
    public string gameSceneName = "GameScene";
    
    // Add direct references to buttons
    public Button mainMenuButton;
    public Button restartButton;
    
    void Start()
    {
        // Set up button listeners directly
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);
            
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);
    }
    
    public void ShowEndGame(bool isVictory)
    {
        gameObject.SetActive(true);
        if (endGameMessageText != null)
            endGameMessageText.text = isVictory ? victoryMessage : defeatMessage;
    }
    
    public void ReturnToMainMenu()
    {
        Debug.Log("Returning to main menu");
        SceneManager.LoadScene(mainMenuSceneName);
    }
    
    public void RestartGame()
    {
        Debug.Log("Restarting game");
        SceneManager.LoadScene(gameSceneName);
    }
}
