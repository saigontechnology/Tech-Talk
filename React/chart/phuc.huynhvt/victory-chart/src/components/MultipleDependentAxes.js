//MultipleDependentAxes
import React from "react";
import { VictoryChart, VictoryAxis, VictoryLine, VictoryTheme } from "victory";

const data = [
  [
    { x: 1, y: 1 },
    { x: 2, y: 2 },
    { x: 3, y: 3 },
    { x: 4, y: 4 },
  ],
  [
    { x: 1, y: 400 },
    { x: 2, y: 350 },
    { x: 3, y: 300 },
    { x: 4, y: 250 },
  ],
  [
    { x: 1, y: 75 },
    { x: 2, y: 85 },
    { x: 3, y: 95 },
    { x: 4, y: 100 },
  ],
];

const maxima = data.map((dataset) => Math.max(...dataset.map((d) => d.y)));

const xOffsets = [50, 200, 350];
const tickPadding = [0, 0, -15];
const anchors = ["end", "end", "start"];
const colors = ["black", "red", "blue"];

const MultipleDependentAxes = () => {
  return (
    <div>
      <VictoryChart
        // Use the material theme from Victory
        theme={VictoryTheme.material}
        // Set the dimensions of the chart
        width={400}
        height={400}
        // Define the visible domain for the y-axis
        domain={{ y: [0, 1] }}
      >
        {/* Independent Axis (usually represents the x-axis) */}
        <VictoryAxis />

        {/* Render multiple dependent axes (usually y-axes) based on data */}
        {data.map((d, i) => (
          <VictoryAxis
            dependentAxis
            key={i}
            // Set the offset for each y-axis
            offsetX={xOffsets[i]}
            // Styling the axis, ticks, and tick labels
            style={{
              axis: { stroke: colors[i] },
              ticks: { padding: tickPadding[i] },
              tickLabels: { fill: colors[i], textAnchor: anchors[i] },
            }}
            // Define the tick values on the y-axis
            tickValues={[0.25, 0.5, 0.75, 1]}
            // Format the tick values by scaling them
            tickFormat={(t) => t * maxima[i]}
          />
        ))}

        {/* Render the data lines for each dataset */}
        {data.map((d, i) => (
          <VictoryLine
            key={i}
            // The dataset for the line
            data={d}
            // Style the line with a specific color
            style={{ data: { stroke: colors[i] } }}
            // Normalize the y-values for the line data
            y={(datum) => datum.y / maxima[i]}
          />
        ))}
      </VictoryChart>
    </div>
  );
};

export default MultipleDependentAxes;
