using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    // Tüm Node'ları saklayan bir dictionary
    private static Dictionary<(int, int), NodeScript> nodes = new Dictionary<(int, int), NodeScript>();

    private void Awake()
    {
        nodes = new Dictionary<(int, int), NodeScript>();
        foreach (Transform child in transform)
        {
            NodeScript node = child.GetComponent<NodeScript>();
            if (node != null)
            {
                (int, int) key = (node.location.x, node.location.y); // X ve Y pozisyonlarını NodeScript'ten alın.
                if (!nodes.ContainsKey(key))
                {
                    nodes.Add(key, node);
                    Debug.Log($"Node eklendi: {key}");
                }
            }
        }
    }


    // Node'ları haritaya eklemek için bir yöntem
    public static void AddNode(NodeScript node)
    {
        (int x, int y) key = (node.location.x, node.location.y);
        if (!nodes.ContainsKey(key))
        {
            nodes[key] = node;
        }
        else
        {
            Debug.LogWarning($"Node zaten var: ({node.location.x}, {node.location.y})");
        }
    }

    // Belirli bir koordinattaki Node'u almak için yöntem
    public static NodeScript GetNodeAt(int x, int y)
    {
        (int, int) key = (x, y);
        if (nodes.TryGetValue(key, out NodeScript node))
        {
            Debug.Log("Node tespit edildi: "+ node);
            return node;
        }
        return null; // Node bulunamadıysa
    }

    public static Vector3 GetNodeWorldPosition(int x, int y)
    {
        NodeScript node = GetNodeAt(x,y);
        return node.transform.position;
    }


    // Debugging için tüm Node'ları yazdırma
    public static void PrintAllNodes()
    {
        foreach (var node in nodes)
        {
            Debug.Log($"Node bulundu: ({node.Key.Item1}, {node.Key.Item2})");
        }
    }


}
