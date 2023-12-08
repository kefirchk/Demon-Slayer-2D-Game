using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 pos;
    //public float speed;

    public float dumping = 1.5f; // сглаживание камеры
    public Vector2 offset = new Vector2(2f, 1f);
    public bool isLeft;
    private int lastX; // в какую сторону последний раз смотрел персонаж


    [SerializeField] public float leftLimit;
    [SerializeField] public float rightLimit;
    [SerializeField] public float bottomLimit;
    [SerializeField] public float upperLimit;


    private void Start()
    {
        offset = new Vector2(Mathf.Abs(offset.x), offset.y);
        FindPlayer(isLeft);
    }

    public void FindPlayer(bool playerIsLeft)
    {
        player = FindObjectOfType<Hero>().transform;
        lastX = Mathf.RoundToInt(player.position.x);
        if(playerIsLeft)
        {
            transform.position = new Vector3(player.position.x - offset.x, player.position.y - offset.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
        }
    }


    private void Awake()
    {
        if (!player)
            player = FindObjectOfType<Hero>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(player)
        {
            int currentX = Mathf.RoundToInt(player.position.x);
            if (currentX > lastX) isLeft = false;
            else if (currentX < lastX) isLeft = true;
            lastX = Mathf.RoundToInt(player.position.x);

            Vector3 target;
            if(isLeft)
            {
                target = new Vector3(player.position.x - offset.x, player.position.y + offset.y, transform.position.z);

            }
            else
            {
                target = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);

            }

            Vector3 currentPosition = Vector3.Lerp(transform.position, target, dumping * Time.deltaTime);
            transform.position = currentPosition;
        }


        //pos = player.position;
        //pos.z = -10f;
        //pos.y = 0;//+= 1f;
        
        //transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * dumping);

        transform.position = new Vector3
            (
            Mathf.Clamp(transform.position.x, rightLimit, leftLimit),
            Mathf.Clamp(transform.position.y, upperLimit, bottomLimit),
            transform.position.z
            );
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(new Vector2(leftLimit, upperLimit), new Vector2(rightLimit, upperLimit));
        Gizmos.DrawLine(new Vector2(leftLimit, bottomLimit), new Vector2(rightLimit, bottomLimit));
        Gizmos.DrawLine(new Vector2(leftLimit, upperLimit), new Vector2(leftLimit, bottomLimit));
        Gizmos.DrawLine(new Vector2(rightLimit, upperLimit), new Vector2(rightLimit, bottomLimit));

    }
}
