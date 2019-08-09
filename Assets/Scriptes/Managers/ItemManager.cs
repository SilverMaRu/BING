using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public int testSpawNum = 100;
    public ItemInfoList itemInfoBase;
    private ItemBaseInfo[] itemInfos;

    public GameObject itemPrefab;
    public ItemSpawRange rangeInfo;

    private void Awake()
    {
        itemInfos = new ItemBaseInfo[itemInfoBase.itemInfos.Length];
        itemInfoBase.itemInfos.CopyTo(itemInfos, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            for (int i = 0; i < testSpawNum; i++)
            {
                CreateItem();
            }
        }
    }

    public GameObject CreateItem()
    {
        int randomInfoIdx = Random.Range(0, itemInfos.Length);
        return CreateItem(randomInfoIdx);
    }

    public GameObject CreateItem(int index)
    {
        Vector3 position = rangeInfo.RandomPosition();
        GameObject newItem = Instantiate(itemPrefab, position, Quaternion.identity);
        PickUp item = newItem.GetComponent<PickUp>();
        item.info = itemInfos[index];
        return newItem;
    }
}

public enum RangeType
{
    Cube,
    Sphere,
    Cylinder
}

[System.Serializable]
public class ItemSpawRange
{
    public RangeType rangeType = RangeType.Cylinder;
    public Vector3 center = Vector3.zero;
    public Vector3 extents = Vector3.zero;

    public Vector3 RandomPosition()
    {
        Vector3 position = center;
        float randomX = 0;
        float randomY = 0;
        float randomZ = 0;
        switch (rangeType)
        {
            case RangeType.Cube:
                randomX = Random.Range(-1f, 1f);
                randomY = Random.Range(-1f, 1f);
                randomZ = Random.Range(-1f, 1f);
                Vector3 randomScale = new Vector3(randomX, randomY, randomZ);
                position = center + Vector3.Scale(extents, randomScale);
                break;
            case RangeType.Sphere:
                randomX = Random.Range(-1f, 1f);
                position = Vector3.Scale(extents, Vector3.right * randomX);
                position = Random.rotation * position;
                position += center;
                break;
            case RangeType.Cylinder:
                randomX = Random.Range(-1f, 1f);
                randomY = Random.Range(-1f, 1f);
                randomZ = Random.Range(0f, 360f);
                position = Vector3.Scale(extents, Vector3.right * randomX + Vector3.up * randomY);
                position = Quaternion.Euler(Vector3.up * randomZ) * position;
                position += center;
                break;
        }
        return position;
    }
}