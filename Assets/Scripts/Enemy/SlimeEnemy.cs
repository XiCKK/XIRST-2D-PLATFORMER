using UnityEngine;
using System.Collections;

public class SlimeEnemy : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField]
    private float attackCooldown;
    [SerializeField]
    private float range;
    [SerializeField]
    private float rangey;
    [SerializeField]
    private int damage;

    [Header ("Collider Parameters")]
    [SerializeField]
    private float colliderDistance;
    [SerializeField]
    private BoxCollider2D boxCollider;

    [Header ("Player Layer")]
    [SerializeField]
    private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;
     
    [Header ("Attack Sound")]
    [SerializeField]
    private AudioClip attackSound;
    [SerializeField]
    private AudioClip hurtEffect;
    [SerializeField]
    private AudioClip dieEffect;

    [Header("iFrames")]
    [SerializeField]
    private float iFrameDuration;
    [SerializeField]
    private int numberOffFlashes;
    private SpriteRenderer spriteRenderer;

    //Reference 
    private Animator anim;
    private Health playerHealth;
    private EnemyPatrol enemyPatrol;

    [Header("Health")]
    [SerializeField]
    private int health;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        //attack only when play in sight
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                //Attack
                cooldownTimer = 0;
                anim.SetTrigger("SlimeAttack");
                SoundManager.instance.PlaySound(attackSound);
            }
        }
        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight();
        }
        
    }

    private bool PlayerInSight()
    {
        rangey = transform.localScale.y;
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * rangey * colliderDistance, 
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y * rangey, boxCollider.bounds.size.z), 0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
        }

        return hit.collider != null;

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * rangey * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y * rangey, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            //Damage Player
            playerHealth.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Projectile")
        {
            if (health > 0)
            {
                SoundManager.instance.PlaySound(hurtEffect);
                StartCoroutine(Invunerebility());
                health--;
            }
            else
            {
                SoundManager.instance.PlaySound(dieEffect);
                Killself();
            }
        }
    }
    private void Killself()
    {
        Destroy(gameObject);
        gameObject.SetActive(false);
    }


    private IEnumerator Invunerebility()
    {
        Physics2D.IgnoreLayerCollision(7, 8, true);
        //invunerability duration
        for (int i = 0; i < numberOffFlashes; i++)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(iFrameDuration / (numberOffFlashes * 2));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(iFrameDuration / (numberOffFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(7, 8, false);
    }
}
