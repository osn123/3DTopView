using UnityEngine;

[CreateAssetMenu(fileName = "New Table Data", menuName = "Excel-like Data/Table Data")]
public class TableData : ScriptableObject
{
    public RowData[] rows;
}