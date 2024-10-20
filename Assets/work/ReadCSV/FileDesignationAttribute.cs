using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileDesignationAttribute : PropertyAttribute
{
    public enum FILEEXTENSION
    {
        NONE = 0,
        CSV = 1,
    }
    // 拡張子フィルター
    public FILEEXTENSION extensionFilter;
}
