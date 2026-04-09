using UnityEngine;
using System.Collections;

public class BowlingBallManager : MonoBehaviour
{
    private BowlingBall[] balls;
    private Vector3[] originalPositions;
    private Quaternion[] originalRotations;

    void Start()
    {
        balls = GetComponentsInChildren<BowlingBall>();


        // Confirm all 4 balls are found
        Debug.Log("BowlingBallManager found " + balls.Length + " balls");

        originalPositions = new Vector3[balls.Length];
        originalRotations = new Quaternion[balls.Length];

        for (int i = 0; i < balls.Length; i++)
        {
            originalPositions[i] = balls[i].transform.position;
            originalRotations[i] = balls[i].transform.rotation;
            Debug.Log("Registered ball: " + balls[i].gameObject.name);
        }

        StartCoroutine(CheckAllBallsThrown());
    }

    IEnumerator CheckAllBallsThrown()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            bool allThrown = true;
            foreach (BowlingBall ball in balls)
            {
                Debug.Log(ball.gameObject.name + " thrown status: " + ball.HasBeenThrown());
                if (!ball.HasBeenThrown())
                {
                    allThrown = false;
                    break;
                }
            }

            if (allThrown)
            {
                Debug.Log("All balls thrown! Respawning...");
                yield return new WaitForSeconds(6f); // pause before respawn
                RespawnBalls();
            }
        }
    }

    void RespawnBalls()
    {
        for (int i = 0; i < balls.Length; i++)
        {
            balls[i].ResetBall(originalPositions[i], originalRotations[i]);
        }

        Debug.Log("Balls respawned!");
    }
}