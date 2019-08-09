using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack
{
    public GameObject ownerGO;
    private AttributesManager attrManager;
    private PlayerFaction ownerFaction { get { return attrManager.faction; } }
    private BackpackGrid[] grids;
    private int backpackSize = 1;
    private int lastCheckIndex = 0;

    public Backpack(GameObject ownerGO) : this(ownerGO, 1)
    {
    }

    public Backpack(GameObject ownerGO, int backpackSize)
    {
        this.ownerGO = ownerGO;
        attrManager = ownerGO.GetComponent<AttributesManager>();
        this.backpackSize = backpackSize;
        grids = new BackpackGrid[backpackSize];
        for(int i = 0; i < backpackSize; i++)
        {
            grids[i] = new BackpackGrid(this);
        }
    }

    public bool CanPutIn(int putInGridIndex, ItemGroup putInItemGroup, out ItemGroup extraItemGroup)
    {
        extraItemGroup = null;
        bool canPutIn = false;
        if (putInGridIndex >= 0 && putInGridIndex < backpackSize)
        {

        }
        else
        {

        }

        return canPutIn;
    }

    public bool CanPutIn(ItemGroup putInItemGroup, out ItemGroup extraItemGroup)
    {
        return CanPutIn(-1, putInItemGroup, out extraItemGroup);
    }

    public int GetEmptyGridIndex()
    {
        int resultIdx = -1;
        for (int i = 0; i < backpackSize; i++)
        {
            if (grids[i].isEmpty)
            {
                resultIdx = i;
                break;
            }
        }
        return resultIdx;
    }
}

public class BackpackGrid
{
    private Backpack ownerBackpack;
    private ItemGroup myItemGroup;
    public bool isEmpty
    {
        get
        {
            Reflash();
            return myItemGroup == null;
        }
    }

    public BackpackGrid(Backpack ownerBackpack)
    {
        this.ownerBackpack = ownerBackpack;
    }

    public bool CanPutIn(ItemGroup putInItemGroup, out ItemGroup extraItemGroup)
    {
        Reflash();
        bool canPutIn = myItemGroup == null || myItemGroup.itemCount > 0 && myItemGroup.CompareTo(putInItemGroup) == 0;
        extraItemGroup = null;
        if (canPutIn && myItemGroup != null)
        {
            ItemBaseInfo putInItemInfo = putInItemGroup.itemInfo;
            int extraCount = putInItemGroup.itemCount + myItemGroup.itemCount - putInItemInfo.maxCountInGrid;
            if (extraCount > 0)
            {
                extraItemGroup = new ItemGroup(putInItemInfo, extraCount);
            }
        }
        return canPutIn;
    }

    public void PutIn(ItemGroup putInItemGroup, out ItemGroup extraItemGroup)
    {
        extraItemGroup = null;
        myItemGroup.itemInfo = putInItemGroup.itemInfo;
        myItemGroup.itemCount += putInItemGroup.itemCount;
        int extraCount = myItemGroup.itemCount - myItemGroup.itemInfo.maxCountInGrid;
        if (extraCount > 0)
        {
            extraItemGroup = new ItemGroup(putInItemGroup.itemInfo, extraCount);
        }
    }

    public bool TryPutIn(ItemGroup putInItemGroup, out ItemGroup extraItemGroup)
    {
        extraItemGroup = null;
        bool canPutIn = CanPutIn(putInItemGroup, out extraItemGroup);
        if (canPutIn) PutIn(putInItemGroup, out extraItemGroup);
        return canPutIn;
    }

    public bool CanTakeOut(int takeOutCount = 0)
    {
        takeOutCount = Mathf.Max(0, takeOutCount);
        bool canTakeOut = canTakeOut = myItemGroup != null && myItemGroup.itemCount > takeOutCount;
        return canTakeOut;
    }

    public ItemGroup TakeOut(int takeOutCount = 0)
    {
        if (takeOutCount <= 0)
        {
            takeOutCount = myItemGroup.itemCount;
        }
        ItemGroup outItemGroup = new ItemGroup(myItemGroup.itemInfo, takeOutCount);
        myItemGroup.itemCount -= takeOutCount;
        Reflash();
        return outItemGroup;
    }

    public bool TryTakeOut(out ItemGroup outItemGroup, int takeOutCount = 0)
    {
        outItemGroup = null;
        bool canTakeOut = CanTakeOut(takeOutCount);
        if (canTakeOut) outItemGroup = TakeOut(takeOutCount);
        return canTakeOut;
    }

    public void Reflash()
    {
        if (myItemGroup != null && myItemGroup.itemCount <= 0) myItemGroup = null;
    }
}

public class ItemGroup
{
    public ItemBaseInfo itemInfo;
    public int itemCount;

    public ItemGroup(ItemBaseInfo itemInfo, int itemCount)
    {
        this.itemInfo = itemInfo;
        this.itemCount = itemCount;
    }

    public int CompareTo(ItemGroup other)
    {
        return itemInfo.itemID.CompareTo(other.itemInfo.itemID);
    }
}
