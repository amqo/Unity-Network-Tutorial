using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	public int bulletDamage = 10;

	void OnTriggerEnter(Collider collider) {

		var other = collider.gameObject;

		if (!other.tag.Equals("Player") && !other.tag.Equals("Enemy"))
			return;

		var health = other.GetComponent<HealthController> ();
		if (health != null)
			health.TakeDamage (bulletDamage);
		
		Destroy (gameObject);
	}
}
