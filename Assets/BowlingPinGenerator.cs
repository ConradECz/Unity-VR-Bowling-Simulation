using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class BowlingPinGenerator : MonoBehaviour
{
    [Header("Resolution")]
    [Tooltip("Number of sides around the circumference. 24-32 looks smooth.")]
    public int sides = 28;

    [Header("Pin dimensions (metres)")]
    [Tooltip("Total height of the pin.")]
    public float totalHeight = 0.381f;

    [Header("Material")]
    public Material pinMaterial;

    void Start()
    {
        GeneratePin();
    }

    Vector2[] GetProfile()
    {
        float h = totalHeight;
        return new Vector2[]
        {
            // height (y),  radius (x)
            new Vector2(0.000f * h, 0.057f),   // base edge
            new Vector2(0.030f * h, 0.060f),   // base widest
            new Vector2(0.100f * h, 0.054f),   // lower taper
            new Vector2(0.250f * h, 0.057f),   // belly start
            new Vector2(0.380f * h, 0.060f),   // belly peak (widest point)
            new Vector2(0.480f * h, 0.052f),   // belly upper
            new Vector2(0.580f * h, 0.034f),   // neck upper taper
            new Vector2(0.630f * h, 0.025f),   // neck narrowest
            new Vector2(0.680f * h, 0.028f),   // neck to head
            new Vector2(0.750f * h, 0.038f),   // head bulge
            new Vector2(0.820f * h, 0.036f),   // head upper
            new Vector2(0.900f * h, 0.024f),   // crown taper
            new Vector2(0.960f * h, 0.010f),   // crown near-tip
            new Vector2(1.000f * h, 0.000f),   // crown tip
        };
    }

    [ContextMenu("Generate Pin")]
    public void GeneratePin()
    {
        Vector2[] profile = GetProfile();
        int rings = profile.Length;
        int vSides = sides;

        int vertsPerRing = vSides + 1;
        int totalVerts = rings * vertsPerRing + 2; // +2 for base centre & tip centre

        Vector3[] verts   = new Vector3[totalVerts];
        Vector2[] uvs     = new Vector2[totalVerts];
        Vector3[] normals = new Vector3[totalVerts];

        // Fill ring vertices
        for (int r = 0; r < rings; r++)
        {
            float y      = profile[r].x;
            float radius = profile[r].y;
            float vCoord = y / totalHeight;

            for (int s = 0; s <= vSides; s++)
            {
                float angle = (s / (float)vSides) * Mathf.PI * 2f;
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;

                int idx = r * vertsPerRing + s;
                verts[idx]   = new Vector3(x, y, z);
                uvs[idx]     = new Vector2(s / (float)vSides, vCoord);
                normals[idx] = new Vector3(x, 0f, z).normalized;
            }
        }

        // Base centre vertex
        int baseCentre = rings * vertsPerRing;
        verts[baseCentre]   = new Vector3(0f, 0f, 0f);
        uvs[baseCentre]     = new Vector2(0.5f, 0f);
        normals[baseCentre] = Vector3.down;

        // Tip centre vertex
        int tipCentre = baseCentre + 1;
        verts[tipCentre]   = new Vector3(0f, totalHeight, 0f);
        uvs[tipCentre]     = new Vector2(0.5f, 1f);
        normals[tipCentre] = Vector3.up;

        // Triangles
        // Side quads between adjacent rings
        int sideTriCount = (rings - 1) * vSides * 6;
        // Base fan
        int baseFanCount = vSides * 3;
        // Tip fan
        int tipFanCount = vSides * 3;

        int[] tris = new int[sideTriCount + baseFanCount + tipFanCount];
        int t = 0;

        // Side quads
        for (int r = 0; r < rings - 1; r++)
        {
            for (int s = 0; s < vSides; s++)
            {
                int curr    = r * vertsPerRing + s;
                int next    = curr + 1;
                int currUp  = (r + 1) * vertsPerRing + s;
                int nextUp  = currUp + 1;

                // First triangle
                tris[t++] = curr;
                tris[t++] = currUp;
                tris[t++] = next;

                // Second triangle
                tris[t++] = next;
                tris[t++] = currUp;
                tris[t++] = nextUp;
            }
        }

        // Base cap fan (facing down, so winding is reversed)
        for (int s = 0; s < vSides; s++)
        {
            tris[t++] = baseCentre;
            tris[t++] = s + 1;
            tris[t++] = s;
        }

        // Tip cap fan
        int lastRingStart = (rings - 1) * vertsPerRing;
        for (int s = 0; s < vSides; s++)
        {
            tris[t++] = tipCentre;
            tris[t++] = lastRingStart + s;
            tris[t++] = lastRingStart + s + 1;
        }

        // Build mesh
        Mesh mesh = new Mesh();
        mesh.name = "BowlingPin";
        mesh.vertices  = verts;
        mesh.triangles = tris;
        mesh.uv        = uvs;
        mesh.normals   = normals;
        mesh.RecalculateNormals();   // smooth shaded normals
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().sharedMesh = mesh;

        if (pinMaterial != null)
            GetComponent<MeshRenderer>().sharedMaterial = pinMaterial;

        Debug.Log($"Bowling pin generated: {verts.Length} verts, {tris.Length / 3} tris.");
    }
}