using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


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

    public int enemiesLeft = 8;
    public int enemiesDefeated = 0;

    public TextMeshProUGUI treasureCountText;
    public TextMeshProUGUI treasureLeftText;

    public TextMeshProUGUI enemiesDefeatedText;
    public TextMeshProUGUI enemiesLeftText;

    // Jumping variables
    public float jumpForce = 5f;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        inventory = GetComponent<PlayerInventory>();

        // Check if inventory is properly initialized
        if (inventory == null)
        {
            Debug.LogError("PlayerInventory component is missing on the player!");
        }

        weapon = FindFirstObjectByType<Weapon>(); // Get the Weapon component

        // Check if weapon is found
        if (weapon == null)
        {
            Debug.LogError("Weapon component could not be found in the scene!");
        }

        Cursor.lockState = CursorLockMode.Locked;

        UpdateEnemyText();
        UpdateTreasureText();

        // Make sure the Rigidbody is not kinematic and gravity is enabled
        if (rb != null)
        {
            rb.useGravity = true;  // Make sure gravity is applied
            rb.isKinematic = false; // Ensure it's not kinematic
        }
    }

    private void Update()
    {
        LookAround();

        // Jump input check
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
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
            isGrounded = false; // Set grounded to false until collision with the ground happens
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

    // Check for collision to determine if the player is grounded
    private void OnCollisionEnter(Collision collision)
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
            if (weapon != null)
            {
                weapon.grenadeCount++; // Increment the grenade count
                weapon.UpdateGrenadeCountText(); // Update the grenade UI count if needed
            }
            Destroy(collision.gameObject); // Destroy the grenade
        }
        else if (collision.gameObject.CompareTag("Treasure"))
        {
            treasureCount++;
            treasureLeft--;
            UpdateTreasureText();
            Destroy(collision.gameObject);
        }
    }
}

//public class PlayerMovement : MonoBehaviour
//{
//    public float moveSpeed = 5f;
//    public float runSpeedMultiplier = 2f;

//    public float mouseSensitivity = 2f;
//    private Rigidbody rb;


//    private float xRotation = 0f;
//    private PlayerInventory inventory;
//    private Weapon weapon; // Reference to the Weapon component

//    public int treasureCount = 0;
//    public int treasureLeft = 3;

//    public int enemiesLeft = 8;
//    public int enemiesDefeated = 0;

//    public TextMeshProUGUI treasureCountText;
//    public TextMeshProUGUI treasureLeftText;

//    public TextMeshProUGUI enemiesDefeatedText;
//    public TextMeshProUGUI enemiesLeftText;

//    private void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        inventory = GetComponent<PlayerInventory>();

//        // Check if inventory is properly initialized
//        if (inventory == null)
//        {
//            Debug.LogError("PlayerInventory component is missing on the player!");
//        }

//        weapon = FindFirstObjectByType<Weapon>(); // Get the Weapon component

//        // Check if weapon is found
//        if (weapon == null)
//        {
//            Debug.LogError("Weapon component could not be found in the scene!");
//        }

//        Cursor.lockState = CursorLockMode.Locked;

//        UpdateEnemyText();
//        UpdateTreasureText();

//        // Make sure the Rigidbody is not kinematic and gravity is enabled
//        if (rb != null)
//        {
//            rb.useGravity = true;  // Make sure gravity is applied
//            rb.isKinematic = false; // Ensure it's not kinematic
//        }
//    }

//    private void Update()
//    {
//        LookAround();


//    }

//    private void FixedUpdate()
//    {
//        Move();
//    }

//    private void Move()
//    {
//        float horizontalInput = Input.GetAxis("Horizontal");
//        float verticalInput = Input.GetAxis("Vertical");

//        Camera camera = Camera.main;
//        Vector3 forward = camera.transform.forward;
//        Vector3 right = camera.transform.right;

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

//    private void OnCollisionEnter(Collision collision)
//    {
//        // Null check for inventory and weapon
//        if (inventory == null)
//        {
//            Debug.LogError("Inventory is not initialized!");
//            return;
//        }

//        if (weapon == null)
//        {
//            Debug.LogError("Weapon is not initialized!");
//            return;
//        }

//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            isGrounded = true; // Ensure the player is grounded when they hit the ground
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
//            collision.gameObject.transform.localPosition = new Vector3(0.47f, -0.15f, 1.02f);
//            collision.gameObject.transform.localRotation = Quaternion.Euler(270f, 180f, 0f);

//            Weapon grenadeLauncher = collision.gameObject.GetComponent<Weapon>();
//            if (grenadeLauncher != null && inventory != null)
//            {
//                inventory.AddWeapon(grenadeLauncher);
//            }
//        }
//        else if (collision.gameObject.CompareTag("GrenadePrefab"))
//        {
//            if (weapon != null)
//            {
//                weapon.grenadeCount++; // Increment the grenade count
//                weapon.UpdateGrenadeCountText(); // Update the grenade UI count if needed
//            }
//            Destroy(collision.gameObject); // Destroy the grenade
//        }
//        else if (collision.gameObject.CompareTag("Treasure"))
//        {
//            treasureCount++;
//            treasureLeft--;
//            UpdateTreasureText();
//            Destroy(collision.gameObject);
//        }
//    }
//}


//public class PlayerMovement : MonoBehaviour
//{
//    public float moveSpeed = 5f;
//    public float runSpeedMultiplier = 2f;
//    public float jumpForce;
//    public float mouseSensitivity = 2f;
//    private Rigidbody rb;
//    private bool isGrounded = false;

//    private float xRotation = 0f;
//    private PlayerInventory inventory;
//    private Weapon weapon; // Reference to the Weapon component

//    public int treasureCount = 0;
//    public int treasureLeft = 3;

//    public int enemiesLeft = 8;
//    public int enemiesDefeated = 0;

//    public TextMeshProUGUI treasureCountText;
//    public TextMeshProUGUI treasureLeftText;

//    public TextMeshProUGUI enemiesDefeatedText;
//    public TextMeshProUGUI enemiesLeftText;

//    private void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        inventory = GetComponent<PlayerInventory>();

//        // Check if inventory is properly initialized
//        if (inventory == null)
//        {
//            Debug.LogError("PlayerInventory component is missing on the player!");
//        }

//        weapon = FindFirstObjectByType<Weapon>(); // Get the Weapon component

//        // Check if weapon is found
//        if (weapon == null)
//        {
//            Debug.LogError("Weapon component could not be found in the scene!");
//        }

//        Cursor.lockState = CursorLockMode.Locked;

//        UpdateEnemyText();
//        UpdateTreasureText();
//    }

//    private void Update()
//    {
//        LookAround();

//        // Jumping
//        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
//        {
//            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z); // Correcting to 'velocity' instead of 'linearVelocity'
//            isGrounded = false;
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

//        Camera camera = Camera.main;
//        Vector3 forward = camera.transform.forward;
//        Vector3 right = camera.transform.right;

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

//    private void OnCollisionEnter(Collision collision)
//    {
//        // Null check for inventory and weapon
//        if (inventory == null)
//        {
//            Debug.LogError("Inventory is not initialized!");
//            return;
//        }

//        if (weapon == null)
//        {
//            Debug.LogError("Weapon is not initialized!");
//            return;
//        }

//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            isGrounded = true;
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
//            collision.gameObject.transform.localPosition = new Vector3(0.47f, -0.15f, 1.02f);
//            collision.gameObject.transform.localRotation = Quaternion.Euler(270f, 180f, 0f);
//            //collision.gameObject.transform.SetParent(transform);
//            //collision.gameObject.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
//            //collision.gameObject.transform.localPosition = new Vector3(-0.15f, -1.18f, 1.74f);
//            //collision.gameObject.transform.localRotation = Quaternion.Euler(270f, 180f, 0f);

//            Weapon grenadeLauncher = collision.gameObject.GetComponent<Weapon>();
//            if (grenadeLauncher != null && inventory != null)
//            {
//                inventory.AddWeapon(grenadeLauncher);
//            }
//        }
//        else if (collision.gameObject.CompareTag("GrenadePrefab"))
//        {
//            if (weapon != null)
//            {
//                weapon.grenadeCount++; // Increment the grenade count
//                weapon.UpdateGrenadeCountText(); // Update the grenade UI count if needed
//            }
//            Destroy(collision.gameObject); // Destroy the grenade
//        }
//        else if (collision.gameObject.CompareTag("Treasure"))
//        {
//            treasureCount++;
//            treasureLeft--;
//            UpdateTreasureText();
//            Destroy(collision.gameObject);
//        }
//    }
//}


//public class PlayerMovement : MonoBehaviour
//{
//    public float moveSpeed = 5f;
//    public float runSpeedMultiplier = 2f;
//    public float jumpForce;
//    public float mouseSensitivity = 2f;
//    private Rigidbody rb;
//    private bool isGrounded = false;

//    private float xRotation = 0f;
//    private PlayerInventory inventory;
//    private Weapon weapon; // Reference to the Weapon component

//    public int treasureCount = 0;
//    public int treasureLeft = 3;

//    public int enemiesLeft = 8;
//    public int enemiesDefeated = 0;

//    public TextMeshProUGUI treasureCountText; 
//    public TextMeshProUGUI treasureLeftText;

//    public TextMeshProUGUI enemiesDefeatedText;
//    public TextMeshProUGUI enemiesLeftText;




//    private void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        inventory = GetComponent<PlayerInventory>();
//        weapon = FindFirstObjectByType<Weapon>(); // Get the Weapon component
//        Cursor.lockState = CursorLockMode.Locked;

//        UpdateEnemyText();
//        UpdateTreasureText();
//    }

//    private void Update()
//    {
//        LookAround();

//        // Jumping
//        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
//        {
//            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
//            isGrounded = false;
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

//        Camera camera = Camera.main;
//        Vector3 forward = camera.transform.forward;
//        Vector3 right = camera.transform.right;

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
//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            isGrounded = true;
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
//            collision.gameObject.transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
//            collision.gameObject.transform.localPosition = new Vector3(-0.15f, -1.18f, 1.74f);
//            collision.gameObject.transform.localRotation = Quaternion.Euler(270f, 180f, 0f);

//            Weapon grenadeLauncher = collision.gameObject.GetComponent<Weapon>();
//            if (grenadeLauncher != null && inventory != null)
//            {
//                inventory.AddWeapon(grenadeLauncher);
//            }
//        }

//        else if (collision.gameObject.CompareTag("GrenadePrefab"))
//        {
//            weapon.grenadeCount++; // Increment the grenade count
//            weapon.UpdateGrenadeCountText(); // Update the grenade UI count if needed
//            Destroy(collision.gameObject); // Destroy whatever collided w the player (grenade)
//        }

//        else if (collision.gameObject.CompareTag("Treasure"))
//        {
//            treasureCount++;
//            treasureLeft--;
//            UpdateTreasureText();
//            Destroy(collision.gameObject);
//        }
//    }
//}




//public class PlayerMovement : MonoBehaviour
//{
//    public float moveSpeed = 5f;
//    public float runSpeedMultiplier = 2f;
//    public float jumpForce;
//    public float mouseSensitivity = 2f;
//    private Rigidbody rb;
//    private bool isGrounded = false;

//    private float xRotation = 0f;
//    private PlayerInventory inventory;
//    private Weapon weapon; // Reference to the Weapon component

//    private void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        inventory = GetComponent<PlayerInventory>();
//        weapon = FindObjectOfType<Weapon>(); // Get the Weapon component
//        Cursor.lockState = CursorLockMode.Locked;
//    }

//    private void Update()
//    {
//        LookAround();

//        // Jumping
//        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
//        {
//            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
//            isGrounded = false;
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

//        Camera camera = Camera.main;
//        Vector3 forward = camera.transform.forward;
//        Vector3 right = camera.transform.right;

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

//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            isGrounded = true;
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
//        //}
//        //if (collision.gameObject.CompareTag("Grenade"))
//        //{
//        //    if (weapon != null)
//        //    {
//        //        weapon.GrenadeCount++; // Use the property to increment grenade count
//        //        Debug.Log($"Grenade collected! Total grenades: {weapon.GrenadeCount}");
//        //    }

//        //    Destroy(collision.gameObject); // Optionally destroy the grenade object
//        //}


//        else if (collision.gameObject.CompareTag("GrenadeLauncher"))
//        {
//            if (inventory.currentWeapon != null)
//            {
//                inventory.currentWeapon.gameObject.SetActive(false);
//            }
//            collision.gameObject.transform.SetParent(transform);
//            collision.gameObject.transform.localScale = new Vector3(2.91f, 2.91f, 2.91f);
//            collision.gameObject.transform.localPosition = new Vector3(-0.15f, -1.18f, 1.74f);
//            collision.gameObject.transform.localRotation = Quaternion.Euler(270f, 180f, 0f);

//            Weapon grenadeLauncher = collision.gameObject.GetComponent<Weapon>();
//            if (grenadeLauncher != null && inventory != null)
//            {
//                inventory.AddWeapon(grenadeLauncher);
//            }
//        }
//    }

//    private void OnCollisionExit(Collision collision)
//    {
//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            isGrounded = false;
//        }
//    }
//}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

////using System.Collections;
////using System.Collections.Generic;
////using UnityEngine;

//public class PlayerMovement : MonoBehaviour
//{
//    public float moveSpeed = 5f;
//    public float runSpeedMultiplier = 2f; // Multiplier for running speed
//    public float jumpForce;
//    public float mouseSensitivity = 2f; // Sensitivity for mouse movement
//    private Rigidbody rb;
//    private bool isGrounded = false;

//    private float xRotation = 0f; // To store the vertical rotation
//    private PlayerInventory inventory; // Reference to the PlayerInventory
//    private Weapon weapon; // Reference to the Weapon component

//    private void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        inventory = GetComponent<PlayerInventory>(); // Get the PlayerInventory component
//        weapon = FindObjectOfType<Weapon>(); // Get the Weapon component
//        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
//    }

//    private void Update()
//    {
//        LookAround();

//        // Inside Update
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
//        }
//    }

//    private void FixedUpdate()
//    {
//        Move();
//        Jump();
//    }

//    private void Move()
//    {
//        float horizontalInput = Input.GetAxis("Horizontal"); // A and D keys
//        float verticalInput = Input.GetAxis("Vertical"); // W and S keys

//        // Get the camera's forward and right directions
//        Camera camera = Camera.main;
//        Vector3 forward = camera.transform.forward;
//        Vector3 right = camera.transform.right;

//        // Flatten the forward and right vectors to ignore the y-axis
//        forward.y = 0;
//        right.y = 0;
//        forward.Normalize();
//        right.Normalize();

//        // Calculate the movement direction based on the camera's orientation
//        Vector3 movement = (horizontalInput * right + verticalInput * forward).normalized;

//        // Determine speed based on whether the SHIFT key is pressed
//        float speed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * runSpeedMultiplier : moveSpeed;

//        // Move the character
//        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
//    }

//    private void Jump()
//    {
//        // Check if the player is grounded before allowing a jump
//        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
//        {
//            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
//            isGrounded = false; // Prevent double jumping
//        }
//    }

//    private void LookAround()
//    {
//        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
//        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

//        // Rotate the player horizontally
//        transform.Rotate(Vector3.up * mouseX);

//        // Adjust the vertical rotation and clamp it
//        xRotation -= mouseY;
//        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

//        // Apply the vertical rotation to the camera
//        Camera camera = GetComponentInChildren<Camera>();
//        if (camera != null)
//        {
//            camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
//        }
//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        // Ensure the player is grounded only when colliding with the ground
//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            isGrounded = true; // Reset jump when grounded
//        }

//        if (collision.gameObject.CompareTag("Gun"))
//        {
//            // Deactivate current weapon if it exists
//            if (inventory.currentWeapon != null)
//            {
//                inventory.currentWeapon.gameObject.SetActive(false); // Hide the currently equipped weapon
//            }

//            // Set the weapon's parent to the player and configure its position/rotation
//            collision.gameObject.transform.SetParent(transform); // Make the weapon's parent the player
//            collision.gameObject.transform.localPosition = new Vector3(0.47f, -0.15f, 1.02f);
//            collision.gameObject.transform.localRotation = Quaternion.Euler(270f, 180f, 0f); // Desired rotation

//            // Add the weapon to inventory
//            Weapon gun = collision.gameObject.GetComponent<Weapon>();
//            if (gun != null && inventory != null)
//            {
//                inventory.AddWeapon(gun); // Add weapon to inventory
//            }
//        }

//        // Check for grenade collision
//        if (collision.gameObject.CompareTag("Grenade"))
//        {
//            if (weapon != null)
//            {
//                weapon.grenadeCount++; // Increment grenade count
//                weapon.UpdateGrenadeCountText(); // Update the displayed grenade count
//            }

//            Destroy(collision.gameObject); // Optionally destroy the grenade object
//        }

//        else if (collision.gameObject.CompareTag("GrenadeLauncher"))
//        {
//            // Deactivate current weapon if it exists
//            if (inventory.currentWeapon != null)
//            {
//                inventory.currentWeapon.gameObject.SetActive(false); // Hide the currently equipped weapon
//            }

//            // Set the weapon's parent to the player and configure its position/rotation
//            collision.gameObject.transform.SetParent(transform); // Make the weapon's parent the player
//            collision.gameObject.transform.localScale = new Vector3(2.91f, 2.91f, 2.91f);
//            collision.gameObject.transform.localPosition = new Vector3(-0.15f, -1.18f, 1.74f);
//            collision.gameObject.transform.localRotation = Quaternion.Euler(270f, 180f, 0f); // Desired rotation

//            // Add the grenade to inventory
//            Weapon grenadeLauncher = collision.gameObject.GetComponent<Weapon>();
//            if (grenadeLauncher != null && inventory != null)
//            {
//                inventory.AddWeapon(grenadeLauncher); // Add weapon to inventory
//            }
//        }
//    }

//    private void OnCollisionExit(Collision collision)
//    {
//        // Ensure the player is no longer considered grounded when leaving the ground
//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            isGrounded = false; // This may be optional depending on your collision detection
//        }
//    }
//}


//public class PlayerMovement : MonoBehaviour
//{
//    public float moveSpeed = 5f;
//    public float runSpeedMultiplier = 2f; // Multiplier for running speed
//    public float jumpForce;
//    public float mouseSensitivity = 2f; // Sensitivity for mouse movement
//    private Rigidbody rb;
//    private bool isGrounded = false;

//    private float xRotation = 0f; // To store the vertical rotation
//    private PlayerInventory inventory; // Reference to the PlayerInventory

//    private void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//        inventory = GetComponent<PlayerInventory>(); // Get the PlayerInventory component
//        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
//    }

//    private void Update()
//    {
//        LookAround();

//        // Inside Update
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
//        }
//    }

//    private void FixedUpdate()
//    {
//        Move();
//        Jump();
//    }

//    private void Move()
//    {
//        float horizontalInput = Input.GetAxis("Horizontal"); // A and D keys
//        float verticalInput = Input.GetAxis("Vertical"); // W and S keys

//        // Get the camera's forward and right directions
//        Camera camera = Camera.main;
//        Vector3 forward = camera.transform.forward;
//        Vector3 right = camera.transform.right;

//        // Flatten the forward and right vectors to ignore the y-axis
//        forward.y = 0;
//        right.y = 0;
//        forward.Normalize();
//        right.Normalize();

//        // Calculate the movement direction based on the camera's orientation
//        Vector3 movement = (horizontalInput * right + verticalInput * forward).normalized;

//        // Determine speed based on whether the SHIFT key is pressed
//        float speed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * runSpeedMultiplier : moveSpeed;

//        // Move the character
//        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
//    }

//    private void Jump()
//    {
//        // Check if the player is grounded before allowing a jump
//        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
//        {
//            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
//            isGrounded = false; // Prevent double jumping
//        }
//    }

//    private void LookAround()
//    {
//        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
//        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

//        // Rotate the player horizontally
//        transform.Rotate(Vector3.up * mouseX);

//        // Adjust the vertical rotation and clamp it
//        xRotation -= mouseY;
//        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

//        // Apply the vertical rotation to the camera
//        Camera camera = GetComponentInChildren<Camera>();
//        if (camera != null)
//        {
//            camera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
//        }
//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        // Ensure the player is grounded only when colliding with the ground
//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            isGrounded = true; // Reset jump when grounded
//        }

//        if (collision.gameObject.CompareTag("Gun"))
//        {
//            // Deactivate current weapon if it exists
//            if (inventory.currentWeapon != null)
//            {
//                inventory.currentWeapon.gameObject.SetActive(false); // Hide the currently equipped weapon
//            }

//            // Set the weapon's parent to the player and configure its position/rotation
//            collision.gameObject.transform.SetParent(transform); // Make the weapon's parent the player
//            collision.gameObject.transform.localPosition = new Vector3(0.469999999f, -0.151335105f, 1.01999998f);
//            collision.gameObject.transform.localRotation = Quaternion.Euler(270f, 180f, 0f); // Desired rotation

//            // Add the weapon to inventory
//            Weapon gun = collision.gameObject.GetComponent<Weapon>();
//            if (gun != null && inventory != null)
//            {
//                inventory.AddWeapon(gun); // Add weapon to inventory
//            }
//        }

//        else if (collision.gameObject.CompareTag("GrenadeLauncher"))
//        {
//            // Deactivate current weapon if it exists
//            if (inventory.currentWeapon != null)
//            {
//                inventory.currentWeapon.gameObject.SetActive(false); // Hide the currently equipped weapon
//            }

//            // Set the weapon's parent to the player and configure its position/rotation
//            collision.gameObject.transform.SetParent(transform); // Make the weapon's parent the player
//            collision.gameObject.transform.localScale = new Vector3(2.91000009f, 2.91000009f, 2.91000009f);
//            collision.gameObject.transform.localPosition = new Vector3(-0.150999993f, -1.17999995f, 1.73500001f);
//            collision.gameObject.transform.localRotation = Quaternion.Euler(270f, 180f, 0f); // Desired rotation

//            // Add the grenade to inventory
//            Weapon grenadeLauncher = collision.gameObject.GetComponent<Weapon>();
//            if (grenadeLauncher != null && inventory != null)
//            {
//                inventory.AddWeapon(grenadeLauncher); // Add weapon to inventory
//            }
//        }
//    }


//    private void OnCollisionExit(Collision collision)
//    {
//        // Ensure the player is no longer considered grounded when leaving the ground
//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            isGrounded = false; // This may be optional depending on your collision detection
//        }
//    }
//}



//-------------------------------------
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerMovement : MonoBehaviour
//{
//    public float moveSpeed = 5f;
//    public float runSpeedMultiplier = 2f; // Multiplier for running speed
//    public float jumpForce = 5f;
//    private Rigidbody rb;
//    private bool isGrounded;

//    private void Start()
//    {
//        rb = GetComponent<Rigidbody>();
//    }

//    private void FixedUpdate()
//    {
//        Move();
//        Jump();
//    }

//    private void Move()
//    {
//        float horizontalInput = Input.GetAxis("Horizontal");
//        float verticalInput = Input.GetAxis("Vertical");

//        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput).normalized;

//        // Determine speed based on whether the SHIFT key is pressed
//        float speed = Input.GetKey(KeyCode.LeftShift) ? moveSpeed * runSpeedMultiplier : moveSpeed;

//        // Move the character
//        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
//    }

//    private void Jump()
//    {
//        // Check if the player is grounded before allowing a jump
//        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
//        {
//            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
//            isGrounded = false; // Prevent double jumping
//        }
//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.CompareTag("Ground"))
//        {
//            isGrounded = true; // Reset jump when grounded
//        }
//    }
//}
