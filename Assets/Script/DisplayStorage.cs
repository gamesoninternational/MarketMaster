using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayStorage : MonoBehaviour
{
    public int MaxProductAvailable, maxTransfer, ProductAvailable;

    public bool StorageFull, HasProduct;
    public GameObject StaffPosition;

    public GameObject NavigationScript;
    //public int ProductAvailable;

    public GameObject ProductPrefab;

    private Transform[] racks; // Array to hold the rack positions

    public string ProductName;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the racks array with child transforms of the StorageRack object
        Transform storageRack = transform.Find("StorageRack");
        racks = new Transform[MaxProductAvailable];
        for (int i = 0; i < MaxProductAvailable; i++)
        {
            racks[i] = storageRack.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ProductAvailable == MaxProductAvailable)
        {
            StorageFull = true;
        }

        if (ProductAvailable < MaxProductAvailable)
        {
            StorageFull = false;
            //NavigationScript.GetComponent<MerchandiserStaff>().FindStorage();
        }

        if (StorageFull == true)
        {
            //NavigationScript.GetComponent<MerchandiserStaff>().FindStorage();
        }
        if (ProductAvailable >= 1)
        {
            HasProduct = true;
        }


        // Update the product instantiation
        AdjustProductAvailable();
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the other GameObject has the tag "Staff"
        if (other.gameObject.CompareTag("Staff"))
        {


            if (ProductAvailable < MaxProductAvailable)
            {
                InstantiateProducts();
                int availableCapacity = MaxProductAvailable - ProductAvailable;
                int amountToTransfer = Mathf.Min(NavigationScript.GetComponent<MerchandiserStaff>().ProductAvailable, maxTransfer, availableCapacity);

                ProductAvailable += amountToTransfer;
                NavigationScript.GetComponent<MerchandiserStaff>().ProductAvailable -= amountToTransfer;

                
            }
            else
            {
                Debug.Log("ProductAvailable has reached maximum capacity: " + MaxProductAvailable);
            }

        }
    }

    /*void OnTriggerExit(Collider other)
    {
        // Check if the other GameObject has the tag "Staff"
        if (other.gameObject.CompareTag("Staff"))
        {
            ProductAvailable = 0;
        }
    }*/

    void AdjustProductAvailable()
    {
        int currentProducts = GetCurrentProductAvailable();
        if (ProductAvailable > currentProducts)
        {
            // Add more products if ProductAvailable increased
            InstantiateProducts();
        }
        else if (ProductAvailable < currentProducts)
        {
            // Remove products if ProductAvailable decreased
            RemoveProducts(currentProducts - ProductAvailable);
        }
    }

    void InstantiateProducts()
    {
        int productsToInstantiate = ProductAvailable;
        
        for (int i = 0; i < racks.Length && productsToInstantiate > 0; i++)
        {
            // Check if the rack already has a child
            if (racks[i].childCount == 0)
            {
                GameObject product = Instantiate(ProductPrefab, racks[i].position, Quaternion.identity);
                product.transform.SetParent(racks[i]); // Parent the product to the corresponding rack
                productsToInstantiate--;
            }
        }
    }

    void RemoveProducts(int excessProducts)
    {
        for (int i = racks.Length - 1; i >= 0 && excessProducts > 0; i--)
        {
            // Check if the rack has a child and remove it
            if (racks[i].childCount > 0)
            {
                Destroy(racks[i].GetChild(0).gameObject);
                excessProducts--;
                //NavigationScript.GetComponent<NavigationScript>().ProductAvailable++;
            }
        }
    }

    int GetCurrentProductAvailable()
    {
        int count = 0;
        foreach (Transform rack in racks)
        {
            if (rack.childCount > 0)
            {
                count++;
            }
        }
        return count;
    }
}
