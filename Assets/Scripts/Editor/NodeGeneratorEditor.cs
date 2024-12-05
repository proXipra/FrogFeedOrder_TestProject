using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NodeGenerator))]
public class NodeGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Varsayılan inspector'ı çiz
        
        NodeGenerator nodeGenerator = (NodeGenerator)target;

        // NodePositionAdjuster bileşeninin olup olmadığını kontrol et
        NodePositionAdjuster nodePosAdjuster = nodeGenerator.GetComponent<NodePositionAdjuster>();

        // Buton ekle
        if (GUILayout.Button("Generate Grid"))
        {
            nodeGenerator.GenerateNode(); // Node oluştur

            if (nodePosAdjuster != null)
            {
                nodePosAdjuster.AdjustNodePosition(); // Eğer mevcutsa yere sabitle
            }
            else
            {
                Debug.LogWarning("NodePositionAdjuster bileşeni mevcut değil!");
            }
        }
    }
}
