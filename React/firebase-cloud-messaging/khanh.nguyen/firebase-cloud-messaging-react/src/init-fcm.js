import * as firebase from "firebase/app";
import "firebase/messaging";

const initializedFirebaseApp = firebase.initializeApp({
  messagingSenderId: "SENDER_ID",
});

const messaging = initializedFirebaseApp.messaging();

messaging.usePublicVapidKey("VAPID_KEY");

export { messaging };
