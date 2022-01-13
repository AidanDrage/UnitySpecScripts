using UnityEngine;


public class FirstPersonController : MonoBehaviour {
	
	public float sensitivity = 2f;
	public float speed = 2f;
	public float runSpeed = 1.5f;
	public float jumpHeight = 0.6f;
	public Camera mainCamera;

    private const float gravity = 9.8f;
	private float rotationY;
	private CharacterController player;
	private Vector3 velocity;

	void Start(){

        //Finds and sets the plyer
		player = GetComponent<CharacterController>();

        //Locks the cursor to the game window
		Cursor.lockState = CursorLockMode.Locked;		
	}

	void Update(){

		UnlockCursor();

		Gravity();

		Movement();

		Look();

		Run();

		Jump();

		player.Move(velocity * Time.deltaTime);
	}

	void Gravity(){
		if(!player.isGrounded) {
			velocity.y -= gravity * Time.deltaTime;
		}
	}

	void Movement(){
		
		//Get input from WASD
		var move = Input.GetAxis ("Vertical") * speed;
		var strafe = Input.GetAxis ("Horizontal") * speed;

		//Set the movement velocity
		velocity.x = strafe;
		velocity.z = move;

		//Make movement happen in the correct orientation
		velocity = transform.rotation * velocity;		
	}

	void Look(){
        //Get input from Mouse
		var rotationX = Input.GetAxis("Mouse X") * sensitivity;
		rotationY -= Input.GetAxis("Mouse Y") * sensitivity;
		rotationY = Mathf.Clamp(rotationY, -60f, 60f);

		//Rotate the character
		transform.Rotate(0, rotationX, 0);

		//Rotate the camera on the Y axis
		mainCamera.transform.localRotation = Quaternion.Euler(rotationY, 0, 0);
	}
		
	void Run(){
	
        //Increases speed if the player presses the run key
		if (Input.GetButtonDown("Run")){

			speed = speed * runSpeed;
			
		}

        //Decreases speed if the player releses the run key
        if (Input.GetButtonUp("Run")){

			speed = speed / runSpeed;

		}
	}

	void Jump(){
		if (Input.GetButtonDown("Jump") && player.isGrounded)
        {
            velocity.y = 0 + (jumpHeight * gravity);
        }
	}

	void UnlockCursor(){

        //Unlocks the cursor from the game window when you press cancel
		if (Input.GetButtonDown("Cancel")){
			Cursor.lockState = CursorLockMode.None;
		}
	}
}
