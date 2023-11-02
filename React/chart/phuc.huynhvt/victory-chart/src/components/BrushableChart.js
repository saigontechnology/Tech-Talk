import React, { useState, useEffect } from "react";
import {
  VictoryTheme,
  VictoryChart,
  VictoryLine,
  VictoryBrushContainer,
  VictoryTooltip,
  VictoryAxis,
  createContainer,
} from "victory";

// The rest of your imports and utility functions remain unchanged...

const VictoryZoomVoronoiContainer = createContainer("zoom", "voronoi");

const BrushableChart = () => {
  const [data, setData] = useState(false);
  const [isFetching, setIsFetching] = useState(true);
  const [zoomDomain, setZoomDomain] = useState(null);
  const [selectedDomain, setSelectedDomain] = useState(null);

  useEffect(() => {
    fetch(
      "https://min-api.cryptocompare.com/data/histoday?fsym=BTC&tsym=USD&limit=2000"
    )
      .then((response) => response.json())
      .then((data) => {
        setData(data.Data);
        setIsFetching(false);
      });
  }, []);

  const handleZoom = (domain) => {
    setSelectedDomain(domain);
  };

  const handleBrush = (domain) => {
    setZoomDomain(domain);
  };

  const numberFormat = new Intl.NumberFormat("de-ch", {
    style: "decimal",
    currency: "USD",
  });

  const convertTimestampToDate = (t) => {
    var a = new Date(t * 1000);
    var year = a.getFullYear();
    var month = a.getMonth().toString().padStart(2, "0");
    var date = a.getDate().toString().padStart(2, "0");
    var time = `${date}.${month}.${year}`;
    return time;
  };

  return (
    <div>
      {!isFetching && (
        <div>
          {/* Main Chart */}
          <VictoryChart
            theme={VictoryTheme.material}   // Using the Material theme from Victory
            width={600} height={350}   // Dimensions of the chart
            scale={{ x: "time" }}   // x-axis is time-based
            padding={{ left: 80, top: 40, right: 80, bottom: 60 }}   // Padding for the chart
            containerComponent={  // Custom container to add Zoom & Voronoi functionalities
              <VictoryZoomVoronoiContainer
                labels={(d) => console.log(d)}   // Just logging the label data for now
                labelComponent={  // Configures the tooltip
                  <VictoryTooltip dy={15} pointerLength={4} cornerRadius={3} />
                }
                responsive={true}   // Adjusts container responsively
                allowZoom={false}   // Disables zooming functionality
                zoomDimension="x"   // Zoom is only allowed in the x-axis
                zoomDomain={zoomDomain}   // Sets the zoom domain
                onZoomDomainChange={handleZoom}   // Handler when zoom domain changes
              />
            }
          >
            {/* X-Axis */}
            <VictoryAxis
              style={{ tickLabels: { angle: -45, textAnchor: "end" } }}   // Rotates the tick labels for better visibility
              tickCount={10}   // Specifies the number of ticks on the axis
              tickFormat={(t) => convertTimestampToDate(t)}   // Converts the UNIX timestamp to a readable date
            />
            {/* Y-Axis */}
            <VictoryAxis
              dependentAxis   // Y-axis is dependent on data
              tickCount={10}
              tickFormat={(t) => `$${numberFormat.format(t)}`}   // Formats the tick to display as currency
            />
            {/* Line Chart */}
            <VictoryLine data={data.map((e) => ({ x: e.time, y: e.close }))} /> 
          </VictoryChart>

          {/* Brush Chart (For selecting a domain on the main chart) */}
          <VictoryLine
            theme={VictoryTheme.material}
            width={600} height={50}   // Dimensions of the brush chart
            padding={{ left: 80, top: 0, right: 80, bottom: 0 }}
            containerComponent={
              <VictoryBrushContainer
                responsive={true}
                brushDimension="x"   // Brushing is allowed only on the x-axis
                brushDomain={selectedDomain}   // Domain that is selected by brushing
                onBrushDomainChange={handleBrush}   // Handler when brush domain changes
              />
            }
            data={data.map((e) => ({ x: e.time, y: e.close }))}   // Maps the fetched data to the line chart
          />
        </div>
      )}
    </div>
  );
};

export default BrushableChart;
