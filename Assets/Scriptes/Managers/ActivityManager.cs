using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ActivityManager : MonoBehaviour
{
    private readonly Type typeOfGameObject = typeof(GameObject);
    private readonly Type[] typeOfOwnActivities = new Type[] { typeof(ActivityMove)
        , typeof(ActivityTurn)
    };

    private BaseActivityInfo[] ownActiviyInfos = new BaseActivityInfo[] { new BaseActivityInfo("Forward", BaseActivityInfo.ParamType.Float)
    , new BaseActivityInfo("Turn", BaseActivityInfo.ParamType.Float)
    };
    private BaseActivity[] ownActivities = new BaseActivity[0];
    private List<BaseActivity> listenActivityList = new List<BaseActivity>();
    public BaseActivity[] listenActivities { get { return listenActivityList.ToArray(); } }
    private List<BaseActivity> currentActivityList = new List<BaseActivity>(1);
    public BaseActivity[] currentActivities { get { return currentActivityList.ToArray(); } }
    public bool hasActivity { get { return currentActivityList.Count > 0; } }
    private GameObject rootGO;

    // Start is called before the first frame update
    void Start()
    {
        rootGO = gameObject;
        int ownActivityCount = typeOfOwnActivities.Length;
        ownActivities = new BaseActivity[ownActivityCount];
        for (int i = 0; i < ownActivityCount; i++)
        {
            ownActivities[i] = CreateActivity(typeOfOwnActivities[i], ownActiviyInfos[i]);
        }
        listenActivityList.AddRange(ownActivities);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (BaseActivity tempActivity in listenActivities)
        {
            if (tempActivity.MeetEnterCondition()) tempActivity.EnterActivity();
        }
        foreach (BaseActivity tempActivity in currentActivities)
        {
            tempActivity.Update();
        }
    }

    private BaseActivity CreateActivity(Type activityType, BaseActivityInfo activityInfo)
    {
        Type[] cstrTypes = new Type[] { typeOfGameObject, activityInfo.GetType() };
        object[] cstrValues = new object[] { rootGO, activityInfo };
        BaseActivity resultActivity = (BaseActivity)activityType.GetConstructor(cstrTypes).Invoke(cstrValues);
        return resultActivity;
    }

    public void EnterActivity(BaseActivity enterActivity)
    {
        listenActivityList.Remove(enterActivity);
        if (!currentActivityList.Contains(enterActivity)) currentActivityList.Add(enterActivity);
    }

    public void ExitActivity(BaseActivity exitActivity)
    {
        if (currentActivityList.Contains(exitActivity)) currentActivityList.Remove(exitActivity);
        listenActivityList.Add(exitActivity);
    }
}
