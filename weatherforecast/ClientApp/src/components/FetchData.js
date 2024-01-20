import React, { Component } from 'react';

export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
    this.state = { loc:[],locNames:[], timeLineValue: "", forecasts: [], postsPerPage: 24,
       currentPage: 1, loading: true, locations: [], locLoading: true  };
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
      const { postsPerPage, currentPage } = this.state;
      const indexOfLastPage = currentPage * postsPerPage;
      const indexOfFirstPage = indexOfLastPage - postsPerPage;
      const currentPosts = forecasts.data.slice(indexOfFirstPage, indexOfLastPage)
      
    return ( <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                    <th>Date</th>
                    <th>Location</th>
                    <th>Temp. (C)</th>
                    <th>Temp. (F)</th>                 
                    </tr>
                </thead>          
            <tbody>   
               {
                  currentPosts.map(forecast =>
                               <tr key={forecast.date}>
                                   <td>{forecast.timeStamp}</td>
                                   <td>{forecast.location}</td>
                                   <td>{forecast.temperatureInC}</td>
                                   <td>{forecast.temperatureInF}</td>                               
                               </tr>)
               }
           </tbody>
      </table>
    );
  }

   renderMultiLocForecastData(forecasts) {
     
    
    return ( <table>
           {
                forecasts.data.map(forecast =>
                    <table className='table table-striped' aria-labelledby="tabelLabel">
                        <thead>
                          <tr>
                            <th>Date</th>
                            <th>Location</th>
                            <th>Temp. (C)</th>
                            <th>Temp. (F)</th>                 
                          </tr>
                        </thead>  
                        <tbody>
                        {forecast.locationForecasts.map(x=>
                            <tr key={x.date}>
                                <td>{x.timeStamp}</td>
                                <td>{x.location}</td>
                                <td>{x.temperatureInC}</td>
                                <td>{x.temperatureInF}</td>                               
                            </tr>)}
                        </tbody>
                    </table>)
           }
           </table>
    );
  }

 
  showPagination = () => {
     const { postsPerPage, forecasts } = this.state;
     const pageNumbers = [];
     const totalPosts = forecasts.length;

     for(let i = 1; i<=Math.ceil(totalPosts/postsPerPage); i++){
       pageNumbers.push(i)
     }

     const pagination = (pageNumbers) => {
       this.setState({currentPage: pageNumbers})
     }

     return(
       <nav>
       <ul className="pagination">
       {pageNumbers.map(number => (
         <li key={number} className={this.state.currentPage === number ? 'page-item active' : 'page-item' }>
         <button onClick={()=> pagination(number)} className="page-link"> {number} </button>
         </li>
       ))}
       </ul>
       </nav>
     )


   }

   handleTimeLineChange=(event)=>{
        this.setState({timeLineValue: event.target.value});
        this.setState({ loading: true });
   }

   handleLocChange=(event)=>{
        const values = [...event.target.selectedOptions].map(opt => opt.value);
        const names = [...event.target.selectedOptions].map(opt => opt.text);
        this.setState({loc: values});
        this.setState({locNames: names}) 
   }


  render() {

    let locContent = this.state.locLoading
      ? <p><em>Loading...</em></p>
      :  this.renderLocationData(this.state.locations);

    return (
      <div>
        <h1 id="tabelLabel" >Weather forecast</h1>
        <p>This component demonstrates fetching data from the server.</p>        
        <table class="table table-bordered ">
            <tbody>
                <tr>
                    <td>Enter the Location Source</td>
                    <td>{locContent }</td>                    
                    <td>TimeLine</td>
                    <td>
                        <select onChange={this.handleTimeLineChange} >
                            <option value="Select">--Select--</option>
                            <option value="minutely">Minutely</option>
                            <option value="hourly">Hourly</option>
                            <option value="daily">Daily</option>
                        </select>
                    </td>
                    <td><button id="search" onClick={this.populateWeatherData}>Search</button></td>
                </tr>
            </tbody>
        </table>
        
        <br /><br />
        {  
            this.state.loading
                ? <p><em>Loading...</em></p>
                : this.state.loc.length <= 1 ? this.renderForecastData(this.state.forecasts):
                this.renderMultiLocForecastData(this.state.forecasts)
                  
            /*this.showPagination()*/
        }
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
