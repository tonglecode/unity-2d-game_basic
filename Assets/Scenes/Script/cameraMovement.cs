using UnityEngine;

public class CameraMovement : MonoBehaviour
{
	public float speed;
	public Transform charactor;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		float LerpX = Mathf.Lerp(transform.position.x, charactor.position.x, speed * Time.deltaTime);
		transform.position = new Vector3(LerpX, transform.position.y, transform.position.z);
    }
}
