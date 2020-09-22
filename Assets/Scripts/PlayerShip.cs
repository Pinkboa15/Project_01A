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
    public bool alive = true;
    public ParticleSystem Sparkles;
    [SerializeField] GameObject gameController = null;
    [SerializeField] GameObject _visualsToDeactivate = null;
    Collider _colliderToDeactive = null;
    Rigidbody _rb = null;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        ParticleSystem Sparkles = GetComponent<ParticleSystem>();
        GameObject gameController = GetComponent<GameObject>();
        _colliderToDeactive = GetComponent<Collider>();
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
        Sparkles.Play();
        _colliderToDeactive.enabled = false;
        _visualsToDeactivate.SetActive(false);
        StartCoroutine(PowerupSequence(gameController));

    }
    IEnumerator PowerupSequence(GameObject controller)
    {
        yield return new WaitForSeconds(2);
        Restart(gameController);
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
    public void Restart(GameObject controller)
    {
        controller.Reload();
    }
}
