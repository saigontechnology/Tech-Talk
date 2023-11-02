import React from 'react';
import { VictoryChart, VictoryGroup, VictoryBar, VictoryTheme } from 'victory';

const HorizontalGroupedBarChart = () => {
  return (
    <div>
      {/* VictoryChart is a wrapper to hold various chart components */}
      <VictoryChart
        theme={VictoryTheme.material}  // Specifies the theme. `VictoryTheme.material` provides styling according to Material Design principles.
        domain={{ y: [0.5, 5.5] }}    // Specifies the visible domain of the chart. This will make the y-axis show from 0.5 to 5.5.
      >
        {/* VictoryGroup groups data components together. */}
        <VictoryGroup
          horizontal   // Determines if the chart is horizontal. Bars will lay horizontally.
          offset={10}  // Space between each bar.
          style={{ data: { width: 6 } }}  // Styling for the data components in the group. Sets the width of bars to 6.
          colorScale={["#2596be", "#e28743", "#76b5c5"]}  // Specifies the colors for bars. First bar will be brown, second will be tomato, and third will be gold.
        >
          {/* Individual bars showing data */}
          <VictoryBar
            data={[
              { x: 1, y: 1 },
              { x: 2, y: 2 },
              { x: 3, y: 3 },
              { x: 4, y: 2 },
              { x: 5, y: 1 }
            ]}
          />
          <VictoryBar
            data={[
              { x: 1, y: 2 },
              { x: 2, y: 3 },
              { x: 3, y: 4 },
              { x: 4, y: 5 },
              { x: 5, y: 5 }
            ]}
          />
          <VictoryBar
            data={[
              { x: 1, y: 1 },
              { x: 2, y: 2 },
              { x: 3, y: 3 },
              { x: 4, y: 4 },
              { x: 5, y: 4 }
            ]}
          />
        </VictoryGroup>
      </VictoryChart>
    </div>
  );
};

export default HorizontalGroupedBarChart;