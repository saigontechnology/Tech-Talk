import React from "react";
import {
  VictoryChart,
  VictoryBrushContainer,
  VictoryAxis,
  VictoryBar,
} from "victory";

function BarChartCustomize({ allowDrag, axisRight }) {
  return (
    <VictoryChart
      domain={{ y: [0, 10] }}
      domainPadding={{ x: 50 }}
      containerComponent={
        <VictoryBrushContainer
          allowDrag={allowDrag}
          brushDimension="x"
          brushDomain={{ x: [0.5, 1.5] }}
        />
      }
    >
      <VictoryBar
        data={[
          { x: "HN", y: 3, z: 5 },
          { x: "DN", y: 2, z: 5 },
          { x: "HCM", y: 8, z: 5 },
          { x: "Vinh", y: 3, z: 5 },
          { x: "ThuDuc", y: 7, z: 5 },
        ]}
      />
      <VictoryAxis />
      <VictoryAxis dependentAxis />
      {axisRight && (
        <VictoryAxis
          dependentAxis
          orientation="right"
          minDomain={0}
          maxDomain={20}
        />
      )}
    </VictoryChart>
  );
}
export default BarChartCustomize;
