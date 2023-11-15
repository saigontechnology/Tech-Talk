// CALLBACK

function httpGetAsync(url, callback) {
  var xmlHttp = new XMLHttpRequest();
  xmlHttp.open("GET", url, true); // true for asynchronous
  xmlHttp.send(null);

  xmlHttp.onreadystatechange = function () {
    if (xmlHttp.readyState == 4 && xmlHttp.status == 200) {
      callback(xmlHttp);
    }
  };
}

httpGetAsync("https://picsum.photos/200/300", (data) => {
  console.log(data);

  // ex2:
  document.getElementById("img_1").setAttribute("src", data.responseURL);
  httpGetAsync("https://picsum.photos/200/300", (data) => {
    document.getElementById("img_2").setAttribute("src", data.responseURL);
    httpGetAsync("https://picsum.photos/200/300", (data) => {
      document.getElementById("img_3").setAttribute("src", data.responseURL);
    });
  });
});

//////////////////

// ex3: PROMISE
function httpGetAsync(url, resolve) {
  var xmlHttp = new XMLHttpRequest();
  xmlHttp.open("GET", url, true); // true for asynchronous
  xmlHttp.send(null);

  xmlHttp.onreadystatechange = function () {
    if (xmlHttp.readyState == 4 && xmlHttp.status == 200) {
      resolve(xmlHttp);
    }
  };
}

const currentPromise = new Promise((resolve, reject) => {
  httpGetAsync("https://picsum.photos/200/300", resolve);
});

const promise2 = new Promise((resolve, reject) => {
  httpGetAsync("https://picsum.photos/200/300", resolve);
});

const promise3 = new Promise((resolve, reject) => {
  httpGetAsync("https://picsum.photos/200/300", resolve);
});

currentPromise
  .then((data) => {
    document.getElementById("img_1").setAttribute("src", data.responseURL);
    return promise2;
  })
  .then((data) => {
    document.getElementById("img_2").setAttribute("src", data.responseURL);
    return promise3;
  })
  .then((data) => {
    document.getElementById("img_3").setAttribute("src", data.responseURL);
  })
  .catch((error) => {
    console.log(error);
  });
// --------------------------------------------------------

const executeAsync = async () => {
  try {
    const response = await currentPromise;
    document.getElementById("img_1").setAttribute("src", response.responseURL);
    const response2 = await promise2;
    document.getElementById("img_2").setAttribute("src", response2.responseURL);
    const response3 = await promise3;
    document.getElementById("img_3").setAttribute("src", response3.responseURL);
  } catch (error) {
    console.log(error);
  }
};
executeAsync();

// callback queue & microtask queue
console.log("Starts here");

setTimeout(() => {
  console.log("SetTimeout callback executed");
}, 0);

const promiseA = new Promise((resolve) => {
  resolve("Resolve promise data");
});
promiseA.then((value) => {
  console.log(value);
});

console.log("Ends here");

//////////
