using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;    // File読み込み用

/// <summary>
/// CSVを読み込むためのクラス
/// </summary>
public class CSVReader2 : MonoBehaviour
{
    private TextAsset csvFile;
    private List<string[]> csvDatas = new List<string[]>(); // CSVの中身を格納するリスト
    [Tooltip("ファイルパスを格納する変数"), FileDesignation, SerializeField] private string filePath;

    void Start()
    {
        LoadCSV();
        Debug.Log(GetCsvDatas());
    }

    /// <summary>
    /// CSVファイルの読み込みを行う関数
    /// </summary>
    private void LoadCSV()
    {
        filePath = Path.GetFileNameWithoutExtension(filePath);
        csvFile = Resources.Load(filePath) as TextAsset;
        StringReader csvReader = new StringReader(csvFile.text);

        while (csvReader.Peek() > -1)
        {
            string cell = csvReader.ReadLine();
            csvDatas.Add(cell.Split(','));  // ','を目安にリストに格納していく   
        }
        csvReader.Close();
    }

    /// <summary>
    /// 読み込んだCSVのデータをすべて受け渡す関数
    /// </summary>
    /// <returns>CSVデータの中身全ての文字列</returns>
    public List<string[]> GetCsvDatas()
    {
        return csvDatas;
    }

    /// <summary>
    /// 読み込んだCSVデータの指定列を受け渡す関数
    /// </summary>
    /// <param name="lineNum">行番号</param>
    /// <returns>CSVデータの指定列の文字列</returns>
    public string[] GetCsvLine(int lineNum)
    {
        return csvDatas[lineNum];
    }

    /// <summary>
    /// 読み込んだCSVの指定されたセルの文字列を受け渡す関数
    /// </summary>
    /// <param name="lineNum">行番号</param>
    /// <param name="columnNum">列番号</param>
    /// <returns></returns>
    public string GetCsvCell(int lineNum, int columnNum)
    {
        return csvDatas[lineNum][columnNum];
    }
}
