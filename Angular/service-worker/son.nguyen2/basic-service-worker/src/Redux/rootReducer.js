import { combineReducers } from "redux";
import { globalReducer } from "./Slices/globalSlice";
import { newsReducer } from "./Slices/newsSlice";

const rootReducer = combineReducers({
  global: globalReducer,
  news: newsReducer,
});

export default rootReducer;
