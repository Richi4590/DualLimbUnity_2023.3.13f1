using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionRagdollPlayer : MonoBehaviour
{
    [SerializeField] ConfigurableJointManager jointManager;

    private void Start()
    {
        jointManager = GameObject.FindObjectOfType<ConfigurableJointManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            jointManager.DoRagdoll(true);
        }

    }
}
