import {
  Card,
  CardContent,
  CardMedia,
  Container,
  Divider,
  Grid,
} from "@material-ui/core";
import { Skeleton } from "@material-ui/lab";
import { covidApi, newsApi } from "Api/covidApi";
import MainLayout from "Components/Layouts";
import { router } from "Constants/constants";
import NewsCardItem from "Features/News/Components/NewsCardItem";
import TabBarNews from "Features/News/Components/TabBarNews";
import React, { useEffect, useState } from "react";
import { useDispatch } from "react-redux";
import { useHistory } from "react-router-dom";
import { newsActions } from "Redux/rootAction";
import { registerServiceWorkerWithScope } from "registerServiceWorker";

const initialFilters = {};

function NewsPage(props) {
  const [newsList, setNewsList] = useState(null);
  const [filters, setFilters] = useState(initialFilters);
  const [isLoading, setIsLoading] = useState();
  const dispatch = useDispatch();
  const history = useHistory();
  
// useEffect(() => {
//   registerServiceWorkerWithScope("./news/");
// }, [])

  useEffect(() => {
    // getNewsList();
    getSummaryOfWorldData();
  }, [filters]);


  // data to fill in title
  const getSummaryOfWorldData = async () => {
    try {
      const summaryWorldData = await covidApi.getSummary();
      const { updated, cases, deaths, recovered } = summaryWorldData;
      
    } catch (error) {}
  };

  const getNewsList = async () => {
    try {
      setIsLoading(true);
      const newsList = await newsApi.getAllNews(filters);
      setNewsList(newsList);
      setIsLoading(false);
    } catch (error) {
      console.log(error);
    }
  };
  const handleChangeCategory = (category) => {
    setFilters({ ...filters, category: category });
  };

  const handleSelectNews = (e, news, index) => {
    const action = newsActions.selectNews(news);
    dispatch(action);
    history.push(`${router.newsDetail}/${index}`);
  };

  return (
    <MainLayout>
      <Container>
        <TabBarNews onChangeCategory={handleChangeCategory} />
        <Divider />
        <Grid container justifyContent="space-between" spacing={3}>
          {isLoading
            ? Array.from(new Array(6)).map((item, index) => (
                <Grid item xs={12} sm={6} md={4} key={index}>
                  <Card>
                    <CardMedia>
                      <Skeleton animation="wave" variant="rect" height={140} />
                    </CardMedia>
                    <CardContent>
                      <React.Fragment>
                        <Skeleton
                          animation="wave"
                          height={10}
                          style={{ marginBottom: 6 }}
                        />
                        <Skeleton animation="wave" height={10} width="80%" />
                      </React.Fragment>
                    </CardContent>
                  </Card>
                </Grid>
              ))
            : (newsList || []).map((news, index) => (
                <Grid
                  item
                  xs={12}
                  sm={6}
                  md={4}
                  key={news.description}
                  onClick={(e) => handleSelectNews(e, news, index)}
                >
                  <NewsCardItem news={news} />
                </Grid>
              ))}
        </Grid>
      </Container>
    </MainLayout>
  );
}

export default NewsPage;
