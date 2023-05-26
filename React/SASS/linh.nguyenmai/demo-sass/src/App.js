import { useState } from "react";
import Form from "./components/Form";
import "./assets/sass/main.scss";

function App() {
  const [formStatus, setFormStatus] = useState("");

  const onSubmit = () => {
    let counter = 0;

    setInterval(() => {
      if (counter === 100) {
        clearInterval();
        setFormStatus("success");
      } else {
        counter += 1;
        setFormStatus("loading");
      }
    }, 10);
  };

  return (
    <main className="container">
      {formStatus === "loading" ? (
        <div className="loader"></div>
      ) : formStatus === "success" ? (
        <h3>Your information is submitted</h3>
      ) : (
        <Form
          header="Sign in"
          subHeader="Welcome back !!!"
          onSubmit={onSubmit}
          status={formStatus}
        />
      )}
    </main>
  );
}

export default App;
