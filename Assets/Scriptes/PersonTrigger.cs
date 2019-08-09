using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonTrigger : MonoBehaviour
{
    public AttributesManager attrManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Item"))
        {
            PickUp item = other.GetComponent<PickUp>();
            ItemBaseInfo info = item.info;
            if (info.effectGroup == EffectGroup.MySelf)
            {
                if (info is AttributesItemInfo)
                {
                    AttributesItemInfo tempInfo = (AttributesItemInfo)info;
                    long receipt = attrManager.AddRevise(tempInfo.reviseAttr, tempInfo.reviseValue, tempInfo.reviseMode);
                    if (tempInfo.duration > 0) StartCoroutine(After(attrManager.RemoveRevise, tempInfo.reviseAttr, receipt, tempInfo.duration));
                }
                else if (info is TransferItemInfo)
                {
                    TransferItemInfo tempInfo = (TransferItemInfo)info;
                    transform.position = tempInfo.GetPosition();
                }
            }
        }
    }

    private IEnumerator After<T1, T2>(System.Action<T1, T2> action, T1 age1, T2 age2, float time)
    {
        yield return new WaitForSeconds(time);
        action(age1, age2);
    }
}
