using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShootController : MonoBehaviour {

	public float fireRate = 0.25f;										
	public float weaponRange = 50f;
	public int gunDamage = 1;									
	public Transform gunEnd;
	public Text ammoText;
	public int ammo;

	private float nextFire;	
	private AudioSource gunAudio;
	private Camera fpsCam;	
	private FirstPersonController player;
	private LineRenderer laserLine;
	private readonly WaitForSeconds shotDuration = new WaitForSeconds(0.07f);							

	void Awake () {

		BlankAmmo ();

	}

	void Start () 
	{

		laserLine = GetComponent<LineRenderer>();

		gunAudio = GetComponent<AudioSource>();

		fpsCam = GetComponentInParent<Camera>();

		player = GetComponentInParent<FirstPersonController> ();

	}
	

	void Update ()
	{
		// Check if the player has pressed the fire button and if enough time has elapsed since they last fired
		if (Input.GetButtonDown ("Fire") && Time.time > nextFire) {

			// Update the time when our player can fire next
			nextFire = Time.time + fireRate;

			// Set the start position for our visual effect for our laser to the position of gunEnd
			laserLine.SetPosition (0, gunEnd.position);

			// Start our shotEffect coroutine to turn our laser line on and off
			StartCoroutine (ShotEffect ());

			ammo -= 1;

            //Keeps track of the ammo and updates the display
			if (ammo != 0) {

				ammoText.text = "Ammo: " + ammo;

			} else {
			
				BlankAmmo ();
			
			}

            //Calls the raycast method to see if we hit anything
			if (player.Raycast (weaponRange)) {

                //Calls the damage method if we hit something
				Damage ();
			
			} else {

				// If we did not hit anything, set the end of the line to a position directly in front of the camera at the distance of weaponRange
				laserLine.SetPosition (1, gunEnd.position + (fpsCam.transform.forward * weaponRange));

			}
		}
	}

	public void BlankAmmo () {
	
        //Sets the ammo text to blank
		ammoText.text = "";
	
	}

	void Damage () {
	
		// Set the end position for our laser line 
		laserLine.SetPosition (1, player.hit.point);

		// Get a reference to a health script attached to the collider we hit
		HPController enemyHealth = player.hit.collider.GetComponent<HPController>();

		// If there was a health script attached
		if (enemyHealth != null) {
			
			// Call the damage function sof that script, passing in our gunDamage variable
			enemyHealth.Damage (gunDamage);

		}
	}

	private IEnumerator ShotEffect() {
		
		// Play the shooting sound effect
		gunAudio.Play ();

		// Turn on our line renderer
		laserLine.enabled = true;

		//Wait for .07 seconds
		yield return shotDuration;

		// Deactivate our line renderer after waiting
		laserLine.enabled = false;

	}

}