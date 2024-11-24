using UnityEngine;

public class Spin : MonoBehaviour
{
    private PlayerInventory playerInventory;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((this.tag == "healthPack"))
        {
            Vector3 rotationToAdd = new Vector3(0, .3f, 0);
            transform.Rotate(rotationToAdd);
        }

        else if (this.tag == "gunAmmo")
        {
            Vector3 rotationToAdd = new Vector3(0, .25f, .25f);
            transform.Rotate(rotationToAdd);
        }

      

        else if (this.tag == "GrenadePrefab")
        {
            Vector3 rotationToAdd = new Vector3(0, .4f, 0);
            transform.Rotate(rotationToAdd);
        }
    }
}
