using UnityEngine;

public class TextRotation : MonoBehaviour
{
    [SerializeField]
    private Transform shipTransform;
    [SerializeField]
    private float radians;
    [SerializeField]
    private Vector2 hardPoint;
    [SerializeField]
    private Vector2 hardPointCoords;

    // Start is called before the first frame update
    void Start()
    {
        shipTransform = this.gameObject.transform.parent.parent.parent.transform;
    }

    // Update is called once per frame
    void Update()
    {
        radians = (-90) * Mathf.Deg2Rad;
        hardPoint = (Vector2)this.transform.position + (Vector2)shipTransform.position;
        hardPointCoords.x = Mathf.Cos(radians);
        hardPointCoords.y = Mathf.Sin(radians);
        hardPoint = hardPointCoords * 0.15f;
        transform.position = (Vector2)shipTransform.position + hardPoint;
        this.gameObject.transform.rotation = Quaternion.identity;   
    }
}
