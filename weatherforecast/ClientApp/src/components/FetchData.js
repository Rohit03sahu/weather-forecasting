import React, { Component } from 'react';

export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
    this.state = { forecasts: [], loading: true };
    this.state = { locations: [], locLoading: true };
  }

  componentDidMount() {
    this.populateWeatherData();
    this.populateLocationData();
  }

  static renderForecastsTable(forecasts) {
    return (
    
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Date</th>
            <th>Temp. (C)</th>
            <th>Temp. (F)</th>
          </tr>
        </thead>
        <tbody>
          {
                    
          }
        </tbody>
      </table>
    );
  }

  static renderLocationData(locations) {
    return (
      
       <select>
           <option value="Select">--Select--</option>
            { 
                locations.map(location => 
                    <option value={location}>{location}</option>)
            }
       </select>
    );
  }


  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchData.renderForecastsTable(this.state.forecasts);

    let locContent = this.state.locLoading
      ? <p><em>Loading...</em></p>
      : FetchData.renderLocationData(this.state.locations);

    return (
      <div>
        <h1 id="tabelLabel" >Weather forecast</h1>
        <p>This component demonstrates fetching data from the server.</p>
        
        <table className='table table-striped' aria-labelledby="tabelLabel">
            <tr>
                <td>Source</td>
                <td>{locContent}</td>
                <td>Destination</td>
                <td>{locContent}</td>
                <td>TimeLine</td>
                <td>
                    <select>
                        <option value="Select">--Select--</option>
                        <option value="minutely">Minutely</option>
                        <option value="hourly">Hourly</option>
                        <option value="daily">Daily</option>
                    </select>
                </td>
                <td><button id="search">Search</button></td>
            </tr>
        </table>

        {contents}
      </div>
    );
  }

  async populateWeatherData() {
    const response = await fetch('https://localhost:7195/api/weather/forecastbytimeline?SourceLocation=delhi&DestLocation=mumbai&timeline=Daily');
    debugger;
    const data = await response.json();
    console.log(data);
    this.setState({ forecasts: data.source, loading: false });
  }

  async populateLocationData() {
    const response = await fetch('https://localhost:7195/api/weather/location');
    const data = await response.json();
    console.log(data);
    this.setState({ locations: data, locLoading: false });
  }
}
