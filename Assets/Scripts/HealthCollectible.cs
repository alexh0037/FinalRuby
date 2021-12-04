using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    
   public AudioClip collectedClip;
   public bool Ammo;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController>();

        if (controller != null)
        {
            if(Ammo == true)
            {
                controller.changeAmmo();
                
            }

            else if (controller.health < controller.maxHealth)
            {
                controller.ChangeHealth(1);
                
            }
            
            Destroy(gameObject);
            
                controller.PlaySound(collectedClip);
        }

    }
}