import { Container, Grid, makeStyles, Paper, Typography } from "@material-ui/core";
import MainLayout from "Components/Layouts";
import React from "react";
import { useSelector } from "react-redux";

const useStyles = makeStyles((theme) => ({
  mainFeaturedPostContent: {
    position: "relative",
    padding: theme.spacing(3),
    [theme.breakpoints.up("md")]: {
      padding: theme.spacing(6),
      paddingRight: 0,
    },
  },
}));

function NewsDetail() {
  const { news } = useSelector((state) => state.news);
  const classes = useStyles();
  return (
    <MainLayout>
      <Container>
        <Paper className={classes.mainFeaturedPost}>
          <Grid container>
            <Grid item xs={12}>
              <img
                src={news.urlToImage}
                alt={news.title}
                style={{ width: "100%", height: "40vh" }}
              />
              <Typography component="h1" variant="h3" color="inherit" gutterBottom>
                {news.title}
              </Typography>
              <Typography variant="h5" color="inherit" paragraph>
                {news.description}
              </Typography>
              <Typography variant="h5" color="inherit" paragraph>
                {news.content}
              </Typography>
            </Grid>
          </Grid>
        </Paper>
      </Container>
    </MainLayout>
  );
}

export default NewsDetail;
