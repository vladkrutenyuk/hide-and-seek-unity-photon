using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	public GameObject pauseMenu;

	
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			pauseMenu.SetActive(!pauseMenu.activeSelf);
			if (pauseMenu.activeSelf)
			{
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
			else
			{
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
			}
		}
	}

	public void OnClick_Leave()
	{
		pauseMenu.SetActive(false);
		PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.player);
		PhotonNetwork.Disconnect();
		SceneManager.LoadScene(0);
	}

	public void OnClick_Quit()
	{
		Application.Quit();
	}
}
