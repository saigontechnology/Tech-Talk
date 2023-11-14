import React, { useEffect } from "react";

const DeepLinkHandler = () => {
  useEffect(() => {
    const handleDeepLink = (event, url) => {
      console.log("Received deep link:", url);
    };

    window.electronAPI?.receiveDeepLink(handleDeepLink);
    return () => {
      window.electronAPI.removeDeepLink(handleDeepLink);
    };
  }, []);

  return (
    <>
      <h3>6/ Deep-link from Electron </h3>
      <h5>
        Open your browser and use url : electronExample://open to open Electron
        app
      </h5>
    </>
  );
};

export default DeepLinkHandler;
