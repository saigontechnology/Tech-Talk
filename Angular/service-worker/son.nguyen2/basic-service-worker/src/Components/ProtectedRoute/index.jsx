// import { router } from "Constants/constants";
// import { Redirect, Route } from "react-router-dom";
// import { checkLogin } from "Utilise/utilise";

// export const AuthRoute = ({ component: Component, ...rest }) => {
//   <Route
//     {...rest}
//     render={(props) => {
//       return !checkLogin() ? <Component {...props} /> : <Redirect to={router.home} />;
//     }}
//   />;
// };

// export const PrivateRoute = ({ component: Component, ...rest }) => {
//   <Route
//     {...rest}
//     render={(props) => {
//       return checkLogin() ? <Component {...props} /> : <Redirect to={router.login} />;
//     }}
//   />;
// };

import { router } from "Constants/constants";
import React from "react";
import { Redirect, Route } from "react-router-dom";
import { checkLogin } from "Utilise/utilise";

export function PrivateRoute({ component: Component, ...rest }) {
  return (
    <Route
      {...rest}
      render={(props) => {
        return checkLogin() ? <Component {...props} /> : <Redirect to={router.news} />;
      }}
    />
  );
}
export function AuthRoute({ component: Component, ...rest }) {
  return (
    <Route
      {...rest}
      render={(props) => {
        return checkLogin() === null ? (
          <Component {...props} />
        ) : (
          <Redirect to={router.home} />
        );
      }}
    />
  );
}
