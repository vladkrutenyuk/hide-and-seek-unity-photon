using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFacingBillboards : MonoBehaviour 
{
	Camera fpsCam;

	public float FixedSize = .003f;
	public Text playerName;

	private void Awake()
	{
		fpsCam = Camera.main;
		PhotonPlayer owner = GetComponentInParent<PhotonView>().owner;

		playerName.text = owner.NickName;
		if (owner.GetTeam() == PunTeams.Team.red)
			playerName.color = Color.red;
		else if (owner.GetTeam() == PunTeams.Team.blue)
			playerName.color = Color.blue;
	}

	public void SetCamera(Camera camera)
	{
		fpsCam = camera;
	}

	void Update()
	{
		// Set true constant rotation
		transform.LookAt
			(transform.position + fpsCam.transform.rotation * Vector3.forward, fpsCam.transform.rotation * Vector3.up);

		// Set true constant scale
		float distance = (fpsCam.transform.position - transform.position).magnitude;
		float size = distance * FixedSize * fpsCam.fieldOfView;
		transform.localScale = Vector3.one * size;
		//transform.forward = transform.position - fpsCam.transform.position;
	}
}
