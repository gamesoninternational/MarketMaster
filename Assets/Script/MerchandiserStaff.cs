using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MerchandiserStaff : MonoBehaviour
{
    public Transform CustomerTarget;
    public Animator animator; // Animator reference set through Inspector
    private NavMeshAgent agent;
    public float turnSpeed = 5f; // Rotation speed

    public int ProductAvailable, MaxProduct;

    public string need;

    public GameObject[] DisplayStorageObjects;
    public GameObject[] StorageObjects;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        FindDisplayStorageEmpty();
        
        // Set destination to CustomerTarget's position
        if (CustomerTarget != null)
        {
            agent.destination = CustomerTarget.position;
        }

        // Calculate movement direction
        Vector3 direction = agent.desiredVelocity.normalized;

        // If the agent is moving, rotate towards the movement direction
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
        }

        // Check if the agent is moving and set the IsWalking parameter accordingly
        bool isWalking = agent.velocity.magnitude > 0.1f;
        animator.SetBool("IsWalking", isWalking);

        FindDisplayStorageEmpty();

    }

    public void FindStorage(){
        // Cari semua objek di dalam scene yang memiliki tag "Storage"
        StorageObjects = GameObject.FindGameObjectsWithTag("Storage");

        bool allStoragesFull = true; // Flag to track if all storages are full

        // Periksa setiap objek Storage apakah memiliki script dengan nama "Storage"
        foreach (GameObject storageObject in StorageObjects)
        {
            Storage storageScript = storageObject.GetComponent<Storage>();
            if (storageScript != null)
            {
                //Debug.Log(storageObject.name + " has the Storage script attached.");

                // Periksa apakah variabel StorageFull aktif atau tidak
                if (storageScript.HasProduct == true)
                {
                    Debug.Log(storageObject.name + " product ada");
                    // Cari variabel GameObject StaffPosition di dalam Storage script
                    if (storageScript.StaffPosition != null)
                    {
                        CustomerTarget = storageScript.StaffPosition.transform;
                        Debug.Log("CustomerTarget is now set to: " + CustomerTarget.name);
                        // Setelah menemukan satu StaffPosition, kita bisa keluar dari loop
                        break;
                    }
                    else
                    {
                        Debug.Log(storageObject.name + " does not have a StaffPosition assigned.");
                    }
                }
                else
                {
                    //allStoragesFull = false; // At least one storage is not full
                    Debug.Log(storageObject.name + " belum ada product");
                }
            }
            else
            {
                Debug.Log(storageObject.name + " does not have the Storage script attached.");
            }
        }

        // Log a message if all storages are full
        if (allStoragesFull)
        {
            Debug.Log("All storage objects are full.");
            //FindAndSetTruckAsTarget();
            
        }
    }

    public void FindDisplayStorageEmpty(){
        DisplayStorageObjects = GameObject.FindGameObjectsWithTag("StorageDisplay");
        bool allStoragesFull = true;

        foreach (GameObject storageDisplayObject in DisplayStorageObjects)
        {
            DisplayStorage storageScript = storageDisplayObject.GetComponent<DisplayStorage>();
            if (storageScript != null)
            {
                // Cari variabel GameObject StaffPosition di dalam Storage script
                if (storageScript.StorageFull == false){
                    if (storageScript.StaffPosition != null)
                    {
                        need = storageScript.ProductName;
                        if (need == storageScript.ProductName){
                            FindStorage();
                            Debug.Log("Cari Warehouse Storage");
                        }
                        /*CustomerTarget = storageScript.StaffPosition.transform;
                        Debug.Log("CustomerTarget is now set to: " + CustomerTarget.name);
                        // Setelah menemukan satu StaffPosition, kita bisa keluar dari loop
                        */
                        break;
                    }
                    else
                    {
                        Debug.Log(storageDisplayObject.name + " does not have a StaffPosition assigned.");
                        
                    }
                }
                //Debug.Log(storageDisplayObject.name + " has the Storage script attached.");

                // Periksa apakah variabel StorageFull aktif atau tidak
                if (storageScript.StorageFull == true)
                {
                    //Debug.Log(storageDisplayObject.name + " Storage Display is full.");
                }
                if (storageScript.StorageFull == false)
                {
                    allStoragesFull = false; // At least one storage is not full
                    //Debug.Log(storageDisplayObject.name + " Storage Display is not full.");
                    
                    
                }
            }
            else
            {
                Debug.Log(storageDisplayObject.name + " does not have the Storage script attached.");
            }
        }
        // Log a message if all storages are full
        if (allStoragesFull)
        {
            Debug.Log("All storage objects are full.");
            //FindAndSetTruckAsTarget();
            need = null;
        }
    }


}
