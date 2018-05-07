using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private Vector2 velocity;

    public float smoothTimeY;
    public float smoothTimeX;
    private BoxCollider2D cameraBox;

    public GameObject target;

	private void Start()
	{
        cameraBox = GetComponent<BoxCollider2D>();
	}

	private void FixedUpdate()
	{
        AspectRatioBoxChange();
        FollowPlayer();
	}

    private void AspectRatioBoxChange()
    {
        //16:10
        if (Camera.main.aspect >= 1.6f && Camera.main.aspect < 1.7f)
        {
            cameraBox.size = new Vector2(23, 14.3f);
        }
        //16:9
        if (Camera.main.aspect >= 1.7f && Camera.main.aspect < 1.8f)
        {
            cameraBox.size = new Vector2(25.47f, 14.3f);
        }
        //5:4
        if (Camera.main.aspect >= 1.25f && Camera.main.aspect < 1.3f)
        {
            cameraBox.size = new Vector2(18, 14.3f);
        }
        //4:3
        if (Camera.main.aspect >= 1.3f && Camera.main.aspect < 1.4f)
        {
            cameraBox.size = new Vector2(19.13f, 14.3f);
        }
        //3:2
        if (Camera.main.aspect >= 1.5f && Camera.main.aspect < 1.6f)
        {
            cameraBox.size = new Vector2(21.6f, 14.3f);
        }
    }

    void FollowPlayer()
    {
        float posX = Mathf.SmoothDamp(transform.position.x, target.transform.position.x, ref velocity.x, smoothTimeX);
        float posY = Mathf.SmoothDamp(transform.position.y, target.transform.position.y, ref velocity.y, smoothTimeY);


        BoxCollider2D cameraBoundaryBox = GameObject.Find("CameraBoundary").GetComponent<BoxCollider2D>();
        float minX = cameraBoundaryBox.bounds.min.x + cameraBox.size.x/2;
        float maxX = cameraBoundaryBox.bounds.max.x - cameraBox.size.x/2;

        if (GameObject.Find("CameraBoundary"))
        {
            transform.position = new Vector3(Mathf.Clamp(posX, cameraBoundaryBox.bounds.min.x + cameraBox.size.x/2, 
                                                         cameraBoundaryBox.bounds.max.x - cameraBox.size.x/2),
                                             Mathf.Clamp(posY, cameraBoundaryBox.bounds.min.y
                                                         + cameraBox.size.y / 2, cameraBoundaryBox.bounds.max.y
                                                         - cameraBox.size.y / 2),
                                             transform.position.z);

        //    if (minX >  maxX)
        //    {
        //transform.position = new Vector3(minX, transform.position.y, transform.position.z);
            //}

        }
        else
        {
            transform.position = new Vector3(posX, posY, transform.position.z);
        }

    }


   


}