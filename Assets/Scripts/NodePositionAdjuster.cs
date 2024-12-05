using UnityEngine;

public class NodePositionAdjuster : MonoBehaviour
{
    public GameObject nodeGenObject;  // Node nesnesi
    public GameObject ground;      // Zemin nesnesi

    public void AdjustNodePosition()
    {
        if (nodeGenObject == null || ground == null) return;

        // Node'un collider'ını al
        
        Collider nodeGenCollider = nodeGenObject.GetComponentInChildren<Collider>();
        if (nodeGenCollider == null)
        {
            Debug.LogError("Grid nesnesinde Collider yok!");
            return;
        }

        // Zeminin yüksekliğini al
        float groundY = ground.transform.position.y;

        // NodeGenerator'ün alt kısmını hesapla
        float gridBottomY = nodeGenCollider.bounds.min.y;

        // Yüksekliği düzeltmek için gereken fark
        float adjustment = groundY - gridBottomY;

        // Yeni konumu hesapla
        Vector3 newPosition = nodeGenObject.transform.position;
        newPosition.y += adjustment;

        // NodeGenerator nesnesini yeni konuma taşı
        nodeGenObject.transform.position = newPosition;

        Debug.Log("Grid nesnesi yüzeye hizalandı.");
    }
}
