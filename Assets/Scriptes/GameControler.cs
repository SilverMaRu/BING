using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void SwitchFactionDelegate();
public class GameControler : MonoBehaviour
{
    public static GameControler ins { get; private set; }
    public event SwitchFactionDelegate SwitchFactionStartEvent;
    public event SwitchFactionDelegate SwitchFactionCompleteEvent;

    public float switchFactionUseTime = 5;

    private void Awake()
    {
        ins = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void SwitchFaction(GameObject originalGhostGO, GameObject originalPeopleGO)
    {
        SwitchFactionStartEvent?.Invoke();
        StartCoroutine(IEnumeratorHelper.After(() => { SwitchFactionCompleteEvent?.Invoke(); }, switchFactionUseTime));
        AttributesManager originalGhostAttrManager = originalGhostGO.GetComponent<AttributesManager>();
        originalGhostAttrManager.faction = PlayerFaction.People;
        originalGhostAttrManager.RemoveAllRevise();
        originalGhostAttrManager.ReviseCurrentSP(0, ReviseType.Normal, ComputeMode.Set);

        AttributesManager originalPeopleAttrManager = originalPeopleGO.GetComponent<AttributesManager>();
        originalPeopleAttrManager.faction = PlayerFaction.Ghost;
        originalPeopleAttrManager.RemoveAllRevise();
        long reviseReceipt = originalPeopleAttrManager.AddReviseTimeScale(-1, ReviseType.PercentBase);
        StartCoroutine(IEnumeratorHelper.After(originalPeopleAttrManager.RemoveReviseTimeScale, reviseReceipt, switchFactionUseTime));
    }

    
}
