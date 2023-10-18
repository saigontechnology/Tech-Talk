import React, { useState, useEffect } from "react";
import { useQuery } from "react-query";
import { VictoryChart, VictoryLine, VictoryTheme } from "victory";
import BarChartCustomize from "../components/BarChartCustomize";
import StockChart from "../components/StockChart";
import BrushableChart from "../components/BrushableChart";
import AnimatingCircularProgressBar from "../components/AnimatingCircularProgressBar";
import HistogramWithSlide from "../components/HistogramWithSlide";
import RadarChart from "../components/RadarChart";
import StackedBard from "../components/StackedBard";
import MultipleDependentAxes from "../components/MultipleDependentAxes";
import HorizontalGroupedBarChart from "../components/HorizontalGroupedBarChart";
import StackedGroupedBarsChart from "../components/StackedGroupedBarsChart";
import "../App.css";

const fetchRandomData = async () => {
  const response = await fetch("https://randomuser.me/api/?results=10");
  const data = await response.json();
  return data.results.map((_, index) => ({ x: index, y: Math.random() * 100 }));
};

// Define common styles outside the component
const commonStyle = {
  maxWidth: "600px",
  border: "1px grey solid",
  padding: "40px",
};

const listChart = [
  { title: "Bar Chart", Component: BarChartCustomize, props: { allowDrag: false } },
  { title: "Bar Chart Allow Drag with Right Axis", Component: BarChartCustomize, props: { allowDrag: true, axisRight: true } },
  { title: "Animating Circular Progress Bar", Component: AnimatingCircularProgressBar },
  { title: "Multiple Dependent Axes", Component: MultipleDependentAxes },
  { title: "Stacked Bard with Central Axis", Component: StackedBard },
  { title: "Radar Chart", Component: RadarChart },
  { title: "Histogram with Slide", Component: HistogramWithSlide },
  { title: "Candle Chart/Stock Chart - with render by seconds", Component: StockChart },
  { title: "Stacked Grouped Bars Chart", Component: StackedGroupedBarsChart },
  { title: "Horizontal Grouped Bar Chart", Component: HorizontalGroupedBarChart },
  { title: "Brushable Chart", Component: BrushableChart },
];

const Chart = () => {
  const [chartData, setChartData] = useState([]);
  const { data } = useQuery("randomData", fetchRandomData, {
    refetchInterval: 5000,
  });

  useEffect(() => {
    if (data) {
      setChartData(data);
    }
  }, [data]);

  return (
    <div className="container">
      {listChart.map((item, index) => (
        <div key={index} className="item" style={commonStyle}>
          {item.title && <h1>{item.title}</h1>}
          <item.Component {...item.props} />
        </div>
      ))}
      <div className="item" style={commonStyle}>
        <VictoryChart theme={VictoryTheme.material}>
          <VictoryLine data={chartData} />
        </VictoryChart>
      </div>
    </div>
  );
};

export default Chart;
