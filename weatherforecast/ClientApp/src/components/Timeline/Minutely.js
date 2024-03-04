import React, { Component } from 'react';
import Select from "react-select";
import { WeatherForecast } from "./WeatherForecast";

function Minutely(){
    return (
            <WeatherForecast timeline="minutely" />
    );
}

export default Minutely;