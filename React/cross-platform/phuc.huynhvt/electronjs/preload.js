const { contextBridge, ipcRenderer } = require("electron");
const os = require("os");

contextBridge.exposeInMainWorld("electronAPI", {
  homeDir: () => os.homedir(),
  osVersion: () => os.version(),
  cpuInfo: () => os.cpus(),
  author: "phuc.huynhvt",
  username: os.userInfo().username,
  ipcRenderer: ipcRenderer,
  // Handle toggle dark mode
  toggle: () => ipcRenderer.invoke("dark-mode:toggle"),
  system: () => ipcRenderer.invoke("dark-mode:system"),
  // Handle show Notification from Electron
  showNotification: (message) => ipcRenderer.send("show-notification", message),
  startDrag: (fileName) => {
    // Handle Drag Drop
    ipcRenderer.send("ondragstart", fileName);
  },
  onDeepLink: (callback) => ipcRenderer.on('deep-link', callback),
  removeDeepLink: (callback) => ipcRenderer.removeListener('deep-link', callback),
  receiveDeepLink: (callback) => ipcRenderer.on('deep-link', callback),
});

