using UnityEngine;

class ProcedualAnimation :  MonoBehaviour
{
    public Transform[] FootTarget;
    public Transform LookTarget;

    public Transform FootTargetLeft;
    public Transform FootTargetRight;
    public Transform HandTargetLeft;
    public Transform HandTargetRight;
    public Transform HandPoleLeft;
    public Transform HandPoleRight;
    public Transform Attraction;

    public void LateUpdate()
    {
        //attraction
        //Attraction.Translate(Vector3.forward * Time.deltaTime * 0.5f);
        //if (Attraction.position.z > 1f)
        //Attraction.position = Attraction.position + Vector3.forward * -2f;

        //footsteps
        DoFootCalculation(FootTargetLeft);
        DoFootCalculation(FootTargetRight);

        //hand and look
        var normDist = Mathf.Clamp((Vector3.Distance(LookTarget.position, Attraction.position) - 0.3f) / 1f, 0, 1);
        //HandTargetLeft.rotation = Quaternion.Lerp(Quaternion.Euler(90, 0, 0), HandTargetLeft.rotation, normDist);
        //HandTargetLeft.position = Vector3.Lerp(Attraction.position, HandTargetLeft.position, normDist);
        //HandPoleLeft.position = Vector3.Lerp(HandTargetLeft.position + Vector3.down * 2, HandTargetLeft.position + Vector3.forward * 2f, normDist);

        //HandTargetRight.rotation = Quaternion.Lerp(Quaternion.Euler(90, 0, 0), HandTargetRight.rotation, normDist);
        //HandTargetRight.position = Vector3.Lerp(Attraction.position, HandTargetRight.position, normDist);
        //HandPoleRight.position = Vector3.Lerp(HandTargetRight.position + Vector3.down * 2, HandTargetRight.position + Vector3.forward * 2f, normDist);

        LookTarget.position = Vector3.Lerp(Attraction.position, LookTarget.position, normDist);
    }

    public void DoFootCalculation(Transform foot)
    {
        var ray = new Ray(foot.transform.position + Vector3.up * 0.5f, Vector3.down);
        var hitInfo = new RaycastHit();
        if (Physics.SphereCast(ray, 0.05f, out hitInfo, 0.50f))
            foot.position = hitInfo.point + Vector3.up * 0.05f;
    }
}

