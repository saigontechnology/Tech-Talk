import { createSlice } from "@reduxjs/toolkit";

const newsSlice = createSlice({
  name: "news",
  initialState: { news: {} },
  reducers: {
    selectNews(state, action) {
      state.news = action.payload;
    },
  },
});

const { actions, reducer } = newsSlice;
export { actions as newsActions, reducer as newsReducer };
