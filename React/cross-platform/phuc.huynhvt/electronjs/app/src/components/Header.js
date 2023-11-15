import React, { useState, useEffect } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSun, faMoon } from "@fortawesome/free-solid-svg-icons";
import "./Header.scss";

function Header({ handleDarkMode }) {
  const checkOnlineStatus = () => navigator.onLine;
  const [isOnline, setIsOnline] = useState(checkOnlineStatus());
  const [isDarkMode, setIsDarkMode] = useState(true);
  const electronAPI = window.electronAPI;

  useEffect(() => {
    const handleStatusChange = () => setIsOnline(checkOnlineStatus());

    window.addEventListener("online", handleStatusChange);
    window.addEventListener("offline", handleStatusChange);

    return () => {
      window.removeEventListener("online", handleStatusChange);
      window.removeEventListener("offline", handleStatusChange);
    };
  }, []);

  const toggleDarkMode = () => {
    electronAPI.toggle().then((isDarkMode) => {
      setIsDarkMode(isDarkMode);
      handleDarkMode(isDarkMode);
    });
  };

  return (
    <>
      <div className={`header ${isDarkMode ? "dark-mode" : "light-mode"}`}>
        <h3>Hi {electronAPI?.username}, </h3>
        <nav>
          <ul>
            <li>
              <a href="/about">
                You are:{" "}
                {isOnline ? (
                  <span className="online-text">Online</span>
                ) : (
                  <span className="offline-text">Offline</span>
                )}
              </a>
            </li>
          </ul>
        </nav>
      </div>
      <div className="wrap">
        <h3>1/ Dark Mode</h3>{" "}
        <button
          className={`${isDarkMode ? "dark-mode-toggle" : "light-mode-toggle"}`}
          onClick={toggleDarkMode}
          aria-pressed={isDarkMode}
        >
          <FontAwesomeIcon icon={isDarkMode ? faSun : faMoon} />
        </button>
      </div>
    </>
  );
}

export default Header;
