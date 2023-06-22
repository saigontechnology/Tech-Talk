import React from "react";
import { messaging } from "./init-fcm";
import { compose, lifecycle, withHandlers, withState } from "recompose";

const renderNotification = (notification, i) => (
  <li key={i}>
    {notification.title}:{notification.body}
  </li>
);

const registerPushListener = (pushNotification) =>
  navigator.serviceWorker.addEventListener("message", (body) => {
    console.log("message data=", body);
    pushNotification(
      body.data.data
        ? body.data.data.message
        : body.data["firebase-messaging-msg-data"].notification
    );
  });

const App = ({ token, notifications }) => {
  console.log("notifications=", notifications);

  const handleGetNotification = async () => {
    await fetch("http://localhost:8000", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ token }),
    });
  };

  return (
    <>
      <h1>React + Firebase Cloud Messaging (Push Notifications)</h1>
      <div>
        Current token is: <p>{token}</p>
      </div>
      <ul>
        Notifications List:
        <button onClick={handleGetNotification}>Get Notification</button>
        {notifications.map(renderNotification)}
      </ul>
    </>
  );
};

export default compose(
  withState("token", "setToken", ""),
  withState("notifications", "setNotifications", []),
  withHandlers({
    pushNotification:
      ({ setNotifications, notifications }) =>
      (newNotification) =>
        setNotifications(notifications.concat(newNotification)),
  }),
  lifecycle({
    async componentDidMount() {
      const { pushNotification, setToken } = this.props;

      messaging
        .requestPermission()
        .then(async function () {
          const token = await messaging.getToken();
          setToken(token);
        })
        .catch(function (err) {
          console.log("Unable to get permission to notify.", err);
        });

      registerPushListener(pushNotification);
    },
  })
)(App);
