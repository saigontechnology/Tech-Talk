import { createSlice } from "@reduxjs/toolkit";

const GlobalSlice = createSlice({
  name: "global",
  initialState: { themeMode: "light", language: "en"},
  reducers: {
    changeThemeMode(state, action) {
      state.themeMode = action.payload.themeMode;
    },
    changeLanguage(state, action) {
      state.language = action.payload.language;
    },
  },
});

const { actions, reducer } = GlobalSlice;
export { actions as globalActions, reducer as globalReducer };
