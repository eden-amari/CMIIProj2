﻿using System.Collections;
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
    public int treasureCount = 0;
    public int treasureLeft = 3;

    public int enemiesLeft = 4;
    public int enemiesDefeated = 0;

    public TextMeshProUGUI treasureCountText;
    public TextMeshProUGUI treasureLeftText;
    //public AudioClip treasureChime;

    public TextMeshProUGUI enemiesDefeatedText;
    public TextMeshProUGUI enemiesLeftText;

    public TextMeshProUGUI livesText;
    public int lives = 3; 


    public float jumpForce = 5f;
    private bool isGrounded;

    public GameObject gameOverScreen;  // Reference to the Game Over screen Canvas
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;          // Optional: Game Over message Text
    public AudioClip loseSound;

    public GameObject winScreen;  // Reference to the Game Over screen Canvas
    public GameObject winPanel;
    public TextMeshProUGUI winText;          // Optional: Game Over message Text
    public AudioClip winSound;

    public GameObject hitScreen;
    public GameObject hitPanel;
    //public AudioClip hitSound;

    public GameObject bgMusic;

    //--------------------------------------new variables for bc ammo broken
    public int bulletCount;
    public TextMeshProUGUI bulletCountText; // Reference to the TextMeshProUGUI component

    public TextMeshProUGUI grenadeCountText; // Reference to the grenade count text
    public int grenadeCount;


    public bool isGameOver = false;
    public bool isWon = false;

    public AudioSource treasureChimeSource;
    public AudioSource hitSoundSource;




    //public void Start()
    //{
    //    rb = GetComponent<Rigidbody>();
    //    inventory = GetComponent<PlayerInventory>();
    //    gameOverScreen.SetActive(false);
    //    winScreen.SetActive(false);

    //    if (inventory == null)
    //    {
    //        Debug.LogError("PlayerInventory component is missing on the player!");
    //    }

    //    bulletCount = 6;
    //    grenadeCount = 2;

    //    Cursor.lockState = CursorLockMode.Locked;

    //    UpdateEnemyText();
    //    UpdateTreasureText();
    //    UpdateLifeText();

    //    // Make sure the Rigidbody is not kinematic and gravity is enabled
    //    if (rb != null)
    //    {
    //        rb.useGravity = true;
    //        rb.isKinematic = false;
    //    }
    //}

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inventory = GetComponent<PlayerInventory>();
        gameOverScreen.SetActive(false);
        winScreen.SetActive(false);

        // Initialize the audio sources explicitly in the Start method
        treasureChimeSource = GetComponents<AudioSource>()[0]; // Assuming it's the first AudioSource component attached
        hitSoundSource = GetComponents<AudioSource>()[1]; // Assuming it's the second AudioSource component attached

        if (inventory == null)
        {
            Debug.LogError("PlayerInventory component is missing on the player!");
        }

        bulletCount = 6;
        grenadeCount = 2;

        Cursor.lockState = CursorLockMode.Locked;

        UpdateEnemyText();
        UpdateTreasureText();
        UpdateLifeText();

        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
        }
    }

    private void Update()
    {
        UpdateWinLose();
        weapon = GetComponent<Weapon>();
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

    private void Jump()
    {
        if (rb != null)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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
        yield return new WaitForSeconds(delay);
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
            bulletCountText.text = "Bullets: " + bulletCount;
        }
    }

    public void UpdateGrenadeCountText()
    {
        if (grenadeCountText != null)
        {
            grenadeCountText.text = "Grenades in the launcher: " + grenadeCount;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
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
            collision.gameObject.transform.localPosition = new Vector3(0.47f, .3f, .8f);
            collision.gameObject.transform.localRotation = Quaternion.Euler(0, 180f, 0f);

            Weapon grenadeLauncher = collision.gameObject.GetComponent<Weapon>();
            if (grenadeLauncher != null && inventory != null)
            {
                inventory.AddWeapon(grenadeLauncher);
            }
        }
   
        else if (collision.gameObject.CompareTag("GrenadePrefab"))
        {
            grenadeCount++;
            grenadeCountText.text = "Grenades in the launcher: " + grenadeCount;
            Debug.Log("Grenade collected! Total grenades: " + grenadeCount);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("gunAmmo"))
        {
            if (bulletCount < 6)
            {
                collision.gameObject.SetActive(false);
                bulletCount++;
                bulletCountText.text = "Bullets: " + bulletCount;
                Debug.Log("Bullet collected! Total bullets: " + bulletCount);
                StartCoroutine(RespawnBullets(collision.gameObject, 3f));
            }
            else
            {
                Debug.Log("You can't hold more than 6 bullets!");
            }
        }

        if (collision.gameObject.CompareTag("Treasure"))
        {
            if (!isGameOver)
            {
                if (treasureChimeSource != null)
                {
                    treasureChimeSource.Play();  // Play the treasure collection sound
                }
                treasureCount++;
                treasureLeft--;
                UpdateTreasureText();
                Destroy(collision.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("enemyBullet"))
        {
            if (hitSoundSource != null)
            {
                hitSoundSource.Play(); // Play the hit sound
            }
            StartCoroutine(HandleEnemyHit());
        }

        else if (collision.gameObject.CompareTag("healthPack"))
        {
            if ((lives >= 1) && (lives < 3))
            {
                collision.gameObject.SetActive(false);
                lives++;
                UpdateLifeText();
                StartCoroutine(RespawnHealthPack(collision.gameObject, 3f));
            }
            else if (lives == 3)
            {
                Debug.Log("You already have the max lives!");
            }
        }
    }

    private IEnumerator HandleEnemyHit()
    {
        hitScreen.SetActive(true);
        hitPanel.SetActive(true);
        
        lives--;
        UpdateLifeText();

        yield return new WaitForSeconds(0.4f);

        hitScreen.SetActive(false);
        hitPanel.SetActive(false);
    }

    //public void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        isGrounded = true;
    //    }

    //    if (collision.gameObject.CompareTag("Gun"))
    //    {
    //        if (inventory.currentWeapon != null)
    //        {
    //            inventory.currentWeapon.gameObject.SetActive(false);
    //        }
    //        collision.gameObject.transform.SetParent(transform);
    //        collision.gameObject.transform.localPosition = new Vector3(0.47f, -0.15f, 1.02f);
    //        collision.gameObject.transform.localRotation = Quaternion.Euler(270f, 180f, 0f);

    //        Weapon gun = collision.gameObject.GetComponent<Weapon>();
    //        if (gun != null && inventory != null)
    //        {
    //            inventory.AddWeapon(gun);
    //        }
    //    }
    //    else if (collision.gameObject.CompareTag("GrenadePrefab"))
    //    {
    //        grenadeCount++;
    //        grenadeCountText.text = "Grenades in the launcher: " + grenadeCount;
    //        Debug.Log("Grenade collected! Total grenades: " + grenadeCount);
    //        Destroy(collision.gameObject);
    //    }

    //    else if (collision.gameObject.CompareTag("gunAmmo"))
    //    {
    //        if (bulletCount < 6)
    //        {
    //            collision.gameObject.SetActive(false);
    //            bulletCount++;
    //            bulletCountText.text = "Bullets: " + bulletCount;
    //            Debug.Log("Bullet collected! Total bullets: " + bulletCount);
    //            StartCoroutine(RespawnBullets(collision.gameObject, 3f));
    //        }
    //        else
    //        {
    //            Debug.Log("You can't hold more than 6 bullets!");
    //        }
    //    }

    //    else if (collision.gameObject.CompareTag("Treasure"))
    //    {
    //        if (!isGameOver)
    //        {
    //            AudioSource treasureChime = GetComponent<AudioSource>();
    //            if (treasureChime != null)
    //            {
    //                treasureChime.Play();
    //            }
    //            treasureCount++;
    //            treasureLeft--;
    //            UpdateTreasureText();
    //            Destroy(collision.gameObject);
    //        }
    //    }

    //    else if (collision.gameObject.CompareTag("enemyBullet"))
    //    {
    //        StartCoroutine(HandleEnemyHit());
    //    }

    //    else if (collision.gameObject.CompareTag("healthPack"))
    //    {
    //        if ((lives >= 1) && (lives < 3))
    //        {
    //            collision.gameObject.SetActive(false);
    //            lives++;
    //            UpdateLifeText();
    //            StartCoroutine(RespawnHealthPack(collision.gameObject, 3f));
    //        }
    //        else if (lives == 3)
    //        {
    //            Debug.Log("You already have the max lives!");
    //        }
    //    }
    //}

    //private IEnumerator HandleEnemyHit()
    //{
    //    hitScreen.SetActive(true);
    //    hitPanel.SetActive(true);

    //    AudioSource hitSound = GetComponent<AudioSource>();
    //    if (hitSound != null)
    //    {
    //        hitSound.Play(); // Play the sound
    //    }
    //    lives--;
    //    UpdateLifeText();
    //    yield return new WaitForSeconds(.4f);
    //    hitScreen.SetActive(false);
    //    hitPanel.SetActive(false);
    //}

    public void UpdateWinLose()
    {
        // Trigger Game Over if lives are less than or equal to 0
        if (lives <= 0 && !isGameOver)
        {
            GetComponent<Collider>().enabled = false;
            Debug.Log("Game Over!");
            isGameOver = true;  // Set Game Over state
            gameOverScreen.SetActive(true);  // Show Game Over screen
            gameOverPanel.SetActive(true);   // Show the Game Over panel
            gameOverText.text = "GAME OVER...";  // Set the text
            Time.timeScale = 0f;  // Pause the game (stop time flow)


            AudioSource[] audioSources = GetComponents<AudioSource>();
            foreach (AudioSource audio in audioSources)
            {
                audio.enabled = false;  // Disable audio source entirely
            }

            bgMusic.SetActive(false);
            // Play lose sound (if desired)
            AudioSource loseSound = GetComponent<AudioSource>();
            if (loseSound != null)
            {
                loseSound.Play();  // Play the lose sound
            }
        }

        // Check if player has won (all treasures collected and enemies defeated)
        if (treasureCount == 3 && treasureLeft == 0 && enemiesLeft == 0 && !isWon)
        {
            GetComponent<Collider>().enabled = false;
            Debug.Log("You win!");
            isWon = true;  // Set Win state
            winScreen.SetActive(true);  // Show Win screen
            winPanel.SetActive(true);   // Show the Win panel

            winText.text = "YOU WIN!";  // Set the text

            AudioSource[] audioSources = GetComponents<AudioSource>();
            foreach (AudioSource audio in audioSources)
            {
                audio.enabled = false;
            }
            bgMusic.SetActive(false);

            AudioSource winSound = GetComponent<AudioSource>();
            if (winSound != null)
            {
                winSound.Play();
            }
            else
            {
                Debug.Log("No win audio detected bro");
            }
        }
    }

}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;
//using TMPro.Examples;
//using UnityEngine.UI;
//using UnityEngine.SceneManagement;


//public class PlayerMovement : MonoBehaviour
//{
//    public float moveSpeed = 5f;
//    public float runSpeedMultiplier = 2f;

//    public float mouseSensitivity = 2f;
//    private Rigidbody rb;

//    private float xRotation = 0f;
//    private PlayerInventory inventory;
//    private Weapon weapon; // Reference to the Weapon component
//    //public winLoseScript winLose;

//    public int treasureCount = 0;
//    public int treasureLeft = 3;

//    public int enemiesLeft = 4;
//    public int enemiesDefeated = 0;

//    public TextMeshProUGUI treasureCountText;
//    public TextMeshProUGUI treasureLeftText;

//    public TextMeshProUGUI enemiesDefeatedText;
//    public TextMeshProUGUI enemiesLeftText;

//    public TextMeshProUGUI livesText;
//    public int lives = 3; // Player lives
//    // Jumping variables
//    public float jumpForce = 5f;
//    private bool isGrounded;

//    public GameObject gameOverScreen;  // Reference to the Game Over screen Canvas
//    public GameObject gameOverPanel;
//    public TextMeshProUGUI gameOverText;          // Optional: Game Over message Text

//    public GameObject winScreen;  // Reference to the Game Over screen Canvas
//    public GameObject winPanel;
//    public TextMeshProUGUI winText;          // Optional: Game Over message Text

//    public GameObject hitScreen;
//    public GameObject hitPanel;

//    //--------------------------------------new variables for bc ammo broken
//    public int bulletCount; 
//    public int grenadeCount;

//    public TextMeshProUGUI bulletCountText; // Reference to the TextMeshProUGUI component
//    public TextMeshProUGUI grenadeCountText; // Reference to the grenade count text


//    public bool isGameOver = false;
//    public bool isWon = false;

//    public AudioClip treasureChime;
//    public AudioClip loseSound;
//    public AudioClip winSound;
//    public GameObject bgMusic;

//    public void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        inventory = GetComponent<PlayerInventory>();
//        //winLose = GetComponent<winLoseScript>();
//        gameOverScreen.SetActive(false);
//        winScreen.SetActive(false);


//        if (inventory == null)
//        {
//            Debug.LogError("PlayerInventory component is missing on the player!");
//        }

//        bulletCount = 6;
//        grenadeCount = 2;

//        //// Check if weapon is found
//        //if (weapon == null)
//        //{
//        //    Debug.LogError("Weapon component could not be found in the scene!");
//        //}

//        Cursor.lockState = CursorLockMode.Locked;

//        UpdateEnemyText();
//        UpdateTreasureText();
//        UpdateLifeText();

//        // Make sure the Rigidbody is not kinematic and gravity is enabled
//        if (rb != null)
//        {
//            rb.useGravity = true;  // Make sure gravity is applied
//            rb.isKinematic = false; // Ensure it's not kinematic
//        }

//        //if (winLose == null)
//        //{
//        //    Debug.LogError("winLoseScript not found!");
//        //}
//    }

//    private void Update()
//    {
//        UpdateWinLose();
//        weapon = GetComponent<Weapon>();
//       // winLose = FindObjectOfType<winLoseScript>();
//        LookAround();

//        // Jump input check
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            Jump();
//        }


//    }

//    private void FixedUpdate()
//    {
//        Move();
//    }

//    private void Move()
//    {

//        float horizontalInput = Input.GetAxis("Horizontal");
//        float verticalInput = Input.GetAxis("Vertical");

//        //Camera camera = Camera.main;
//        Vector3 forward = transform.forward;
//        Vector3 right = transform.right;

//        forward.y = 0;
//        right.y = 0;
//        forward.Normalize();
//        right.Normalize();

//        Vector3 movement = (horizontalInput * right + verticalInput * forward).normalized;

//        float speed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * runSpeedMultiplier : moveSpeed;

//        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
//    }

//    private void LookAround()
//    {
//        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
//        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

//        transform.Rotate(Vector3.up * mouseX);
//        xRotation -= mouseY;
//        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

//        Camera camera = GetComponentInChildren<Camera>();
//        if (camera != null)
//        {
//            camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
//        }
//    }

//    // Function to make the player jump
//    private void Jump()
//    {
//        if (rb != null)
//        {
//            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);  // Apply upward force for the jump
//            //isGrounded = false; // Set grounded to false until collision with the ground happens
//        }
//    }

//    public void UpdateTreasureText()
//    {
//        if (treasureCountText != null)
//        {
//            treasureCountText.text = "Treasure Boxes Collected: " + treasureCount;
//        }

//        if (treasureLeftText != null)
//        {
//            treasureLeftText.text = "Treasure Boxes Remaining: " + treasureLeft;
//        }
//    }

//    public void UpdateEnemyText()
//    {
//        if (enemiesDefeatedText != null)
//        {
//            enemiesDefeatedText.text = "Enemies Defeated: " + enemiesDefeated;
//        }

//        if (enemiesLeftText != null)
//        {
//            enemiesLeftText.text = "Enemies Remaining: " + enemiesLeft;
//        }
//    }

//    public void UpdateLifeText()
//    {
//        if (livesText != null)
//        {
//            livesText.text = "Lives left: " + lives;
//        }
//    }
//    private IEnumerator RespawnHealthPack(GameObject healthPack, float delay)
//    {
//        // Wait for the specified delay time
//        yield return new WaitForSeconds(delay);

//        // Reactivate the health pack after the delay
//        healthPack.SetActive(true);
//    }

//    public IEnumerator RespawnBullets(GameObject bullet, float delay)
//    {
//        yield return new WaitForSeconds(delay);
//        bullet.SetActive(true);
//    }

//    public void UpdateBulletCountText()
//    {
//        if (bulletCountText != null)
//        {
//            bulletCountText.text = "Bullets: " + bulletCount; // Update the TextMeshPro text
//        }
//    }

//    // Update the grenade count UI text
//    public void UpdateGrenadeCountText()
//    {
//        if (grenadeCountText != null)
//        {
//            grenadeCountText.text = "Grenades in the launcher: " + grenadeCount;
//        }
//    }

//    // Check for collision to determine if the player is grounded
//    public void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            isGrounded = true; // The player is grounded when they hit the ground
//        }

//        if (collision.gameObject.CompareTag("Gun"))
//        {
//            if (inventory.currentWeapon != null)
//            {
//                inventory.currentWeapon.gameObject.SetActive(false);
//            }
//            collision.gameObject.transform.SetParent(transform);
//            collision.gameObject.transform.localPosition = new Vector3(0.47f, -0.15f, 1.02f);
//            collision.gameObject.transform.localRotation = Quaternion.Euler(270f, 180f, 0f);

//            Weapon gun = collision.gameObject.GetComponent<Weapon>();
//            if (gun != null && inventory != null)
//            {
//                inventory.AddWeapon(gun);
//            }
//        }
//        else if (collision.gameObject.CompareTag("GrenadeLauncher"))
//        {
//            if (inventory.currentWeapon != null)
//            {
//                inventory.currentWeapon.gameObject.SetActive(false);
//            }
//            collision.gameObject.transform.SetParent(transform);
//            collision.gameObject.transform.localPosition = new Vector3(0.8f, 0.5f, 1.02f);
//            collision.gameObject.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

//            Weapon grenadeLauncher = collision.gameObject.GetComponent<Weapon>();
//            if (grenadeLauncher != null && inventory != null)
//            {
//                inventory.AddWeapon(grenadeLauncher);
//            }
//        }
//        else if (collision.gameObject.CompareTag("GrenadePrefab"))
//        {
//           grenadeCount++; // Increment the grenade count
//           grenadeCountText.text = "Grenades in the launcher: " + grenadeCount;
//           Debug.Log("Grenade collected! Total grenades: " + grenadeCount);
//           Destroy(collision.gameObject); // Destroy the grenade

//        }

//        else if (collision.gameObject.CompareTag("gunAmmo"))
//        {
//            if (bulletCount < 6)
//            {
//                collision.gameObject.SetActive(false); //working
//                bulletCount++; //working
//                bulletCountText.text = "Bullets: " + bulletCount;
//                Debug.Log("Bullet collected! Total bullets: " + bulletCount);
//                StartCoroutine(RespawnBullets(collision.gameObject, 3f));
//            }

//            else
//            {
//                Debug.Log("You can't hold more than 6 bullets!");
//            }
//        }


//        else if (collision.gameObject.CompareTag("Treasure"))
//        {
//            if (!isGameOver)
//            {
//                AudioSource treasureChime = GetComponent<AudioSource>();
//                if (treasureChime != null)
//                {
//                    treasureChime.Play(); // Play the sound
//                }
//                treasureCount++;
//                treasureLeft--;
//                UpdateTreasureText();
//                Destroy(collision.gameObject);
//            }  // Prevent collecting treasure if game is over


//        }

//        //else if (collision.gameObject.CompareTag("Treasure"))
//        //{
//        //    AudioSource treasureChime = GetComponent<AudioSource>();
//        //    if (treasureChime != null)
//        //    {
//        //        treasureChime.Play(); // Play the sound
//        //    }
//        //    treasureCount++;
//        //    treasureLeft--;
//        //    UpdateTreasureText();
//        //    Destroy(collision.gameObject);
//        //}


//        else if (collision.gameObject.CompareTag("enemyBullet"))
//        {
//            hitScreen.SetActive(true);  // Show Game Over screen
//            hitPanel.SetActive(true);   // Show the Game Over panel
//            lives--;
//            UpdateLifeText();
//            yield return new WaitForSeconds(1f);
//            hitScreen.SetActive(false);  // Show Game Over screen
//            hitPanel.SetActive(false);   // Show the Game Over panel
//        }


//        else if (collision.gameObject.CompareTag("healthPack"))
//        {
//            if ((lives >= 1) && (lives < 3))
//            {
//                // Deactivate the health pack
//                collision.gameObject.SetActive(false);

//                // Increment lives and update the life text
//                lives++;
//                UpdateLifeText();

//                // Start the respawn coroutine to reactivate the health pack after 3 seconds
//                StartCoroutine(RespawnHealthPack(collision.gameObject, 3f));
//            }
//            else if (lives == 3)
//            {
//                Debug.Log("You already have the max lives!");
//            }
//        }
//    }


//    public void UpdateWinLose()
//    {
//        // Trigger Game Over if lives are less than or equal to 0
//        if (lives <= 0 && !isGameOver)
//        {
//            GetComponent<Collider>().enabled = false;
//            Debug.Log("Game Over!");
//            isGameOver = true;  // Set Game Over state
//            gameOverScreen.SetActive(true);  // Show Game Over screen
//            gameOverPanel.SetActive(true);   // Show the Game Over panel
//            gameOverText.text = "GAME OVER...";  // Set the text
//            Time.timeScale = 0f;  // Pause the game (stop time flow)


//            AudioSource[] audioSources = GetComponents<AudioSource>();
//            foreach (AudioSource audio in audioSources)
//            {
//                audio.enabled = false;  // Disable audio source entirely
//            }

//            bgMusic.SetActive(false);
//            // Play lose sound (if desired)
//            AudioSource loseSound = GetComponent<AudioSource>();
//            if (loseSound != null)
//            {
//                loseSound.Play();  // Play the lose sound
//            }
//        }

//        // Check if player has won (all treasures collected and enemies defeated)
//        if (treasureCount == 3 && treasureLeft == 0 && enemiesLeft == 0 && !isWon)
//        {
//            GetComponent<Collider>().enabled = false;
//            Debug.Log("You win!");
//            isWon = true;  // Set Win state
//            winScreen.SetActive(true);  // Show Win screen
//            winPanel.SetActive(true);   // Show the Win panel

//            winText.text = "YOU WIN!";  // Set the text

//            AudioSource[] audioSources = GetComponents<AudioSource>();
//            foreach (AudioSource audio in audioSources)
//            {
//                audio.enabled = false;  
//            }
//            bgMusic.SetActive(false);

//            AudioSource winSound = GetComponent<AudioSource>();
//            if (winSound != null)
//            {
//                winSound.Play(); 
//            }
//            else
//            {
//                Debug.Log("No win audio detected bro");
//            }
//        }
//    }

//}

