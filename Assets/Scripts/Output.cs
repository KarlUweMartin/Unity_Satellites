using TMPro;
using UnityEngine;

public class Output : MonoBehaviour
{
    public static Output Instance { get; private set; }

    private void Start()
    {
        Instance = this;
    }

    public bool Visible 
    {
        get => _label.activeSelf;
        set => _label.SetActive(value);
    }

    public string Text
    {
        get => _text.text;
        set => _text.text = value;        
    }

    public Color32 Color 
    {
        get => _text.color;
        set => _text.color = value;
    }

    [SerializeField] private GameObject _label;
    [SerializeField] private TextMeshProUGUI _text;
}
