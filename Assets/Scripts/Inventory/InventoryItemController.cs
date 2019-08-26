﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 库存物品自身控制脚本
/// </summary>
public class InventoryItemController : MonoBehaviour {
    private Transform m_transform;
    private Image m_Img;
    private Text m_Text;

    private void Awake()
    {
        m_transform = gameObject.GetComponent<Transform>();
        m_Img = gameObject.GetComponent<Image>();
        m_Text = m_transform.Find("Num").GetComponent<Text>();
    }
    /// <summary>
    /// 创建Item预制体
    /// </summary>
    /// <param name="fileName">图标文件名</param>
    /// <param name="count">物品数量</param>
    public void SetItem(string fileName, int count)
    {
        
        this.m_Img.sprite = Resources.Load<Sprite>("Inventory/" + fileName);
        this.m_Text.text = count.ToString();
    }


}