using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRegistration : MonoBehaviour {

    public UnityEngine.UI.Text displayNameInput, userNameInput, passwordInput;

	public void RegisterPlayerBttn()
    {
        Debug.Log("Registering player...");
        new GameSparks.Api.Requests.RegistrationRequest()
            .SetDisplayName(displayNameInput.text)
            .SetUserName(userNameInput.text)
            .SetPassword(passwordInput.text)
            .Send((response) =>
           {
               if (!response.HasErrors)
               {
                   Debug.Log("Player Registered \n Display Name: " + response.DisplayName);
               }
               else
               {
                   Debug.Log("Error Registering Player... \n" + response.Errors.JSON.ToString());
               }
           });
    }
}
