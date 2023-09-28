const { app, BrowserWindow } = require("electron");
const url = require('url');
const path = require('path');
const isDev = require('electron-is-dev');

const createWindow = () => {
  // Create the browser window.
  const mainWindow = new BrowserWindow({
    width: 800,
    height: 600,
    webPreferences: {
      preload: path.join(__dirname, "./preload.js"),
      contextIsolation:true,
      nodeIntegration:true,
    },
  });
  mainWindow.loadURL(
    // isDev
    //   ? 'http://localhost:3000'
      // :
       `file://${path.join(__dirname, './app/build/index.html')}`
  );
  // // Open the DevTools.
  // if (isDev) {
  //   mainWindow.webContents.openDevTools();
  // }
}; 

// This method will be called when Electron has finished
// initialization and is ready to create browser windows.
// Some APIs can only be used after this event occurs.
app.whenReady().then(() => {
  createWindow();
});
