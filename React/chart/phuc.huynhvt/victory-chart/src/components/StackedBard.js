//Stacked Bard with Central Axis
import React from "react";
import {
  VictoryChart,
  VictoryStack,
  VictoryBar,
  VictoryAxis,
  VictoryLabel,
} from "victory";

const dataA = [
  { x: "Personal Drones", y: 57 },
  { x: "Smart Thermostat", y: 40 },
  { x: "Television", y: 38 },
  { x: "Smartwatch", y: 37 },
  { x: "Fitness Monitor", y: 25 },
  { x: "Tablet", y: 19 },
  { x: "Camera", y: 15 },
  { x: "Laptop", y: 13 },
  { x: "Phone", y: 12 },
];

const dataB = dataA.map((point) => {
  const y = Math.round(point.y + 3 * (Math.random() - 0.5));
  return { ...point, y };
});

const width = 400;
const height = 400;

function StackedBard() {
  return (
    <VictoryChart
      // Set the chart orientation to horizontal.
      horizontal
      // Define the height and width of the chart.
      height={height}
      width={width}
      // Set the padding around the chart.
      padding={40}
    >
      <VictoryStack
        // Define styles for the data bars and their labels.
        style={{ data: { width: 25 }, labels: { fontSize: 15 } }}
      >
        {/* First data set rendered as bars */}
        <VictoryBar
          // Set the fill color for the bars.
          style={{ data: { fill: "green" } }}
          // The data set for this bar group.
          data={dataA}
          // Modify the y-values for the bars.
          y={(data) => -Math.abs(data.y)}
          // Format the labels for the bars.
          labels={({ datum }) => `${Math.abs(datum.y)}%`}
        />

        {/* Second data set rendered as bars */}
        <VictoryBar
          // Set the fill color for the bars.
          style={{ data: { fill: "#8dc63f" } }}
          // The data set for this bar group.
          data={dataB}
          // Format the labels for the bars.
          labels={({ datum }) => `${Math.abs(datum.y)}%`}
        />
      </VictoryStack>

      {/* Axis for the chart */}
      <VictoryAxis
        // Styles for the axis, ticks, and tick labels.
        style={{
          axis: { stroke: "transparent" },
          ticks: { stroke: "transparent" },
          tickLabels: { fontSize: 15, fill: "black" },
        }}
        // Custom component for tick labels.
        tickLabelComponent={
          <VictoryLabel
            // Position tick labels in the center of the chart.
            x={width / 2}
            textAnchor="middle"
          />
        }
        // Define which data points should have tick labels.
        tickValues={dataA.map((point) => point.x).reverse()}
      />
    </VictoryChart>
  );
}

export default StackedBard;
