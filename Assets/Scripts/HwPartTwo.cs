using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class HwPartTwo : MonoBehaviour
{
    [SerializeField] private bool _intFloatToggle = false;
    [SerializeField] private int _intValue;
    [SerializeField] private float _floatValue;
    [SerializeField] private int[] _intArray;
    [SerializeField] private float[] _floatArray;
    [SerializeField] private Toggle _intFloatToggleUI;
    [SerializeField] private Button _getArrayButton;
    [SerializeField] private Button _getArrayRefButton;
    [SerializeField] private Button _getArrayOutButton;
    [SerializeField] private Button _saveToFile;
    [SerializeField] private Button _saveToFileWithSerialization;
    [SerializeField] private Button _loadFromFile;
    [SerializeField] private Button _loadFromFileWithSerialization;
    
    private int _arraySize = 5;

    struct Storage
    {
        public bool storageIntFloatToggle;
        public int storageIntValue;
        public float storageFloatValue;
        public int[] storageIntArray;
        public float[] storageFloatArray;

        public Storage(bool storageIntFloatToggle, int storageIntValue, float storageFloatValue, int[] storageIntArray, float[] storageFloatArray)
        {
            this.storageIntFloatToggle = storageIntFloatToggle;
            this.storageIntValue = storageIntValue;
            this.storageFloatValue = storageFloatValue;
            this.storageIntArray = storageIntArray;
            this.storageFloatArray = storageFloatArray;
        }
    }

    private void SetToggle(bool toggleValue)
    {
        _intFloatToggle = toggleValue;
    }

    private void GetArray()
    {
        if (_intFloatToggle)
        {
            _intArray = CreateArray<int>(5);
            FillArrayInt(_intArray, _intValue);
        }
        else
        {
            _floatArray = CreateArray<float>(5);
            FillArrayFloat(_floatArray, _floatValue);
        }
    }

    private void GetArrayRef()
    {
        if (_intFloatToggle)
        {
            _intArray = CreateArray<int>(5);
            FillArrayIntRef(_intArray, ref _intValue);
        }
        else
        {
            _floatArray = CreateArray<float>(5);
            FillArrayFloatRef(_floatArray, ref _floatValue);
        }
    }

    private void GetArrayOut()
    {
        if (_intFloatToggle)
        {
            _intArray = CreateArray<int>(5);
            FillArrayIntOut(_intArray, out _intValue);
        }
        else
        {
            _floatArray = CreateArray<float>(5);
            FillArrayFloatOut(_floatArray, out _floatValue);
        }
    }

    private Storage CreateStorage()
    {
        return new Storage(_intFloatToggle, _intValue, _floatValue, _intArray, _floatArray);
    }

    private void SaveFile()
    {
        var valueCopy = CreateStorage();
        const string filePath = "Assets/outputFile.txt";

        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            writer.WriteLine(valueCopy.storageIntFloatToggle);
            writer.WriteLine(valueCopy.storageIntValue);
            writer.WriteLine(valueCopy.storageFloatValue);
            writer.WriteLine(string.Join(",", valueCopy.storageIntArray));
            writer.WriteLine(string.Join(",", valueCopy.storageFloatArray));
        }
    }

    private void SaveSerializationFile()
    {
        var valueCopy = CreateStorage();
        const string filePath = "Assets/outputJsonFile.txt";
        string jsonStorage = JsonUtility.ToJson(valueCopy);
        
        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            writer.Write(jsonStorage);
        }
    }

    private void LoadFromFile()
    {
        const string filePath = "Assets/outputFile.txt";
        List<string> fileStorage = new List<string>();
        string nextLine;

        using (var reader = new StreamReader(filePath))
        {
            for(;(nextLine = reader.ReadLine()) != null;)
            {
                fileStorage.Add(nextLine);
            }
        }
        _intFloatToggle = Convert.ToBoolean(fileStorage[0]);
        _intValue = Convert.ToInt32(fileStorage[1]);
        _floatValue = Convert.ToSingle(fileStorage[2]);
        for (var i = 0; i < fileStorage[3].Split(',').Length; i++)
        {
            _intArray[i] = Convert.ToInt32(fileStorage[3].Split(',')[i]);
        }
        for (var i = 0; i < fileStorage[4].Split(',').Length; i++)
        {
            _floatArray[i] = Convert.ToSingle(fileStorage[4].Split(',')[i]);
        }
    }

    private void LoadFromSerializationFile()
    {
        const string filePath = "Assets/outputJsonFile.txt";
        Storage jsonFileStorage;
        
        using (var reader = new StreamReader(filePath))
        {
            jsonFileStorage = JsonUtility.FromJson<Storage>(reader.ReadToEnd());
        }
        _intFloatToggle = jsonFileStorage.storageIntFloatToggle;
        _intValue = jsonFileStorage.storageIntValue;
        _floatValue = jsonFileStorage.storageFloatValue;
        _intArray = jsonFileStorage.storageIntArray;
        _floatArray = jsonFileStorage.storageFloatArray;
    }

    private T[] CreateArray<T>(int size)
    {
        return new T[size];
    }

    private void FillArrayInt(int[] array, int intValue)
    {
        array[0] = intValue;

        for (var i = 1; i < array.Length; i++)
        {
            try
            {
                if(array[i - 1] > 46340)
                {
                    throw new System.OverflowException();
                }
                array[i] = array[i - 1] * array[i - 1];
            }
            catch (System.OverflowException e)
            {
                Debug.Log($"{e.Message} Будет добавлен элемент с максимальным значением типа int");
                array[i] = int.MaxValue;
            }
        }
    }

    private void FillArrayIntRef(int[] array, ref int intValue)
    {
        FillArrayInt(array, intValue);
    }

    private void FillArrayIntOut(int[] array, out int intValue)
    {
        intValue = 2;
        FillArrayInt(array, intValue);
    }

    private void FillArrayFloat(float[] array, float floatValue)
    {
        array[0] = floatValue;

        for (int i = 1; i < array.Length; i++)
        {
            try
            {
                if (array[i - 1] > 18446743523953736799)
                {
                    throw new System.OverflowException();
                }
                array[i] = array[i - 1] * array[i - 1];
            }
            catch (System.OverflowException e)
            {
                Debug.Log($"{e.Message} Будет добавлен элемент с максимальным значением типа float");
                array[i] = float.MaxValue;
            }
        }
    }
    private void FillArrayFloatRef(float[] array, ref float floatValue)
    {
        FillArrayFloat(array, floatValue);
    }

    private void FillArrayFloatOut(float[] array, out float floatValue)
    {
        floatValue = 2f;
        FillArrayFloat(array, floatValue);
    }

    private void Awake()
    {
        _intArray = new int[_arraySize];
        _floatArray = new float[_arraySize];
        _intFloatToggleUI.onValueChanged.AddListener(SetToggle);
        _getArrayButton.onClick.AddListener(GetArray);
        _getArrayRefButton.onClick.AddListener(GetArrayRef);
        _getArrayOutButton.onClick.AddListener(GetArrayOut);
        _saveToFile.onClick.AddListener(SaveFile);
        _saveToFileWithSerialization.onClick.AddListener(SaveSerializationFile);
        _loadFromFile.onClick.AddListener(LoadFromFile);
        _loadFromFileWithSerialization.onClick.AddListener(LoadFromSerializationFile);
    }
}
