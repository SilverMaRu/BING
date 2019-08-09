using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemNameInfo", menuName = "ScriptableObject/TransferItemInfo")]
public class TransferItemInfo : ItemBaseInfo
{
    public float newPositionX;
    public float newPositionY;
    public float newPositionZ;

    public TransferItemInfo Clone()
    {
        TransferItemInfo cloneInfo = CreateInstance<TransferItemInfo>();
        cloneInfo.itemName = itemName;
        cloneInfo.itemID = itemID;
        cloneInfo.newPositionX = newPositionX;
        cloneInfo.newPositionY = newPositionY;
        cloneInfo.newPositionZ = newPositionZ;
        return cloneInfo;
    }

    public Vector3 GetPosition()
    {
        return new Vector3(newPositionX, newPositionY, newPositionZ);
    }
}
