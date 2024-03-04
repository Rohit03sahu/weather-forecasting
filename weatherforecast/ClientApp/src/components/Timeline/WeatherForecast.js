import React, { Component } from 'react';
import Select from "react-select";

export class WeatherForecast extends Component {
  static displayName = WeatherForecast.name;

  constructor(props) {
    super(props);
    console.log(props);
    this.state = { loc:[],locNames:[], timeLineValue: props.timeline, forecasts: [], loading: true, locations: [], locLoading: true  };
    this.handleLocChange = this.handleLocChange.bind(this);
  }

  componentDidMount() {
    this.populateLocationData();
  }
  renderLocationData(locations) {
      
    return (   
       <select class="form-select" aria-label="Default select example" onChange={this.handleLocChange} multiple>
           <option value="Select" >--Select--</option>
            { 
                locations.locResponse.map(x=>                          
                            <option value={x.latLong}>{x.name}</option>)
            }
       </select>
    );
  }

  renderForecastData(forecasts) {    
    return (
            <div>  
                <div>
                    <table className='table table-bordered'>
                        <tr>
                            <td><strong>Location : </strong> {forecasts.location}</td>
                            <td><strong>Min Temp (C) : </strong> {forecasts.minTempInC}</td>
                            <td><strong>Max Temp (C) : </strong> {forecasts.maxTempInC}</td>
                            <td><strong>Avg Temp (C) : </strong> {forecasts.avgTempInC}</td>
                        </tr>
                    </table>
                </div>
                <div>
                    <table className='table table-bordered'>             
                        <thead>
                                <tr>
                                    <th>Date</th>
                                    <th>Temp. (C)</th>
                                    <th>Temp. (F)</th>                 
                                </tr>
                            </thead>          
                        <tbody>   
                           {
                              forecasts.data.map(forecast =>
                                           <tr key={forecast.date}>
                                               <td>{forecast.timeStamp}</td>
                                               <td>{forecast.temperatureInC}</td>
                                               <td>{forecast.temperatureInF}</td>                               
                                           </tr>)
                           }
                       </tbody>
                  </table>
              </div>
      </div>
    );
  }

   renderMultiLocForecastData(forecasts) {    
    return ( <div>
           {
                forecasts.data.map(forecast =>
                    <div>
                        <div>
                            <table class="table table-bordered ">
                                <tr>
                                    <td><strong>Location : </strong> {forecast.location}</td>
                                    <td><strong>Min Temp (C) : </strong> {forecast.minTempInC}</td>
                                    <td><strong>Max Temp (C) : </strong> {forecast.maxTempInC}</td>
                                    <td><strong>Avg Temp (C) : </strong> {forecast.avgTempInC}</td>
                                </tr>
                            </table>
                        </div>
                        <div>
                            <table class="table table-bordered ">
                                <thead>
                                  <tr>
                                        <th>Date</th>
                                        <th>Temp. (C)</th>
                                        <th>Temp. (F)</th>
                                        <th>Delta Temp. (C)</th>
                                  </tr>
                                </thead>  
                                <tbody>
                                {forecast.locationForecasts.map(x=>
                                    <tr key={x.date}>
                                        <td>{x.timeStamp}</td>
                                        <td>{x.temperatureInC}</td>
                                        <td>{x.temperatureInF}</td>
                                        <td>{x.deltaInC}</td>
                                    </tr>)}
                                </tbody>
                            </table>
                        </div>
                    </div>)
           }
           </div>
    );
  }

   handleLocChange=(event)=>{
        const values = [...event.target.selectedOptions].map(opt => opt.value);
        const names = [...event.target.selectedOptions].map(opt => opt.text);
        this.setState({loc: values});
        this.setState({locNames: names});
        this.setState({ loading: true });
   }

  render() {

    let locContent = this.state.locLoading
      ? <p><em>Loading...</em></p>
      :  this.renderLocationData(this.state.locations);

    return (
        <div>
          <div>           
           
            <br />
            <table class="table">
                <tbody>
                    <tr>
                        <td class="text-center"> <strong>Enter the Location</strong> </td>
                        <td class="text-center">{locContent }</td>                    
                        <td class="text-center"><button class="btn-primary" id="search" onClick={this.populateWeatherData}>Search</button></td>
                    </tr>
                </tbody>
            </table>
            </div>

            <div>
            <br /><br />
            {  
                this.state.loading
                    ? <p><em>Loading...</em></p>
                    : this.state.loc.length <= 1 ? this.renderForecastData(this.state.forecasts):
                    this.renderMultiLocForecastData(this.state.forecasts)
            }
          </div>
      </div>
    );
  }

  populateWeatherData=async ()=> {
      let leng = this.state.loc.length;    
      if(leng>=2)
      {
            await fetch('https://localhost:7195/api/weather/multilocforecastbytimeline',
                                        {
                                          method: 'POST',
                                          headers: {
                                            'Accept': 'application/json',
                                            'Content-Type': 'application/json',
                                          },
                                          body: JSON.stringify({
                                            locations: this.state.loc,
                                            timeLine: this.state.timeLineValue,
                                          })
                                        })
                                        .then((response) => response.json())
                                        .then((data) => {
                                              console.log(data);
                                              this.setState({ forecasts: data, loading: false });
                                           })
                                           .catch((err) => {
                                              console.log(err.message);
                                           });
           /* const data = await response.json();
            this.setState({ forecasts: data, loading: false }); */
      }
      else
      {
            let Loc=this.state.loc[0];
            let LocName=this.state.locNames[0];
            let queryParam='location='+Loc+'&locationname'+LocName+'&timeline='+this.state.timeLineValue;
            const response = await fetch('https://localhost:7195/api/weather/forecastbytimeline?'+queryParam);
            const data = await response.json();
            this.setState({ forecasts: data, loading: false });
      }
  }


  async populateLocationData() {
    const response = await fetch('https://localhost:7195/api/weather/location');
    const data = await response.json();
    this.setState({ locations: data, locLoading: false });
  }
}
