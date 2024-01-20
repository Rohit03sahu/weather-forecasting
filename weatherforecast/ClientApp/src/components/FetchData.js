import React, { Component } from 'react';
import 'react-select/dist/react-select.css';
import 'react-virtualized/styles.css';
import 'react-virtualized-select/styles.css';

import VirtualizedSelect from 'react-virtualized-select';

export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
    this.state = { loc:[],locNames:[], timeLineValue: "", forecasts: [], loading: true, locations: [], locLoading: true  };
    this.handleTimeLineChange = this.handleTimeLineChange.bind(this);
    this.handleLocChange = this.handleLocChange.bind(this);
    //this.handleDestinationChange = this.handleDestinationChange.bind(this);
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
                <table className='table table-striped' aria-labelledby="tabelLabel">
                    <tr>
                        <td>Location : {forecasts.location}</td><td></td>
                        <td>Min Temp (C) : {forecasts.minTempInC}</td><td></td>
                        <td>Max Temp (C) : {forecasts.maxTempInC}</td><td></td>
                        <td>Avg Temp (C) : {forecasts.avgTempInC}</td><td></td>
                    </tr>
                </table>
            
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
                                    <td><strong>Location : </strong> {forecast.location}</td><td></td>
                                    <td><strong>Min Temp (C) : </strong> {forecast.minTempInC}</td><td></td>
                                    <td><strong>Max Temp (C) : </strong> {forecast.maxTempInC}</td><td></td>
                                    <td><strong>Avg Temp (C) : </strong> {forecast.avgTempInC}</td><td></td>
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

   handleTimeLineChange=(event)=>{
        this.setState({timeLineValue: event.target.value});
        
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
           
            <h1 id="tabelLabel" >Weather forecast</h1>
            <p>This component demonstrates fetching data from the server.</p>        
            <table style={{ width:"100%"}}>
                <tbody>
                    <tr style={{ textalign:"centre", width:"100%"}}>
                        <td style={{ width:"15%"}}> <strong>Enter the Location</strong> </td>
                        <td style={{ width:"30%"}}>{locContent }</td>                    
                        <td style={{ width:"15%"}}> <strong>TimeLine</strong> </td>
                        <td style={{ width:"30%"}}>
                            <select class="form-select" onChange={this.handleTimeLineChange} >
                                <option value="Select">--Select--</option>
                                <option value="minutely">Minutely</option>
                                <option value="hourly">Hourly</option>
                                <option value="daily">Daily</option>
                            </select>
                        </td>
                        <td style={{ width:"20%"}}><button class="btn-primary" id="search" onClick={this.populateWeatherData}>Search</button></td>
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
