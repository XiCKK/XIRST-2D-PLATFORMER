using UnityEditor;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    [SerializeField]
    private float healthValue;

    [Header("Sound Effect")]
    [SerializeField]
    private AudioClip healthcollect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" )
        {
            SoundManager.instance.PlaySound(healthcollect);
            collision.GetComponent<Health>().AddHealth(healthValue);
            gameObject.SetActive(false);
        }
    }
}
