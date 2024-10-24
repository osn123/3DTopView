using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

/// <summary>
/// Fileを選択するときに使用する属性
/// </summary>
[CustomPropertyDrawer(typeof(FileDesignationAttribute))]
public class FileDesignation : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        FileDesignationAttribute fileAttribute = (FileDesignationAttribute)attribute;

        // 使用するプロパティの配置
        Rect rectFileOpenButton = new Rect(position.x + position.width - 30, position.y, 30, position.height);
        Rect rectFilePath = new Rect(position.x, position.y, position.width - rectFileOpenButton.width - 5, position.height);
        Rect rectFileExtension = new Rect(position.x + 60, position.y, position.width - 325, position.height);

        // 各プロパティをインスペクターに表示
        EditorGUI.TextField(rectFilePath, label, property.stringValue);
        fileAttribute.extensionFilter = (FileDesignationAttribute.FILEEXTENSION)EditorGUI.EnumPopup(rectFileExtension, fileAttribute.extensionFilter);
        if (GUI.Button(rectFileOpenButton, "..."))
        {
            string directoryPath = "";
            string pathName = "";
            if (File.Exists(property.stringValue))
            {
                directoryPath = Path.GetDirectoryName(property.stringValue);
            }

            if (fileAttribute.extensionFilter.ToString() == "NONE")
            {
                pathName = EditorUtility.OpenFilePanel("select ", directoryPath, "");
            }
            else
            {
                pathName = EditorUtility.OpenFilePanel("select ", directoryPath, fileAttribute.extensionFilter.ToString());
            }

            // ファイル選択後の処理
            if (!string.IsNullOrEmpty(pathName))
            {
                property.stringValue = pathName;
            }
        }
    }
}
