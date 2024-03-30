using UnityEngine;

[CreateAssetMenu(fileName = "NewShipData", menuName = "Ship Data", order = 51)]
public class ShipLayoutSO : ScriptableObject
{
    public Sprite sprite;
    public GameObject gunPrefab;
    public float rotationSpeed;
    public float thrustSpeed;
    public float driftAmount;
    public int RGB;

    public Vector2 hitBoxOffset, hitBoxSize;
    public bool _leftThrusterB, _rightThrusterB, _middleThrusterB,
        _leftStrafeThrusterB, _rightStrafeThrusterB, _retroThrusterB, _retroThruster2B;
}