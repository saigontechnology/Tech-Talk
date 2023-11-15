import "./App.scss";
import { useState, useEffect } from "react";
import Header from "./components/Header";
import NotificationButton from "./components/NotificationButton";
import FileDragDrop from "./components/FileDragDrop";
import WebEmbedded from "./components/WebEmbedded";
import DeepLinkHandler from "./components/DeepLinkHandler";

function App() {
  const electronAPI = window.electronAPI;
  const [isDarkMode, setIsDarkMode] = useState(true);

  const handleDarkMode = (result) => {
    setIsDarkMode(result);
  };

  return (
    <div className={`App ${isDarkMode ? "dark-mode" : "light-mode"}`}>
      {/* 1/ Example dark mode */}
      <Header handleDarkMode={handleDarkMode} />
      {/* 2/ Example get info of system */}
      <h3>2/ Get system info from Electron</h3>
      <div id="text" style={{ textAlign: "center", padding: "1rem" }}>
        <table id="info">
          <thead>
            <tr>
              <td colSpan={2} style={{ textAlign: "center" }}>
                Information
              </td>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td>OS</td>
              <td>{electronAPI?.osVersion()}</td>
            </tr>
            <tr>
              <td>HomeDir</td>
              <td>{electronAPI?.homeDir()}</td>
            </tr>
            <tr>
              <td>Author</td>
              <td>{electronAPI?.author}</td>
            </tr>
          </tbody>
        </table>
      </div>
      {/* 3/ Example push notification from Electron */}
      <NotificationButton />
      {/* 4/ Example Web Embedded */}
      <WebEmbedded />
      {/* 5/ Example Drag & Drop */}
      <FileDragDrop />
      {/* 6/ Example Deep link */}
      {/* <DeepLinkHandler /> */}
    </div>
  );
}
export default App;
