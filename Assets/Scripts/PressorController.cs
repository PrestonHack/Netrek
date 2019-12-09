using UnityEngine;
using Photon.Pun;

public class PressorController : MonoBehaviour
{
    public bool pressorOn;
    [SerializeField]
    private EndPointControllerPressor endPointController;
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private BoxCollider2D pressorCollider;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private PhotonView photonView;
    public Vector2 point;
    public Vector2 direction;
    [SerializeField]
    private Vector2 start;
    [SerializeField]
    private Vector2 end;
    [SerializeField]
    public float distance;
    [SerializeField]
    public float currentLength;
    [SerializeField]
    private float maxRange = 2.25f;
    [SerializeField]
    private float angle;
    [SerializeField]
    private Vector2 hardPointCoords;
    [SerializeField]
    private float radians;


    void Update()
    {
        //setting hardpoint position manually
        radians = (endPointController.shipTransform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad;
        hardPointCoords.x = Mathf.Cos(radians);
        hardPointCoords.y = Mathf.Sin(radians);
        hardPointCoords = hardPointCoords * 0.1f;
        transform.position = (Vector2)endPointController.shipTransform.position + hardPointCoords;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPointController.gameObject.transform.position);

        if (pressorOn)
        {
            RotateTowards(endPointController.gameObject.transform.position);
            setCollider();
            lineRenderer.material.color = Random.ColorHSV(0.15f, 0.15f, 1f, 1f, 0.9f, 1f);
            lineRenderer.material.SetColor("_EmissionColor", Random.ColorHSV(0.15f, 0.15f, 1f, 1f, 0.1f, 0.5f));
        }
        else
        {
            RotateTowards(cam.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    public void off()
    {
        photonView.RPC("turnOff", RpcTarget.AllBuffered);
    }

    public void toggle()
    {
        photonView.RPC("enableTractor", RpcTarget.AllBuffered);
    }

    [PunRPC]
    public void turnOff()
    {
        lineRenderer.enabled = false;
        pressorCollider.enabled = false;
        pressorOn = false;
        endPointController.isOn = false;
    }

    [PunRPC]
    public void enableTractor()
    {
        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            pressorCollider.enabled = true;
            pressorOn = true;
            endPointController.isOn = true;
            endPointController.endPointCollider.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
            pressorCollider.enabled = false;
            pressorOn = false;
            endPointController.isOn = false;
            endPointController.gameObject.transform.position = this.gameObject.transform.position;
            endPointController.endPointCollider.enabled = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<Transform>().root.name != this.gameObject.GetComponentInParent<Transform>().root.name)
        {
            moveSelf(collision);
        }
    }

    private void moveSelf(Collider2D collision)
    {
        endPointController.shipTransform.position -= (collision.transform.position - endPointController.shipTransform.position).normalized * 0.0005f;
        endPointController.point = collision.transform.position;
    }

    private void setCollider()
    {
        Vector2 size = pressorCollider.size;
        size.y = new Vector2(transform.position.x - lineRenderer.GetPosition(1).x, transform.position.y - lineRenderer.GetPosition(1).y).magnitude;
        pressorCollider.size = size;
        pressorCollider.offset = new Vector2(0, pressorCollider.size.y / 2);
    }

    void RotateTowards(Vector2 target)
    {
        float offset = -90f;
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle + offset, Vector3.forward);
        this.transform.rotation = rotation;
    }
}

