using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void TouchDelegate(Part sender, TouchEventDate eventDate);
public class Part : MonoBehaviour
{
    public event TouchDelegate TouchEvent;

    public PartType partType = PartType.Body;
    public bool isEnable { get; private set; } = false;
    public PartManager partManager { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (isEnable && other.tag.Equals("Player"))
        {
            Part otherPart = other.GetComponent<Part>();
            if(otherPart != null && TouchEvent != null && otherPart.isEnable)
            {
                TouchEvent(this, new TouchEventDate(otherPart));
            }
        }
    }

    public void Disable()
    {
        isEnable = false;
    }

    public void SetEnable(PlayerFaction onwerFaction)
    {
        isEnable = onwerFaction == PlayerFaction.People && partType == PartType.Body || onwerFaction == PlayerFaction.Ghost && partType == PartType.RightHand;
    }
}

public enum PartType
{
    Head,
    Body,
    LeftUpperArm,
    LeftLowerArm,
    LeftHand,
    RightUpperArm,
    RightLowerArm,
    RightHand,
    Hip,
    LeftUpperLeg,
    LeftLowerLeg,
    LeftFoot,
    RightUpperLeg,
    RightLowerLeg,
    RightFoot
}

public class TouchEventDate
{
    public Part otherPart;

    public TouchEventDate(Part otherPart)
    {
        this.otherPart = otherPart;
    }
}