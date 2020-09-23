using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerupSpeed : MonoBehaviour
{
    [Header("Powerup Settings")]
    [SerializeField] float _speedIncreaseAmount = 20;
    [SerializeField] float _powerupDuration = 5;
    

    [Header("Setup")]
    [SerializeField] GameObject _visualsToDeactivate = null;
    [SerializeField] AudioClip _powerupSound = null;

    Collider _colliderToDeactive = null;
    bool _poweredUp = false;

    private void Awake()
    {
        _colliderToDeactive = GetComponent<Collider>();

        EnableObject();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerShip playerShip
            = other.gameObject.GetComponent<PlayerShip>();
        // if we have a valid player and not already powered up
        if (playerShip != null && _poweredUp == false)
        {
            //Start powerup Timer. Restart, if it's already started
            StartCoroutine(PowerupSequence(playerShip));
        }
    }

    IEnumerator PowerupSequence(PlayerShip playerShip)
    {
        // set boolean for detectng lockout
        _poweredUp = true;

        ActivatePowerup(playerShip);
        // stimulate this object being disabled. We dont
        // REALLY wabt to disable it, because we still need
        // script behavior to continue functioning
        DisableObject();

        //wait for the required duration
        yield return new WaitForSeconds(_powerupDuration);
        // reset
        DeactivatePowerup(playerShip);
        EnableObject();

        // set boolean to release lockout
        _poweredUp = false;
    }

    void ActivatePowerup(PlayerShip playerShip)
    {
        if(playerShip != null)
        {
            // powerup player
            playerShip.SetSpeed(_speedIncreaseAmount);
            // visuals
            playerShip.SetBoosters(true);
        }
    }

    void DeactivatePowerup(PlayerShip playerShip)
    {
        // revert player powerup. - will subtract
        playerShip?.SetSpeed(-_speedIncreaseAmount);
        // visuals
        playerShip?.SetBoosters(false);
    }

    public void DisableObject()
    {
        AudioHelper.PlayClip2D(_powerupSound, 1);
        // disable collider, so it can't be retriggered
        _colliderToDeactive.enabled = false;
        // disable visuals, to simulate deactivated
        _visualsToDeactivate.SetActive(false);
        //TODO reactivate particle flash/audio
    }

    public void EnableObject()
    {
        
        // enable collider, so it can be retriggered
        _colliderToDeactive.enabled = true;
        // enable visuals again, to draw player attention
        _visualsToDeactivate.SetActive(true);
        //TODO reactivate particle flash/audio
    }

}
