﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 库存面板控制器
/// </summary>
public class InventoryPanelController : MonoBehaviour
{
    //持有自身M V脚本
    private InventoryPanelModel m_inventoryModel;
    private InventoryPanelView m_inventoryView;

    List<GameObject> slotList = null;   //物品槽数组
    private int inventoryCount = 24;
    void Start()
    {
        m_inventoryModel = gameObject.GetComponent<InventoryPanelModel>();
        m_inventoryView = gameObject.GetComponent<InventoryPanelView>();

        slotList = new List<GameObject>();


        CreateAllSlot();
        CreateAllItem();
    }

    /// <summary>
    /// 生成所有物品槽
    /// </summary>
    private void CreateAllSlot()
    {
        for (int i = 0; i < inventoryCount; i++)
        {
            GameObject go = Instantiate<GameObject>(m_inventoryView.Slot_Prefab, m_inventoryView.Grid_Transform);
            slotList.Add(go);
        }
    }
    /// <summary>
    /// 生成所有物品
    /// </summary>
    private void CreateAllItem()
    {
        List<InventoryItem> list = m_inventoryModel.ReadJson("InventoryJsonData");
        
        for (int i = 0; i < list.Count; i++)
        {
            GameObject item = Instantiate<GameObject>(m_inventoryView.Item_Prefab, slotList[i].GetComponent<Transform>());
            item.GetComponent<InventoryItemController>().SetItem(list[i].ItemName,list[i].ItemNum);
        }


    }
}