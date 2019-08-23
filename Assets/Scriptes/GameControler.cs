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
    public ReviseInfo[] changeToGhostRevises;
    public ReviseInfo[] changeToPeopleRevises;

    public GameObject originalGhostGO;
    public GameObject originalPeopleGO;

    private void Awake()
    {
        ins = this;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && originalGhostGO != null && originalPeopleGO != null)
        {
            SwitchFaction(originalGhostGO, originalPeopleGO);
            GameObject tempGO = originalGhostGO;
            originalGhostGO = originalPeopleGO;
            originalPeopleGO = tempGO;
        }
    }

    public void SwitchFaction(GameObject originalGhostGO, GameObject originalPeopleGO)
    {
        SwitchFactionStartEvent?.Invoke();
        StartCoroutine(IEnumeratorHelper.After(() => { SwitchFactionCompleteEvent?.Invoke(); }, switchFactionUseTime));
        AttributesManager originalGhostAttrManager = originalGhostGO.GetComponent<AttributesManager>();
        originalGhostAttrManager.faction = PlayerFaction.People;
        originalGhostAttrManager.RemoveAllItemRevise();
        foreach (ReviseInfo tempReviseInfo in changeToPeopleRevises)
        {
            long reviseReceipt = originalGhostAttrManager.AddItemRevise(tempReviseInfo);
        }

        AttributesManager originalPeopleAttrManager = originalPeopleGO.GetComponent<AttributesManager>();
        originalPeopleAttrManager.faction = PlayerFaction.Ghost;
        originalPeopleAttrManager.RemoveAllItemRevise();
        foreach(ReviseInfo tempReviseInfo in changeToGhostRevises)
        {
            long reviseReceipt = originalPeopleAttrManager.AddItemRevise(tempReviseInfo);
        }
        ActivityManager originalPeopleActivityManager = originalPeopleGO.GetComponent<ActivityManager>();
        originalPeopleActivityManager.enabled = false;
        StartCoroutine(IEnumeratorHelper.After(() => { originalPeopleActivityManager.enabled = true; }, switchFactionUseTime));
    }


}
