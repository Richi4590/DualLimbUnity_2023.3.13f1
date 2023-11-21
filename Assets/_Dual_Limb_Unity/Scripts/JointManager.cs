using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigurableJointManager : MonoBehaviour
{
    [SerializeField] private Transform jointRoot;
    [SerializeField] public List<ConfigurableJoint> additionalJoints;
    private bool isRagdolling = false;

    //                    Original XDrive, YDrive,              XMotion,                    YMotion,                ZMotion
    private Dictionary<GameObject, (ConfigurableJoint, JointDrive, JointDrive, ConfigurableJointMotion, ConfigurableJointMotion, ConfigurableJointMotion)> jointMap = new Dictionary<GameObject, (ConfigurableJoint, JointDrive, JointDrive, ConfigurableJointMotion, ConfigurableJointMotion, ConfigurableJointMotion)>();
    private Dictionary<GameObject, Transform> defaultJointTransformMap = new Dictionary<GameObject, Transform>();

    private void Awake()
    {
        if (jointRoot == null)
            GameObject.Find("metarig");

        List<ConfigurableJoint> jointList = Utilities.getListOfComponentsRecursiveGeneric<ConfigurableJoint>(jointRoot);
        jointList.AddRange(additionalJoints);

        foreach (ConfigurableJoint joint in jointList)
        {
            jointMap.Add(joint.gameObject, (joint, joint.angularXDrive, joint.angularYZDrive, joint.xMotion, joint.yMotion, joint.zMotion));
            defaultJointTransformMap.Add(joint.gameObject, joint.transform);
        }
    }

    /*
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            isRagdolling = !isRagdolling;
            DoRagdoll(isRagdolling);
        }

    }
    */

    public void DoRagdoll(bool doRagdoll)
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
                jointEntry.Value.Item1.xMotion = ConfigurableJointMotion.Free;
                jointEntry.Value.Item1.yMotion = ConfigurableJointMotion.Free;
                jointEntry.Value.Item1.zMotion = ConfigurableJointMotion.Free;
            }

            isRagdolling = true;
        }
        else
        {
            foreach (var jointEntry in jointMap)
            {
                jointEntry.Value.Item1.angularXDrive = jointEntry.Value.Item2;
                jointEntry.Value.Item1.angularYZDrive = jointEntry.Value.Item3;
                //jointEntry.Value.Item1.xMotion = jointEntry.Value.Item4;
                //jointEntry.Value.Item1.yMotion = jointEntry.Value.Item5;
                //jointEntry.Value.Item1.zMotion = jointEntry.Value.Item6;
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
