using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class TankAnimator : MonoBehaviour
{
    public Transform tankModel;

    public float groundRayLength = 1f;
    public float groundedOffset;

    public float rotationSpeed = 1;

    [Header("Auto params")]
    public float groundedRadius;

    public LayerMask groundLayers;

    private Quaternion _targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        var thirdPersonController = GetComponent<ThirdPersonController>();
        // groundedOffset = thirdPersonController.GroundedOffset;
        groundedRadius = thirdPersonController.GroundedRadius;
        groundLayers = thirdPersonController.GroundLayers;
    }

    // Update is called once per frame
    void Update()
    {
        tankModel.rotation = Quaternion.Slerp(tankModel.rotation, _targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        // set sphere position, with offset
        var spherePosition = new Vector3(
            transform.position.x,
            transform.position.y - groundedOffset,
            transform.position.z);
        var hasHit = Physics.SphereCast(
            spherePosition, groundedRadius, -transform.up, out var hitInfo, groundRayLength, groundLayers);

        if (hasHit)
        {
            // Debug.Log($"Hit: {hitInfo.collider.name}");
            var forward = Vector3.ProjectOnPlane(transform.forward, hitInfo.normal);
            _targetRotation = Quaternion.LookRotation(forward, hitInfo.normal);
        }
        else
        {
            // Debug.Log($"hasHit {hasHit}");
            _targetRotation = transform.rotation;
            // enable ragdoll
        }
    }
}