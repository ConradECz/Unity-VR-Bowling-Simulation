using UnityEngine;
using UnityEngine.Rendering;

public class BowlingPin : MonoBehaviour
{
    private Renderer pinRenderer;
    private Collider pinCollider;
    private Rigidbody rb;
    private bool isKnockedDown = false;

    void Start()
    {
        pinRenderer = GetComponent<Renderer>();
        pinCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isKnockedDown && (collision.gameObject.CompareTag("Ball") || collision.gameObject.CompareTag("Player")))
        {
            KnockDown();
        }
    }

    void KnockDown()
    {
        isKnockedDown = true;

        //Reports to ScoreManager that a pin has been knocked down
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.PinKnockedDown();
        }

        // URP transparency setup
        Material mat = pinRenderer.material;
        mat.SetFloat("_Surface", 1);
        mat.SetFloat("_Blend", 0);
        mat.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        mat.renderQueue = (int)RenderQueue.Transparent;

        // Set transparency level (0 = invisible, 1 = fully visible)
        Color color = pinRenderer.material.color;
        color.a = 0.1f;
        pinRenderer.material.color = color;

        // Disable collider so nothing can interact with it
        pinCollider.enabled = false;

        // Stop physics simulation
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }

    public void ResetPin()
    {
        isKnockedDown = false;

        // Revert to opaque
        Material mat = pinRenderer.material;
        mat.SetFloat("_Surface", 0);
        mat.SetInt("_SrcBlend", (int)BlendMode.One);
        mat.SetInt("_DstBlend", (int)BlendMode.Zero);
        mat.SetInt("_ZWrite", 1);
        mat.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
        mat.renderQueue = (int)RenderQueue.Geometry;

        Color color = mat.color;
        color.a = 1f;
        mat.color = color;

        // Re-enable collider
        pinCollider.enabled = true;

        // Re-enable physics
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }

    public bool IsKnockedDown()
    {
        return isKnockedDown;
    }
}