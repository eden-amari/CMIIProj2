using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TMPro.Examples;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float runSpeedMultiplier = 2f;

    public float mouseSensitivity = 2f;
    private Rigidbody rb;

    private float xRotation = 0f;
    private PlayerInventory inventory;
    private Weapon weapon; // Reference to the Weapon component
    //public winLoseScript winLose;

    public int treasureCount = 0;
    public int treasureLeft = 3;

    public int enemiesLeft = 4;
    public int enemiesDefeated = 0;

    public TextMeshProUGUI treasureCountText;
    public TextMeshProUGUI treasureLeftText;

    public TextMeshProUGUI enemiesDefeatedText;
    public TextMeshProUGUI enemiesLeftText;

    public TextMeshProUGUI livesText;
    public int lives = 3; // Player lives
    // Jumping variables
    public float jumpForce = 5f;
    private bool isGrounded;

    public GameObject gameOverScreen;  // Reference to the Game Over screen Canvas
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;          // Optional: Game Over message Text

    public GameObject winScreen;  // Reference to the Game Over screen Canvas
    public GameObject winPanel;
    public TextMeshProUGUI winText;          // Optional: Game Over message Text
    //--------------------------------------new variables for bc ammo broken
    public int bulletCount; 
    public int grenadeCount;

    public TextMeshProUGUI bulletCountText; // Reference to the TextMeshProUGUI component
    public TextMeshProUGUI grenadeCountText; // Reference to the grenade count text


    public bool isGameOver = false;
    public bool isWon = false;

    public AudioClip treasureChime;
    public AudioClip loseSound;
    public AudioClip winSound;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        inventory = GetComponent<PlayerInventory>();
        //winLose = GetComponent<winLoseScript>();
        gameOverScreen.SetActive(false);
        winScreen.SetActive(false);


        if (inventory == null)
        {
            Debug.LogError("PlayerInventory component is missing on the player!");
        }

        bulletCount = 6;
        grenadeCount = 2;

        //// Check if weapon is found
        //if (weapon == null)
        //{
        //    Debug.LogError("Weapon component could not be found in the scene!");
        //}

        Cursor.lockState = CursorLockMode.Locked;

        UpdateEnemyText();
        UpdateTreasureText();
        UpdateLifeText();

        // Make sure the Rigidbody is not kinematic and gravity is enabled
        if (rb != null)
        {
            rb.useGravity = true;  // Make sure gravity is applied
            rb.isKinematic = false; // Ensure it's not kinematic
        }

        //if (winLose == null)
        //{
        //    Debug.LogError("winLoseScript not found!");
        //}
    }

    private void Update()
    {
        UpdateWinLose();
        weapon = GetComponent<Weapon>();
       // winLose = FindObjectOfType<winLoseScript>();
        LookAround();

        // Jump input check
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Camera camera = Camera.main;
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 movement = (horizontalInput * right + verticalInput * forward).normalized;

        float speed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * runSpeedMultiplier : moveSpeed;

        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    private void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        Camera camera = GetComponentInChildren<Camera>();
        if (camera != null)
        {
            camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }

    // Function to make the player jump
    private void Jump()
    {
        if (rb != null)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);  // Apply upward force for the jump
            //isGrounded = false; // Set grounded to false until collision with the ground happens
        }
    }

    public void UpdateTreasureText()
    {
        if (treasureCountText != null)
        {
            treasureCountText.text = "Treasure Boxes Collected: " + treasureCount;
        }

        if (treasureLeftText != null)
        {
            treasureLeftText.text = "Treasure Boxes Remaining: " + treasureLeft;
        }
    }

    public void UpdateEnemyText()
    {
        if (enemiesDefeatedText != null)
        {
            enemiesDefeatedText.text = "Enemies Defeated: " + enemiesDefeated;
        }

        if (enemiesLeftText != null)
        {
            enemiesLeftText.text = "Enemies Remaining: " + enemiesLeft;
        }
    }

    public void UpdateLifeText()
    {
        if (livesText != null)
        {
            livesText.text = "Lives left: " + lives;
        }
    }
    private IEnumerator RespawnHealthPack(GameObject healthPack, float delay)
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(delay);

        // Reactivate the health pack after the delay
        healthPack.SetActive(true);
    }

    public IEnumerator RespawnBullets(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        bullet.SetActive(true);
    }

    public void UpdateBulletCountText()
    {
        if (bulletCountText != null)
        {
            bulletCountText.text = "Bullets: " + bulletCount; // Update the TextMeshPro text
        }
    }

    // Update the grenade count UI text
    public void UpdateGrenadeCountText()
    {
        if (grenadeCountText != null)
        {
            grenadeCountText.text = "Grenades in the launcher: " + grenadeCount;
        }
    }

    // Check for collision to determine if the player is grounded
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // The player is grounded when they hit the ground
        }

        if (collision.gameObject.CompareTag("Gun"))
        {
            if (inventory.currentWeapon != null)
            {
                inventory.currentWeapon.gameObject.SetActive(false);
            }
            collision.gameObject.transform.SetParent(transform);
            collision.gameObject.transform.localPosition = new Vector3(0.47f, -0.15f, 1.02f);
            collision.gameObject.transform.localRotation = Quaternion.Euler(270f, 180f, 0f);

            Weapon gun = collision.gameObject.GetComponent<Weapon>();
            if (gun != null && inventory != null)
            {
                inventory.AddWeapon(gun);
            }
        }
        else if (collision.gameObject.CompareTag("GrenadeLauncher"))
        {
            if (inventory.currentWeapon != null)
            {
                inventory.currentWeapon.gameObject.SetActive(false);
            }
            collision.gameObject.transform.SetParent(transform);
            collision.gameObject.transform.localPosition = new Vector3(0.47f, -0.15f, 1.02f);
            collision.gameObject.transform.localRotation = Quaternion.Euler(270f, 180f, 0f);

            Weapon grenadeLauncher = collision.gameObject.GetComponent<Weapon>();
            if (grenadeLauncher != null && inventory != null)
            {
                inventory.AddWeapon(grenadeLauncher);
            }
        }
        else if (collision.gameObject.CompareTag("GrenadePrefab"))
        {
           grenadeCount++; // Increment the grenade count
           grenadeCountText.text = "Grenades in the launcher: " + grenadeCount;
           Debug.Log("Grenade collected! Total grenades: " + grenadeCount);
           Destroy(collision.gameObject); // Destroy the grenade
           
        }

        else if (collision.gameObject.CompareTag("gunAmmo"))
        {
            if (bulletCount < 6)
            {
                collision.gameObject.SetActive(false); //working
                bulletCount++; //working
                bulletCountText.text = "Bullets: " + bulletCount;
                Debug.Log("Bullet collected! Total bullets: " + bulletCount);
                StartCoroutine(RespawnBullets(collision.gameObject, 3f));
            }

            else
            {
                Debug.Log("You can't hold more than 6 bullets!");
            }
        }



        else if (collision.gameObject.CompareTag("Treasure"))
        {
            AudioSource treasureChime = GetComponent<AudioSource>();
            if (treasureChime != null)
            {
                treasureChime.Play(); // Play the sound
            }
            treasureCount++;
            treasureLeft--;
            UpdateTreasureText();
            Destroy(collision.gameObject);
        }


        else if (collision.gameObject.CompareTag("enemyBullet"))
        {
            lives--;
            UpdateLifeText();

        }


        else if (collision.gameObject.CompareTag("healthPack"))
        {
            if ((lives >= 1) && (lives < 3))
            {
                // Deactivate the health pack
                collision.gameObject.SetActive(false);

                // Increment lives and update the life text
                lives++;
                UpdateLifeText();

                // Start the respawn coroutine to reactivate the health pack after 3 seconds
                StartCoroutine(RespawnHealthPack(collision.gameObject, 3f));
            }
            else if (lives == 3)
            {
                Debug.Log("You already have the max lives!");
            }
        }
    }

    //private void RestartGame()
    //{
    //    Time.timeScale = 1f;  // Resume the game
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reload the current scene
    //}

    public void UpdateWinLose()
    {
        // Trigger Game Over if lives are less than or equal to 0
        if (lives <= 0 && !isGameOver)
        {
            Debug.Log("Game Over!");
            isGameOver = true;  // Set Game Over state
            gameOverScreen.SetActive(true);  // Show Game Over screen
            gameOverPanel.SetActive(true);   // Show the Game Over panel
            gameOverText.text = "GAME OVER...";  // Set the text
            Time.timeScale = 0f;  // Pause the game (stop time flow)
            AudioSource loseSound = GetComponent<AudioSource>();
            if (loseSound != null)
            {
                loseSound.Play(); // Play the sound
            }
        }

        // Check if player has won (all treasures collected and enemies defeated)
        if (treasureCount == 3 && treasureLeft == 0 && enemiesLeft == 0 && !isWon)
        {
            Debug.Log("You win!");
            isWon = true;  // Set Win state
            winScreen.SetActive(true);  // Show Win screen
            winPanel.SetActive(true);   // Show the Win panel
          
            winText.text = "YOU WIN!";  // Set the text

            AudioSource winSound = GetComponent<AudioSource>();
            if (winSound != null)
            {
                winSound.Play(); // Play the sound
            }
        }
    }

}

