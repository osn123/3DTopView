using UnityEngine;

[CreateAssetMenu(fileName = "New Row Data", menuName = "Excel-like Data/Row Data")]
public class RowData : ScriptableObject
{
    public int id;
    public string nname;
    public float value;
    // 必要に応じて他のフィールドを追加
}