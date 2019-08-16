using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityReachOut : ActivityBase
{
    private AttributesManager attrManager;
    private Transform reachOutIKTran;
    private long reviseReceipt;
    
    private ActivityReachOutInfo reachOutInfo;
    private float reachOutUsedTime = 0;
    private float countermandUsedTime = 0;

    public ActivityReachOut(GameObject ownerGO) : base(ownerGO)
    {
        attrManager = ownerGO.GetComponent<AttributesManager>();
        reachOutInfo = activityInfo as ActivityReachOutInfo;
        reachOutIKTran = ownerGO.transform.Find(reachOutInfo.IKGameObjectName);
        if (reachOutIKTran == null) Debug.LogWarning("Not Found " + reachOutInfo.IKGameObjectName + " On OnwerGO");
    }

    public ActivityReachOut(GameObject ownerGO, ActivityBaseInfo activityInfo) : base(ownerGO, activityInfo)
    {
        attrManager = ownerGO.GetComponent<AttributesManager>();
        reachOutInfo = activityInfo as ActivityReachOutInfo;
        reachOutIKTran = ownerGO.transform.Find(reachOutInfo.IKGameObjectName);
        if (reachOutIKTran == null) Debug.LogWarning("Not Found " + reachOutInfo.IKGameObjectName + " On OnwerGO");
    }

    public override bool MeetEnterCondition()
    {
        return Input.GetKey(KeyCode.Mouse1);
    }

    public override void EnterActivity()
    {
        base.EnterActivity();
        AddRevise();
        reachOutUsedTime = 0;
        activityManager.OnAnimatorIKAction -= Countermand;
        activityManager.OnAnimatorIKAction += ReachOut;
    }

    public override bool MeetExitCondition()
    {
        return !Input.GetKey(KeyCode.Mouse1);
    }

    public override void ExitActivity()
    {
        base.ExitActivity();
        countermandUsedTime = 0;
        activityManager.OnAnimatorIKAction -= ReachOut;
        activityManager.OnAnimatorIKAction += Countermand;
    }

    private void AddRevise()
    {
        reviseReceipt = attrManager.AddRevise(reachOutInfo.reviseInfo);
    }

    private void ReachOut(int layerIndex)
    {
        animator.SetIKPosition(reachOutInfo.effectGoal, reachOutIKTran.position);
        animator.SetIKRotation(reachOutInfo.effectGoal, reachOutIKTran.rotation);
        float weight = reachOutUsedTime / reachOutInfo.reachOutUseTime;
        animator.SetIKPositionWeight(reachOutInfo.effectGoal, weight);
        animator.SetIKRotationWeight(reachOutInfo.effectGoal, weight);
        reachOutUsedTime += Time.deltaTime;
    }

    private void Countermand(int layerIndex)
    {
        float weight = countermandUsedTime / reachOutInfo.reachOutUseTime;
        animator.SetIKPositionWeight(reachOutInfo.effectGoal, weight);
        animator.SetIKRotationWeight(reachOutInfo.effectGoal, weight);
        countermandUsedTime += Time.deltaTime;
    }
}
