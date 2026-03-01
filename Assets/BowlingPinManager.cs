using UnityEngine;
using System.Collections;

public class BowlingPinManager : MonoBehaviour
{
    private BowlingPin[] pins;
    private Vector3[] originalPositions;
    private Quaternion[] originalRotations;

    void Start()
    {
        // Get all BowlingPin scripts from children
        pins = GetComponentsInChildren<BowlingPin>();

        // Save original positions and rotations for respawning
        originalPositions = new Vector3[pins.Length];
        originalRotations = new Quaternion[pins.Length];

        for (int i = 0; i < pins.Length; i++)
        {
            originalPositions[i] = pins[i].transform.position;
            originalRotations[i] = pins[i].transform.rotation;
        }

        StartCoroutine(CheckAllPinsDown());
    }

    IEnumerator CheckAllPinsDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // check every second

            bool allDown = true;
            foreach (BowlingPin pin in pins)
            {
                if (!pin.IsKnockedDown())
                {
                    allDown = false;
                    break;
                }
            }

            if (allDown)
            {
                Debug.Log("All pins down! Respawning...");
                yield return new WaitForSeconds(2f); // pause before respawn
                RespawnPins();
            }
        }
    }

    void RespawnPins()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            pins[i].transform.position = originalPositions[i];
            pins[i].transform.rotation = originalRotations[i];
            pins[i].ResetPin();
        }

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetRoundScore();
        }
    }
}