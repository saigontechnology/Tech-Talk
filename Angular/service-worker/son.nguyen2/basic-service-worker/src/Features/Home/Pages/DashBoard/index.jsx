import { Container, Grid, Typography } from "@material-ui/core";
import { covidApi } from "Api/covidApi";
import MainLayout from "Components/Layouts";
import BarChartCovid from "Features/Home/Components/BarChart";
import ButtonGroup from "Features/Home/Components/ButtonGroup";
import LineChartCovid from "Features/Home/Components/LineChart";
import WorldMap from "Features/Home/Components/Map";
import TableCountriesCovid from "Features/Home/Components/Table";
import moment from "moment";
import { useSnackbar } from "notistack";
import React, { useEffect, useState } from "react";
import { Trans, useTranslation } from "react-i18next";
import { useSelector } from "react-redux";
import {
  countNumberOfDay,
  filterContinentDataMapToChart,
  transformDataMapToChart,
  transformToMapData,
} from "Utilise/utilise";

HomePage.propTypes = {};

function HomeTitle({ summaryData }) {
  const { updatedAt, cases, deaths, recovered } = summaryData;
  const [t] = useTranslation();


  return (
    <>
      <Trans i18nKey="home.title" t={t}>
        Globally, as of
        <strong style={{ color: "green" }}>{{ updatedAt }}</strong>, there have been
        <strong style={{ color: "blue" }}>{{ cases }}</strong> confirmed cases of
        COVID-19, including <strong style={{ color: "red" }}>{{ deaths }}</strong> deaths,
        <strong style={{ color: "blue" }}>{{ recovered }}</strong> recovered reported to
        WHO
      </Trans>
    </>
  );
}

function HomePage(props) {
  const [countriesData, setCountriesData] = useState();
  const [continentsData, setContinentsData] = useState();
  const [timelineData, setTimelineData] = useState();
  const [mapData, setMapData] = useState();
  const [summaryData, setSummaryData] = useState();
  const [isLoading, setIsLoading] = useState(true);
  const [timeGap, setTimeGap] = useState(1);
  const { enqueueSnackbar } = useSnackbar();
  const [t] = useTranslation();
  const globalState = useSelector((state) => state.global);

  // eslint-disable-next-line react-hooks/exhaustive-deps
  useEffect(() => getSummaryOfWorldData(), [globalState]);
  useEffect(() => getContinentsData(), []);
  // eslint-disable-next-line react-hooks/exhaustive-deps
  useEffect(() => getTimeLineData(), [timeGap]);
  useEffect(() =>{ 
    getAllCountryData();
  }, []);
  // data to fill in title
  
  const getSummaryOfWorldData = async () => {
    try {
      const summaryWorldData = await covidApi.getSummary();
      const { updated, cases, deaths, recovered } = summaryWorldData;
      setSummaryData({
        ...summaryData,
        updatedAt:
          globalState.language === "en"
            ? moment(updated).format("LL")
            : moment(updated).format("DD/MM/YYYY"),
        cases: new Intl.NumberFormat().format(+cases),
        deaths: new Intl.NumberFormat().format(+deaths),
        recovered: new Intl.NumberFormat().format(+recovered),
      });
    } catch (error) {}
  };

  
  // data of line chart
  const getTimeLineData = async () => {
    try {
      const numberOfDayFromToday = countNumberOfDay();
      const timelineData = await covidApi.getTimelineOfWorld({
        lastdays: numberOfDayFromToday,
      });
      const dataMapToChart = transformDataMapToChart(timelineData, timeGap);
      setTimelineData(dataMapToChart);
    } catch (error) {
      console.log(error);
    }
  };

  // data of world map and table
  const getAllCountryData = async () => {
    try {
      setIsLoading(true);
      const data = await covidApi.getAllCountry();
      const mapData = transformToMapData(data);
      setMapData(mapData);
      setCountriesData(data);
      setIsLoading(false);
    } catch (error) {
      console.log({ error });
    }
  };

  // data of bar chart
  const getContinentsData = async () => {
    try {
      const continentsData = await covidApi.getAllContinent();
      setContinentsData(filterContinentDataMapToChart(continentsData));
    } catch (error) {
      console.log({ error });
    }
  };

  const handleChangeShowTimelyLineChart = (value) => {
    setTimeGap(value);
  };

  return (
    <MainLayout>
      <Container>
        <Grid container spacing={1}>
          <Grid item xs={12}>
            <WorldMap countriesData={mapData} isLoading={isLoading} />
          </Grid>
          <Grid item xs={12}>
            <Typography variant="h5">
              {summaryData && <HomeTitle summaryData={summaryData} />}
            </Typography>
          </Grid>
          <Grid item xs={12} sm={5} style={{ marginTop: "38.4px" }}>
            {continentsData && <BarChartCovid continentsData={continentsData} />}
          </Grid>
          <Grid item xs={12} sm={7}>
            {timelineData && (
              <ButtonGroup onChangeShowType={handleChangeShowTimelyLineChart} />
            )}
            {timelineData && <LineChartCovid timelineData={timelineData} />}
          </Grid>
          <Grid item xs={12}>
            <h3>{t("home.tableTitle")}</h3>
            {countriesData && <TableCountriesCovid countriesData={countriesData} />}
          </Grid>
        </Grid>
      </Container>
    </MainLayout>
  );
}

export default HomePage;
