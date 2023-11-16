module.exports = (config: any) => {
   config.set({
     basePath: "./",
     plugins: [
       "karma-spec-reporter",
       "karma-jasmine",
       "karma-vite",
       "karma-coverage",
       "karma-chrome-launcher",
       "karma-firefox-launcher",
     ],
     browsers: ["Chrome", "Firefox"],
     frameworks: ["jasmine", "vite"],
     reporters: ["spec"],
     files: [
       {
         pattern: "src/**/*.test.ts",
         type: "module",
         watched: false,
         served: false,
       },
     ],
   });
};
