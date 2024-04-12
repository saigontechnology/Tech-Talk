import map from "@highcharts/map-collection/custom/world.geo.json";
import { Box } from "@material-ui/core";
import Loading from "Components/Loading";
// Import Highcharts
import Highcharts from "highcharts";
import HighchartsReact from "highcharts-react-official";
import HighchartsMap from "highcharts/modules/map";
import React, { useEffect, useState } from "react";
import { useSelector } from "react-redux";
import { createOptionForMap } from "Utilise/utilise";

HighchartsMap(Highcharts);

const WorldMap = ({ countriesData, isLoading }) => {
  const [option, setOption] = useState({});
  const globalState = useSelector((state) => state.global);

  useEffect(() => {
    (async () => {
      try {
        const options = createOptionForMap(
          map,
          countriesData,
          "Covid Map",
          "update until today"
        );
        setOption(options);
      } catch (error) {}
    })();
  }, [countriesData, globalState.themeMode]);
  return (
    <>
      {isLoading ? (
        <Box height="50vh" width="100%">
          <Loading />
        </Box>
      ) : (
        <HighchartsReact
          highcharts={Highcharts}
          options={option}
          constructorType={"mapChart"}
          immutable
        />
      )}
    </>
  );
};

export default React.memo(WorldMap);
