using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
public class ColliderWireframe : MonoBehaviour
{
    [SerializeField] private Material lineMaterial;
    [SerializeField] private float lineWidth = 0.05f;

    private void Start()
    {
        DrawWireframe();
    }

    private void DrawWireframe()
    {
        BoxCollider box = GetComponent<BoxCollider>();

        Vector3 size = box.size;
        Vector3 center = box.center;
        Vector3 half = size * 0.5f;

        Vector3[] corners = new Vector3[8]
        {
            center + new Vector3(-half.x, -half.y, -half.z),
            center + new Vector3( half.x, -half.y, -half.z),
            center + new Vector3( half.x, -half.y,  half.z),
            center + new Vector3(-half.x, -half.y,  half.z),

            center + new Vector3(-half.x,  half.y, -half.z),
            center + new Vector3( half.x,  half.y, -half.z),
            center + new Vector3( half.x,  half.y,  half.z),
            center + new Vector3(-half.x,  half.y,  half.z),
        };

        int[,] edges = new int[,]
        {
            {0,1},{1,2},{2,3},{3,0}, 
            {4,5},{5,6},{6,7},{7,4}, 
            {0,4},{1,5},{2,6},{3,7}  
        };

        for (int i = 0; i < edges.GetLength(0); i++)
        {
            GameObject lineObj = new GameObject("Edge_" + i);
            lineObj.transform.SetParent(this.transform, false);

            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            lr.material = lineMaterial;
            lr.startWidth = lr.endWidth = lineWidth;
            lr.positionCount = 2;
            lr.useWorldSpace = false; 
            lr.SetPosition(0, corners[edges[i, 0]]);
            lr.SetPosition(1, corners[edges[i, 1]]);
        }
    }
}