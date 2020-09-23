using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerShip playerShip
             = other.gameObject.GetComponent<PlayerShip>();
        if(playerShip != null)
        {
            playerShip.PassedFinishLine();
        }
            
    }
}
