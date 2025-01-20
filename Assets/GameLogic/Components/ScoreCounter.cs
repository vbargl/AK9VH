using TMPro;
using UnityEngine;

namespace GameLogic.Components
{
    public class ScoreCounter : MonoBehaviour
    {
        public TMP_Text scoreText;
    
        private int _score;
    
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start() => 
            AddScore(0);

        public void OnCollisionEnter2D(Collision2D _) =>
            AddScore(1);

        // Update is called once per frame

        public void AddScore() => AddScore(1);
        public void AddScore(int scoreToAdd)
        {
            _score += scoreToAdd;
            scoreText.text = _score == 1 ? $"{_score} fruit" : $"{_score} fruits";
        }
    }
}
