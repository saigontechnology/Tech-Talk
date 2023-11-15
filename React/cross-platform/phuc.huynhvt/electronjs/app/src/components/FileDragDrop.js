import React from "react";

const FileDragDrop = () => {
  const electronAPI = window.electronAPI;

  const handleDragOver = (e) => {
    e.preventDefault();
  };

  const handleDrop = (e) => {
    e.preventDefault();
    const filePath = e.dataTransfer.files[0].path;
    electronAPI.startDrag(filePath);
  };

  return (
    <>
      <div>
        <h3>5/ Drag - Drop file from Electron </h3>
        <div
          className="dragdropbox"
          onDragOver={handleDragOver}
          onDrop={handleDrop}
        >
          Drop files here to upload
        </div>
      </div>
    </>
  );
};

export default FileDragDrop;
