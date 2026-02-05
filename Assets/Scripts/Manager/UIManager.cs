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
    
    public TextMeshProUGUI maskBitText = null;
    
    public int networkBitValue = 0;
    
    [SerializeField] uint[] hostBits = new uint[8];

    uint maskBit = 0b11111111111111111111111100000000;

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

    private void Start()
    {
        hostBits[0] = 0b10000000;
        for (int i = 1; i < hostBits.Length; i++)
        {
            hostBits[i] = (hostBits[i - 1] >> 1) | 0b10000000;
        }
    }

    private void Update()
    {
        networkBitValue = (int)Slider.value;
        networkBitValue = Mathf.Clamp(networkBitValue, 24, 30);
        NetworkAndHostBit();
        int index = (int)networkBitValue - 24;
        string value = (index - 1) < 0 ? "0" : hostBits[index - 1].ToString();
        maskBitText.text = $"255.255.255.{value}";
        if (string.IsNullOrEmpty(fields[0].text) || string.IsNullOrEmpty(fields[1].text) ||
            string.IsNullOrEmpty(fields[2].text) || string.IsNullOrEmpty(fields[3].text))
        {
            CurrentNetwork(" ");
            CurrentHostRange(" ", " ");
        }
        else
        {
            string network =
                $"{fields[0].text}.{fields[1].text}.{fields[2].text}.{int.Parse(fields[3].text) & int.Parse(value)}";
            CurrentNetwork(network);
            CurrentHostRange($"{fields[0].text}.{fields[1].text}.{fields[2].text}.{(int.Parse(fields[3].text) & int.Parse(value)) + 1}", $"{fields[0].text}.{fields[1].text}.{fields[2].text}.{(256 >> index) - 2}");
        }
        
        
    }
}
