using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartManager : MonoBehaviour
{
    private Part[] managedParts = new Part[0];

    public AttributesManager attrManager;

    // Start is called before the first frame update
    void Start()
    {
        managedParts = transform.GetComponentsInChildren<Part>();
        foreach (Part tempPart in managedParts)
        {
            tempPart.partManager = this;
            tempPart.SetEnable(attrManager.faction);
            tempPart.TouchEvent += OnTouch;
        }
        
        attrManager.PlayerFactionChangeEvent += OnPlayerFactionChange;
    }

    private void OnTouch(Part sender, TouchEventDate eventDate)
    {
        PlayerFaction onwerFaction = attrManager.faction;
        Part otherPart = eventDate.otherPart;
        PartType otherPartType = otherPart.partType;
        if (onwerFaction == PlayerFaction.Ghost && otherPartType == PartType.Body)
        {
            GameControler.ins.SwitchFaction(gameObject, otherPart.partManager.gameObject);
        }
        else if (onwerFaction == PlayerFaction.People && otherPartType == PartType.RightHand)
        {
            GameControler.ins.SwitchFaction(otherPart.partManager.gameObject, gameObject);
        }
    }

    private void OnPlayerFactionChange(PlayerFaction currentFaction)
    {
        foreach (Part tempPart in managedParts)
        {
            tempPart.SetEnable(currentFaction);
        }
    }
}
