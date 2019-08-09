using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemNameInfo", menuName = "ScriptableObject/AttributesItemInfo")]
public class AttributesItemInfo : ItemBaseInfo
{
    public ReviseField reviseAttr;
    public float reviseValue;
    public ReviseMode reviseMode;
    // 道具持续时间 (小于等于0时, 视为一次性道具)
    public float duration;

    public AttributesItemInfo Clone()
    {
        AttributesItemInfo cloneInfo = CreateInstance<AttributesItemInfo>();
        cloneInfo.itemName = itemName;
        cloneInfo.itemID = itemID;
        cloneInfo.reviseAttr = reviseAttr;
        cloneInfo.reviseValue = reviseValue;
        cloneInfo.reviseMode = reviseMode;
        return cloneInfo;
    }
}
