using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Truck : MonoBehaviour
{
    public GameObject NavigationScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        // Check if the other GameObject has the tag "Staff"
        if (other.gameObject.CompareTag("Staff"))
        {
            Debug.Log("Masuk Pak Eko");
            NavigationScript.GetComponent<NavigationScript>().ProductAvailable = NavigationScript.GetComponent<NavigationScript>().MaxProduct;
        }
    }
}
