using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;

    public ParticleSystem smokeEffect;

    public int damage = 1;
    
    Rigidbody2D rigidbody2D;
    float timer;
    float OilSpillTime = 0;
    int direction = 1;
    bool broken = true;
    public static int RobotCount = 0;
    public GameObject Oil;
    public bool isbroken{get{return broken;}}
    Animator animator;
    public float OilSpawnTime = 3f;
    
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if(!broken)
        {
            return;
        }
        
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }

        if(OilSpillTime >  OilSpawnTime && Oil != null)
        {
            OilSpillTime = 0;
            Instantiate(Oil, this.transform.position, Quaternion.identity);
        }

        OilSpillTime += Time.deltaTime;

    }
    
    void FixedUpdate()
    {
        //remember ! inverse the test, so if broken is true !broken will be false and return won’t be executed.
        if(!broken)
        {
            return;
        }
        
        Vector2 position = rigidbody2D.position;
        
        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }
        
        rigidbody2D.MovePosition(position);
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController >();

        if (player != null)
        {
            player.ChangeHealth(-damage);
        }

        if(other.gameObject.tag == "Robot")
        {
            Debug.Log("Robot Detected");
            EnemyController x = other.gameObject.GetComponent<EnemyController>();

            if(x.isbroken == false)
            {
                x.brake();
            }
        }

        if(other.gameObject.tag == "Projectile")
        {
            Fix();
            Destroy(other.gameObject);
        }
    }

    public void brake()
    {
        broken = true;
        rigidbody2D.simulated = true;
        //optional if you added the fixed animation
        animator.SetTrigger("Brake");
        RobotCount --;
        smokeEffect.Play();
    }
    //Public because we want to call it from elsewhere like the projectile script
    public void Fix()
    {
        broken = false;
        rigidbody2D.simulated = false;
        //optional if you added the fixed animation
        animator.SetTrigger("Fixed");
        RobotCount ++;
        smokeEffect.Stop();
    }
}