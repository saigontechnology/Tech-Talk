import React, { useState, useEffect } from "react";
import {
  VictoryChart,
  VictoryCandlestick,
  VictoryAxis,
  VictoryTheme,
} from "victory";

//       {/* The VictoryChart component to visualize the data. */}
//       <VictoryChart
//         containerComponent={
//           // The VictoryZoomContainer allows interaction on the chart.
//           // It can provide zoom capabilities to the VictoryChart.
//           <VictoryZoomContainer
//             zoomDimension="x"   // Specifies that only the x-axis is zoomable.
//             allowZoom={true}    // Enables the zoom feature.
//           />
//         }
//       >
//         {/* The VictoryLine component displays the data as a line chart. */}
//         <VictoryLine data={data} />
//       </VictoryChart>
//     </div>

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
        // The visual theme of the chart.
        theme={VictoryTheme.material}
        // Add padding to the domain for a better visual appeal.
        domainPadding={{ x: 25, y: 20 }}
        // Define the scales for the axes. "linear" is a straight-line scale.
        scale={{ x: "linear", y: "linear" }}
        // Limit the visible area of the chart to y values between 0 and 120.
        domain={{ y: [0, 120] }}
      >
        {/* Dependent Axis, typically the Y-axis. */}
        <VictoryAxis dependentAxis />
        {/* Independent Axis, typically the X-axis. */}
        <VictoryAxis />
        <VictoryCandlestick
          // Define the colors of the candles based on if they are positive or negative.
          candleColors={{ positive: "#5f5c5b", negative: "#c43a31" }}
          // The data the chart will visualize.
          data={data}
          // Define the data keys for the various parts of the candlestick.
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
