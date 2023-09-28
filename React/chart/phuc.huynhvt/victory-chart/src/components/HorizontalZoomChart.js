import React from 'react';
import { VictoryChart, VictoryLine, VictoryZoomContainer } from 'victory';

const data = [
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
      <h2>Chart with Horizontal Zoom</h2>
      <VictoryChart
        containerComponent={
          <VictoryZoomContainer 
            zoomDimension="x" 
            allowZoom={true}
          />
        }
      >
        <VictoryLine data={data} />
      </VictoryChart>
    </div>
  );
}

export default HorizontalZoomChart;