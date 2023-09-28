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
          <VictoryChart
            theme={VictoryTheme.material}
            width={600}
            height={350}
            scale={{ x: "time" }}
            padding={{ left: 80, top: 40, right: 80, bottom: 60 }}
            containerComponent={
              <VictoryZoomVoronoiContainer
                labels={(d) =>
                  {console.log(d)}
                  // `${convertTimestampToDate(d.x)} \n $${numberFormat.format(
                  //   d.y
                  // )}`
                }
                labelComponent={
                  <VictoryTooltip dy={15} pointerLength={4} cornerRadius={3} />
                }
                responsive={true}
                allowZoom={false}
                zoomDimension="x"
                zoomDomain={zoomDomain}
                onZoomDomainChange={handleZoom}
              />
            }
          >
            <VictoryAxis
              style={{ tickLabels: { angle: -45, textAnchor: "end" } }}
              tickCount={10}
              tickFormat={(t) => convertTimestampToDate(t)}
            />
            <VictoryAxis
              dependentAxis
              tickCount={10}
              tickFormat={(t) => `$${numberFormat.format(t)}`}
            />
            <VictoryLine data={data.map((e) => ({ x: e.time, y: e.close }))} />
          </VictoryChart>

          <VictoryLine
            theme={VictoryTheme.material}
            width={600}
            height={50}
            padding={{ left: 80, top: 0, right: 80, bottom: 0 }}
            containerComponent={
              <VictoryBrushContainer
                responsive={true}
                brushDimension="x"
                brushDomain={selectedDomain}
                onBrushDomainChange={handleBrush}
              />
            }
            data={data.map((e) => ({ x: e.time, y: e.close }))}
          />
        </div>
      )}
    </div>
  );
};

export default BrushableChart;
