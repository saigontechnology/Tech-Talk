import { yupResolver } from "@hookform/resolvers/yup";
import { Input } from "@material-ui/core";
import Avatar from "@material-ui/core/Avatar";
import Box from "@material-ui/core/Box";
import Checkbox from "@material-ui/core/Checkbox";
import CssBaseline from "@material-ui/core/CssBaseline";
import FormControlLabel from "@material-ui/core/FormControlLabel";
import Grid from "@material-ui/core/Grid";
import Paper from "@material-ui/core/Paper";
import { makeStyles } from "@material-ui/core/styles";
import Typography from "@material-ui/core/Typography";
import LockOutlinedIcon from "@material-ui/icons/LockOutlined";
import InputTextField from "Components/FormFields/inputTextField";
import { router } from "Constants/constants";
import { useSnackbar } from "notistack";
import React, { useState } from "react";
import { useForm } from "react-hook-form";
import { useTranslation } from "react-i18next";
import { Link, useHistory } from "react-router-dom";
import * as yup from "yup";
import Loading from "Components/Loading";

function Copyright() {
  const [t] = useTranslation();
  const classes = useStyles();
  return (
    <Box>
      <Typography variant="body2" color="textSecondary" align="center">
        {"Copyright © "}
        {new Date().getFullYear()}
      </Typography>
      <Link className={classes.link} to={router.news}>
        {t("common.backToNews")}
      </Link>
    </Box>
  );
}

const useStyles = makeStyles((theme) => ({
  root: {
    height: "100vh",
  },
  image: {
    backgroundImage: "url(https://source.unsplash.com/random)",
    backgroundRepeat: "no-repeat",
    backgroundColor:
      theme.palette.type === "light" ? theme.palette.grey[50] : theme.palette.grey[900],
    backgroundSize: "cover",
    backgroundPosition: "center",
  },
  paper: {
    margin: theme.spacing(8, 4),
    display: "flex",
    flexDirection: "column",
    alignItems: "center",
  },
  avatar: {
    margin: theme.spacing(1),
    backgroundColor: theme.palette.secondary.main,
  },
  form: {
    width: "100%", // Fix IE 11 issue.
    marginTop: theme.spacing(1),
  },
  submit: {
    margin: theme.spacing(3, 0, 2),
    backgroundColor: "blue",
    color: "white",
    borderRadius: "4px",
  },
  link: {
    textDecoration: "none",
    color: theme.palette.text.primary,
  },
}));
const initialValues = { email: "", password: "" };

const schema = yup.object().shape({
  email: yup
    .string()
    .required("Please fill your email")
    // .matches(/^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/, 'you email is not valid'),
    .email(),
  password: yup
    .string()
    .required("Please enter your password")
    .min(5, "password must have at least 5 letters"),
});

export default function LoginPage() {
  const history = useHistory();
  const { enqueueSnackbar } = useSnackbar();
  const [isLoading, setIsLoading] = useState(false);
  const [t] = useTranslation();
  const form = useForm({
    mode: "onSubmit",
    defaultValues: initialValues,
    resolver: yupResolver(schema),
  });

  const handleSubmitLogin = async (data) => {
    setIsLoading(true);
    if (data.password !== "covid19") {
      enqueueSnackbar("Password incorrect", {
        variant: "error",
        preventDuplicate: true,
      });
      setIsLoading(false);
      return;
    }
    setTimeout(() => {
      window.localStorage.setItem("user", JSON.stringify(data));
      enqueueSnackbar("Login successfully", {
        variant: "success",
        preventDuplicate: true,
      });
      setIsLoading(false);
      history.push(router.home);
    }, 1000);
  };

  const classes = useStyles();
  return (
    <Grid container component="main" className={classes.root}>
      {isLoading && <Loading />}
      <CssBaseline />
      <Grid item xs={false} sm={4} md={7} className={classes.image} />
      <Grid item xs={12} sm={8} md={5} component={Paper} elevation={6} square>
        <div className={classes.paper}>
          <Avatar className={classes.avatar}>
            <LockOutlinedIcon />
          </Avatar>
          <Typography component="h1" variant="h5">
            {t("common.login")}
          </Typography>
          <form className={classes.form} onSubmit={form.handleSubmit(handleSubmitLogin)}>
            <InputTextField name="email" label="email" form={form} />
            <InputTextField name="password" label="password" form={form} />

            <FormControlLabel
              control={<Checkbox value="remember" color="primary" />}
              label={t("common.login_remember")}
            />
            <Input
              type="submit"
              fullWidth
              // variant="contained"
              color="primary"
              className={classes.submit}
            >
              {t("common.login")}
            </Input>
            <Grid container>
              <Grid item xs>
                <Link to={router.register} className={classes.link}>
                  {t("common.login_forgotPassword")}
                </Link>
              </Grid>
              <Grid item>
                <Link to={router.register} className={classes.link}>
                  {t("common.login_haveAccount")}
                </Link>
              </Grid>
            </Grid>
            <Box mt={5}>
              <Copyright />
            </Box>
          </form>
        </div>
      </Grid>
    </Grid>
  );
}
