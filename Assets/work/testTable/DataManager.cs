using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DataManager : MonoBehaviour
{
    public TableData tableData;

    void Start()
    {
        // データへのアクセス例
        foreach (RowData row in tableData.rows)
        {
            Debug.Log($"ID: {row.id}, Name: {row.name}, Value: {row.value}");
        }
    }

    // 特定のIDを持つ行を検索
    public RowData FindRowById(int id)
    {
        return System.Array.Find(tableData.rows, row => row.id == id);
    }
}