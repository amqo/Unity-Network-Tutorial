using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class HealthController : NetworkBehaviour {

	public RectTransform healthBar;

	private const int maxHealth = 100;

	[SyncVar(hook = "OnChangeHealth")]
	private int currentHealth = maxHealth;

	private NetworkStartPosition[] spawnPoints;

	void Start() {
		if (isLocalPlayer) {
			spawnPoints = FindObjectsOfType<NetworkStartPosition> ();
		}
	}

	public void TakeDamage(int amount) {

		if (!isServer)
			return;

		currentHealth -= amount;
		
		if (currentHealth <= 0) {

			if (gameObject.tag.Equals ("Enemy")) {
				Destroy (gameObject);
				return;
			}

			currentHealth = maxHealth;
			RpcRespawn ();
		}
	}

	[ClientRpc]
	void RpcRespawn() {
		if (isLocalPlayer) {
			
			Vector3 spawnPoint = Vector3.zero;
			if (spawnPoints != null && spawnPoints.Length > 0) {
				spawnPoint = spawnPoints [Random.Range (0, spawnPoints.Length)].transform.position;
			}

			transform.position = spawnPoint;
		}
	}
		
	void OnChangeHealth(int currentHealth) {
		healthBar.sizeDelta = new Vector2 (
			currentHealth,
			healthBar.sizeDelta.y);
	}
}
