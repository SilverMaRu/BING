using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MaxAttributesChangeDelegate(float currentValue);
public delegate void CurrentAttributesChangeDelegate(float currentValue, float maxValue);
public delegate void PlayerFactionChangeDelegate(PlayerFaction currentFaction);
public class AttributesManager : MonoBehaviour
{
    private static List<AttributesManager> _allAttrManager = new List<AttributesManager>();
    public static AttributesManager[] allAttrManager { get { return _allAttrManager.ToArray(); } }
    public event MaxAttributesChangeDelegate MaxSPChangeEvent;
    public event CurrentAttributesChangeDelegate CurrentSPChangeEvent;
    public event PlayerFactionChangeDelegate PlayerFactionChangeEvent;

    public Attributes peopleAttributes;
    public Attributes ghostAttributes;
    private PlayerFaction _faction = PlayerFaction.People;
    public PlayerFaction faction
    {
        get
        {
            return _faction;
        }
        set
        {
            _faction = value;
            PlayerFactionChangeEvent?.Invoke(_faction);
        }
    }
    private Dictionary<long, float> receiptReviseMaxSPPairs = new Dictionary<long, float>();
    public float baseMaxSP
    {
        get
        {
            float resultValue = 0;
            switch (faction)
            {
                case PlayerFaction.People:
                    resultValue = peopleAttributes.maxSP;
                    break;
                case PlayerFaction.Ghost:
                    resultValue = ghostAttributes.maxSP;
                    break;
            }
            return resultValue;
        }
    }
    public float maxSP
    {
        get
        {
            float finalMaxSP = baseMaxSP;
            float[] reviseMaxSPArray = new float[receiptReviseMaxSPPairs.Count];
            receiptReviseMaxSPPairs.Values.CopyTo(reviseMaxSPArray, 0);
            foreach (float tempRevise in reviseMaxSPArray)
            {
                finalMaxSP += tempRevise;
            }
            return Mathf.Max(0, finalMaxSP);
        }
    }
    private float _currentSP;
    public float currentSP
    {
        get { return Mathf.Min(_currentSP, maxSP); }
        set
        {
            _currentSP = Mathf.Clamp(value, 0, maxSP);
            CurrentSPChangeEvent?.Invoke(_currentSP, maxSP);
        }
    }
    private Dictionary<long, float> receiptReviseRecoverSPPairs = new Dictionary<long, float>();
    public float baseRecoverSP
    {
        get
        {
            float resultValue = 0;
            switch (faction)
            {
                case PlayerFaction.People:
                    resultValue = peopleAttributes.recoverSP;
                    break;
                case PlayerFaction.Ghost:
                    resultValue = ghostAttributes.recoverSP;
                    break;
            }
            return resultValue;
        }
    }
    public float recoverSP
    {
        get
        {
            float finalRecoverSP = baseRecoverSP;
            float[] reviseRecoverSPArray = new float[receiptReviseRecoverSPPairs.Count];
            receiptReviseRecoverSPPairs.Values.CopyTo(reviseRecoverSPArray, 0);
            foreach (float tempRevise in reviseRecoverSPArray)
            {
                finalRecoverSP += tempRevise;
            }
            return finalRecoverSP;
        }
    }
    private Dictionary<long, float> receiptReviseTimeScalePairs = new Dictionary<long, float>();
    public float baseTimeScale
    {
        get
        {
            float resultValue = 0;
            switch (faction)
            {
                case PlayerFaction.People:
                    resultValue = peopleAttributes.timeScale;
                    break;
                case PlayerFaction.Ghost:
                    resultValue = ghostAttributes.timeScale;
                    break;
            }
            return resultValue;
        }
    }
    public float timeScale
    {
        get
        {
            float finalTimeScale = baseTimeScale;
            float[] reviseTimeScaleArray = new float[receiptReviseTimeScalePairs.Count];
            receiptReviseTimeScalePairs.Values.CopyTo(reviseTimeScaleArray, 0);
            foreach (float tempRevise in reviseTimeScaleArray)
            {
                finalTimeScale += tempRevise;
            }
            return Mathf.Max(0, finalTimeScale);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //peopleAttributes = ghostAttributes.Clone();
        currentSP = maxSP;
    }

    // Update is called once per frame
    void Update()
    {
        currentSP += recoverSP * Time.deltaTime * timeScale;
    }

    public long AddRevise(ReviseField reviseField, float reviseValue, ReviseType reviseMode, ComputeMode computeMode)
    {
        long receipt = -1;
        switch (reviseField)
        {
            case ReviseField.MaxSP:
                receipt = AddReviseMaxSP(reviseValue, reviseMode);
                break;
            case ReviseField.CurrentSP:
                ReviseCurrentSP(reviseValue, reviseMode, computeMode);
                break;
            case ReviseField.RecoverSP:
                receipt = AddReviseRecoverSP(reviseValue, reviseMode);
                break;
            case ReviseField.TimeScale:
                receipt = AddReviseTimeScale(reviseValue, reviseMode);
                break;
        }

        return receipt;
    }

    public long AddRevise(ReviseInfo info)
    {
        return AddRevise(info.reviseField, info.reviseValue, info.reviseMode, info.computeMode);
    }

    public void RemoveRevise(ReviseField reviseField, long receipt)
    {
        switch (reviseField)
        {
            case ReviseField.MaxSP:
                RemoveReviseMaxSP(receipt);
                break;
            case ReviseField.RecoverSP:
                RemoveReviseRecoverSP(receipt);
                break;
            case ReviseField.TimeScale:
                RemoveReviseTimeScale(receipt);
                break;
        }
    }

    public void RemoveAllRevise()
    {
        receiptReviseMaxSPPairs.Clear();
        receiptReviseRecoverSPPairs.Clear();
        receiptReviseTimeScalePairs.Clear();
    }

    public long AddReviseMaxSP(float reviseValue, ReviseType reviseMode)
    {
        long receipt = System.DateTime.Now.ToBinary();
        float revise = reviseValue;
        switch (reviseMode)
        {
            case ReviseType.PercentBase:
                revise = baseMaxSP * reviseValue;
                break;
            case ReviseType.PercentCurrent:
                revise = maxSP * reviseValue;
                break;
        }
        receiptReviseMaxSPPairs.Add(receipt, revise);
        MaxSPChangeEvent?.Invoke(maxSP);
        return receipt;
    }

    public void RemoveReviseMaxSP(long receipt)
    {
        if (receiptReviseMaxSPPairs.ContainsKey(receipt))
        {
            receiptReviseMaxSPPairs.Remove(receipt);
            MaxSPChangeEvent?.Invoke(maxSP);
        }
    }

    public void ReviseCurrentSP(float reviseValue, ReviseType reviseMode, ComputeMode computeMode)
    {
        float revise = reviseValue;
        switch (reviseMode)
        {
            case ReviseType.PercentBase:
                revise = maxSP * reviseValue;
                break;
            case ReviseType.PercentCurrent:
                revise = currentSP * reviseValue;
                break;
            case ReviseType.PercentUsed:
                revise = (maxSP - currentSP) * reviseValue;
                break;
        }
        switch (computeMode)
        {
            case ComputeMode.Add:
                currentSP += revise;
                break;
            case ComputeMode.Set:
                if (revise <= 0)
                {
                    currentSP = Mathf.Min(currentSP, -revise);
                }
                else
                {
                    currentSP = Mathf.Max(currentSP, revise);
                }
                break;
        }
    }

    public long AddReviseRecoverSP(float reviseValue, ReviseType reviseMode)
    {
        long receipt = System.DateTime.Now.ToBinary();
        float revise = reviseValue;
        switch (reviseMode)
        {
            case ReviseType.PercentBase:
                revise = baseRecoverSP * reviseValue;
                break;
            case ReviseType.PercentCurrent:
                revise = recoverSP * reviseValue;
                break;
        }
        receiptReviseRecoverSPPairs.Add(receipt, revise);
        return receipt;
    }

    public void RemoveReviseRecoverSP(long receipt)
    {
        if (receiptReviseRecoverSPPairs.ContainsKey(receipt)) receiptReviseRecoverSPPairs.Remove(receipt);
    }

    public long AddReviseTimeScale(float reviseValue, ReviseType reviseMode)
    {
        long receipt = System.DateTime.Now.ToBinary();
        float revise = reviseValue;
        switch (reviseMode)
        {
            case ReviseType.PercentBase:
                revise = baseTimeScale * reviseValue;
                break;
            case ReviseType.PercentCurrent:
                revise = timeScale * reviseValue;
                break;
        }
        receiptReviseTimeScalePairs.Add(receipt, revise);
        return receipt;
    }

    public void RemoveReviseTimeScale(long receipt)
    {
        if (receiptReviseTimeScalePairs.ContainsKey(receipt)) receiptReviseTimeScalePairs.Remove(receipt);
    }

    public static AttributesManager[] GetFactionAllAttrManager(PlayerFaction faction)
    {
        List<AttributesManager> factionAttrManager = new List<AttributesManager>();
        foreach (AttributesManager tempAttrManager in allAttrManager)
        {
            if (tempAttrManager.faction == faction)
            {
                factionAttrManager.Add(tempAttrManager);
            }
        }
        return factionAttrManager.ToArray();
    }

    public static PlayerFaction GetEnemyFaction(PlayerFaction myFaction)
    {
        PlayerFaction enemyFaction = PlayerFaction.Ghost;
        switch (myFaction)
        {
            case PlayerFaction.People:
                enemyFaction = PlayerFaction.Ghost;
                break;
            case PlayerFaction.Ghost:
                enemyFaction = PlayerFaction.People;
                break;
        }
        return enemyFaction;
    }

    public static AttributesManager FindNearbyPlayer(PlayerFaction targetFaction, Vector3 center, float radius)
    {
        AttributesManager resultAttrManager = null;
        AttributesManager[] teammateAttrManager = GetFactionAllAttrManager(targetFaction);
        bool onePeopleFaction = teammateAttrManager.Length <= 1;
        float currentSqrRadius = Mathf.Sqrt(radius);
        foreach (AttributesManager tempTeammate in teammateAttrManager)
        {
            Vector3 centerToTeammate = tempTeammate.transform.position - center;
            float sqrMagnitude = centerToTeammate.sqrMagnitude;
            if (!onePeopleFaction && sqrMagnitude != 0 && sqrMagnitude <= currentSqrRadius)
            {
                resultAttrManager = tempTeammate;
                currentSqrRadius = sqrMagnitude;
            }
            else if (onePeopleFaction)
            {
                resultAttrManager = tempTeammate;
            }
        }
        return resultAttrManager;
    }

    public static AttributesManager[] FindRangePlayer(PlayerFaction targetFaction, Vector3 center, float radius)
    {
        List<AttributesManager> rangeTeammateList = new List<AttributesManager>();
        AttributesManager[] teammateAttrManager = AttributesManager.GetFactionAllAttrManager(targetFaction);
        float sqrRadius = Mathf.Sqrt(radius);
        foreach (AttributesManager tempTeammate in teammateAttrManager)
        {
            Vector3 centerToTeammate = tempTeammate.transform.position - center;
            float sqrMagnitude = centerToTeammate.sqrMagnitude;
            if (sqrMagnitude <= sqrRadius) rangeTeammateList.Add(tempTeammate);
        }
        return rangeTeammateList.ToArray();
    }
}

[System.Serializable]
public class ReviseInfo
{
    public ReviseField reviseField = ReviseField.CurrentSP;
    public float reviseValue = 0;
    public ReviseType reviseMode = ReviseType.Normal;
    public ComputeMode computeMode = ComputeMode.Add;
}

public enum ReviseType
{
    Normal,
    PercentBase,
    PercentCurrent,
    PercentUsed
}

public enum ComputeMode
{
    Add,
    Set
}

public enum ReviseField
{
    MaxSP,
    CurrentSP,
    RecoverSP,
    TimeScale
}

public enum PlayerFaction
{
    People,
    Ghost
}
