
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private float attackCooldown;
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private GameObject[] granades;

    [Header ("Sound Effect")]
    [SerializeField]
    private AudioClip granadeSound;

    private Animator anim;
    private PlayerMovement playerMovement;
    private float CooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space) && CooldownTimer > attackCooldown && playerMovement.canAttack())
            Attack();

        CooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        SoundManager.instance.PlaySound(granadeSound);
        anim.SetTrigger("Attack");
        CooldownTimer = 0;

        granades[FindFireball()].transform.position = firePoint.position;
        granades[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }
    private int FindFireball()
    {
        for (int i = 0; i < granades.Length; i++)
        {
            if (!granades[i].activeInHierarchy)
            {
                return i;
            }
        }
        return 0;
    }
}
