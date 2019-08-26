﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 库存面板 视图脚本
/// </summary>
public class InventoryPanelView : MonoBehaviour {
    private Transform m_transofom; 

    private GameObject item_Prefab;
    private GameObject slot_Prefab;
    private Transform grid_Transform;

    public GameObject Item_Prefab { get { return item_Prefab; } }
    public GameObject Slot_Prefab { get { return slot_Prefab; } }
    public Transform Grid_Transform { get { return grid_Transform; } }

	void Awake () {
        m_transofom = gameObject.GetComponent<Transform>();
        item_Prefab = Resources.Load<GameObject>("Inventory/InventoryItem");
        slot_Prefab = Resources.Load<GameObject>("Inventory/InventorySlot");
        grid_Transform = m_transofom.Find("BackGround/Grid").GetComponent<Transform>();
    }
	

}