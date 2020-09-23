using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerShip : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 12f;
    [SerializeField] float _turnSpeed = 3f;
    [SerializeField] int _scaleSize = 2;
    [Header("Feedback")]
    [SerializeField] TrailRenderer _trail = null;
    public ParticleSystem Sparkles;
    [SerializeField] GameInput gameController = null;
    [SerializeField] GameObject _visualsToDeactivate = null;
    [SerializeField] AudioClip _deathSound = null;
    [SerializeField] AudioClip _winSound = null;
    Rigidbody _rb = null;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        ParticleSystem Sparkles = GetComponent<ParticleSystem>();
        GameInput gameController = GetComponent<GameInput>();
    }

    private void Start()
    {
        Sparkles.Stop();
    }

    private void FixedUpdate()
    {
        MoveShip();
        TurnShip();
    }

    // use forces to build momentum forward/backward
    void MoveShip()
    {
        // S/Down = -1, W/Up = 1, None = 0. Scale by moveSpeed 
        float moveAmountThisFrame = Input.GetAxisRaw("Vertical") * _moveSpeed;
        // combine our direction with our calculated amount
        Vector3 moveDirection = transform.forward * moveAmountThisFrame;
        // apply the movement to the physics object
        _rb.AddForce(moveDirection);
    }
    // don't use forces for this. We want rotations to be precise
    void TurnShip()
    {
        // A/Left = -1, D/Right = 1, None + 0. Scale by turnSpeed
        float turnAmountthisFrame = Input.GetAxisRaw("Horizontal") * _turnSpeed;
        // specify an axis to apply our turn amount (x,y,z) as a rotation
        Quaternion turnOffset = Quaternion.Euler(0, turnAmountthisFrame, 0);
        // spin the rigidbody
        _rb.MoveRotation(_rb.rotation * turnOffset);
    }

    public void Kill()
    {
        Debug.Log("Player has been killed!");
        _moveSpeed = 0;
        _turnSpeed = 0;
        this.gameObject.transform.position = this.gameObject.transform.position;
        _visualsToDeactivate.SetActive(false);
        Sparkles.Play();
        gameController.GameOver();
        AudioHelper.PlayClip2D(_deathSound, 1);
        //this.gameObject.SetActive(false);
        StartCoroutine(PowerupSequence(gameController));
    }
    public void PassedFinishLine()
    {
        _visualsToDeactivate.SetActive(false);
        AudioHelper.PlayClip2D(_winSound, 1);
        gameController.Win();
    }
    IEnumerator PowerupSequence(GameInput controller)
    {
        yield return new WaitForSeconds(3);
        controller.ReloadLevel();
    }

    public void SetSpeed(float speedChange)
    {
        _moveSpeed += speedChange;
        //TODO audio/visuals
    }

    public void SetBoosters(bool activateState)
    {
        _trail.enabled = activateState;
    }
    public void SetSize(int sizeChange)
    {
        _scaleSize /= sizeChange;
        gameObject.transform.localScale = new Vector3(_scaleSize, _scaleSize, _scaleSize);
    }
    public void BacktoNormalSize(int sizeChange)
    {
        _scaleSize *= sizeChange;
        gameObject.transform.localScale = new Vector3(_scaleSize, _scaleSize, _scaleSize);
    }
}
