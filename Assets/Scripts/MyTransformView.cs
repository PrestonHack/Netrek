using UnityEngine;
using Photon.Pun;

public class MyTransformView : MonoBehaviour , IPunObservable
{
    [SerializeField]
    private PhotonView photonView;
    [SerializeField]
    private Transform transform;
    [SerializeField]
    private Vector2 networkPosition;
    [SerializeField]
    private Quaternion networkRotation;
    [SerializeField]
    private float lag;
    [SerializeField]
    private Vector2 start;
    [SerializeField]
    private Vector2 future;
    [SerializeField]
    private Vector2 heading;
    [SerializeField]        
    private float distance;
    [SerializeField]
    private Vector2 direction;
    [SerializeField]
    private Vector2 velocity;
    [SerializeField]
    private float timePassed;

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        start = new Vector2(transform.position.x, transform.position.y);

        heading = new Vector2((start.x - transform.position.x),(start.y - transform.position.y));
        distance = heading.magnitude;
        direction = heading / distance;
        velocity = heading / Time.deltaTime;




        
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.transform.position);
            stream.SendNext(this.transform.rotation);
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();
            lag = Mathf.Abs((float)(PhotonNetwork.Time - info.timestamp));
            timePassed = (float)PhotonNetwork.GetPing() / 1000f;
            future = transform.position * velocity * lag;

        }
    }

}
