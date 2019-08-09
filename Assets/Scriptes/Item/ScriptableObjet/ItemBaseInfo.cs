using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBaseInfo : ScriptableObject
{
    public string itemName;
    public int itemID;
    // 单个背包格子最大叠加数
    public int maxCountInGrid = 999;
    public EffectGroup effectGroup;
    // 范围半径
    public float radius;
}
// 作用群体
public enum EffectGroup
{
    // 自己
    MySelf,
    // 一位随机队友(包括自己)
    RandomTeammate,
    // 最近的一名队友(不包括自己)
    OneNearbyTeammate,
    // 范围内随机一名队友(包括自己)
    RangeRandomTeammate,
    // 范围内的所有队友(包括自己)
    RangeTeammate,
    // 全队
    AllTeammate,
    // 随机一名敌人
    RandomEnemy,
    // 最近的一名敌人
    OneNearbyEnemy,
    // 范围内随机一名敌人
    RangeRandomEnemy,
    // 范围内的所有敌人
    RangeEnemy,
    // 所有敌人
    AllEnemy,
}
