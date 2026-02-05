using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region  Singleton
    private static UIManager m_instance = null;
    public static UIManager Instance => m_instance;
        
    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion
    
    public List<TMP_InputField> fields = new List<TMP_InputField>();
    public TextMeshProUGUI currentNetwork = null;
    public TextMeshProUGUI currentHostRange = null;
    
    public Slider Slider = null;

    public TextMeshProUGUI networkBit = null;
    public TextMeshProUGUI hostBit = null;
    
    public int networkBitValue = 0;

    public uint maskBit = 0b11111111111111111111111100000000;

    public void NumberDec()
    {
        Slider.value =  Slider.value - 1;
    }
    
    public void NumberInc()
    {
        Slider.value =  Slider.value + 1;
    }

    public void CurrentNetwork(string text)
    {
        currentNetwork.text = text;
    }

    public void NetworkAndHostBit()
    {
        networkBit.text = networkBitValue.ToString();
        hostBit.text = (32 - networkBitValue).ToString();
    }

    public void CurrentHostRange(string from, string to)
    {
        networkBitValue = (int)Slider.value;
        currentHostRange.text = $"{from}\n<color=\"yellow\">to</color>\n{to}";
    }

    private void Update()
    {
        networkBitValue = (int)Slider.value;
        networkBitValue = Mathf.Clamp(networkBitValue, 24, 32);
        NetworkAndHostBit();
    }
}
