using UnityEngine;

[CreateAssetMenu(fileName = "NewShipData", menuName = "Ship Data", order = 51)]
public class ShipLayoutSO : ScriptableObject
{
    public Sprite sprite;
    public GameObject gunPrefab;
    public float rotationSpeed;
    public float thrustSpeed;
    public float driftAmount;
    //public bool canRotate;
    //public bool canThrust;
    //public bool canDrift;
}