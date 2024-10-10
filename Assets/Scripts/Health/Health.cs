using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField]
    private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("iFrames")]
    [SerializeField]
    private float iFrameDuration;
    [SerializeField]
    private int numberOffFlashes;
    private SpriteRenderer spriteRenderer;

    [Header("Component")]
    [SerializeField]
    private Behaviour[] components;
    private bool invulnerable;

    [Header("Sound Effect")]
    [SerializeField]
    private AudioClip dieEffect;
    [SerializeField]
    private AudioClip hurtEffect;
    [SerializeField]
    private AudioClip spawnEffect;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            //iframes
            anim.SetTrigger("Hurt");
            StartCoroutine(Invunerebility());
            SoundManager.instance.PlaySound(hurtEffect);
        }
        else
        {
            if (!dead)
            {
                //Deativate all attached component classes
                foreach (Behaviour component in components)
                {
                    component.enabled = false;
                }
                anim.SetTrigger("Die");
                anim.SetBool("Grounded", true);
                dead = true;
                SoundManager.instance.PlaySound(dieEffect);
            }

        }
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    public void Respawn()
    {
        dead = false;
        AddHealth(startingHealth);
        //reset player animation to not die
        anim.ResetTrigger("Die");
        anim.Play("Idle");
        StartCoroutine (Invunerebility());
        SoundManager.instance.PlaySound(spawnEffect);

        foreach (Behaviour component in components)
        {
            component.enabled = true;
        }
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
