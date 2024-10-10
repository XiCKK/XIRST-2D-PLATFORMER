using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    [SerializeField]
    private AudioClip finishEffect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //Go To Next Level
            SoundManager.instance.PlaySound(finishEffect);
            ScenceController.instance.NextLevel();
        }
    }
}
