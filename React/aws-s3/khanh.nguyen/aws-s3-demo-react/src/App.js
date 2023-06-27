import "./App.css";
import { useState } from "react";

function App() {
  const [selectedFile, setSelectedFile] = useState();
  const [isFilePicked, setIsFilePicked] = useState(false);
  const changeHandler = (event) => {
    setSelectedFile(event.target.files[0]);
    setIsFilePicked(true);
  };

  const handleSubmission = async () => {
    const url = await fetch("http://localhost:8000", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ name: selectedFile.name }),
    }).then((response) => response.text());
    console.log("url=", url);
    await fetch(url, {
      method: "PUT",
      headers: {
        "Content-Type": selectedFile.type,
      },
      body: selectedFile,
    });
  };
  return (
    <div className="App">
      <header className="App-header">
        <div>
          <input type="file" name="file" onChange={changeHandler} />
          {isFilePicked ? (
            <div>
              <p>Filename: {selectedFile.name}</p>
              <p>Filetype: {selectedFile.type}</p>
              <p>Size in bytes: {selectedFile.size}</p>
              <p>
                lastModifiedDate:{" "}
                {selectedFile.lastModifiedDate.toLocaleDateString()}
              </p>
            </div>
          ) : (
            <p>Select a file to show details</p>
          )}
          <div>
            <button onClick={handleSubmission}>Submit</button>
          </div>
        </div>
      </header>
    </div>
  );
}

export default App;
