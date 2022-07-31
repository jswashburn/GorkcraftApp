import { useEffect, useState } from "react";
import jwt_decode from "jwt-decode";

export default function App() {
  const [user, setUser] = useState<any>(null);

  function handleCallbackResponse(
    response: google.accounts.id.CredentialResponse
  ) {
    var userObject = jwt_decode(response.credential);
    setUser(userObject);
    console.log(userObject);
  }

  useEffect(() => {
    /* global google */
    google.accounts.id.initialize({
      client_id:
        "306912146319-97vc6au4h8jp5himt0jgtq3c6uss832o.apps.googleusercontent.com",
      callback: handleCallbackResponse,
    });

    google.accounts.id.prompt();
  }, []);

  return (
    <div className="App">
      {user && (
        <div className="Profile">
          <img src={user.picture} />
          <h2>Signed in!</h2>
        </div>
      )}
    </div>
  );
}
