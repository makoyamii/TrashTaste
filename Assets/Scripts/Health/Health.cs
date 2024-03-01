using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public static event Action OnPlayerDeath;
    [Header ("Health")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("iFrames")]
    [SerializeField]private float iFramesDuration;
    [SerializeField]private float numberOfFlashes;
    private SpriteRenderer spriteRend;

    [Header("DeathSound")]

    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;
    public Animator transition;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }
    public void TakeDamage(float _damage)
    {
        SoundManager.instance.PlaySound(hurtSound);
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            //iframes
            StartCoroutine(Invulnerability());
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("die");
                GetComponent<PlayerMovement>().enabled = false;
                dead = true;
                SoundManager.instance.PlaySound(deathSound);
                OnPlayerDeath.Invoke();
            }
        }
    }
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    private IEnumerator Invulnerability() {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        //invulnerability duration
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds((iFramesDuration) / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds((iFramesDuration) / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }

    IEnumerator LoadLevel(int levelIndex) {
        transition.SetTrigger("Start");

        //Wait
        yield return new WaitForSeconds(2);
        
        //Load scene
        SceneManager.LoadScene(levelIndex);
    }
}