import React, { Component } from 'react';
import Select from "react-select";
import { WeatherForecast } from "./WeatherForecast";

function Hourly (){
  
    return (
            <WeatherForecast timeline="hourly" />
    );
 }

export default Hourly;