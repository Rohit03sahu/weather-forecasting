import React, { useState } from 'react';
import Select from "react-select";
import { useCookies } from 'react-cookie';

function LocationSearch(props){
    const [locations, setLocations] = useState([]);
    const optionList = [
                          { value: "red", label: "Red" },
                          { value: "green", label: "Green" },
                          { value: "yellow", label: "Yellow" },
                          { value: "blue", label: "Blue" },
                          { value: "white", label: "White" }
                       ];
    //const [selectedOptions, setSelectedOptions] = useState();
    /*function handleSelect(data) {
       setSelectedOptions(data);
    }*/
    return (
        <div>
           <table class="table">
            <tr>
                <td style={{"width":"25%"}}></td>
                <td scope="col">
                    <form class="d-flex" role="search">
                        <input class="form-control me-2" type="search" placeholder="Search City" aria-label="Search"></input>
                    </form>
                    <div className="dropdown-container">
                    
                    </div>
                </td>
                <td style={{"width":"25%"}}></td>
                
            </tr>
        </table>
      </div>
    );

    async function populateLocationData() {
    const response = await fetch('https://localhost:7195/api/weather/location');
    const data = await response.json();
    this.setState({ locations: data, locLoading: false });
  }
}

export default LocationSearch;