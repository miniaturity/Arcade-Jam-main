using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
public class Card
{
    public Image cardCover { get; private set; }
    public string title { get; private set; }
    public string desc { get; private set; }
    public GameObject powerPrefab { get; private set; }
    
    public Card(
        Image cc,
        string t,
        string d,
        GameObject p
    )
    {
        cardCover = cc;
        title = t;
        desc = d;
        powerPrefab = p;
    }
}