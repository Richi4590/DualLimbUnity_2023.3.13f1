using DitzelGames.FastIK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurableJointManager : MonoBehaviour
{
    [SerializeField] private Transform jointRoot;
    [SerializeField] private ConfigurableJoint spine;
    [SerializeField] public List<ConfigurableJoint> additionalJoints;
    [SerializeField] public float massWhenRagdolling;
    [SerializeField] private List<FastIKFabric> kinematicArmScripts;

    private bool isRagdolling = false;

    //                                                  Original XDrive, YDrive, SlerpDrive, original mass
    private Dictionary<GameObject, (ConfigurableJoint, JointDrive, JointDrive, JointDrive, float)> jointMap = new Dictionary<GameObject, (ConfigurableJoint, JointDrive, JointDrive, JointDrive, float)>();
    private Dictionary<GameObject, Transform> defaultJointTransformMap = new Dictionary<GameObject, Transform>();

    private void Awake()
    {
        if (jointRoot == null)
            GameObject.Find("metarig");

        List<ConfigurableJoint> jointList = Utilities.getListOfComponentsRecursiveGeneric<ConfigurableJoint>(jointRoot);
        jointList.AddRange(additionalJoints);

        foreach (ConfigurableJoint joint in jointList)
        {
            jointMap.Add(joint.gameObject, (joint, joint.angularXDrive, joint.angularYZDrive, joint.slerpDrive, joint.GetComponent<Rigidbody>().mass));
            defaultJointTransformMap.Add(joint.gameObject, joint.transform);
        }
    }
    public void RotateJointsTowardsTargetQuaternion(Quaternion targetQuaternion)
    {
        spine.targetRotation = targetQuaternion;
    }

    public void DoRagdollDummyPlayer(bool doRagdoll)
    {
        if (doRagdoll)
        {
            foreach (var jointEntry in jointMap)
            {
                JointDrive XDrive = jointEntry.Value.Item2;
                JointDrive YZDrive = jointEntry.Value.Item3;
                XDrive.positionSpring = 0;
                YZDrive.positionSpring = 0;
                jointEntry.Value.Item1.angularXDrive = XDrive;
                jointEntry.Value.Item1.angularYZDrive = YZDrive;
                jointEntry.Value.Item1.GetComponent<Rigidbody>().mass = massWhenRagdolling;
            }

            isRagdolling = true;
        }
        else
        {
            foreach (var jointEntry in jointMap)
            {
                jointEntry.Value.Item1.angularXDrive = jointEntry.Value.Item2;
                jointEntry.Value.Item1.angularYZDrive = jointEntry.Value.Item3;
                jointEntry.Value.Item1.GetComponent<Rigidbody>().mass = jointEntry.Value.Item5;
            }

            isRagdolling = false;
        }
    }

    public void DoRagdoll(bool doRagdoll)
    {
        if (doRagdoll)
        {
            foreach (FastIKFabric ik in kinematicArmScripts)
            {
                ik.shouldCalculateIK = false;
                ik.enabled = true;
            }


            foreach (var jointEntry in jointMap)
            {
                JointDrive XDrive = jointEntry.Value.Item2;
                JointDrive YZDrive = jointEntry.Value.Item3;
                JointDrive SlerpDrive = jointEntry.Value.Item4;
                
                XDrive.positionSpring = 0;
                YZDrive.positionSpring = 0;
                XDrive.positionDamper = 0;
                YZDrive.positionDamper = 0;
                
                jointEntry.Value.Item1.angularXDrive = XDrive;
                jointEntry.Value.Item1.angularYZDrive = YZDrive;

                SlerpDrive.positionSpring = 0;
                SlerpDrive.positionDamper = 0;

                jointEntry.Value.Item1.slerpDrive = SlerpDrive;
                jointEntry.Value.Item1.GetComponent<Rigidbody>().mass = massWhenRagdolling;

                jointEntry.Value.Item1.massScale = jointEntry.Value.Item1.massScale * 10;
            }

            isRagdolling = true;
        }
        else
        {
            foreach (FastIKFabric ik in kinematicArmScripts)
            {
                ik.shouldCalculateIK = true;
                ik.enabled = false;
            }

            foreach (var jointEntry in jointMap)
            {
                jointEntry.Value.Item1.angularXDrive = jointEntry.Value.Item2;
                jointEntry.Value.Item1.angularYZDrive = jointEntry.Value.Item3;
                jointEntry.Value.Item1.slerpDrive = jointEntry.Value.Item4;
                jointEntry.Value.Item1.GetComponent<Rigidbody>().mass = jointEntry.Value.Item5;
                jointEntry.Value.Item1.massScale = jointEntry.Value.Item1.massScale / 10;
            }

            isRagdolling = false;
        }
    }

    public void FreezeBody(bool freezeBody)
    {
        if (freezeBody)
        {
            foreach (var jointEntry in jointMap)
            {
                jointEntry.Value.Item1.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
        else
        {
            foreach (var jointEntry in jointMap)
            {
                jointEntry.Value.Item1.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
    }

    public void ResetToDefaultBodyPosition()
    {
        foreach (var jointTransformEntry in defaultJointTransformMap)
        {
            jointTransformEntry.Key.transform.position = jointTransformEntry.Value.position;
        }
    }

    public void ResetToDefaultBodyRotation()
    {
        foreach (var jointTransformEntry in defaultJointTransformMap)
        {
            jointTransformEntry.Key.transform.rotation = jointTransformEntry.Value.rotation;
        }
    }

    public void ResetToDefaultBodyPositionAndRotation()
    {
        foreach (var jointTransformEntry in defaultJointTransformMap)
        {
            jointTransformEntry.Key.transform.position = jointTransformEntry.Value.position;
            jointTransformEntry.Key.transform.rotation = jointTransformEntry.Value.rotation;
        }
    }




}