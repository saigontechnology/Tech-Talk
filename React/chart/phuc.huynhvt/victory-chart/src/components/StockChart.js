import React, { useState, useEffect } from "react";
import {
  VictoryChart,
  VictoryCandlestick,
  VictoryAxis,
  VictoryTheme,
} from "victory";

const sampleData = [
  { x: 1, open: 5, close: 10, high: 15, low: 2, label: "right-side-up" },
  { x: 2, open: 10, close: 15, high: 20, low: 5, label: "right-side-up" },
  { x: 3, open: 15, close: 20, high: 22, low: 10, label: "right-side-up" },
  { x: 4, open: 20, close: 10, high: 25, low: 7, label: "right-side-up" },
  { x: 5, open: 10, close: 8, high: 15, low: 5, label: "right-side-up" },
  { x: 6, open: 50, close: 44, high: 60, low: 32, label: "right-side-up" },
];

function StockChart() {
  const [data, setData] = useState(sampleData);

  useEffect(() => {
    const interval = setInterval(() => {
      setData((prevData) => [
        ...prevData,
        {
          x: prevData.length + 1,
          open: randomNumber(1, 100),
          close: randomNumber(1, 100),
          high: randomNumber(1, 100),
          low: randomNumber(1, 100),
          label: "111 right-side-up",
        },
      ]);
    }, 1000);

    return () => clearInterval(interval);
  }, []);

  const randomNumber = (min, max) => {
    return Math.random() * (max - min) + min;
  };
  return (
    <>
      <VictoryChart
        theme={VictoryTheme.material}
        domainPadding={{ x: 25, y: 20 }}
        scale={{ x: "linear", y: "linear" }}
        domain={{ y: [0, 120] }}
      >
        <VictoryAxis dependentAxis />
        <VictoryAxis />
        <VictoryCandlestick
          candleColors={{ positive: "#5f5c5b", negative: "#c43a31" }}
          data={data}
          x="x"
          open="open"
          close="close"
          high="high"
          low="low"
        />
      </VictoryChart>
    </>
  );
}

export default StockChart;
