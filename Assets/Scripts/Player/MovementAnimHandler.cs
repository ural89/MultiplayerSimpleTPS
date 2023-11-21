using System;
using System.Collections;
using System.Collections.Generic;
using Fusion.KCC;
using UnityEngine;

public class MovementAnimHandler : MonoBehaviour
{
    [SerializeField] private float animationMultiplier = 1f;
    [SerializeField] private Animator anim;
    private Action<float> onStep;
    private Health health;
    private const float MIN_MAGNITUDE_TO_MOVE = 0.01f;
    private KCC kcc;
    private void Awake()
    {
        kcc = GetComponent<KCC>();
        health = GetComponent<Health>();

    }
    private void Start()
    {
        health.OnDie += Health_OnDie;
    }

    private void Health_OnDie()
    {
        anim.SetBool("IsDead", true);
    }

    private void Update()
    {
        if (kcc != null)
        {
            UpdateAnimation(kcc.Data.RealVelocity);

        }
    }
    public void UpdateAnimation(Vector3 moveDelta)
    {

        if (moveDelta.magnitude < MIN_MAGNITUDE_TO_MOVE)
            moveDelta = Vector3.zero;
        var velX = transform.InverseTransformDirection(moveDelta).x;
        var velZ = transform.InverseTransformDirection(moveDelta).z;
        anim.SetFloat("VelocityX", velX * animationMultiplier);
        anim.SetFloat("VelocityZ", velZ * animationMultiplier);
        //        Debug.Log(moveDelta.magnitude);

    }

    public void AnimationEvent_OnStep(float stepIntencity)
    {
        onStep?.Invoke(stepIntencity);
    }

    public void AddListener(Action<float> onStep)
    {
        this.onStep += onStep;
    }

    public void RemoveListener(Action<float> onStep)
    {
        this.onStep -= onStep;
    }
}
