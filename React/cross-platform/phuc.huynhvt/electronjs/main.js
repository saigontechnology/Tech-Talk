const {
  app,
  BrowserWindow,
  ipcMain,
  nativeTheme,
  Notification,
} = require("electron");
const path = require("path");
const isDev = require("electron-is-dev");

let mainWindow;

// CREATE NOTIFICATION
function showNotification(title, message) {
  new Notification({
    title: title,
    body: message,
  }).show();
  console.log("SHow");
}
// END CREATE NOTIFICATION

const createWindow = () => {
  // Create the browser window.
  mainWindow = new BrowserWindow({
    width: 800,
    height: 600,
    webPreferences: {
      preload: path.join(__dirname, "./preload.js"),
      contextIsolation: true,
      nodeIntegration: true,
    },
  });
  mainWindow.loadURL(
    isDev
      ? "http://localhost:3000"
      : `file://${path.join(__dirname, "./app/build/index.html")}`
  );

  ipcMain.handle("dark-mode:toggle", () => {
    if (nativeTheme.shouldUseDarkColors) {
      nativeTheme.themeSource = "light";
    } else {
      nativeTheme.themeSource = "dark";
    }
    return nativeTheme.shouldUseDarkColors;
  });

  // mainWindow.webContents.openDevTools();
  setTimeout(() => {
    showNotification(
      "Notification",
      "Your app has been running for 5 seconds!"
    );
  }, 5000); // 5000 milliseconds = 5 seconds
};
app.setAsDefaultProtocolClient("electronExample");

app.whenReady().then(createWindow);

// Handle drag drop
ipcMain.on("ondragstart", (event, filePath) => {
  const message = "File" + filePath.split("/").pop() + " uploaded";
  showNotification("Upload File Success", message);
});

ipcMain.on("show-notification", (_, message) => {
  new Notification({ title: "Notification", body: message }).show();
});
// End Handle drag drop

// Handle Deep Link
app.on("open-url", (event, url) => {
  event.preventDefault();
  handleDeepLink(url);
});

function handleDeepLink(url) {
  // Send URL to renderer process
  if (mainWindow) {
    mainWindow.webContents.send("deep-link", url);
  }
}
// End Handle Deep Link
