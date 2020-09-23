using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupShrink : MonoBehaviour
{
    [Header("Powerup Settings")]
    [SerializeField] int _SizeDecreaseAmount = 2;
    [SerializeField] float _powerupDuration = 30;

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
        if (playerShip != null && _poweredUp == false)
        {
            StartCoroutine(PowerupSequence(playerShip));
        }
    }

    IEnumerator PowerupSequence(PlayerShip playerShip)
    {
        _poweredUp = true;

        ActivatePowerup(playerShip);
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
        if (playerShip != null)
        {
            // powerup player
            playerShip.SetSize(_SizeDecreaseAmount);
        }
    }

    void DeactivatePowerup(PlayerShip playerShip)
    {
        // revert player powerup. - will subtract
        playerShip?.BacktoNormalSize(_SizeDecreaseAmount);
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
