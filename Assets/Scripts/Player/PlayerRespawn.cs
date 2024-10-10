using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Transform spawnpoint;
    private Health playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
    }

    public void Respawn()
    {
        transform.position = spawnpoint.position; //Move to spawn
        //restore player health and reset animation
        playerHealth.Respawn();


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Spawnpoint")
        {
            spawnpoint = collision.transform;
        }
    }
}
