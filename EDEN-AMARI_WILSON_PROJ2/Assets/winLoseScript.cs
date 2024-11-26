using UnityEngine;
using UnityEngine.SceneManagement; // For scene management (if needed)
using UnityEngine.UI; // For accessing UI elements
using TMPro;

public class winLoseScript : MonoBehaviour
{
    public GameObject gameOverScreen;  // Reference to the Game Over screen Canvas
    public TextMeshProUGUI gameOverText;          // Optional: Game Over message Text

    public GameObject winScreen;  // Reference to the Game Over screen Canvas
    public TextMeshProUGUI winText;          // Optional: Game Over message Text

    public Button restartButton;       // Button for restarting the game
    
    public bool isGameOver = false;
    public bool isWon = false;

    void Start()
    {
        // Initially, hide the Game Over screen
        gameOverScreen.SetActive(false);

        winScreen .SetActive(false);

        // Assign Button listeners for restart and quit
        restartButton.onClick.AddListener(RestartGame);
        
    }

    public void Update()
    {
        
        // Example of checking a game-over condition (e.g., player health)
        if (isGameOver)
        {
            ShowGameOverScreen();
        }

        if (isWon)
        {
            ShowWinScreen();
        }
    }

    // Call this method when game over condition is met (e.g., player death)


    // Method to display the Game Over screen
    public void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);  
        Time.timeScale = 0f; 
        gameOverText.text = "GAME OVER!";

    }

    public void ShowWinScreen()
    {
        winScreen.SetActive(true);
        Time.timeScale = 0f;
        winText.text = "YOU WIN!";
    }

    private void RestartGame()
    {
        Time.timeScale = 1f;  // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reload the current scene
    }

}
