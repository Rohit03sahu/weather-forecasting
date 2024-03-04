import React, { Component } from 'react';
import Select from "react-select";
import { WeatherForecast } from "./WeatherForecast";

function Daily(){

    return (
            <WeatherForecast timeline="daily"/>
    );
}

export default Daily;