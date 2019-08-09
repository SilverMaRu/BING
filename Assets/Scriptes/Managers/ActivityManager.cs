using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActivityManager : MonoBehaviour
{
    private readonly Type typeOfGameObject = typeof(GameObject);

    public ActivityBaseInfo[] ownActiviyInfos;
    private ActivityBase[] ownActivities = new ActivityBase[0];

    private List<ActivityBase> listenActivityList = new List<ActivityBase>();
    public ActivityBase[] listenActivities { get { return listenActivityList.ToArray(); } }

    private List<ActivityBase> currentActivityList = new List<ActivityBase>(1);
    public ActivityBase[] currentActivities { get { return currentActivityList.ToArray(); } }

    public bool loopingActivity { get { return currentActivityList.Count > 0; } }

    public bool isPunishing { get; set; }

    private GameObject rootGO;

    // Start is called before the first frame update
    void Start()
    {
        rootGO = gameObject;
        int ownActivityCount = ownActiviyInfos.Length;
        ownActivities = new ActivityBase[ownActivityCount];
        for (int i = 0; i < ownActivityCount; i++)
        {
            ownActivities[i] = CreateActivity(TypeHelper.GetType(ownActiviyInfos[i].activityTypeName), ownActiviyInfos[i]);
        }
        listenActivityList.AddRange(ownActivities);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (ActivityBase tempActivity in listenActivities)
        {
            if (tempActivity.MeetEnterCondition()) tempActivity.EnterActivity();
        }
        foreach (ActivityBase tempActivity in currentActivities)
        {
            tempActivity.Update();
        }
        //for(int i = 0; i < listenActivities.Length; i++)
        //{
        //    if (listenActivities[i].MeetEnterCondition()) listenActivities[i].EnterActivity();
        //}
        //for (int i = 0; i < currentActivities.Length; i++)
        //{
        //    currentActivities[i].Update();
        //}
    }

    private ActivityBase CreateActivity(Type activityType, ActivityBaseInfo activityInfo)
    {
        Type[] cstrTypes = new Type[] { typeOfGameObject, activityInfo.GetType() };
        object[] cstrValues = new object[] { rootGO, activityInfo };
        ActivityBase resultActivity = (ActivityBase)activityType.GetConstructor(cstrTypes).Invoke(cstrValues);
        return resultActivity;
    }

    public void EnterActivity(ActivityBase enterActivity)
    {
        listenActivityList.Remove(enterActivity);
        if (!currentActivityList.Contains(enterActivity)) currentActivityList.Add(enterActivity);
    }

    public void ExitActivity(ActivityBase exitActivity)
    {
        if (currentActivityList.Contains(exitActivity)) currentActivityList.Remove(exitActivity);
        listenActivityList.Add(exitActivity);
    }
}
