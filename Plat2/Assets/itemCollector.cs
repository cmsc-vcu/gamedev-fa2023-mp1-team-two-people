using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    private int points = 0;
    [SerializeField] private Text pointsText;
    private bool isWin = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Pineapple"))
        {
            Destroy(collision.gameObject);
            points++;
            pointsText.text = "Points:" + points;
            if (points == 20)
            {
                pointsText.text = "YOU WIN!";
                isWin = true;
            }
        }
    }

    private void Update()
    {
        // Toggle the visibility of pointsText when the player wins (20 points)
        if (isWin)
        {
            pointsText.enabled = (Time.time % 2) < 1;
        }
    }
}
