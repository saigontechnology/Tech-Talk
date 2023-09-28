import React, { useState, useEffect } from "react";
import { useQuery } from "react-query";
import { VictoryChart, VictoryLine, VictoryTheme, VictoryBar } from "victory";
import BarChartCustomize from "../components/BarChartCustomize";
import StockChart from "../components/StockChart";
import HorizontalZoomChart from "../components/HorizontalZoomChart";
import BrushableChart from "../components/BrushableChart";

const fetchRandomData = async () => {
  const response = await fetch("https://randomuser.me/api/?results=10");
  const data = await response.json();
  return data.results.map((_, index) => ({ x: index, y: Math.random() * 100 }));
};

function Chart() {
  const [chartData, setChartData] = useState([]);
  const { data } = useQuery("randomData", fetchRandomData, {
    refetchInterval: 5000, // Refetch evssssery 5 seconds
  });

  useEffect(() => {
    if (data) {
      setChartData(data);
    }
  }, [data]);

  return (
    <>
      <div style={{ display: "flex" }}>
        <div style={{ width: "600px" }}>
          <BarChartCustomize allowDrag={false} />
        </div>
        <div style={{ width: "600px" }}>
          <BarChartCustomize allowDrag axisRight />
        </div>
      </div>
      <div style={{ display: "flex" }}>
        <div style={{ width: "600px" }}>
          <StockChart />
        </div>
        <div style={{ width: "600px" }}>
          <VictoryChart theme={VictoryTheme.material}>
            <VictoryLine data={chartData} />
          </VictoryChart>
        </div>
      </div>
      <div style={{ display: "flex" }}>
        <div style={{ width: "1600px" }}>
          <BrushableChart />
        </div>
      </div>
    </>
  );
}
export default Chart;
