//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Bullet : MonoBehaviour
//{

//    private PlayerMovement playerMovement;
//    private Weapon weapon;

//    public float despawnTimer = .2f;
//    void Start()
//    {
//        playerMovement = FindFirstObjectByType<PlayerMovement>();
//        weapon = FindFirstObjectByType<Weapon>();
//    }

//    void Awake()
//    {
//        Destroy(gameObject, despawnTimer);
//    }

//    void OnCollisionEnter(Collision collision)
//    {

//        //if (gameObject.CompareTag("playerBullet") && (collision.gameObject.CompareTag("Enemy")))
//        //{
//        //    // Destroy the enemy object
//        //    Destroy(gameObject);
//        //}

//        if (gameObject.CompareTag("enemyBullet") && (collision.gameObject.CompareTag("Player")))
//        {
//            playerMovement.lives--;
//            playerMovement.UpdateLifeText();
//            Destroy(gameObject);
//        }



//        //else if (gameObject.CompareTag("gunAmmo") && collision.gameObject.CompareTag("Player"))
//        //{
//        //    Debug.Log("Ammo collision detected");

//        //    if (weapon != null)
//        //    {
//        //        if (weapon.bulletCount < 6)
//        //        {
//        //            gameObject.SetActive(false);
//        //            weapon.bulletCount++;
//        //            weapon.UpdateBulletCountText();
//        //            Debug.Log("Bullet count increased to: " + weapon.bulletCount);

//        //            StartCoroutine(playerMovement.RespawnBullets(gameObject, 3f));
//        //        }
//        //        else
//        //        {
//        //            Debug.Log("You can only hold 6 bullets!");
//        //        }
//        //    }
//        //    else
//        //    {
//        //        Debug.LogError("Weapon reference is null!");
//        //    }
//        //}
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if ((Collider.CompareTag("gunAmmo")) && (other.CompareTag("Player")))
//        {
//            // Pick up the health pack (you can add any additional logic like restoring health here)
//            Debug.Log("Ammo Picked Up!");

//            // Disable the health pack (hide it)
//            gameObject.SetActive(false);

//            // Start the respawn process
//            StartCoroutine(RespawnBullets());

//            if (weapon.bulletCount < 6)
//            {
//                weapon.bulletCount++;
//                weapon.UpdateBulletCountText();
//                Debug.Log("Bullet count increased to: " + weapon.bulletCount);
//            }
//            else
//            {
//                Debug.Log("You can only hold 6 bullets!");
//            }
//        }
//    }
//    public IEnumerator RespawnBullets(GameObject bullet, float delay)
//    {
//        yield return new WaitForSeconds(delay);
//        bullet.SetActive(true);
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Weapon weapon;

    public float despawnTimer = .2f;

    void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        weapon = FindFirstObjectByType<Weapon>();
    }

    void Awake()
    {
        Destroy(gameObject, despawnTimer);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (gameObject.CompareTag("enemyBullet") && collision.gameObject.CompareTag("Player"))
        {
            playerMovement.lives--;
            playerMovement.UpdateLifeText();
            Destroy(gameObject);
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("gunAmmo") && gameObject.CompareTag("Player"))
    //    {
    //        // Pick up the ammo
    //        Debug.Log("Ammo Picked Up!");

    //        // Disable the ammo (hide it)
    //        gameObject.SetActive(false);

    //        // Start the respawn process
    //       // StartCoroutine(RespawnBullets(gameObject, 3f));

    //        if (weapon.bulletCount < 6)
    //        {
    //            weapon.bulletCount++;
    //            weapon.UpdateBulletCountText();
    //            Debug.Log("Bullet count increased to: " + weapon.bulletCount);
    //        }
    //        else
    //        {
    //            Debug.Log("You can only hold 6 bullets!");
    //        }
    //    }
    //}

    //// Fixed coroutine with parameters
    //public IEnumerator RespawnBullets(GameObject bullet, float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    bullet.SetActive(true);
    //}
}
