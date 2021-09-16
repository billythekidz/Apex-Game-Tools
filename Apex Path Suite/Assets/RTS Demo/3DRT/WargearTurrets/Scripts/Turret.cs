using UnityEngine;

public class Turret : MonoBehaviour
{
    public Transform target;
    public Transform pivot;
    public Transform yaw;
    public Transform[] pitch;
    public bool enablePitch = true;
    public bool enableYaw = true;

    public float yawSpeed = 10;
    public float pitchSpeed = 10;

    public float dampSpeed = 0.1f;

    private float yawVelocity;
    private float pitchVelocity;

    //private void Reset()
    //{
    //    if (transform.childCount == 1)
    //    {
    //        this.pivot = transform.GetChild(0);
    //        if (pivot.childCount == 1)
    //        {
    //            this.yaw = pivot.GetChild(0);
    //            var pitch = new List<Transform>();

    //            foreach (Transform p in yaw)
    //            {
    //                pitch.Add(p);
    //            }

    //            this.pitch = pitch.ToArray();
    //        }
    //    }
    //}

    //private bool AngleInRange(float A, float D)
    //{
    //    if (A < (360 - D) && A > 180)
    //    {
    //        return false;
    //    }
    //    if (A > D && A < 180)
    //    {
    //        return false;
    //    }
    //    return true;
    //}

    //private bool AngleBetween(float angle, float A, float B)
    //{
    //    angle = (360 + angle % 360) % 360;
    //    A = (360 + A % 360) % 360;
    //    B = (360 + B % 360) % 360;
    //    if (A < B)
    //        return A <= angle && angle <= B;
    //    return A <= angle || angle <= B;
    //}

    private void Update()
    {
        if (target == null || yaw == null || pitch == null)
        {
            return;
        }

        if (enableYaw)
        {
            var LE = Quaternion.LookRotation(target.position - yaw.position).eulerAngles;
            var E = yaw.localEulerAngles;
            E.z = Mathf.SmoothDampAngle(E.z, LE.y, ref yawVelocity, dampSpeed, yawSpeed);
            yaw.localEulerAngles = E;
        }

        if (enablePitch && pitch.Length > 0)
        {
            var LE = Quaternion.LookRotation(target.position - pitch[0].position).eulerAngles;
            var PE = pitch[0].localEulerAngles;
            PE.x = Mathf.SmoothDampAngle(PE.x, LE.x, ref pitchVelocity, dampSpeed, pitchSpeed);
            foreach (var p in this.pitch)
            {
                p.localEulerAngles = PE;
            }
        }
    }
}