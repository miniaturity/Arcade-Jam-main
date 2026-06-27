using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
public class Card : MonoBehaviour
{
    [SerializeField] private Image cardCover;
    [SerializeField] private string title;
    [SerializeField] private string desc;
    [SerializeField] private GameObject powerPrefab;
    private bool shown = false;
    private void Start()
    {
        
    }
}