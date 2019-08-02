using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityMove : BaseActivity
{
    private Vector3 inputDirection
    {
        get
        {
            Vector3 result = Input.GetAxis("V") * CameraCtrl.rigTrans.forward + Input.GetAxis("H") * CameraCtrl.cameraTrans.right;
            result.Normalize();
            return result;
        }
    }
    private Coroutine smoothStop;
    private const float AXIS_ABS_MIN = 0;
    private const float AXIS_ABS_MAX = 0.5f;

    public ActivityMove(GameObject ownerGO) : base(ownerGO)
    {
    }

    public ActivityMove(GameObject ownerGO, BaseActivityInfo activityInfo) : base(ownerGO, activityInfo)
    {
    }

    public override bool MeetEnterCondition()
    {
        Vector3 inputDir = inputDirection;
        return inputDir != Vector3.zero
            && Vector3.Dot(inputDir, ownerGO.transform.forward) >= 0f;
    }

    public override void EnterActivity()
    {
        base.EnterActivity();
        if (smoothStop != null) activityManager.StopCoroutine(smoothStop);
    }

    public override void Update()
    {
        float axisAbsMax = AXIS_ABS_MAX;
        if (Input.GetKey(KeyCode.LeftShift)) axisAbsMax = AXIS_ABS_MAX * 2;
        SetAnimatorParam(axisAbsMax, 0.2f, Time.deltaTime);
        base.Update();
    }

    public override bool MeetExitCondition()
    {
        Vector3 inputDir = inputDirection;
        return inputDir == Vector3.zero
            || Vector3.Dot(inputDir, ownerGO.transform.forward) < 0f;
    }

    public override void ExitActivity()
    {
        smoothStop = activityManager.StartCoroutine(SmoothStop());
        base.ExitActivity();
    }

    private IEnumerator SmoothStop()
    {
        float startTime = Time.time;
        while (animator.GetFloat(activityInfo.animatorParamName) > 0)
        {
            SetAnimatorParam(AXIS_ABS_MIN, 0.2f, Time.deltaTime);
            yield return null;
        }
    }
}
