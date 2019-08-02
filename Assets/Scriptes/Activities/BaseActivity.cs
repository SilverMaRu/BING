using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ActivityBaseDelegate(BaseActivity sender, ActivityEventDate eventDate);

public class BaseActivity
{
    public static event ActivityBaseDelegate EnterActivityEvent;
    public static event ActivityBaseDelegate ExitActivityEvent;
    protected GameObject ownerGO;
    protected BaseActivityInfo activityInfo;
    protected ActivityManager activityManager { get { return ownerGO.GetComponent<ActivityManager>(); } }
    protected Animator animator { get { return ownerGO.GetComponent<Animator>(); } }

    public BaseActivity(GameObject ownerGO)
    {
        this.ownerGO = ownerGO;
        activityInfo = FindActivityInfo();
    }
    public BaseActivity(GameObject ownerGO, BaseActivityInfo activityInfo)
    {
        this.ownerGO = ownerGO;
        this.activityInfo = activityInfo;
    }

    public virtual bool MeetEnterCondition()
    {
        return false;
    }

    public virtual void EnterActivity()
    {
        Debug.Log("Enter " + GetType().Name);
        //#region 离开当前行为
        //BaseActivity[] currentActivitys = null;
        //if (activityManager.TryGetCurrentActivity(out currentActivitys)) currentActivitys.ExitActivity();
        //#endregion 离开当前行为
        #region 如果动画机参数类型为Trigger/Bool, 则触发/设为true
        switch (activityInfo.animatorParamType)
        {
            case BaseActivityInfo.ParamType.Bool:
            case BaseActivityInfo.ParamType.Trigger:
                SetAnimatorParam(1);
                break;
        }
        #endregion 如果动画机参数类型为Trigger/Bool, 则触发/设为true
        #region 把该行为添加到为当前行为列表
        activityManager.EnterActivity(this);
        #endregion 把该行为添加到为当前行为列表
        #region 发送事件
        if (EnterActivityEvent != null)
        {
            ActivityEventDate eventDate = new ActivityEventDate(ownerGO);
            EnterActivityEvent(this, eventDate);
        }
        #endregion 发送事件
    }

    public virtual void Update() {
        // 如果达到了离开条件, 执行离开
        if (MeetExitCondition()) ExitActivity();
    }

    public virtual bool MeetExitCondition()
    {
        return true;
    }

    public virtual void ExitActivity()
    {
        Debug.Log("Exit " + GetType().Name);
        #region 如果动画机参数类型为Bool, 则设置为false
        if (activityInfo.animatorParamType == BaseActivityInfo.ParamType.Bool) SetAnimatorParam(0);
        #endregion 如果动画机参数类型为Bool, 则设置为false
        #region 把该行为从当前行为列表中移除
        activityManager.ExitActivity(this);
        #endregion 把该行为从当前行为列表中移除
        #region 发送事件
        if (EnterActivityEvent != null)
        {
            ActivityEventDate eventDate = new ActivityEventDate(ownerGO);
            ExitActivityEvent(this, eventDate);
        }
        #endregion 发送事件
    }

    /// <summary>
    /// 从项目中查找并返回行为信息
    /// </summary>
    /// <returns></returns>
    protected virtual BaseActivityInfo FindActivityInfo()
    {
        return null;
    }

    /// <summary>
    /// 设置动画机参数
    /// 参数类型为Bool时, value 大于 0 为true; value 小于等于 0 为false
    /// 参数类型为Int时, 把value强转为Int类型
    /// </summary>
    /// <param name="paramType">动画机参数类型</param>
    /// <param name="value">动画机参数值</param>
    /// <param name="dampTime"></param>
    /// <param name="deltaTime"></param>
    protected virtual void SetAnimatorParam(float value = 0, float dampTime = 0, float deltaTime = 0)
    {
        string paramName = activityInfo.animatorParamName;
        switch (activityInfo.animatorParamType)
        {
            case BaseActivityInfo.ParamType.Int:
                animator.SetInteger(paramName, (int)value);
                break;
            case BaseActivityInfo.ParamType.Float:
                animator.SetFloat(paramName, value, dampTime, deltaTime);
                break;
            case BaseActivityInfo.ParamType.Bool:
                animator.SetBool(paramName, value > 0);
                break;
            case BaseActivityInfo.ParamType.Trigger:
                animator.SetTrigger(paramName);
                break;
        }
    }
}

public class BaseActivityInfo
{
    public enum ParamType { Int, Float, Bool, Trigger }
    public string animatorParamName;
    public ParamType animatorParamType = ParamType.Float;

    public BaseActivityInfo() { }
    public BaseActivityInfo(string animatorParamName, ParamType animatorParamType)
    {
        this.animatorParamName = animatorParamName;
        this.animatorParamType = animatorParamType;
    }
}

public class ActivityEventDate
{
    public GameObject ownerGO;

    public ActivityEventDate(GameObject ownerGO)
    {
        this.ownerGO = ownerGO;
    }
}
