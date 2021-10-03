using UnityEngine;

public class GameOverOnCollide : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Obstacle")) GameOverController.Instance.GameOver();
    }
}