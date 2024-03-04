import { WeatherForecast } from "./components/Timeline/WeatherForecast";
import Daily from "./components/Timeline/Daily";
import Hourly from "./components/Timeline/Hourly";
import Minutely from "./components/Timeline/Minutely";

const AppRoutes = [
 /* {
    path: '/weather-forecast',
    element: <WeatherForecast />
  },*/
  {
    path: '/weather-forecast/daily',
    element: <Daily />
  },
  {
    path: '/weather-forecast/hourly',
    element: <Hourly />
  }
  ,{
    path: '/weather-forecast/minutely',
    element: <Minutely />
  }
];

export default AppRoutes;
