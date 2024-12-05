using UnityEngine;

public class NodeGenerator : MonoBehaviour
{
    public GameObject nodePrefab; // Prefab olarak atanacak bir Node objesi
 
    public static NodeGenerator Instance { get; private set; }

    [SerializeField]
    private int nodeSize; // Inspector'dan ayarlanabilir

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Birden fazla instance varsa diğerini yok et
        }
    }

    // nodeSize değerini dışarıdan okuyabilmek için bir getter
    public int GetNodeSize()
    {
        return nodeSize;
    }  // Grid boyutu (AxA Kare alan)

    public void GenerateNode()
    {
        // Mevcut grid varsa temizle
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            DestroyImmediate(child.gameObject);
        }

        // Yeni grid oluştur
        for (int x = 0; x < GetNodeSize(); x++)
        {
            for (int y = 0; y < GetNodeSize(); y++)
            {
                // Node prefabını instantiate et
                GameObject newNode = Instantiate(nodePrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
                
                // Node'un adı ve pozisyonu
                newNode.name = $"Node_{x}_{y}";
                
                // Node'un Location değerlerini ata
                NodeScript nodeComponent = newNode.GetComponent<NodeScript>();
                if (nodeComponent != null)
                {  
                    nodeComponent.location = new Location { x = x, y = y };
                    NodeManager.AddNode(nodeComponent);
                }
            }
        }
        Debug.Log("Grid oluşturuldu!");
    }
}
