using UnityEngine;

public class Teleport : MonoBehaviour {

	public GameObject otherPortal;

	void OnTriggerEnter(Collider other) {

        //If the player enters the portal trigger move it to slightly infront of the other portal
		if (other.tag == "Player") {
		
			other.transform.position = otherPortal.transform.position + otherPortal.transform.forward * 2;

		}
	
	}
}
