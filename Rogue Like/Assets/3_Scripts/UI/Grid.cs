using UnityEngine;

[ExecuteInEditMode]
public class Grid : MonoBehaviour
{
    public Material lineMaterial;    // assign a simple unlit material
    public Color lineColor = Color.white;
    public float cellSize = 1f;
    public int lineCount = 50; // how many lines each direction

    void OnPostRender()
    {
        if (!lineMaterial) return;
        lineMaterial.SetPass(0);
        GL.Begin(GL.LINES);
        GL.Color(lineColor);

        float extent = lineCount * cellSize;
        // vertical lines
        for (int i = -lineCount; i <= lineCount; i++)
        {
            float x = i * cellSize;
            GL.Vertex(new Vector3(x, -extent, 0));
            GL.Vertex(new Vector3(x, extent, 0));
        }
        // horizontal lines
        for (int j = -lineCount; j <= lineCount; j++)
        {
            float y = j * cellSize;
            GL.Vertex(new Vector3(-extent, y, 0));
            GL.Vertex(new Vector3(extent, y, 0));
        }

        GL.End();
    }
}
