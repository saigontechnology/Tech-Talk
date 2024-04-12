import HighchartsReact from "highcharts-react-official";
import Highcharts from "highcharts";
import React, { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";
import { useSelector } from "react-redux";
import { createOptionForBarChart } from "Utilise/utilise";

BarChartCovid.propTypes = {};

function BarChartCovid({ continentsData }) {
  const [option, setOption] = useState({});
  const globalState = useSelector((state) => state.global);
  const [t] = useTranslation();

  const title = t("home.titleBarChart");
  const subTitle = t("home.subTitleChart");

  useEffect(() => {
    (async () => {
      try {
        const options = createOptionForBarChart(
          Highcharts,
          continentsData,
          title,
          subTitle
        );
        setOption(options);
      } catch (error) {}
    })();
  }, [continentsData, globalState.themeMode, title, subTitle]);

  return <HighchartsReact highcharts={Highcharts} options={option} immutable />;
}

export default React.memo(BarChartCovid);
