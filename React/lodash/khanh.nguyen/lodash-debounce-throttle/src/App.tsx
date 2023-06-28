import React, { ChangeEventHandler, useState } from "react";
import "./App.css";
import debounce from "lodash/debounce";
import throttle from "lodash/throttle";
interface ICount {
  count: number;
  setCount: (number: number) => void;
}

const InputWithJustOnchange = ({ count, setCount }: ICount) => {
  let number = count;
  const onChange: ChangeEventHandler<HTMLInputElement> = (e) => {
    number += 1;
    setCount(number);
  };

  return <input onChange={onChange} />;
};

const InputWithDebouncedOnchange = ({ count, setCount }: ICount) => {
  let number = count;
  const onChange: ChangeEventHandler<HTMLInputElement> = (e) => {
    number += 1;
    setCount(number);
    console.time("stream abc");
  };

  const debouncedOnChange = debounce(onChange, 2000, { leading: true });
  console.timeEnd("stream abc");

  return <input onChange={debouncedOnChange} />;
};

const InputWithThrottledOnchange = ({ count, setCount }: ICount) => {
  let number = count;
  const onChange: ChangeEventHandler<HTMLInputElement> = (e) => {
    number += 1;
    setCount(number);
  };

  const throttledOnChange = throttle(onChange, 1000, {
    leading: false,
    trailing: true,
  });

  return <input onChange={throttledOnChange} />;
};

function App() {
  const [normalCount, setNormalCount] = useState(0);
  const [debounceCount, setDebounceCount] = useState(0);
  const [throttleCount, setThrottleCount] = useState(0);

  return (
    <div className="App">
      <h1 style={{ textAlign: "center" }}>Debounce and throttle examples</h1>
      <div className="container">
        <div className="column">
          <h3>Just onChange callback: {normalCount}</h3>
          <InputWithJustOnchange
            count={normalCount}
            setCount={setNormalCount}
          />
        </div>
        <div className="column">
          <h3>Debounced onChange: {debounceCount}</h3>
          <InputWithDebouncedOnchange
            count={debounceCount}
            setCount={setDebounceCount}
          />
        </div>
        <div className="column">
          <h3>Throttled onChange: {throttleCount}</h3>
          <InputWithThrottledOnchange
            count={throttleCount}
            setCount={setThrottleCount}
          />
        </div>
      </div>
    </div>
  );
}

export default App;
