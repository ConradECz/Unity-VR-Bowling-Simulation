using UnityEngine;

public class BowlingBall : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasBeenThrown = false;

    [SerializeField] private float throwDetectionSpeed = 0.5f; // speed threshold to detect a throw

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Detect if the ball has been thrown based on its speed
        if (!hasBeenThrown && rb != null && rb.linearVelocity.magnitude > throwDetectionSpeed)
        {
            hasBeenThrown = true;
            Debug.Log(gameObject.name + " has been Thrown! Speed: " + rb.linearVelocity.magnitude);
        }
    }

    public bool HasBeenThrown()
    {
        return hasBeenThrown;
    }

    public void ResetBall(Vector3 originalPosition, Quaternion originalRotation)
    {
        hasBeenThrown = false;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = false;

        transform.position = originalPosition;
        transform.rotation = originalRotation;
    }
}