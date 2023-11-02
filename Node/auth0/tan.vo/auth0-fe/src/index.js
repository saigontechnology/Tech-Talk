import React from "react";
import ReactDOM from "react-dom/client";
import "./index.css";
import UniversalLogin from "./components/UniversalLogin";
import reportWebVitals from "./reportWebVitals";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import { Auth0Provider } from "@auth0/auth0-react";
import Authentication from "./components/Authentication";

const router = createBrowserRouter([
  {
    path: "/",
    element: (
      <Auth0Provider
        domain="universal-login.au.auth0.com"
        clientId="0brzbsWqBx1Jm014DZXO7L8BKag281iq"
        authorizationParams={{
          redirect_uri: window.location.origin,
        }}
      >
        <UniversalLogin />{" "}
      </Auth0Provider>
    ),
  },
  {
    path: "/authentication",
    element: <Authentication />,
  },
]);

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(<RouterProvider router={router} />);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
