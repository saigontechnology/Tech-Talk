/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        'pantone': {
          'beige': '#E8E4D9',
          'taupe': '#D4CEC3',
          'navy': '#2C3E50',
          'navy-dark': '#34495E',
          'orange': '#E67E22',
          'orange-dark': '#D35400',
          'burgundy': '#C0392B',
          'burgundy-dark': '#A93226',
        }
      }
    },
  },
  plugins: [],
} 