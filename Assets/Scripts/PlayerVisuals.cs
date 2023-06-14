using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Animator _animtor;
    private PlayerController _controller;

    private void Awake()
    {    
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animtor = GetComponent<Animator>();
        _controller = GetComponent<PlayerController>();
    }

    private void Start()
    {
        _controller.OnJump += OnJump;
    }

    private void OnJump(object sender, EventArgs args)
    {
        _animtor.SetTrigger("Jump");
    }

    private void LateUpdate()
    {
        _spriteRenderer.flipX = _controller.IsLookingLeft;
        _animtor.SetFloat("MoveSpeed", _controller.MoveSpeed);
        _animtor.SetBool("IsFalling", _controller.IsFalling);
    }
}
