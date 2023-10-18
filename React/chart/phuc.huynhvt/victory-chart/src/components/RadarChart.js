import React, { useState } from "react";
import {
  VictoryChart,
  VictoryTheme,
  VictoryGroup,
  VictoryArea,
  VictoryPolarAxis,
  VictoryLabel,
} from "victory";

const characterData = [
  { strength: 1, intelligence: 250, luck: 1, stealth: 40, charisma: 50 },
  { strength: 2, intelligence: 300, luck: 2, stealth: 80, charisma: 90 },
  { strength: 5, intelligence: 225, luck: 3, stealth: 60, charisma: 120 },
];

const RadarChart = () => {
  // Define state using useState hooks
  const [data, setData] = useState(processData(characterData));
  const [maxima, setMaxima] = useState(getMaxima(characterData));

  // Compute the maxima based on provided data
  function getMaxima(data) {
    const groupedData = Object.keys(data[0]).reduce((memo, key) => {
      memo[key] = data.map((d) => d[key]);
      return memo;
    }, {});
    return Object.keys(groupedData).reduce((memo, key) => {
      memo[key] = Math.max(...groupedData[key]);
      return memo;
    }, {});
  }

  // Process the data to fit the chart's requirements
  function processData(data) {
    const maxByGroup = getMaxima(data);
    const makeDataArray = (d) => {
      return Object.keys(d).map((key) => {
        return { x: key, y: d[key] / maxByGroup[key] };
      });
    };
    return data.map((datum) => makeDataArray(datum));
  }

  return (
    <VictoryChart
      // Set the chart to use polar coordinates.
      polar
      // Apply the material theme.
      theme={VictoryTheme.material}
      // Define the y-axis domain.
      domain={{ y: [0, 1] }}
    >
      <VictoryGroup
        // Set the colors for each data group.
        colorScale={["#8de67f", "#9a7fe6", "#e6c37f"]}
        // Define styles for the group.
        style={{ data: { fillOpacity: 0.2, strokeWidth: 2 } }}
      >
        {data.map((data, i) => (
          <VictoryArea key={i} data={data} />
        ))}
      </VictoryGroup>

      {Object.keys(maxima).map((key, i) => (
        <VictoryPolarAxis
          key={`polarAxis-${i}`}
          // This axis is dependent on the data.
          dependentAxis
          // Style definitions for the axis.
          style={{
            axisLabel: { padding: 10 },
            axis: { stroke: "none" },
            grid: { stroke: "grey", strokeWidth: 0.25, opacity: 0.5 },
          }}
          // Component for tick labels.
          tickLabelComponent={<VictoryLabel labelPlacement="vertical" />}
          // Position of label relative to the axis.
          labelPlacement="perpendicular"
          // Value for the axis.
          axisValue={i + 1}
          // Label text for the axis.
          label={key}
          // Format for the tick labels.
          tickFormat={(t) => Math.ceil(t * maxima[key])}
          // Specific values to show ticks.
          tickValues={[0.25, 0.5, 0.75]}
        />
      ))}

      <VictoryPolarAxis
        // Position of label relative to the axis.
        labelPlacement="parallel"
        // Format for the tick labels.
        tickFormat={() => ""}
        // Styling for the axis.
        style={{
          axis: { stroke: "none" },
          grid: { stroke: "grey", opacity: 0.5 },
        }}
      />
    </VictoryChart>
  );
};

export default RadarChart;
