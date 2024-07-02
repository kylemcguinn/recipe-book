/** @type {import('tailwindcss').Config} */
export default {
  purge: ['./index.html', './src/**/*.{vue,js,ts,jsx,tsx}'],
  content: [],
  theme: {
    extend: {
      lineClamp: {
        7: '7',
      },
    }
  },
  plugins: [
    require('@tailwindcss/forms'),
  ],
}

