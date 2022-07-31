import { useEffect, useState } from "react";
import jwt_decode from "jwt-decode";

export default function App() {
  const [user, setUser] = useState<any>(null);

  function handleCallbackResponse(
    response: google.accounts.id.CredentialResponse
  ) {
    console.log(response.credential);
    var userObject = jwt_decode(response.credential);
    setUser(userObject);
    console.log(userObject);
  }

  useEffect(() => {
    google.accounts.id.initialize({
      client_id:
        "306912146319-97vc6au4h8jp5himt0jgtq3c6uss832o.apps.googleusercontent.com",
      callback: handleCallbackResponse,
    });

    if (user) return;

    google.accounts.id.prompt((notification) => {
      if (notification.isNotDisplayed() || notification.isSkippedMoment()) {
        google.accounts.id.renderButton(document.getElementById("signInDiv")!, {
          type: "standard",
          shape: "pill",
          size: "large",
          theme: "filled_blue",
          text: "continue_with",
          logo_alignment: "center",
        });
      }
    });
  }, []);

  return (
    <div className="App">
      {user && (
        <div className="Profile">
          <img src={user.picture} />
          <h2>Signed in!</h2>
        </div>
      )}
      {!user && <div id="signInDiv"></div>}
    </div>
  );
}
