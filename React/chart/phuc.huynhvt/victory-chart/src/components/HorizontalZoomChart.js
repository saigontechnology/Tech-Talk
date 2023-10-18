import React from "react";
import { VictoryChart, VictoryLine, VictoryZoomContainer } from "victory";

const dataHorizontalZoom = [
  { x: 1, y: 2 },
  { x: 2, y: 3 },
  { x: 3, y: 5 },
  { x: 4, y: 7 },
  { x: 5, y: 4 },
  { x: 6, y: 3 },
  { x: 7, y: 6 },
  { x: 8, y: 9 },
  { x: 1, y: 2 },
  { x: 2, y: 3 },
  { x: 3, y: 5 },
  { x: 4, y: 7 },
  { x: 5, y: 4 },
  { x: 6, y: 3 },
  { x: 7, y: 6 },
  { x: 8, y: 9 },
  { x: 1, y: 2 },
  { x: 2, y: 3 },
  { x: 3, y: 5 },
  { x: 4, y: 7 },
  { x: 5, y: 4 },
  { x: 6, y: 3 },
  { x: 7, y: 6 },
  { x: 8, y: 9 },
];

function HorizontalZoomChart() {
  return (
    <div>
      {/* Title for the chart */}
      <h2>Chart with Horizontal Zoom</h2>

      {/* The VictoryChart component to visualize the data. */}
      <VictoryChart
        containerComponent={
          // The VictoryZoomContainer allows interaction on the chart.
          // It can provide zoom capabilities to the VictoryChart.
          <VictoryZoomContainer
            zoomDimension="x" // Specifies that only the x-axis is zoomable.
            allowZoom={true} // Enables the zoom feature.
          />
        }
      >
        {/* The VictoryLine component displays the data as a line chart. */}
        <VictoryLine data={dataHorizontalZoom} />
      </VictoryChart>
    </div>
  );
}

export default HorizontalZoomChart;
