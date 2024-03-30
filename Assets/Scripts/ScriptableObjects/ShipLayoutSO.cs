using UnityEngine;

[CreateAssetMenu(fileName = "NewShipData", menuName = "Ship Data", order = 51)]
public class ShipLayoutSO : ScriptableObject
{
    public Sprite sprite;
    public GameObject prefab;
    public int RGB;
}