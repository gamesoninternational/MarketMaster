using UnityEngine;

public class ProductTarget : MonoBehaviour
{
    public int ProductAvailable;
    public int MaxProductAvailable;
    public int maxTransfer;

    // Fungsi untuk memindahkan nilai dari skrip lain
    public void PindahkanProductAvailable(ProductSource source)
    {
        if (ProductAvailable < MaxProductAvailable)
        {
            
            int availableCapacity = MaxProductAvailable - ProductAvailable;
            int amountToTransfer = Mathf.Min(source.ProductAvailable, maxTransfer, availableCapacity);

            ProductAvailable += amountToTransfer;
            source.ProductAvailable -= amountToTransfer;

            Debug.Log("ProductAvailable has been moved: " + amountToTransfer);
            Debug.Log("Source ProductAvailable: " + source.ProductAvailable);
            Debug.Log("Target ProductAvailable: " + ProductAvailable);
        }
        else
        {
            Debug.Log("ProductAvailable has reached maximum capacity: " + MaxProductAvailable);
        }
        if (ProductAvailable >= MaxProductAvailable)
        {
            
        }
    }

    // Untuk tujuan demo, panggil fungsi ini dari Start
    void Start()
    {
        // Temukan skrip sumber (asumsikan berada pada GameObject yang sama atau bisa dicari dengan cara lain)
        ProductSource source = FindObjectOfType<ProductSource>();

        // Inisialisasi MaxProductAvailable untuk tujuan demo
        //MaxProductAvailable = 6;

        // Pindahkan nilai
        //PindahkanProductAvailable(source);
    }
}
