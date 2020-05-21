using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DebugTenjin : BaseTenjin {

	public override void Connect(){
		Debug.Log ("Connecting " + ApiKey);
	}

	public override void Connect(string deferredDeeplink){
		Debug.Log ("Connecting with deferredDeeplink " + deferredDeeplink);
	}

	public override void Init(string apiKey){
		Debug.Log ("Initializing  with api key");
	}

	public override void InitWithSharedSecret(string apiKey, string sharedSecret)
	{
		Debug.Log("Initializing with shared secret");
	}

	public override void InitWithAppSubversion(string apiKey, int appSubversion)
	{
		Debug.Log("Initializing with shared secret + subversion: " + appSubversion);
	}

	public override void InitWithSharedSecretAppSubversion(string apiKey, string sharedSecret, int appSubversion)
	{
		Debug.Log("Initializing with shared secret + subversion: " + appSubversion);
	}

	public override void SendEvent (string eventName){
		Debug.Log ("Sending Event " + eventName);
	}

	public override void SendEvent (string eventName, string eventValue){
		Debug.Log ("Sending Event " + eventName + " : " + eventValue);
	}

	public override void Transaction(string productId, string currencyCode, int quantity, double unitPrice, string transactionId, string receipt, string signature){
		Debug.Log ("Transaction " + productId + ", " + currencyCode + ", " + quantity + ", " + unitPrice + ", " + transactionId + ", " + receipt + ", " + signature);
	}

	public override void GetDeeplink(Tenjin.DeferredDeeplinkDelegate deferredDeeplinkDelegate) {
		Debug.Log ("Sending DebugTenjin::GetDeeplink");
	}

	public override void OptIn(){
		Debug.Log ("OptIn ");
	}

	public override void OptOut(){
		Debug.Log ("OptOut ");
	}

	public override void OptInParams(List<string> parameters){
		Debug.Log ("OptInParams");
	}

	public override void OptOutParams(List<string> parameters){
		Debug.Log ("OptOutParams" );
	}

	public override void AppendAppSubversion(int subversion)
	{
		Debug.Log("AppendAppSubversion: " + subversion);
	}
}
