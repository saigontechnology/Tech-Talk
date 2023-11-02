import React, { useState, useEffect } from 'react';
import { VictoryPie, VictoryLabel, VictoryAnimation } from 'victory';

function AnimatingCircularProgressBar() {
  // Define the component's states using useState hook.
  const [percent, setPercent] = useState(25);
  const [data, setData] = useState(getData(0));

  useEffect(() => {
    let currentPercent = 25;
    const interval = setInterval(() => {
      currentPercent += (Math.random() * 25);
      currentPercent = (currentPercent > 100) ? 0 : currentPercent;
      setPercent(currentPercent);
      setData(getData(currentPercent));
    }, 2000);

    return () => clearInterval(interval);
  }, []);

  function getData(percentage) {
    return [{ x: 1, y: percentage }, { x: 2, y: 100 - percentage }];
  }

  return (
    <div>
      <svg viewBox="0 0 400 400" width="100%" height="100%">
        <VictoryPie
          // Ensures that the chart can be embedded within an SVG or another Victory component.
          standalone={false}
          
          // Defines animation configurations for the pie chart.
          animate={{ duration: 1000 }}
          
          // Sets the width and height of the pie chart.
          width={400} height={400}
          
          // Data that the pie chart will display.
          data={data}
          
          // Radius from the center of the pie to start rendering data.
          innerRadius={120}
          
          // Defines the corner radius of each segment in the pie chart.
          cornerRadius={25}
          
          // Returns null to ensure labels are not shown on each segment.
          labels={() => null}
          
          // Styles each segment of the pie based on its data.
          style={{
            data: { fill: ({ datum }) => {
              const color = datum.y > 30 ? "#8dc63f" : "red";
              return datum.x === 1 ? color : "transparent";
            }}
          }}
        />
        <VictoryAnimation duration={1000} data={{ percent }}>
          {(newProps) => {
            return (
              <VictoryLabel
                textAnchor="middle" verticalAnchor="middle"
                x={200} y={200}
                text={`${Math.round(newProps.percent)}%`}
                style={{ fontSize: 45 }}
              />
            );
          }}
        </VictoryAnimation>
      </svg>
    </div>
  );
}

export default AnimatingCircularProgressBar;
