using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;  //�t������

public class CSVReader : MonoBehaviour
{
    private TextAsset _csvFile;   //CSV�t�@�C��

    private List<string[]> _csvData = new List<string[]>();  //CSV�t�@�C���̒��g�����郊�X�g

    void Start()
    {
        _csvFile = Resources.Load("test") as TextAsset;   //Resource�ɂ���w��̃p�X��CSV�t�@�C�����i�[
        StringReader reader = new StringReader(_csvFile.text);      //TextAsset��StringReader�ɕϊ�

        while (reader.Peek() != -1)
        {
            string line = reader.ReadLine();   //�P�s���ǂ� 
            _csvData.Add(line.Split(','));    //�ǂ݂���Data�����X�g��Add����
        }

        for (int i = 1; i < _csvData.Count; i++)
        {
            //Debug.Log("���O::::" + _csvData[i][0] + ",  HP::::" + _csvData[i][1]);
            Debug.Log(_csvData[0][0] + " : " + _csvData[i][0] + " , " +
                _csvData[0][1] + " : " + _csvData[i][1]);
        }
    }
}