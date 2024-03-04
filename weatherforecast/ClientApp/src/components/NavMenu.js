import Button from 'react-bootstrap/Button';
import Container from 'react-bootstrap/Container';
import Form from 'react-bootstrap/Form';
import Nav from 'react-bootstrap/Nav';
import Navbar from 'react-bootstrap/Navbar';
import NavDropdown from 'react-bootstrap/NavDropdown';

function NavMenu() {

  return (
  
            <div class="topnav">
                <a href="weather-forecast/daily">Today</a>
                <a href="weather-forecast/daily">Daily</a>
                <a href="weather-forecast/hourly">Hourly</a>
                <a href="weather-forecast/minutely">Minutely</a>
                <a href="weather-forecast/minutely">Monthly</a>
                <a class="split" href="weather-forecast">Home</a>
           
            {/* <ul  class="nav nav-pills nav-fill">
                <li class="nav-item">
                    <a class="flex-sm-fill text-sm-center nav-link active" href="weather-forecast/daily">Today</a>
                </li>
                <li class="nav-item">
                    <a class="flex-sm-fill text-sm-center nav-link" href="weather-forecast/daily">Daily</a>
                </li>            
                <li class="nav-item">
                    <a class="flex-sm-fill text-sm-center nav-link" href="weather-forecast/hourly">Hourly</a>
                </li>
                <li class="nav-item">
                    <a class="flex-sm-fill text-sm-center nav-link" href="weather-forecast/minutely">Minutely</a>
                </li>
                <li class="nav-item">
                    <a class="flex-sm-fill text-sm-center nav-link" href="weather-forecast/daily">Monthly</a>
                </li>
              <li class="nav-item dropdown">
                <a class="nav-link dropdown-toggle" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                Dropdown
                </a>
                <ul class="dropdown-menu">
                <li><a class="dropdown-item" href="#">Action</a></li>
                <li><a class="dropdown-item" href="#">Another action</a></li>
                <li></li>
                <li><a class="dropdown-item" href="#">Something else here</a></li>
                </ul>
            </li>
            <li class="nav-item">
                <a class="nav-link disabled" aria-disabled="true">Disabled</a>
            </li> 
            </ul>*/}
          </div>
  );
}

export default NavMenu;