using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using extOSC;

public class OscProcess : MonoBehaviour
{
    public AsteroidDodger asteroidDodger;
    public OSCReceiver oscReceiver;
    public OSCTransmitter oscTransmitter;

    public int photoInMin = 0;
    public int photoInMax = 1023;
    public float photoOutMin = -4.0f;
    public float photoOutMax = 4.0f;

    void Start()
    {
        oscReceiver.Bind("/photo", TraiterMessagePhoto);
    }

    void TraiterMessagePhoto(OSCMessage message)
    {
        // Validez qu'il y a bien le nombre attendu d'arguments (1 dans l'exemple) :
        if (message.Values.Count != 1)
        {
            Debug.Log("Le message " + message.Address + " n'a pas le bon nombre d'arguments");
            return; // Quitte la fonction sans exécuter la suite
        }

        // Vérifiez que l'argument est du type attendu (int dans l'exemple) :
        if (message.Values[0].Type != OSCValueType.Int)
        {
            Debug.Log("Le premier argument du message " + message.Address + " n'est pas un entier");
            return; // Quitte la fonction sans exécuter la suite
        }

        // Récupérer la valeur de l'argument :
        int valeur = message.Values[0].IntValue;

        // Ajustement de la valeur
        float ajustee = (
            ((float)valeur - photoInMin) / (photoInMax - photoInMin) * (photoOutMax - photoOutMin) + photoOutMin
        );

        // DESTINATION
        asteroidDodger.SetShipY(ajustee);
    }

    void Update()
    {
        if ( asteroidDodger.GetShipIsVisible() ) {
          var oscMessage = new OSCMessage("/lumiere"); 
          oscMessage.AddValue( OSCValue.Int(1) ); // Ajoute l'entier 1
          oscTransmitter.Send( oscMessage ); // Envoyer le message 
        } else {
          var oscMessage = new OSCMessage("/lumiere"); 
          oscMessage.AddValue( OSCValue.Int(0) ); // Ajoute l'entier 0
          oscTransmitter.Send( oscMessage ); // Envoyer le message 
        }
    }
}