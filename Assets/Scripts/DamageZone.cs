using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    public bool confusion;
    void OnTriggerStay2D(Collider2D other)
    {
        RubyController controller = other.GetComponent<RubyController >();

        if (controller != null)
        {
            if (confusion == true)
            {
                controller.setconfusion();
            }
            else
            {
            controller.ChangeHealth(-1);
            }
        }
    }
}
