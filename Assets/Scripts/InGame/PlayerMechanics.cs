using UnityEngine;


public class PlayerMechanics : MonoBehaviour
{
	[Space]
	[Header("General floats")]
	public float speedMove = 5f;
	public float speedJump = 3f;
    public float speedClimb = 4f;
    public float climbLimit = 1f;
	public float g = 9.81f;
	public float mouseSensitivity = 3f;
	public float minRotX = -70f;
	public float maxRotX = 80f;
	float currentRotationX;

	[Space]
	[HideInInspector]
	public Vector3 velocity = Vector3.zero;
	float moveX, moveY, moveZ;
	bool uCanDoubleJump = true;

	[Header("General components")]
	public CharacterController controller;
	public Camera fpsCam;
	public PlayerSync plNet;

	void Update ()
	{
		if (!plNet.isMine)
			return;
		Controller();
	}

	void Controller()
	{
		if (controller != null)
		{
			#region Основная физика движения
			if (IsGrounded())
			{
				#region Наземное движение
				uCanDoubleJump = true;
				climbLimit = 1f;

				moveX = Mathf.Lerp(moveX, Input.GetAxisRaw("Horizontal"), Time.deltaTime * 10);
				moveZ = Mathf.Lerp(moveZ, Input.GetAxisRaw("Vertical"), Time.deltaTime * 10);
				velocity = new Vector3(moveX, 0, moveZ);
				velocity = transform.TransformDirection(velocity);
				ConsiderSlopeForVelocityY();

				if (Mathf.Abs(velocity.magnitude) > 1)
				{
					velocity = velocity.normalized * speedMove;
				}
				else
				{
					velocity *= speedMove;
				}

				//isJumping = false;
				if (Input.GetKeyDown(KeyCode.Space))
				{
					velocity.y = speedJump;
					//isJumping = true;
				}

				Vector3 center = transform.position + (Vector3.up * controller.radius);
				Debug.DrawRay(center, velocity, Color.blue);
				// Debug.Log(velocity.magnitude);
				#endregion Наземное движение

			}
			else
			{
				#region Движение в воздухе

				if (uCanDoubleJump && Input.GetKeyDown(KeyCode.Space))
				{
					uCanDoubleJump = false;
					velocity.y = speedJump;
				}

				velocity.y -= Time.deltaTime * g;
				moveX = Mathf.Lerp(moveX, Input.GetAxisRaw("Horizontal"), Time.deltaTime * 3);
				moveZ = Mathf.Lerp(moveZ, Input.GetAxisRaw("Vertical"), Time.deltaTime * 3);
				moveY = velocity.y;

				velocity = new Vector3(moveX, 0, moveZ);
				velocity = transform.TransformDirection(velocity);
				if (Mathf.Abs(velocity.magnitude) > 1)
				{
					velocity = velocity.normalized * speedMove;
				}
				else
				{
					velocity *= speedMove;
				}

				velocity.y = moveY;
				#endregion Движение в воздухе
			}

			#region Бег по стенам
			if (wallForward())
			{
				if (!roofUp() && Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.W))
				{
					if (climbLimit > 0)
					{
						velocity.y = speedClimb;
						velocity.x = 0;
						velocity.z = 0;
						climbLimit -= Time.deltaTime;
					}

				}
				else
				{
					climbLimit = 0;
				}
			}
			#endregion Бег по стенам

			controller.Move(velocity * Time.deltaTime);
			#endregion

			#region Вращение игрока вокруг оси Y (влево-вправо)
			Vector3 rotationY = new Vector3(0, Input.GetAxis("Mouse X"), 0) * mouseSensitivity;
			controller.transform.Rotate(rotationY);
			#endregion

			#region Вращение камеры вокруг оси X (вверх-вниз)
			if (fpsCam != null)
			{
				float rotationX = Input.GetAxis("Mouse Y") * mouseSensitivity;
				currentRotationX -= rotationX;
				currentRotationX = Mathf.Clamp(currentRotationX, minRotX, maxRotX);
				fpsCam.transform.localEulerAngles = new Vector3(currentRotationX, 0, 0);
			}
			#endregion
		}
	}

	public bool wallForward()
    {
        RaycastHit wallDetector;
        return Physics.Raycast(transform.position + (Vector3.up * controller.radius), gameObject.transform.forward, out wallDetector, controller.radius + 0.1f);
    }

    public bool roofUp()
    {
        RaycastHit roofDetector;
        Vector3 center = transform.position + Vector3.up * controller.height;
        return Physics.SphereCast(center, controller.radius, Vector3.up, out roofDetector, 0.3f);
    }

    public bool IsGrounded()
	{
		RaycastHit sphereHit;
		Vector3 center = transform.position + (Vector3.up * controller.radius);
		return Physics.SphereCast(center, controller.radius, Vector3.down, out sphereHit, 0.05f);
	}

	public float CosAngleBetweenVectors3d (Vector3 v1, Vector3 v2)
	{
		float cos_a = (v1.x * v2.x + v1.y * v2.y + v1.z * v2.z) /
			(Mathf.Sqrt(v1.x * v1.x + v1.y * v1.y + v1.z * v1.z) * Mathf.Sqrt(v2.x * v2.x + v2.y * v2.y + v2.z * v2.z));
		return cos_a;
	}

	public void ConsiderSlopeForVelocityY()
	{
		RaycastHit hit;
		Vector3 center = transform.position + (Vector3.up * controller.radius);
		if (Physics.SphereCast(center, controller.radius, Vector3.down, out hit, 0.1f))
		{
			//float cosA = CosAngleBetweenVectors3d(hit.normal, velocity); // Косинус угла между нормалью и направлением скорости	
			float A = Mathf.Acos(CosAngleBetweenVectors3d(hit.normal, velocity)) * 180f / Mathf.PI; // Сам угл в градусах
			float B;
			//Debug.Log("угол = " + A);
			Debug.DrawRay(hit.point, hit.normal, Color.red);
			
			if(A > 91f & A <= 135f) // Подъем по склону
			{
				B = A - 90f;
				velocity.y = Mathf.Tan(B * Mathf.PI / 180f) * velocity.magnitude;

			}else if (A > 45f & A < 89f) // Спуск по склону
			{	
				B = 90f - A;
				velocity.y = -Mathf.Tan(B * Mathf.PI / 180f) * velocity.magnitude * 1.1f;
			}
		}
	}
}
