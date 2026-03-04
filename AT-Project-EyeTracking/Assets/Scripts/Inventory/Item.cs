using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item", order = 1)]
public class Item : ScriptableObject
{
    public string id;
    public string objName;
    public Sprite icon;
    public GameObject prefab;
   
}
