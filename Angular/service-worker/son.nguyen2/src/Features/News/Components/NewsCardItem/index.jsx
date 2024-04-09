import Card from "@material-ui/core/Card";
import CardActionArea from "@material-ui/core/CardActionArea";
import CardContent from "@material-ui/core/CardContent";
import CardMedia from "@material-ui/core/CardMedia";
import { makeStyles } from "@material-ui/core/styles";
import Typography from "@material-ui/core/Typography";
import _ from "lodash";
import React from "react";

const useStyles = makeStyles({
  root: {
    maxWidth: "100%",
    height: 350,
  },
  cardContent: {
    height: 210,
  },
});

export default function NewsCardItem({ news }) {
  const classes = useStyles();

  return (
    <Card className={classes.root}>
      <CardActionArea>
        <CardMedia
          component="img"
          alt="Contemplative Reptile"
          height="140"
          image={news?.urlToImage || "https://source.unsplash.com/random"}
          title={news.name}
        />
        <CardContent className={classes.cardContent}>
          <Typography gutterBottom variant="h5" component="h2">
            {news.title}
          </Typography>
          <Typography variant="body2" color="textSecondary" component="p">
            {_.truncate(news.description, { length: 140, separator: "" })}
          </Typography>
        </CardContent>
      </CardActionArea>
    </Card>
  );
}
