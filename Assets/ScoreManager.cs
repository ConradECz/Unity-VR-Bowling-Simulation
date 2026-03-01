using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public TextMeshProUGUI scoreText;
    private int totalScore = 0;
    private int pinsHitThisRound = 0;

    void Awake()
    {
        // Singleton so any script can access it
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        UpdateScoreUI();
    }

    public void PinKnockedDown()
    {
        pinsHitThisRound++;
        totalScore += 1; // each pin worth 1 point
        UpdateScoreUI();
        Debug.Log("Score: " + totalScore);
    }

    public void ResetRoundScore()
    {
        pinsHitThisRound = 0;
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + totalScore;
    }

    public int GetTotalScore()
    {
        return totalScore;
    }
}