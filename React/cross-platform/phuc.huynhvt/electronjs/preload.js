const { contextBridge } = require("electron");
const os = require("os");

contextBridge.exposeInMainWorld("electron", {
  homeDir: () => os.homedir(),
  osVersion: () => os.version(),
  cpuInfo: () => os.cpus(),
  author: "phuc.huynhvt",
  username: os.userInfo().username,
});
