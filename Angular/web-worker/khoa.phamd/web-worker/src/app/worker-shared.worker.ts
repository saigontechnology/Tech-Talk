declare var onconnect: ((event: MessageEvent) => void) | null; // fix issue Cannot find name 'onconnect'.

const connections: MessagePort[] = [];
const taskList: { id: number; value: string }[] = [];

onconnect = (e) => {
  const port = e.ports[0];
  connections.push(port);

  port.onmessage = (event) => {
    if (event.data.type === 'addTask') {
      if (event.data.value) {
        taskList.push({ id: new Date().getTime(), value: event.data.value });
      }
      connections.forEach((connection) => {
        connection.postMessage(taskList);
      });
    }
    
    if (event.data.type === 'deleteTask') {
      const index = taskList.findIndex((item) => item.id === event.data.value);
      if (index < 0) return;
      taskList.splice(index, 1);
      connections.forEach((connection) => {
        connection.postMessage(taskList);
      });
    }
  };
  port.start();
};
