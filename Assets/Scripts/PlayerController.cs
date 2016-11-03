using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	public float bulletSpeed = 6f;

	private float x, z;
	private MeshRenderer meshRenderer;

	void Update () {

		if (!isLocalPlayer)
			return; 
	
		x = Input.GetAxis ("Horizontal");
		z = Input.GetAxis ("Vertical");

		if (Input.GetKeyDown (KeyCode.Space)) {
			CmdFire ();
		}

	}

	[Command]
	void CmdFire() {

		var bullet = (GameObject) Instantiate (
			             bulletPrefab,
			             bulletSpawn.position,
			             bulletSpawn.rotation);

		bullet.GetComponent<Rigidbody> ().velocity = 
			bullet.transform.forward * bulletSpeed;
		
		NetworkServer.Spawn (bullet);
		Destroy (bullet, 2f);
	}

	void FixedUpdate() {

		if (!isLocalPlayer)
			return;
		
		float x_Movement = x * Time.deltaTime * 150f;
		float z_Movement = z * Time.deltaTime * 3f;
		
		transform.Rotate (0, x_Movement, 0);
		transform.Translate (0, 0, z_Movement);
	}
		
	public override void OnStartLocalPlayer() {
		meshRenderer = GetComponent<MeshRenderer> ();
		meshRenderer.material.color = Color.blue;
	}
}
