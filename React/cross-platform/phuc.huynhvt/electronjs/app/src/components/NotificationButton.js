import React from "react";

const NotificationButton = () => {
  const electronAPI = window.electronAPI;

  const showNotification = () => {
    electronAPI.showNotification("Hello! This is your notification.");
  };

  return (
    <div className="wrap">
      <h3>3/ Notification: Click button to show notification from Electron</h3>
      <div className="btn-wrap">
        <button className="button" onClick={showNotification}>
          Show Notification
        </button>
      </div>
    </div>
  );
};

export default NotificationButton;
