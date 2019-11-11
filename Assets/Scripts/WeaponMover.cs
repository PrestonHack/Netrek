using UnityEngine;

public class WeaponMover : MonoBehaviour
{
    [SerializeField]
    private Vector3 crosshairPosition;
    [SerializeField]
    private Vector2 crosshairPoint;
    [SerializeField]
    private float angle;
    [SerializeField]
    private float rotationSpeed = 5;
    [SerializeField]
    private Camera cam;

    void Update()
    {
        crosshairPosition = Input.mousePosition;
        crosshairPoint = cam.ScreenToWorldPoint(new Vector3(crosshairPosition.x, crosshairPosition.y, crosshairPosition.z));
        crosshairPoint = new Vector2(crosshairPoint.x, crosshairPoint.y);
        RotateTowards(crosshairPoint);

    }
    private void FixedUpdate()
    {
    }

    private void RotateTowards(Vector2 target)
    {
        float offset = -90f;
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
        this.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}
