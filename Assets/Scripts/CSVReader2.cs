using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;  //付け足す

public class CSVReader : MonoBehaviour
{
    private TextAsset _csvFile;   //CSVファイル

    private List<string[]> _csvData = new List<string[]>();  //CSVファイルの中身を入れるリスト

    void Start()
    {
        _csvFile = Resources.Load("test") as TextAsset;   //Resourceにある指定のパスのCSVファイルを格納
        StringReader reader = new StringReader(_csvFile.text);      //TextAssetをStringReaderに変換

        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();   //１行ずつ読む 
            _csvData.Add(line.Split(','));    //読みこんだDataをリストにAddする
        }

        for (int i = 1; i < _csvData.Count; i++)
        {
            //Debug.Log("名前::::" + _csvData[i][0] + ",  HP::::" + _csvData[i][1]);
            Debug.Log(_csvData[0][0] + " : " + _csvData[i][0] + " , " +
                _csvData[0][1] + " : " + _csvData[i][1]);
        }
    }
}