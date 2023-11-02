import React from 'react';
import { VictoryChart, VictoryGroup, VictoryStack, VictoryBar } from 'victory';

const StackedGroupedBarsChart = () => {
  // A function to generate data for bars.
  const getBarData = () => {
    return [1, 2, 3, 4, 5].map(() => {
      return [
        { x: 1, y: Math.random() },
        { x: 2, y: Math.random() },
        { x: 3, y: Math.random() }
      ];
    });
  };

  return (
    <div>
      {/* VictoryChart is a wrapper to hold various chart components */}
      <VictoryChart 
        domainPadding={{ x: 50 }} // Adds padding to the x-axis of the chart domain.
        width={400}  // Specifies the width of the chart.
        height={400} // Specifies the height of the chart.
      >
        <VictoryGroup 
          offset={20}  // Specifies the amount of space between bars.
          style={{ data: { width: 15 } }}  // Styling for the data components in the group. Sets the width of bars to 15.
        >
          {/* VictoryStack stacks bars on top of each other */}
          <VictoryStack colorScale={"red"}>  
            {getBarData().map((data, index) => {
              return <VictoryBar key={index} data={data}/>; // Represents individual bars.
            })}
          </VictoryStack>
          <VictoryStack colorScale={"green"}>
            {getBarData().map((data, index) => {
              return <VictoryBar key={index} data={data}/>;
            })}
          </VictoryStack>
          <VictoryStack colorScale={"blue"}>
            {getBarData().map((data, index) => {
              return <VictoryBar key={index} data={data}/>;
            })}
          </VictoryStack>
        </VictoryGroup>
      </VictoryChart>
    </div>
  );
};

export default StackedGroupedBarsChart;