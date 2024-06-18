using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationScript : MonoBehaviour
{
    public Transform CustomerTarget;
    public Animator animator; // Animator reference set through Inspector
    private NavMeshAgent agent;
    public float turnSpeed = 5f; // Rotation speed
    public float destinationThreshold = 1f; // Threshold distance to consider agent as arrived
    public int ProductAvailable, MaxProduct;

    public GameObject[] StorageObjects;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (ProductAvailable == 0)
        {
            FindAndSetTruckAsTarget();
        }
    }

    // Update is called once per frame
    void Update()
    {
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

        // Check if ProductAvailable is 0 and change the target to Truck if necessary
        if (ProductAvailable == 0)
        {
            FindAndSetTruckAsTarget();
        }

        if (ProductAvailable == MaxProduct)
        {
            Debug.Log("Cari Storage yang kosong");
            StartCoroutine(StartFindStorage());
        }

        // Check if the agent has reached the destination
        /*if (ProductAvailable == 0 && CustomerTarget != null && agent.remainingDistance <= destinationThreshold && !agent.pathPending)
        {
            if (CustomerTarget.CompareTag("Truck"))
            {
                Debug.Log("Agent has reached the Truck");
                if(ProductAvailable == 0){
                    ProductAvailable = MaxProduct;
                }
            }
        }*/
    }

    // Method to find the nearest truck and set it as the target
    public void FindAndSetTruckAsTarget()
    {
        CustomerTarget = GameObject.FindWithTag("Truck").transform;
        GameObject nearestTruck = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject truck in GameObject.FindGameObjectsWithTag("Truck"))
        {
            float distance = Vector3.Distance(transform.position, truck.transform.position);
            if (distance < nearestDistance)
            {
                nearestTruck = truck;
                nearestDistance = distance;
                //Debug.Log("sampe di Truck");
            }
        }

        if (nearestTruck != null)
        {
            CustomerTarget = nearestTruck.transform;
            agent.destination = CustomerTarget.position;
            //Debug.Log("sampe di Truck");
        }
    }

    public void FindStorage()
    {
        // Cari semua objek di dalam scene yang memiliki tag "Storage"
        StorageObjects = GameObject.FindGameObjectsWithTag("Storage");

        // Hitung jumlah objek dengan tag "Storage"
        //int storageCount = StorageObjects.Length;

        // Tampilkan jumlah objek yang ditemukan di konsol
        //Debug.Log("Number of Storage objects detected: " + storageCount);

        bool allStoragesFull = true; // Flag to track if all storages are full

        // Periksa setiap objek Storage apakah memiliki script dengan nama "Storage"
        foreach (GameObject storageObject in StorageObjects)
        {
            Storage storageScript = storageObject.GetComponent<Storage>();
            if (storageScript != null)
            {
                Debug.Log(storageObject.name + " has the Storage script attached.");

                // Periksa apakah variabel StorageFull aktif atau tidak
                if (storageScript.StorageFull)
                {
                    Debug.Log(storageObject.name + " is full.");
                }
                else
                {
                    allStoragesFull = false; // At least one storage is not full
                    Debug.Log(storageObject.name + " is not full.");

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
            FindAndSetTruckAsTarget();
        }
    }


    IEnumerator StartFindStorage(){
        yield return new WaitForSeconds(1f);
        FindStorage();
    }
}
