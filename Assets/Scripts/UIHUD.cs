using System;
using TMPro;
using UnityEngine;

public class UIHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake()
    {
        
    }

    private void Start()
    {
        ScoreManager.Instance.OnScoreChanged += UpdateScore;
    }

    private void OnDestroy()
    {
        ScoreManager.Instance.OnScoreChanged -= UpdateScore;
    }

    private void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }
}
