using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarScript : MonoBehaviour {


	//Holders to identify the wheel colliders being used for the car
	//Can pass in the rear wheels to make the car RWD alternatively
	public WheelCollider FrontLWheel;
	public WheelCollider FrontRWheel;

	/*
	 * This Required some research
	 * TORQUE and RPM are the MEASURED quantities of engine output. POWER is CALCULATED from torque and RPM, by the following equation. 
	 * HP (Horsepower) = Torque x RPM ÷ 5252 
	 * An engine produces POWER by applying a TORQUE on a rotating shaft. 
	 * 
	 * A transmission works by providing a ratio between the speed of the engine to the speed of the car, this allows different ratios between RPM and Vehicle Speed
	 * 
	 * So when a car shifts, and the ratio changes from 1.4 to 1.2 with a constant car speed the rpm is reduced by half.
	 * 
	 */

	//Allows us to keep track of the current gear as we accellerate
	public int CurrentGear = 0;

	//Different Engines have different gear ratios, I may use this later to make different cars handle differently
	public float[] gearRatio;


	//the current RPM of the engine
	private float EngineRPM = 0.0F;

	//The 
	public float EngineTorque = 0;
	public float MaxEngineRPM = 3000.0F;
	public float MinEngineRPM = 1000.0F;


	// Use this for initialization
	void Start () {

		rigidbody.centerOfMass += new Vector3(0.0F, -0.75F, 0.25F);

			
		}
		
		// Update is called once per frame
	void Update () {

		// I found most of this code on the internet
	
		// Compute the engine RPM based on the average RPM of the two wheels, then call the shift gear function
		EngineRPM = (FrontLWheel.rpm + FrontRWheel.rpm)/2 * gearRatio[CurrentGear];
		ShiftGears();
		
		// set the audio pitch to the percentage of RPM to the maximum RPM plus one, this makes the sound play
		// up to twice it's pitch, where it will suddenly drop when it switches gears.
		//audio.pitch = Mathf.Abs(EngineRPM / MaxEngineRPM) + 1.0 ;
		// this line is just to ensure that the pitch does not reach a value higher than is desired.
		//if ( audio.pitch > 2.0 ) {
		//	audio.pitch = 2.0;
		//}
		
		// finally, apply the values to the wheels.	The torque applied is divided by the current gear, and
		// multiplied by the user input variable.
		FrontLWheel.motorTorque = EngineTorque / gearRatio[CurrentGear] * Input.GetAxis("Vertical");
		FrontRWheel.motorTorque = EngineTorque / gearRatio[CurrentGear] * Input.GetAxis("Vertical");
		
		// the steer angle is an arbitrary value multiplied by the user input.
		FrontLWheel.steerAngle = 10 * Input.GetAxis("Horizontal");
		FrontRWheel.steerAngle = 10 * Input.GetAxis("Horizontal");
	}

	void ShiftGears() {
		// this funciton shifts the gears of the vehcile, it loops through all the gears, checking which will make
		// the engine RPM fall within the desired range. The gear is then set to this "appropriate" value.
		int AppropriateGear;
		if ( EngineRPM >= MaxEngineRPM ) {
			AppropriateGear= CurrentGear;
			
			for ( int i = 0; i < gearRatio.Length; i ++ ) {
				if ( FrontLWheel.rpm * gearRatio[i] < MaxEngineRPM ) {
					AppropriateGear = i;
					break;
				}
			}
			
			CurrentGear = AppropriateGear;
		}
		
		if ( EngineRPM <= MinEngineRPM ) {
			AppropriateGear = CurrentGear;
			
			for ( int j = gearRatio.Length - 1; j >= 0; j -- ) {
				if ( FrontLWheel.rpm * gearRatio[j] > MinEngineRPM ) {
					AppropriateGear = j;
					break;
				}
			}
			
			CurrentGear = AppropriateGear;
		}
	}


}
