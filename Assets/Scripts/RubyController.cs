using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{

 public float speed = 3.0f;
    
    public int maxHealth = 5;
    
    public GameObject projectilePrefab;
    
    public AudioClip throwSound;
    public AudioClip hitSound;
    
    public int health { get { return currentHealth; }}
    int currentHealth;
    
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    
    public int GearCount;
    Animator animator;
    Vector2 lookDirection = new Vector2(1,0);
    
    AudioSource audioSource;
    public ParticleSystem hit;
    public ParticleSystem heal;

    private bool slide;
    private float slidetime;
    public void slip()
    {
        slide = true;
        slidetime = 1f;
    }
    float confusetime = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();

        GearCount = 20;

        slide = false; 
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(currentHealth <= 0)
        {
            rigidbody2d.velocity = Vector2.zero;
            animator.SetFloat("Speed", 0f);
            return;
        }

           if(slide = true && slidetime > 0)
           {
               slidetime -= Time.deltaTime;
               return;
           }
           if(slidetime <= 0)
           {
               slide = false;
           }
           if(confusetime > 0)
           {
               confusetime -= Time.deltaTime;
               vertical = Input.GetAxis("Horizontal") * -1;
               horizontal = Input.GetAxis("Vertical") * -1;
           }
            else
            {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
            }
        
        Vector2 move = new Vector2(horizontal, vertical);
        
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }
        
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        
        if(Input.GetKeyDown(KeyCode.C )&& GearCount > 0)
        {
            Launch();
        }
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
    }
    
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + (speed + slidetime) * horizontal * Time.deltaTime;
        position.y = position.y + (speed + slidetime) * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void changeAmmo()
    {
        GearCount ++; 
    }
    public void ChangeHealth(int amount)
    {
        if(amount > 0)
        {
            heal.Play();
        }
        if (amount < 0)
        {
            if (isInvincible)
                return;
            
            isInvincible = true;
            invincibleTimer = timeInvincible;
            
            PlaySound(hitSound);
            hit.Play();
        }
        
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }
       public void setconfusion()
       {
           confusetime = 5f;
       } 
    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        GearCount --;

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        
        PlaySound(throwSound);
    } 
    
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}