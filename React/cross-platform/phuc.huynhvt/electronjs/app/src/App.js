import logo from "./logo.svg";
import "./App.css";
import { useState, useEffect } from "react";
function App() {
  const [isOnline, setIsOnline] = useState(navigator.onLine);
  const electronData = window.electron;
  useEffect(() => {
    const handleStatusChange = () => {
      setIsOnline(navigator.onLine);
    };
    window.addEventListener("online", handleStatusChange);
    window.addEventListener("offline", handleStatusChange);
    return () => {
      window.removeEventListener("online", handleStatusChange);
      window.removeEventListener("offline", handleStatusChange);
    };
  }, [isOnline]);

  return (
    <div className="App">
      <h3>Hi {electronData.username}, </h3>
      <h1 className="online">
        You are:{" "}
        {isOnline ? (
          <span style={{ color: "green" }}>Online</span>
        ) : (
          <span style={{ color: "red" }}>Offline</span>
        )}
      </h1>
      <div id="text" style={{width:"300px", textAlign:'center'}}>
      <table id="customers">
        <tr>
          <th>OS</th>
          <th>HomeDir</th>
          <th>Author</th>
        </tr>
        <tr>
          <td> {electronData.osVersion()}</td>
          <td>{electronData.homeDir()}</td>
          <td>{electronData.author}</td>
        </tr>
      </table>
      </div>
    </div>
  );
}

export default App;
