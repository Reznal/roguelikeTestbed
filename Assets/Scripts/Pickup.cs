using UnityEngine;
using Random = UnityEngine.Random;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    private PickupType _pickupType;
    public PickupType PickupType => _pickupType;

    [SerializeField]
    private ScriptableObject _scriptableValue;
    public ScriptableObject ScriptableValue => _scriptableValue;

    private float _speed;

    private void Awake()
    {
        _speed = Random.Range(45, 90);
        GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value);
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 0, _speed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Collided");
        if (col.CompareTag("Player"))
            col.GetComponent<Player.Player>().ApplyPickup(this);
    }
}

public enum PickupType : byte
{
    Weapon,
    Effect,
    Attack
}
