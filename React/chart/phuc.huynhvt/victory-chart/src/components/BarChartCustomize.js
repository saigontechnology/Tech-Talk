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
      // It specifies the visible domain of the chart. In this case, for the y-axis, you're defining the visible range as from 0 to 10.
      // Note: { y: [0, 10] } means only values from 0 to 10 will be shown on the y-axis.
      domainPadding={{ x: 50 }}
      // It adds padding to the domain of a chart. { x: 50 } means that it will add padding of 50 units to the x-axis.
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
        style={{
          data: {
            fill: (d) => {
              return "rgb(141, 198, 63)";
            },
          },
        }}
      />
      <VictoryAxis /> 
      {/* is for the x-axis. */}
      <VictoryAxis dependentAxis />
        {/* dependentAxis: It tells the chart that this axis is dependent on the data. Typically, the y-axis is the dependent axis because it represents the data values. */}
      {axisRight && (
        <VictoryAxis
          dependentAxis //  dependentAxis: It's another y-axis.
          orientation="right" // orientation: "right" places the axis on the right.
          minDomain={0} // minDomain and maxDomain: They specify the minimum and maximum values for this axis.
          maxDomain={20}
        />
      )}
    </VictoryChart>
  );
}
export default BarChartCustomize;
