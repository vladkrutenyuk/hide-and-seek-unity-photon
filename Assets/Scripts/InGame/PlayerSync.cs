using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSync : Photon.MonoBehaviour
{
	[SerializeField]
	private new PhotonView photonView;
	public Camera fpsCam;
	public GameObject selfInfo;

	GameObject[] players;

	private Vector3 selfPos;
	private Quaternion selfRot;

	[HideInInspector]
	public bool isMine;

	private void Awake ()
	{
		if (photonView.isMine)
		{
			fpsCam.gameObject.SetActive(true);;
			selfInfo.SetActive(false);
		}
		// Set Player's name as name of this gameobject
		this.gameObject.name = photonView.owner.NickName;

		// Set true camera for players' billboards
		players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject player in players)
		{
			if (player.GetPhotonView().owner.IsLocal)
				return;
			player.GetComponentInChildren<CameraFacingBillboards>().SetCamera(fpsCam);
		}
	}
	
	
	private void Update ()
	{
		if (photonView.isMine)
			isMine = true;
		else
		{
			isMine = false;
			smoothNetMovement();
		}	
	}

	private void smoothNetMovement()
	{
		transform.position = Vector3.Lerp(transform.position, selfPos, Time.deltaTime * 8);
	}

	private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
			stream.SendNext(transform.position);
		else
			selfPos = (Vector3)stream.ReceiveNext();
	}
}
