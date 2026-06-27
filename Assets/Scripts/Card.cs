using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Card : MonoBehaviour
{
    public Texture cardCover;
    public string title;
    public string desc;
    public GameObject powerPrefab;

    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _desc;
    [SerializeField] private GameObject _coverArt;
    private void Awake()
    {
        _title.text = title;
        _desc.text = desc;
        _coverArt.GetComponent<RawImage>().texture = cardCover;
    }
}