using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerShip : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 12f;
    [SerializeField] float _turnSpeed = 3f;
    public ParticleSystem Sparkles;

    Rigidbody _rb = null;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        ParticleSystem Sparkles = GetComponent<ParticleSystem>();
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
        this.gameObject.SetActive(false);
    }
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            Sparkles.Play();
        }
        if(Input.GetKeyUp(KeyCode.W))
        {
            Sparkles.Stop();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Sparkles.Play();
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            Sparkles.Stop();
        }
        if (Input.GetKey(KeyCode.Q))
        {
            _moveSpeed = _moveSpeed + 0.2f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            _moveSpeed = _moveSpeed - 0.2f;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
