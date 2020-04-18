using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public static MoveCamera Instance { get; private set; }

    new public Camera camera;
    public Transform player;

    private void Awake()
    {
        if( Instance == null )
        {
            Instance = this;
        }
        camera = GetComponentInChildren<Camera>();
    }
    void Update()
    {
        transform.position = player.transform.position;
    }
}