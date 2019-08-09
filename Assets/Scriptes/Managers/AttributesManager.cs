using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MaxAttributesChangeDelegate(float currentValue);
public delegate void CurrentAttributesChangeDelegate(float currentValue, float maxValue);
public class AttributesManager : MonoBehaviour
{
    public event MaxAttributesChangeDelegate MaxSPChangeEvent;
    public event CurrentAttributesChangeDelegate CurrentSPChangeEvent;

    public Attributes useAttributes;
    public PlayerFaction faction = PlayerFaction.People;
    private Attributes attr;
    private List<float> reviseMaxSPList = new List<float>();
    private Dictionary<long, float> receiptReviseMaxSPPairs = new Dictionary<long, float>();
    public float maxSP
    {
        get
        {
            float finalMaxSP = attr.maxSP;
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
    private List<float> reviseRecoverSPList = new List<float>();
    private Dictionary<long, float> receiptReviseRecoverSPPairs = new Dictionary<long, float>();
    public float recoverSP
    {
        get
        {
            float finalRecoverSP = attr.recoverSP;
            float[] reviseRecoverSPArray = new float[receiptReviseRecoverSPPairs.Count];
            receiptReviseRecoverSPPairs.Values.CopyTo(reviseRecoverSPArray, 0);
            foreach (float tempRevise in reviseRecoverSPArray)
            {
                finalRecoverSP += tempRevise;
            }
            return finalRecoverSP;
        }
    }
    private List<float> reviseTimeScaleList = new List<float>();
    private Dictionary<long, float> receiptReviseTimeScalePairs = new Dictionary<long, float>();
    public float timeScale
    {
        get
        {
            float finalTimeScale = attr.timeScale;
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
        attr = useAttributes.Clone();
        currentSP = attr.maxSP;
    }

    // Update is called once per frame
    void Update()
    {
        currentSP += recoverSP * Time.deltaTime * timeScale;
    }

    public long AddRevise(ReviseField reviseField, float reviseValue, ReviseMode reviseMode)
    {
        long receipt = -1;
        switch (reviseField)
        {
            case ReviseField.MaxSP:
                receipt = AddReviseMaxSP(reviseValue, reviseMode);
                break;
            case ReviseField.CurrentSP:
                ReviseCurrentSP(reviseValue, reviseMode);
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

    public long AddReviseMaxSP(float reviseValue, ReviseMode reviseMode)
    {
        long receipt = System.DateTime.Now.ToBinary();
        float revise = reviseValue;
        switch (reviseMode)
        {
            case ReviseMode.PERCENT_BASE:
                revise = attr.maxSP * reviseValue;
                break;
            case ReviseMode.PERCENT_CURRENT:
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

    public void ReviseCurrentSP(float reviseValue, ReviseMode reviseMode)
    {
        float revise = reviseValue;
        switch (reviseMode)
        {
            case ReviseMode.PERCENT_BASE:
                revise = maxSP * reviseValue;
                break;
            case ReviseMode.PERCENT_CURRENT:
                revise = currentSP * reviseValue;
                break;
        }
        currentSP += revise;
    }

    public long AddReviseRecoverSP(float reviseValue, ReviseMode reviseMode)
    {
        long receipt = System.DateTime.Now.ToBinary();
        float revise = reviseValue;
        switch (reviseMode)
        {
            case ReviseMode.PERCENT_BASE:
                revise = attr.recoverSP * reviseValue;
                break;
            case ReviseMode.PERCENT_CURRENT:
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

    public long AddReviseTimeScale(float reviseValue, ReviseMode reviseMode)
    {
        long receipt = System.DateTime.Now.ToBinary();
        float revise = reviseValue;
        switch (reviseMode)
        {
            case ReviseMode.PERCENT_BASE:
                revise = attr.timeScale * reviseValue;
                break;
            case ReviseMode.PERCENT_CURRENT:
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
}

public enum ReviseMode
{
    NORMAL,
    PERCENT_BASE,
    PERCENT_CURRENT
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
